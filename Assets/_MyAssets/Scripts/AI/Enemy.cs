using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] private EEnemyType type;
    [SerializeField] private Transform origin;

    private Transform player;
    private Transform currentTarget;

    public EEnemyType Type { get { return type; } }

    void Start()
    {
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
    }


    //Sub Enemy일 경우 외부 Trigger에서 호출
    public void StopAndReturnToOrigin()
    {
        currentTarget = origin;
    }

    public void StartChasing()
    {
        currentTarget = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartChasing();
        }
    }
}
