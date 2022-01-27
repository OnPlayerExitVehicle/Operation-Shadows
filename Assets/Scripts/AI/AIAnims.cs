using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnims : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;

    private void Update()
    {
        if(agent)
        {
            speed = agent.velocity.magnitude;
            anim.SetFloat("speed", speed);
        }
    }
}
