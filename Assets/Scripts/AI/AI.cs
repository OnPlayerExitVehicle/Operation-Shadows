using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum NPCState
{
    Patrol, Chasing, Shooting, Idle
}
[RequireComponent(typeof(NavMeshAgent))]
public class AI : MonoBehaviour
{
    [SerializeField] private bool startPatrolling;

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
    [SerializeField] private AIAnims aiAnim;
    [SerializeField] private float exitShootingPlusRadius;

    private const float walkSpeed = 2;
    private const float runSpeed = 6;
    
    private bool isShooted = false;

    [SerializeField] private float gunTolerance;

    public GameObject asdas;

    public AIGunSystem aigun; // sets on inspector

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        positionCounter = 1;
    }

    private void Start()
    {
        if (startPatrolling)
        {
            state = NPCState.Patrol;
            navMeshAgent.SetDestination(positions[1]);
        }
        else
        {
            state = NPCState.Idle;
            navMeshAgent.SetDestination(transform.position);
        }
        
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
            case NPCState.Idle:
                ProcessIdle();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, awarenessRadius);
        //Gizmos.DrawSphere(transform.position, shootRadius);
    }

    private void ProcessIdle()
    {
        
        CheckAndTargetIfEnemyFound();
    }

    private void ProcessPatrol()
    {
        SetPatrolPath();
        CheckAndTargetIfEnemyFound();
    }

    private void CheckAndTargetIfEnemyFound()
    {
        if (CheckForEnemies(awarenessRadius))
        {
            navMeshAgent.SetDestination(target.position);
            state = NPCState.Chasing;
            navMeshAgent.speed = runSpeed;
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
                        if (hit.collider.gameObject.CompareTag("PlayerRagdollPartsForHit") || hit.collider.gameObject.CompareTag("Pot"))
                        {
                            float angle = Vector3.Angle(transform.position, hit.transform.position);
                            target = hit.collider.transform;
                            return true;
                        }

                    }
                    else if(isShooted)
                    {
                        state = NPCState.Chasing;
                        navMeshAgent.speed = runSpeed;
                        return true;
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
                navMeshAgent.speed = runSpeed;
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
        if(!CheckForEnemies(shootRadius + exitShootingPlusRadius))
        {
            navMeshAgent.enabled = true;
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(target.position);
            state = NPCState.Chasing;
            navMeshAgent.speed = runSpeed;
        }
        else
        {
            aigun.MyInput(target.gameObject);
            this.transform.LookAt(target);
        }
    }

    public void OnGetHit(Transform trans)
    {
        if(state == NPCState.Patrol || state == NPCState.Idle || state == NPCState.Chasing)
        {
            navMeshAgent.SetDestination(trans.position);
            target = trans;
            isShooted = true;
            state = NPCState.Shooting;
            shootRadius = Vector3.Magnitude(trans.position - transform.position);
        }
    }
}
