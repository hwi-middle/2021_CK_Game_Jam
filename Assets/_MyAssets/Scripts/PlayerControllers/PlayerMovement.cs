using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //�� ����
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public bool hasAlternativeSpeed = false;
    public float alternativeSpeedFactor = 1f; // ���� Shift Ű�� ������ ���� �ӵ�(�Ϲ� �ӵ����� �����ų� ������ ����)
    public bool canJump = false;
    public float jumpHeight = 3f;

    //�̵� ���� ���� ó����
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //�߼Ҹ� ���
    public List<AudioData> audioDatas = new List<AudioData>();
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
        gravity = GRAVITY_CONSTANT * gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
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
            move *= alternativeSpeedFactor;
        }

        //�÷��̾� xz��� �̵�
        controller.Move(move * Time.deltaTime);

        //�÷��̾� y�� �̵�
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //�߰��� �Ҹ� ���
        time += Time.deltaTime;
        bool isFootstepSoundRequired;
        if (isLeftShiftKeyDown)
        {
            isFootstepSoundRequired = time * alternativeSpeedFactor > frequency;
        }
        else
        {
            isFootstepSoundRequired = time > frequency;
        }

        //�����̰� �ִ��� Ȯ��
        if (move.x != 0 || move.z != 0) isMoving = true;
        else isMoving = false;

        if (isMoving && isGrounded && isFootstepSoundRequired)
        {
            time = 0f;

            int clipIndex = 0;
            do
            {
                clipIndex = Random.Range(0, audioDatas.Count);
            } while (prevClipIndex == clipIndex || !audioDatas[clipIndex].isActivated);

            Debug.Log(clipIndex);

            prevClipIndex = clipIndex;
            audioSource.clip = audioDatas[clipIndex].clip;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
