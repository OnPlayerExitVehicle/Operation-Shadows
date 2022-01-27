using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpawnCharacter : MonoBehaviour
{
    private NetworkPlayer myPlayer;
    void Start()
    {
        foreach (NetworkPlayer player in GameObject.FindObjectsOfType<NetworkPlayer>())
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                myPlayer = player;
            }
        }
        myPlayer.SpawnChar();
    }
}
