using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum NPCState
{
    Patrol, Chasing, Shooting
}

public class AI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [SerializeField] private NPCState state;
    [SerializeField] private float viewAngle;
    [SerializeField] private float awaranessRadius;
    [SerializeField] private float shootRadius;


    private void Start()
    {
        state = NPCState.Patrol;
    }

    private void Update()
    {
        switch(state)
        {
            
            case NPCState.Patrol:
                ProcessPatrol();
                break;
            case NPCState.Chasing:
                ProcessChasing();
                break;
            case NPCState.Shooting:
                ProcessShooting();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    private void ProcessPatrol()
    {

    }

    private void ProcessChasing()
    {

    }

    private void ProcessShooting()
    {

    }
}
