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
        PV = GetComponent<PhotonView>();
        PlayerStats playerStats = GetComponentInParent<PlayerStats>();
        playerStats.charName = this;
        string isname = PhotonNetwork.NickName;
        PV.RPC("RPC_SetName", RpcTarget.All, isname);
        if (PV.IsMine && false)
            this.gameObject.SetActive(false);
    }

    [PunRPC]
    private void RPC_SetName(string isim)
    {
        name.text = isim;
    }



}
