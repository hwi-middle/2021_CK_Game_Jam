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

    //�켱������ ���ڰ� �������� ����
    //�켱���� �浹�� ���ο� �۾� �켱
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
                    Debug.Log("������ ����: " + currentTarget.name);
                }
            }
        }
    }


    //Sub Enemy�� ��� �ܺ� Trigger���� ȣ��
    //�켱���� ����
    public void StopAndReturnToOrigin()
    {
        const int PRIORITY = 1;

        //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
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

        //�켱������ �� ���� �۾��� ���� ���� ��� ��ŵ
        if (ComparePriority(PRIORITY, currentTaskPriority) > 0)
        {
            return;
        }

        currentTaskPriority = PRIORITY;
        currentTarget = player;
    }

    //C�� strcmpó�� �ܼ� ����ó��
    //������� ������ ù ��° ������ �켱������ �� ���� ��
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
