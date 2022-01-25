using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static PhotonLobby lobby;

    public GameObject earlyLobbyGO;
    public GameObject lobbyGO;
    public GameObject roomGO;

    public Button earlyLobbyTamamButton;
    public GameObject createRoom;
    public GameObject geriButton;
    public GameObject joinRoom;

    public Text odaAdi;
    public Text odaAdiText;
    public Text onlinePlayerCount;

    public string roomName;
    public int maxPlayer = 6;

    public Text logText;
    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        //InvokeRepeating("OnlinePlayerCount", 2, 2);
        earlyLobbyGO.SetActive(true);
        earlyLobbyTamamButton.interactable = false;
        lobbyGO.SetActive(false);
        roomGO.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Servera Bağlanıldı");
        logText.text = "Servera Bağlanıldı";
        earlyLobbyTamamButton.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnCreateRoomButtonClicked()
    {
        
        int rn = Random.Range(100000, 999999);
        roomName = rn.ToString();
        RoomOptions roomOps = new RoomOptions();
        roomOps.BroadcastPropsChangeToAll = true;
        roomOps.IsVisible = false;
        roomOps.MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers;
        PhotonNetwork.CreateRoom(roomName, roomOps);
        CreatedOrJoinedRoom();
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Oda kuruldu");
        logText.text = "Oda kuruldu ";
    }
    private void CreatedOrJoinedRoom()
    {
        /*createRoom.SetActive(false);
        joinRoom.SetActive(false);
        odaAdi.transform.parent.gameObject.SetActive(false);
        geriButton.SetActive(true);*/
        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        odaAdiText.text = roomName;
    }
    public void OnBackButtonPressed()
    {
        PhotonNetwork.LeaveRoom();
        /*createRoom.SetActive(true);
        joinRoom.SetActive(true);
        odaAdi.transform.parent.gameObject.SetActive(true);
        geriButton.SetActive(false); */
        lobbyGO.SetActive(true);
        roomGO.SetActive(false);
        odaAdiText.text = "";
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Odadan gidildi");
        logText.text = "Odadan gidildi";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Lobi Oluşturulamadı. Büyük ihtimal lobi isimleri çakıştı. Bir daha dene yavrum");
        logText.text = "Lobi Oluşturulamadı. Büyük ihtimal lobi isimleri çakıştı. Bir daha dene yavrum";
    }
    public void OnJoinRoomButtonClicked()
    {
        if (odaAdi.text == "")
        {
            Debug.Log("Oda Adı yaz önce!");
            logText.text = "Oda Adı yaz önce!";
        }
        else
        {
            roomName = odaAdi.text;
            PhotonNetwork.JoinRoom(roomName);
            CreatedOrJoinedRoom();
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Odaya katılınmıyo. Ya dolu ya da adı yanlış");
        logText.text = "Odaya katılınmıyo. Ya dolu ya da adı yanlış";
        OnBackButtonPressed();
    }
    private void OnlinePlayerCount()
    {
        string countPlayersOnline;
        countPlayersOnline = PhotonNetwork.CountOfPlayers.ToString() + "/20 Players Online";
        onlinePlayerCount.text = countPlayersOnline;
    }
    public void OnEarlyLobbyTamamClicked()
    {
        string playername = earlyLobbyGO.transform.GetChild(0).GetChild(2).GetComponent<Text>().text;
        PhotonNetwork.NickName = playername;
        earlyLobbyGO.SetActive(false);
        lobbyGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        OnlinePlayerCount();
    }
}
