using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnTime = 5f;

    public string[] objects;

    public int staticY = 50;
    public int randomXMin, randomXMax;
    public int randomZMin, randomZMax;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            InvokeRepeating("SpawnObject", spawnTime, spawnTime);
    }

    private void SpawnObject()
    {
        PhotonNetwork.Instantiate(Path.Combine("SceneSpawn", PickObject()), PickCoordinate(), Quaternion.identity, 0);
    }

    private string PickObject()
    {
        int r = Random.Range(0, objects.Length);
        return objects[r];
    }

    private Vector3 PickCoordinate()
    {
        int x = Random.Range(randomXMin, randomXMax);
        int z = Random.Range(randomZMin, randomZMax);
        Vector3 coord = new Vector3(x, staticY, z);
        return coord;
    }
}
