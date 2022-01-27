using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BombaThrower : MonoBehaviour
{
    public GameObject bomba;
    public float bombForce;
    public Transform throwPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject bomb = PhotonNetwork.Instantiate(Path.Combine("SceneSpawn", bomba.name), throwPoint.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().AddForce(throwPoint.forward * bombForce, ForceMode.Impulse);
        }
    }
}
