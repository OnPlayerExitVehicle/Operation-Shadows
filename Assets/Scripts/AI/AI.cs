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
        //navMeshAgent.SetDestination(positions[1]);
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
        //SetPatrolPath();
        CheckForEnemies(awarenessRadius);
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

    private void CheckForEnemies(float radius)
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
                            state = NPCState.Chasing;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void ProcessChasing()
    {

        //if not in shoot range
        //Set chase speed
        navMeshAgent.SetDestination(target.position);3
    }

    private void ProcessShooting()
    {

        //if in shoot range shoot gun
        //if player exits range exit state
    }
}
