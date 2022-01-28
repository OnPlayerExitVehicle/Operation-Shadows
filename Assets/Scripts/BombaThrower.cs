using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BombaThrower : MonoBehaviour
{
    private PhotonView PV;
    public GameObject bomba;
    public float bombForce;
    public Transform throwPoint;
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine && Input.GetKeyDown(KeyCode.G))
        {
            PV.RPC("InstantiateAndThrowBomb", RpcTarget.All);
        }
    }

    [PunRPC]
    private void InstantiateAndThrowBomb()
    {
        GameObject bomb = Instantiate(bomba, throwPoint.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody>().AddForce(throwPoint.forward * bombForce, ForceMode.Impulse);
    }
}
