using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string playerName;
    public int teamId;
    public GameObject playerAvatar;

    // Start is called before the first frame update
    void Start()
    {
        playerName = PhotonNetwork.NickName;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
