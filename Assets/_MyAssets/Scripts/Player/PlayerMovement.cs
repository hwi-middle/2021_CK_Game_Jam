using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //�̵� ó��
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public bool hasAlternativeSpeed = false;
    public float alternativeSpeedScale = 1f; // ���� Shift Ű�� ������ ���� �ӵ�(�Ϲ� �ӵ����� �����ų� ������ ����)
    public bool canJump = false;
    public float jumpHeight = 3f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //ü�� ó��
    public bool hasHealth = false;
    public bool automaticallyDecreaseHealth = false;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecreasementAmount = 1f;
    public float healthDecreasementFrequency = 5f;
    public bool isDead = false;

    //���¹̳� ó��
    public bool hasStamina = false;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDecreasementAmount = 20f;
    public float staminaIncreasementAmount = 20f;
    public float staminaIncresementDelay = 2f;
    private float idleTime = 0f;

    //���콺 ������ ó��
    public bool shouldCameraFreeze = false;
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    private Camera cam;
    Quaternion camRotation;
    Quaternion bodyRotation;

    //ī�޶� ��鸲(Head Bobbing) ó��
    public bool useHeadBob;
    public Transform headTransform;
    public Transform camTransform;

    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    public float headBobSmoothing = 0.1f;

    private float walkingTime = 0f;
    private Vector3 targetCameraPos;

    //�������� ó��
    public bool isStunned = false;
    public bool isInvincible = false;

    //�߼Ҹ� ��� ó��
    public List<AudioData> audioDatas = new List<AudioData>();
    private List<AudioData> activatedAudioDatas = new List<AudioData>();
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;

    //�ٴڿ� ��Ҵ��� üũ ó��
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private LayerMask groundMask;

    //�̱��� ó��
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

        sensitivityX = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        sensitivityY = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

        currentHealth = maxHealth;
        if (hasHealth)
        {
            StartCoroutine(DecreaseHealth());
        }
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
        //��� �� ������ ���� �Ұ�
        if (isDead || isStunned) return;

        //�ν����Ϳ��� gravityScale�� ����� �� �����Ƿ� Update �޼��忡�� ó��
        gravity = GRAVITY_CONSTANT * gravityScale;

        SetCamera(); //���콺 ó��
        CheckGrounded(); //���� ���� üũ

        //�̵� ���� Ű �Է� �޾ƿ���
        float x;
        float y = velocity.y;
        float z;
        GetInputAxis(out x, out z, out isMoving);

        //Left ShiftŰ�� ���ȴ��� Ȯ��
        bool isLeftShiftKeyDown = false;
        if (hasAlternativeSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            isLeftShiftKeyDown = true;

            //�ʿ� �� ���¹̳� �ý��� ����
            if (hasStamina && isMoving)
            {
                currentStamina -= staminaDecreasementAmount * Time.deltaTime;
                if (currentStamina < 0)
                {
                    currentStamina = 0;
                }
            }

            if (isGrounded && currentStamina > 0)
            {
                shouldAlternativeSpeedApplied = true;
            }
        }

        //����Ű�� �������� Ȯ��
        if (canJump && Input.GetButtonDown("Jump") && isGrounded)
        {
            //������ ���̿� �°� ����
            y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //����Ű�� ������ �����ӿ��� LeftShiftŰ�� ������ �־����� Ȯ��
            if (!isLeftShiftKeyDown)
            {
                shouldAlternativeSpeedApplied = false;  //���������� �ʾҴٸ� �����߿� Alternative Speed�� �������� ����
            }
        }

        //��� �ð��� ���� ���¹̳� ȸ��
        if (isMoving)
        {
            walkingTime += Time.deltaTime;
            if (isLeftShiftKeyDown)
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

        //HeadBob ����
        targetCameraPos = headTransform.position + CalculateHeadBobOffset(walkingTime);
        camTransform.position = Vector3.Lerp(camTransform.position, targetCameraPos, headBobSmoothing);

        if ((camTransform.position - targetCameraPos).magnitude <= Mathf.Epsilon)
        {
            camTransform.position = targetCameraPos;
        }

        //Alternative Speed�� �����ؾ��ϴ��� Ȯ�� �� ����
        if (shouldAlternativeSpeedApplied)
        {
            x *= alternativeSpeedScale;
            z *= alternativeSpeedScale;
        }

        //�߷�����
        y += gravity * Time.deltaTime;

        //�÷��̾� �̵�
        velocity = (transform.right * x * speed) + (transform.forward * z * speed) + (transform.up * y);
        //velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        PlayFootstepSound(isLeftShiftKeyDown, velocity);
    }

    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
    }

    void SetCamera()
    {
        if (shouldCameraFreeze) return;

        sensitivityX = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        sensitivityY = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

        float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;

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

            //�߷��� ��ø����Ǵ� ���� ����
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
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //�����̰� �ִ��� Ȯ��
        if (x != 0 || z != 0) isMoving = true;
        else isMoving = false;
    }

    void PlayFootstepSound(bool isLeftShiftKeyDown, Vector3 v)
    {
        //�߰��� �Ҹ� ����� �ʿ����� Ȯ��
        time += Time.deltaTime;
        bool isFootstepSoundRequired;

        //Ŭ�� ���� Ȯ��
        CountActivatedAudioClips();
        int activatedAudioClipsNum = activatedAudioDatas.Count;

        //Ȱ��ȭ�� Ŭ���� ������ ������� ����
        if (activatedAudioClipsNum == 0)
        {
            isFootstepSoundRequired = false;
        }

        //Left ShiftŰ�� ������ ������ alternativeSpeedScale���� ���� frequency�� ����
        else if (isLeftShiftKeyDown)
        {
            isFootstepSoundRequired = time * alternativeSpeedScale > frequency;
        }
        //���ÿ��� frequency��ŭ �ð��� �������� Ȯ��
        else
        {
            isFootstepSoundRequired = time > frequency;
        }

        //�߰��� �Ҹ� ���
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

            currentHealth -= healthDecreasementAmount;
            if (currentHealth <= 0)
            {
                isDead = true;
                break;
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
