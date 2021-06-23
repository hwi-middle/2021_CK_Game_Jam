using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //�̵����� ��
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public bool hasAlternativeSpeed = false;
    public float alternativeSpeedScale = 1f; // ���� Shift Ű�� ������ ���� �ӵ�(�Ϲ� �ӵ����� �����ų� ������ ����)
    public bool canJump = false;
    public float jumpHeight = 3f;

    //���콺 ������ ó��
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    private Camera cam;
    Quaternion camRotation;
    Quaternion bodyRotation;

    //�̵� ���� ���� ó����
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //�߼Ҹ� ���
    public List<AudioData> audioDatas = new List<AudioData>();
    private List<AudioData> activatedAudioDatas = new List<AudioData>();
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;

    //�ٴڿ� ��Ҵ��� üũ�ϴ� groundCheck ������Ʈ
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
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
        //�ν����Ϳ��� gravityScale�� ����� �� �����Ƿ� Update �޼��忡�� ó��
        gravity = GRAVITY_CONSTANT * gravityScale;

        //���콺 ó��
        SetCamera();

        //���� ���� üũ
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

        //�̵� ���� Ű �Է� �޾ƿ���
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;

        //Left ShiftŰ�� ���ȴ��� Ȯ��
        bool isLeftShiftKeyDown = false;
        if (hasAlternativeSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            isLeftShiftKeyDown = true;
            if (isGrounded)
            {
                shouldAlternativeSpeedApplied = true;
            }
        }

        //����Ű�� �������� Ȯ��
        if (canJump && Input.GetButtonDown("Jump") && isGrounded)
        {
            //������ ���̿� �°� ����
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //����Ű�� ������ �����ӿ��� LeftShiftŰ�� ������ �־����� Ȯ��
            if (!isLeftShiftKeyDown)
            {
                shouldAlternativeSpeedApplied = false;  //���������� �ʾҴٸ� �����߿� Alternative Speed�� �������� ����
            }
        }

        //Alternative Speed�� �����ؾ��ϴ��� Ȯ�� �� ����
        if (shouldAlternativeSpeedApplied)
        {
            move *= alternativeSpeedScale;
        }

        //�÷��̾� xz��� �̵�
        controller.Move(move * Time.deltaTime);

        //�÷��̾� y�� �̵�
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //�߰��� �Ҹ� ����� �ʿ����� Ȯ��
        time += Time.deltaTime;
        bool isFootstepSoundRequired;
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

        //�����̰� �ִ��� Ȯ��
        if (move.x != 0 || move.z != 0) isMoving = true;
        else isMoving = false;

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

            Debug.Log(clipIndex);

            prevClipIndex = clipIndex;
            audioSource.clip = activatedAudioDatas[clipIndex].clip;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    void SetCamera()
    {
        float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;

        //X�� ȸ������ -90 ~ +90���� �����ϴ� Clamp �ʿ���

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
