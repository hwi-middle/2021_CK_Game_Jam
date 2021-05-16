using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //값 설정
    private const float GRAVITY_CONSTANT = -9.81f;
    public float gravityScale = 1.0f;
    private float gravity;
    public float speed = 12f;
    public float alternativeSpeedFactor = 1f; // 왼쪽 Shift 키가 눌렸을 때의 속도(일반 속도보다 느리거나 빠르게 지정)
    public float jumpHeight = 3f;

    //바닥에 닿았는지 체크하는 groundCheck 오브젝트
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //이동 관련
    public CharacterController controller;
    Vector3 velocity;
    private bool isGrounded;
    private bool isMoving = false;

    //발소리 재생
    public AudioClip[] audioClips;
    private int prevClipIndex = -1;
    private AudioSource audioSource;
    public float frequency;
    private float time = 0f;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gravity = GRAVITY_CONSTANT * gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //착지 상태 체크
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //중력이 중첩적용되는 것을 방지
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        //이동 처리
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;

        bool isLeftShiftKeyDown = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isLeftShiftKeyDown = true;
            move *= alternativeSpeedFactor;
        }

        if (move.x != 0 || move.z != 0) isMoving = true;
        else isMoving = false;

        controller.Move(move * Time.deltaTime);

        //점프 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //발자국 소리 출력
        time += Time.deltaTime;
        bool isFootstepSoundRequired;
        if (isLeftShiftKeyDown)
        {
            isFootstepSoundRequired = time  * alternativeSpeedFactor > frequency ;
        }
        else
        {
            isFootstepSoundRequired = time > frequency;
        }

        if (isMoving && isGrounded && isFootstepSoundRequired)
        {
            time = 0f;

            int clipIndex = 0;
            do
            {
                clipIndex = Random.Range(0, audioClips.Length);
            } while (prevClipIndex == clipIndex);
            prevClipIndex = clipIndex;
            audioSource.clip = audioClips[clipIndex];
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
