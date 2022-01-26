using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
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
    }

    public void GiveMeDamage(int damage)
    {
        PV.RPC("RPC_GiveMeDamage", RpcTarget.All, damage);
    }

    private void CheckAmIDead()
    {
        if (_health <= 0)
            KillMe();
    }

    private void KillMe()
    {
        print("�ld�m");
        Destroy(this.gameObject);
        //doragdoll in future
    }

    [PunRPC]
    private void RPC_GiveMeDamage(int dmg)
    {
        health -= dmg;
    }
}
