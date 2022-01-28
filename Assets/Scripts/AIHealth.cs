using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private AI ai;
    private PhotonView PV; 
    private int _health = 100;
    public int health { get { return _health; } set
        {
            _health = value;
            CheckAmIDead();
        } }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        ai = GetComponent<AI>();
    }

    public void GiveMeDamage(int damage)
    {
        PV.RPC("RPC_GiveMeDamage", RpcTarget.All, damage);
    }

    public void WarnAI(Transform trans)
    {
        ai.OnGetHit(trans);
    }
    

    private void CheckAmIDead()
    {
        if (_health <= 0)
            KillMe();
    }

    private void KillMe()
    {
        print("öldüm");
        DoRagdoll();
        LooseParent();
        Destroy(this.gameObject);
        DisableAI();
        //doragdoll in future
    }

    private void DisableAI()
    {
        GetComponent<AI>().enabled = false;
    }

    private void LooseParent()
    {
        this.transform.GetChild(0).parent = null;
    }

    private void DoRagdoll()
    {
        transform.GetChild(0).GetComponent<Animator>().enabled = false;
        foreach (Rigidbody rb in this.GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }

    [PunRPC]
    private void RPC_GiveMeDamage(int dmg)
    {
        health -= dmg;
    }
}
