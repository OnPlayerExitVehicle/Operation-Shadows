using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillTable : MonoBehaviour
{
    public static KillTable KT;
    private PhotonView PV;

    private Transform killTablePanel;
    public GameObject singleKillPanel;

    private void Awake()
    {
        KT = this;
        PV = GetComponent<PhotonView>();
        killTablePanel = transform.GetChild(0);
    }

    public void CreateKillTablePart(string killed, string died)
    {
        
        PV.RPC("RPC_CreateKillTablePart", RpcTarget.All, killed, died);
    }

    [PunRPC]
    private void RPC_CreateKillTablePart(string killed, string died)
    {
        GameObject singlePanel = Instantiate(singleKillPanel, killTablePanel);
        Text killedText = singlePanel.transform.GetChild(0).GetComponent<Text>();
        Text diedText = singlePanel.transform.GetChild(2).GetComponent<Text>();

        killedText.text = killed;
        
        diedText.text = died;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == killed)
            {
                if ((int)player.CustomProperties["Team"] == 0)
                    killedText.color = Color.red;
                if ((int)player.CustomProperties["Team"] == 1)
                    killedText.color = Color.blue;
            }
            if (player.NickName == died)
            {
                if ((int)player.CustomProperties["Team"] == 0)
                    diedText.color = Color.red;
                if ((int)player.CustomProperties["Team"] == 1)
                    diedText.color = Color.blue;
            }
        }
    }
}
