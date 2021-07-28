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
                currentTarget = player;
                break;
            case EEnemyType.Sub:
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

        if(type == EEnemyType.Main)
        {
            if (agent.remainingDistance >= 60)
            {
                agent.speed = 6;
            }
            else
            {
                agent.speed = 3;
            }
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentTaskPriority = MAX_PRIORITY;
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

        //우선순위가 더 높은 작업이 진행 중일 경우 스킵
        if (currentTaskPriority < newPriority)
        {
            return;
        }

        currentTaskPriority = newPriority;
        currentTarget = origin;
    }

    public void StartChasing()
    {
        int newPriority;

        newPriority = GetTaskPriority(EEnemyTask.ChaseNormal);
        //우선순위가 더 높은 작업이 진행 중일 경우 스킵
        if (currentTaskPriority < newPriority)
        {
            return;
        }

        currentTaskPriority = newPriority;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            StartChasing();
        }
    }
}
