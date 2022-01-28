using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPartsForHit : MonoBehaviour
{
    public PlayerStats playerStats;
    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }
    public void NoticeHit(int damage)
    {
        playerStats.DamageDealer(damage, transform.position);
    }
}
