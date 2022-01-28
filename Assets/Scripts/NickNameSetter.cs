using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NickNameSetter : MonoBehaviour
{
    public TextMeshPro name;
    private PhotonView PV;
    void Start()
    {
        PlayerStats playerStats = GetComponentInParent<PlayerStats>();
        PV = playerStats.GetComponent<PhotonView>();
        if (PV.IsMine)
            return;
        name.text = PhotonNetwork.NickName;
        playerStats.charName = this;
    }

}
