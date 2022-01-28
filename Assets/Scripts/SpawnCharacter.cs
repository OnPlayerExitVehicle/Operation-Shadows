using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class SpawnCharacter : MonoBehaviour
{
    private NetworkPlayer myPlayer;
    void Start()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        foreach (NetworkPlayer player in GameObject.FindObjectsOfType<NetworkPlayer>())
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                myPlayer = player;
            }
        }
        Invoke("SpawnCoolDown", 1);
    }
    private void SpawnCoolDown()
    {
        myPlayer.SpawnChar();
    }
}
