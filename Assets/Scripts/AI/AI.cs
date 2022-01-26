using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum NPCState
{
    Patrol, Chasing, Shooting
}
[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [SerializeField] private NPCState state;
    [SerializeField] private float viewAngle;
    [SerializeField] private float awarenessRadius;
    [SerializeField] private float shootRadius;
    [SerializeField] private Transform? target;
    [SerializeField] private Vector3[] positions;
    [SerializeField] int positionCounter;
    [SerializeField] float patrolCheckRadius = 0.1f;

    public GameObject asdas;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        positionCounter = 1;
    }

    private void Start()
    {
        state = NPCState.Patrol;
        navMeshAgent.SetDestination(positions[1]);
    }

    private void FixedUpdate()
    {
        //Debug.Log(transform.forward.z);
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
        Gizmos.DrawSphere(transform.position, awarenessRadius);
        //Gizmos.DrawSphere(transform.position, shootRadius);
    }

    private void ProcessPatrol()
    {
        SetPatrolPath();
        if(CheckForEnemies(awarenessRadius))
        {
            navMeshAgent.SetDestination(target.position);
            state = NPCState.Chasing;
        }
    }

    private void SetPatrolPath()
    {
        int length = positions.Length;
        if(navMeshAgent.remainingDistance < 0.01f)
        {
            positionCounter++;
            positionCounter %= length;
            navMeshAgent.SetDestination(positions[positionCounter]);
        }
    }

    private bool CheckForEnemies(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        if (colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].CompareTag("Player"))
                {
                    RaycastHit hit;
                    
                    if (Physics.Linecast(transform.position, colliders[i].transform.position, out hit))
                    {
                        asdas = hit.collider.gameObject;
                        if (hit.collider.gameObject.CompareTag("Player"))
                        {
                            float angle = Vector3.Angle(transform.position, hit.transform.position);
                            target = hit.collider.transform;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void ProcessChasing()
    {
        if(!CheckForEnemies(shootRadius)) // SHOOT RADÝUSTA KÝMSE YOK AMK
        {
            if(CheckForEnemies(awarenessRadius))
            {
                navMeshAgent.SetDestination(target.position);
            }
            else
            {
                state = NPCState.Patrol;
            }
            
        }
        else
        {
            navMeshAgent.enabled = false;
            state = NPCState.Shooting;
        }
    }

    private void ProcessShooting()
    {
        if(!CheckForEnemies(shootRadius))
        {
            navMeshAgent.enabled = true;
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(target.position);
            state = NPCState.Chasing;
        }
        else
        {
            Debug.Log("Shooting");
        }
    }
}
