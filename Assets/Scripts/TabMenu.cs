using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    public static TabMenu TM;
    public GameObject tabMenuPart;
    private Transform panelObject;
    public List<NetworkPlayer> oyuncuList = new List<NetworkPlayer>();
    private bool hazir;

    private void Awake()
    {
        TM = this;
    }
    void Start()
    {
        panelObject = this.transform.GetChild(0);
        panelObject.gameObject.SetActive(false);

        Invoke("FindPlayers", 2f);
    }
    public void FindPlayers()
    {
        if (oyuncuList.Count > 0)
            oyuncuList = new List<NetworkPlayer>();
        foreach (GameObject oyuncu in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            oyuncuList.Add(oyuncu.GetComponent<NetworkPlayer>());
        }


        hazir = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hazir)
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ListMenu();
            if (PhotonNetwork.IsMasterClient)
                GameSetup.GS.CursorActive();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            UnListMenu();
            if (PhotonNetwork.IsMasterClient)
                GameSetup.GS.CursorDeactive();
        }
    }
    private void ListMenu()
    {
        panelObject.gameObject.SetActive(true);

        if (oyuncuList.Count > 0)
            oyuncuList = new List<NetworkPlayer>();
        foreach (GameObject oyuncu in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            oyuncuList.Add(oyuncu.GetComponent<NetworkPlayer>());
        }
        foreach (NetworkPlayer oyuncu in oyuncuList)
        {
            GameObject listPartPrefab = Instantiate(tabMenuPart, panelObject);

            Text name = listPartPrefab.transform.GetChild(0).GetComponent<Text>();
            Text kills = listPartPrefab.transform.GetChild(1).GetComponent<Text>();
            Text deaths = listPartPrefab.transform.GetChild(2).GetComponent<Text>();
            Button kick = listPartPrefab.transform.GetChild(3).GetComponent<Button>();

            name.text = oyuncu.PV.Owner.NickName;
            kills.text = oyuncu.kills.ToString();
            deaths.text = oyuncu.deaths.ToString();
            kick.onClick.AddListener(() => KickPlayer(oyuncu.PV.Owner));

            if (PhotonNetwork.IsMasterClient && oyuncu.PV.Owner != PhotonNetwork.LocalPlayer)
                kick.gameObject.SetActive(true);
            else
                kick.gameObject.SetActive(false);
        }
    }

    private void UnListMenu()
    {
        for (int i = panelObject.childCount - 1; i > 0; i--)
        {
            Destroy(panelObject.GetChild(i).gameObject);
        }
        if (oyuncuList.Count > 0)
            oyuncuList = new List<NetworkPlayer>();

        panelObject.gameObject.SetActive(false);
    }

    public void KickPlayer(Player kicklenecekOyuncu)
    {
        PhotonNetwork.CloseConnection(kicklenecekOyuncu);
        UnListMenu();
        ListMenu();
    }
}
