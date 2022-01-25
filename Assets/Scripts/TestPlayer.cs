using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    public GameObject[] players;

    public int health = 100;

    private PhotonView PV;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            healthText = GameObject.FindGameObjectWithTag("TestHealthText").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthText)
        {
            healthText.text = health.ToString();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PV.IsMine)
            {
                GetPlayers();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PV.IsMine)
            {
                players[0].GetComponent<TestPlayer>().PV.RPC("Damage", RpcTarget.All);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PV.IsMine)
            {
                players[1].GetComponent<TestPlayer>().PV.RPC("Damage", RpcTarget.All);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PV.IsMine)
            {
                players[2].GetComponent<TestPlayer>().PV.RPC("Damage", RpcTarget.All);
            }
        }
    }
    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("TestPlayer");
    }

    [PunRPC]
    private void Damage()
    {
        health -= 10;
    }
}
