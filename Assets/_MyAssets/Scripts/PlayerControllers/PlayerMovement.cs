using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //값 설정
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public bool hasAlternativeSpeed = false;
    public float alternativeSpeedFactor = 1f; // 왼쪽 Shift 키가 눌렸을 때의 속도(일반 속도보다 느리거나 빠르게 지정)
    public bool canJump = false;
    public float jumpHeight = 3f;

    //이동 관련 내부 처리용
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;
    private bool shouldAlternativeSpeedApplied = false;

    //발소리 재생
    public List<AudioData> audioDatas = new List<AudioData>();
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;

    //바닥에 닿았는지 체크하는 groundCheck 오브젝트
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;

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
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //점프키를 누르는 프레임에서 LeftShift키도 누르고 있었는지 확인
            if (!isLeftShiftKeyDown)
            {
                shouldAlternativeSpeedApplied = false;  //누르고있지 않았다면 점프중에 Alternative Speed를 적용하지 않음
            }
        }

        //Alternative Speed를 적용해야하는지 확인 후 적용
        if (shouldAlternativeSpeedApplied)
        {
            move *= alternativeSpeedFactor;
        }

        //플레이어 xz평면 이동
        controller.Move(move * Time.deltaTime);

        //플레이어 y축 이동
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //발걸음 소리 출력
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

        //움직이고 있는지 확인
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
