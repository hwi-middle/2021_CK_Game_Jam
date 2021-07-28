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

    //�켱������ ���ڰ� �������� ����
    //�켱���� �浹�� ���ο� �۾� �켱
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

    //Sub Enemy�� ��� �ܺ� Trigger���� ȣ��
    //�켱���� ����
    public void StopAndReturnToOrigin()
    {
        Debug.LogError("����");

        int newPriority = GetTaskPriority(EEnemyTask.Return);

        //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
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
        //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
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
