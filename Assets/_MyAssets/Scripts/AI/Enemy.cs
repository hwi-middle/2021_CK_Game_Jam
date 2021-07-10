using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private EEnemyType type;
    public EEnemyType Type { get { return type; } }
    [SerializeField] private Transform origin;

    private Transform player;
    private Transform currentTarget;
    private EEnemyTask currentTask;

    //우선순위는 숫자가 작을수록 높음
    //우선순위 충돌시 새로운 작업 우선
    private int currentTaskPriority;
    private const int MAX_PRIORITY = 10000;

    void Start()
    {
        currentTaskPriority = MAX_PRIORITY;
        agent = GetComponent<NavMeshAgent>();
        player = PlayerMovement.Instance.transform;

        switch (type)
        {
            case EEnemyType.Main:
                currentTask = EEnemyTask.ChaseNormal;
                currentTarget = player;
                break;
            case EEnemyType.Sub:
                currentTask = EEnemyTask.Return;
                currentTarget = origin;
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    void Update()
    {
        agent.SetDestination(currentTarget.position);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentTaskPriority = MAX_PRIORITY;
                    currentTask = EEnemyTask.Nothing;
                    Debug.Log("목적지 도착: " + currentTarget.name);
                }
            }
        }
    }

    //Sub Enemy일 경우 외부 Trigger에서 호출
    //우선순위 높음
    public void StopAndReturnToOrigin()
    {
        Debug.LogError("복귀");

        int newPriority = GetTaskPriority(EEnemyTask.Return);
        currentTask = EEnemyTask.Return;

        //우선순위가 더 높은 작업이 진행 중일 경우 스킵
        if (currentTaskPriority < newPriority)
        {
            return;
        }

        currentTaskPriority = newPriority;
        currentTarget = origin;
    }

    public void StartChasing(bool force)
    {
        int newPriority;

        if (force)
        {
            Debug.LogError("강제 추격");
            newPriority = GetTaskPriority(EEnemyTask.ChaseForce);
            currentTask = EEnemyTask.ChaseForce;
        }
        else
        {
            Debug.LogError("일반 추격");

            newPriority = GetTaskPriority(EEnemyTask.ChaseNormal);
            currentTask = EEnemyTask.ChaseNormal;

            //우선순위가 더 높은 작업이 진행 중일 경우 스킵
            if (currentTaskPriority < newPriority)
            {
                return;
            }
            currentTaskPriority = newPriority;
        }

        currentTarget = player;
    }

    private int GetTaskPriority(EEnemyTask t)
    {
        switch (t)
        {
            case EEnemyTask.Return:
                return 1;
            case EEnemyTask.ChaseForce:
                return 2;

            case EEnemyTask.ChaseNormal:
                return 3;
            case EEnemyTask.Nothing:
                return MAX_PRIORITY;
            default:
                Debug.Assert(false);
                return MAX_PRIORITY;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && currentTask == EEnemyTask.Return)
        {
            StartChasing(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && currentTask != EEnemyTask.ChaseForce)
        {
            StartChasing(false);
        }
    }
}
