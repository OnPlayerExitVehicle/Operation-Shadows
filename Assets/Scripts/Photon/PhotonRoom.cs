using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;

    public int roundsToPlay = 50;
    public GameObject roundsUI;

    public bool isGameLoaded;
    public int currentScene;

    //Player Info
    public Player[] photonPlayers;
    public int playersInRoom;
    public int myNumbersInRoom;

    public int playersInGame;

    private ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();

    public Text logText;

    private void Awake()
    {
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();

        startButton.SetActive(false);
        roundsUI.SetActive(false);
    }

    void Update()
    {

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        ClearPlayerListings();
        ListPlayers();

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Succsessfully joined room");
        logText.text = "Odaya katıldın";
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            roundsUI.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
            roundsUI.SetActive(false);
        }
        SetPlayersTeams(0);
        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
    }
    void ClearPlayerListings()
    {
        for (int i = playersPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }
    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                Image tempTeam = tempListing.transform.GetChild(1).GetComponent<Image>();
                tempText.text = player.NickName;
                if (player.CustomProperties.ContainsKey("Team"))
                {
                    int team = (int)player.CustomProperties["Team"];
                    if (team == 0)
                        tempTeam.color = Color.red;
                    else
                        tempTeam.color = Color.blue;

                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        logText.text = newPlayer.NickName + " odaya geeeeelldi";

        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
    }
    public void SetPlayersTeams(int team)
    {
        hash["Team"] = team;
        PhotonNetwork.SetPlayerCustomProperties(hash);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName +" odadan ayrıldı");
        if(logText != null)
            logText.text = otherPlayer.NickName + " odadan gitti";
        playersInRoom--;

        if (!isGameLoaded)
        {
            ClearPlayerListings();
            ListPlayers();
        }

        if (isGameLoaded)
            TabMenu.TM.FindPlayers();
    }

    public void StartGame()
    {
        //int arr = int.Parse(roundsUI.transform.GetChild(2).GetComponent<Text>().text);
        int arr;
        if (roundsUI.GetComponent<InputField>().text == "")
            arr = 50;
        else
            arr = int.Parse(roundsUI.GetComponent<InputField>().text);
        PV.RPC("RPC_Rounds", RpcTarget.All, arr);
        startButton.GetComponent<Button>().interactable = false;
        isGameLoaded = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.gameScene);
    }

    [PunRPC]
    private void RPC_Rounds(int rounds)
    {
        roundsToPlay = rounds;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.multiplayerSettings.gameScene)
        {
            isGameLoaded = true;

            {
                RPC_CreatePlayer();
            }
        }
    }
    public override void OnLeftRoom()
    {
        if (isGameLoaded)
            GameSetup.GS.DisconnectPlayer();
    }

    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}