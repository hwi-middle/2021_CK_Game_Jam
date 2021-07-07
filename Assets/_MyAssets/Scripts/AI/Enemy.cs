using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private EEnemyType type;
    [SerializeField] private Transform origin;

    private Transform player;
    private Transform currentTarget;

    public EEnemyType Type { get { return type; } }

    //우선순위는 숫자가 작을수록 높음
    //우선순위 충돌시 새로운 작업 우선
    private int currentTaskPriority;
    private const int MAX_PRIORITY = 10000; 

    void Start()
    {
        currentTaskPriority = MAX_PRIORITY;
        agent = GetComponent<NavMeshAgent>();
        player = PlayerMovement.Instance.transform;


        switch(type)
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

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentTaskPriority = MAX_PRIORITY;
                    Debug.Log("목적지 도착: " + currentTarget.name);
                }
            }
        }
    }


    //Sub Enemy일 경우 외부 Trigger에서 호출
    //우선순위 높음
    public void StopAndReturnToOrigin()
    {
        const int PRIORITY = 1;

        //우선순위가 더 높은 작업이 진행 중일 경우 스킵
        if(ComparePriority(PRIORITY, currentTaskPriority) > 0)
        {
            return;
        }

        currentTaskPriority = PRIORITY;
        currentTarget = origin;
    }

    public void StartChasing()
    {
        const int PRIORITY = 2;

        //우선순위가 더 높은 작업이 진행 중일 경우 스킵
        if (ComparePriority(PRIORITY, currentTaskPriority) > 0)
        {
            return;
        }

        currentTaskPriority = PRIORITY;
        currentTarget = player;
    }

    //C의 strcmp처럼 단순 뺄셈처리
    //결과값이 음수면 첫 번째 인자의 우선순위가 더 높은 것
    public int ComparePriority(int a, int b)
    {
        return a - b;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            StartChasing();
        }
    }
}
