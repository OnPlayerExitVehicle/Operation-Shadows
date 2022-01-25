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

    private void Start()
    {
        state = NPCState.Patrol;
    }

    private void Update()
    {
        switch(state)
        {
            /*
            case NPCState.Patrol:
                ProcessPatrol();
                break;
            */
            case NPCState.Chasing:
                ProcessChasing();
                break;
            case NPCState.Shooting:
                ProcessShooting();
                break;
        }
    }
    private void ProcessChasing()
    {

    }

    private void ProcessShooting()
    {

    }
}
