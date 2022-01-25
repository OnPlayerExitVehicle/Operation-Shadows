using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    private PhotonView pV;
    public float topHizi = 500f;

    private void Awake()
    {
        pV = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && pV.IsMine)
        {
            pV.RequestOwnership();
            Vector3 knockback = collision.transform.position - this.transform.position;
            this.GetComponent<Rigidbody>().AddForce(-knockback.normalized * topHizi);
        }
    }
}
