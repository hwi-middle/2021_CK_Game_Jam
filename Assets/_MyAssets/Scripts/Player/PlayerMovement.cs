using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //이동 처리
    public FixedJoystick joystick;
    public TouchField touchField;
    public Text runToggleText;
    private bool isRunToggleOn = false; //기본상태는 걷기

    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public float alternativeSpeedScale = 1f; // 왼쪽 Shift 키가 눌렸을 때의 속도(일반 속도보다 느리거나 빠르게 지정)

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //체력 처리
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecreasementAmount = 1f;
    public float healthDecreasementFrequency = 5f;
    public bool isDead = false;
    public bool isDying = false;
    public EDieType dieType = EDieType.None;

    //스태미너 처리
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDecreasementAmount = 20f;
    public float staminaIncreasementAmount = 20f;
    public float staminaIncresementDelay = 2f;
    private float idleTime = 0f;

    //마우스 움직임 처리
    public bool shouldCameraFreeze = false;
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    private Camera cam;
    Quaternion camRotation;
    Quaternion bodyRotation;

    //카메라 흔들림(Head Bobbing) 처리
    public Transform headTransform;
    public Transform camTransform;

    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    public float headBobSmoothing = 0.1f;

    private float walkingTime = 0f;
    private Vector3 targetCameraPos;

    //기절상태 처리
    public bool isStunned = false;
    public bool isStunInvincible = false;

    //저주상태 처리
    public bool isCursed = false;
    public bool isCurseInvincible = false;

    //Freeze 상태 처리
    public bool shouldMoveFreeze = false;
    public bool shouldDamageFreeze = false;

    //발소리 재생 처리
    public List<AudioData> audioDatas = new List<AudioData>();
    private List<AudioData> activatedAudioDatas = new List<AudioData>();
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;

    //바닥에 닿았는지 체크 처리
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private LayerMask groundMask;

    //뽑기, 판독의 동작 처리
    public bool doingTask;

    //싱글톤 처리
    static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go == null)
            {
                Debug.LogError("Player not found");
            }

            instance = go.GetComponent<PlayerMovement>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

        sensitivityX = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        sensitivityY = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

        currentHealth = maxHealth;

        StartCoroutine(DecreaseHealth());


        currentStamina = maxStamina;

        controller = GetComponent<CharacterController>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        audioSource = GetComponent<AudioSource>();

        cam = Camera.main;
        SetCursorLockState(CursorLockMode.Locked);
        camRotation = cam.transform.localRotation;
        bodyRotation = transform.localRotation;

        camTransform = cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //사망 및 기절시 조작 불가
        if (isDead || isStunned) return;

        //인스펙터에서 gravityScale이 변경될 수 있으므로 Update 메서드에서 처리
        gravity = GRAVITY_CONSTANT * gravityScale;

        SetCamera(); //마우스 처리
        CheckGrounded(); //착지 상태 체크

        //이동 관련 키 입력 받아오기
        float x;
        float y = velocity.y;
        float z;
        GetInputAxis(out x, out z, out isMoving);

        //Left Shift키가 눌렸는지 확인
        bool isRunKeyDown = false;

#if UNITY_STANDALONE_WIN
        isRunKeyDown = Input.GetKey(KeyCode.LeftShift);
#elif UNITY_ANDROID || UNITY_IOS
        isRunKeyDown = isRunToggleOn;
#endif

        if (isRunKeyDown)
        {
            //스태미너 시스템 적용
            if (isMoving)
            {
                currentStamina -= staminaDecreasementAmount * Time.deltaTime;
                if (currentStamina < 0)
                {
                    isRunToggleOn = false;
                    runToggleText.text = "WALK";
                    currentStamina = 0;
                }
            }

            if (isGrounded && currentStamina > 0)
            {
                shouldAlternativeSpeedApplied = true;
            }
        }

        //대기 시간에 따라 스태미너 회복
        if (isMoving)
        {
            walkingTime += Time.deltaTime;
            if (isRunKeyDown)
            {
                idleTime = 0f;
            }
            else
            {
                idleTime += Time.deltaTime;
            }
        }
        else
        {
            walkingTime = 0f;
            idleTime += Time.deltaTime;
        }

        if (idleTime >= staminaIncresementDelay)
        {
            currentStamina += staminaIncreasementAmount * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }

        //HeadBob 적용
        targetCameraPos = headTransform.position + CalculateHeadBobOffset(walkingTime);
        camTransform.position = Vector3.Lerp(camTransform.position, targetCameraPos, headBobSmoothing);

        if ((camTransform.position - targetCameraPos).magnitude <= Mathf.Epsilon)
        {
            camTransform.position = targetCameraPos;
        }

        //Alternative Speed를 적용해야하는지 확인 후 적용
        if (shouldAlternativeSpeedApplied)
        {
            x *= alternativeSpeedScale;
            z *= alternativeSpeedScale;
        }

        //저주상태일시
        if (isCursed)
        {
            x *= -1;
            z *= -1;
        }

        //중력적용
        y += gravity * Time.deltaTime;

        //플레이어 이동
        velocity = (transform.right * x * speed) + (transform.forward * z * speed) + (transform.up * y);
        //velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        PlayFootstepSound(isRunKeyDown, velocity);
    }

    public void RunToggle()
    {
        if (isRunToggleOn)
        {
            isRunToggleOn = false;
            runToggleText.text = "WALK";
        }
        else
        {
            isRunToggleOn = true;
            runToggleText.text = "RUN";
        }
    }

    public void SetCursorLockState(CursorLockMode mode)
    {
#if UNITY_STANDALONE_WIN
        Cursor.lockState = mode;
#endif
    }

    void SetCamera()
    {
        if (shouldCameraFreeze) return;

        sensitivityX = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        sensitivityY = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

#if UNITY_STANDALONE_WIN
        float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;
#elif UNITY_ANDROID || UNITY_IOS
        float yRotation = touchField.dragDiffPerFrame.x * sensitivityX * 0.5f;
        float xRotation = touchField.dragDiffPerFrame.y * sensitivityY * 0.5f;
#endif
        camRotation *= Quaternion.Euler(-xRotation, 0f, 0f);
        camRotation = ClampRotationAroundXAxis(camRotation);
        bodyRotation *= Quaternion.Euler(0f, yRotation, 0f);

        cam.transform.localRotation = camRotation;
        transform.localRotation = bodyRotation;
    }

    Vector3 CalculateHeadBobOffset(float t)
    {
        float horizontalOffset = 0f;
        float verticalOffset = 0f;
        Vector3 offset = Vector3.zero;

        if (t > 0)
        {
            float bobAmount = t * bobFrequency;
            if (shouldAlternativeSpeedApplied)
            {
                bobAmount *= alternativeSpeedScale;
            }

            horizontalOffset = Mathf.Cos(bobAmount) * bobHorizontalAmplitude;
            verticalOffset = Mathf.Sin(2 * bobAmount) * bobVerticalAmplitude;

            offset = headTransform.right * horizontalOffset + headTransform.up * verticalOffset;
        }

        return offset;
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            shouldAlternativeSpeedApplied = false;

            //중력이 중첩적용되는 것을 방지
            if (velocity.y < 0)
            {
                velocity.y = 0f;
            }
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90f, 90f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    void GetInputAxis(out float x, out float z, out bool isMoving)
    {
        if (shouldMoveFreeze)
        {
            x = 0f;
            z = 0f;
            isMoving = false;
            return;
        }
#if UNITY_STANDALONE_WIN
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
#elif UNITY_ANDROID || UNITY_IOS
        x = joystick.Horizontal;
        z = joystick.Vertical;
#endif

        //움직이고 있는지 확인
        if (x != 0 || z != 0) isMoving = true;
        else isMoving = false;
    }

    void PlayFootstepSound(bool isLeftShiftKeyDown, Vector3 v)
    {
        //발걸음 소리 출력이 필요한지 확인
        time += Time.deltaTime;
        bool isFootstepSoundRequired;

        //클립 개수 확인
        CountActivatedAudioClips();
        int activatedAudioClipsNum = activatedAudioDatas.Count;

        //활성화된 클립이 없으면 출력하지 않음
        if (activatedAudioClipsNum == 0)
        {
            isFootstepSoundRequired = false;
        }

        //Left Shift키가 눌렸을 때에는 alternativeSpeedScale값에 따라 frequency가 조절
        else if (isLeftShiftKeyDown)
        {
            isFootstepSoundRequired = time * alternativeSpeedScale > frequency;
        }
        //평상시에는 frequency만큼 시간이 지났는지 확인
        else
        {
            isFootstepSoundRequired = time > frequency;
        }

        //발걸음 소리 출력
        if (isFootstepSoundRequired && isMoving && isGrounded)
        {
            time = 0f;

            int clipIndex;
            if (activatedAudioClipsNum == 1)
            {
                clipIndex = 0;
            }
            else
            {
                do
                {
                    clipIndex = Random.Range(0, activatedAudioDatas.Count);
                } while (prevClipIndex == clipIndex || !audioDatas[clipIndex].isActivated);
            }

            //Debug.Log(clipIndex);

            prevClipIndex = clipIndex;
            audioSource.clip = activatedAudioDatas[clipIndex].clip;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    void CountActivatedAudioClips()
    {
        activatedAudioDatas.Clear();
        for (int i = 0; i < audioDatas.Count; i++)
        {
            if (audioDatas[i].isActivated)
            {
                activatedAudioDatas.Add(audioDatas[i]);
            }
        }
    }

    IEnumerator DecreaseHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthDecreasementFrequency);

            if (shouldDamageFreeze) break;

            currentHealth -= healthDecreasementAmount;
            if (currentHealth <= 0)
            {
                isDead = true;
                dieType = EDieType.TimeOver;
                break;
            }
        }
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}