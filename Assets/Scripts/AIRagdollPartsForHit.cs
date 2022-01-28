using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRagdollPartsForHit : MonoBehaviour
{
    public AIHealth aiScript;
    private void Start()
    {
        aiScript = GetComponentInParent<AIHealth>();
    }
    public void NoticeHit(int damage)
    {
        aiScript.GiveMeDamage(damage);
    }

    public void WarnAI(Transform trans)
    {
        aiScript.WarnAI(trans);
    }
}
