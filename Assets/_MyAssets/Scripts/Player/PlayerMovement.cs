using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //이동관련 값
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public bool hasAlternativeSpeed = false;
    public float alternativeSpeedScale = 1f; // 왼쪽 Shift 키가 눌렸을 때의 속도(일반 속도보다 느리거나 빠르게 지정)
    public bool canJump = false;
    public float jumpHeight = 3f;

    //스태미너 관련 값
    public bool hasStamina = false;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDecreasementAmount = 20f;

    //마우스 움직임 처리
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    private Camera cam;
    Quaternion camRotation;
    Quaternion bodyRotation;

    //이동 관련 내부 처리용
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //발소리 재생
    public List<AudioData> audioDatas = new List<AudioData>();
    private List<AudioData> activatedAudioDatas = new List<AudioData>();
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;

    //바닥에 닿았는지 체크하는 groundCheck 오브젝트
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private LayerMask groundMask;

    //싱글톤
    static PlayerMovement instance;
    public static PlayerMovement Instance { get { Init(); return instance; } }

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
        currentStamina = maxStamina;
        controller = GetComponent<CharacterController>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        audioSource = GetComponent<AudioSource>();

        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        camRotation = cam.transform.localRotation;
        bodyRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //인스펙터에서 gravityScale이 변경될 수 있으므로 Update 메서드에서 처리
        gravity = GRAVITY_CONSTANT * gravityScale;

        //마우스 처리
        SetCamera();

        //착지 상태 체크
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

        //이동 관련 키 입력 받아오기
        float x;
        float z;
        GetInputAxis(out x, out z);

        float y = velocity.y;

        //Left Shift키가 눌렸는지 확인
        bool isLeftShiftKeyDown = false;
        if (hasAlternativeSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            isLeftShiftKeyDown = true;
            if (isGrounded)
            {
                shouldAlternativeSpeedApplied = true;
            }
        }

        //점프키를 눌렀는지 확인
        if (canJump && Input.GetButtonDown("Jump") && isGrounded)
        {
            //설정된 높이에 맞게 점프
            y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //점프키를 누르는 프레임에서 LeftShift키도 누르고 있었는지 확인
            if (!isLeftShiftKeyDown)
            {
                shouldAlternativeSpeedApplied = false;  //누르고있지 않았다면 점프중에 Alternative Speed를 적용하지 않음
            }
        }

        //Alternative Speed를 적용해야하는지 확인 후 적용
        if (shouldAlternativeSpeedApplied)
        {
            x *= alternativeSpeedScale;
            z *= alternativeSpeedScale;
        }

        //중력적용
        y += gravity * Time.deltaTime;

        //플레이어 이동
        velocity = (transform.right * x * speed) + (transform.forward * z * speed) + (transform.up * y);
        //velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        PlayFootstepSound(isLeftShiftKeyDown, velocity);
    }

    void SetCamera()
    {
        float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;

        camRotation *= Quaternion.Euler(-xRotation, 0f, 0f);
        camRotation = ClampRotationAroundXAxis(camRotation);
        bodyRotation *= Quaternion.Euler(0f, yRotation, 0f);

        cam.transform.localRotation = camRotation;
        transform.localRotation = bodyRotation;
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

    void GetInputAxis(out float x, out float z)
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
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

            //필요 시 스태미너 시스템 적용
            if (hasStamina)
            {
                currentStamina -= staminaDecreasementAmount * Time.deltaTime;
                if (currentStamina < 0)
                {
                    currentStamina = 0;
                }
            }
        }
        //평상시에는 frequency만큼 시간이 지났는지 확인
        else
        {
            isFootstepSoundRequired = time > frequency;
        }

        //움직이고 있는지 확인
        if (v.x != 0 || v.z != 0) isMoving = true;
        else isMoving = false;

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
}
