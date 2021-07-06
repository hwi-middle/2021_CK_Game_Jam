using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] private EEnemyType type;
    [SerializeField] private Transform target;
    [SerializeField] private Transform origin;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(type == EEnemyType.Main)
        {
            StartChasing();
        }
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }


    //Sub Enemy�� ��� �ܺ� Trigger���� ȣ��
    public void StopAndReturnToOrigin()
    {

    }

    public void StartChasing()
    {

    }
}
