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
                    Debug.Log("������ ����: " + currentTarget.name);
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
        currentTask = EEnemyTask.Return;

        //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
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
            Debug.LogError("���� �߰�");
            newPriority = GetTaskPriority(EEnemyTask.ChaseForce);
            currentTask = EEnemyTask.ChaseForce;
        }
        else
        {
            Debug.LogError("�Ϲ� �߰�");

            newPriority = GetTaskPriority(EEnemyTask.ChaseNormal);
            currentTask = EEnemyTask.ChaseNormal;

            //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
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
