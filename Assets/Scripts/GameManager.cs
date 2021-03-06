using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public GunSystem playerGunSystem;
    [HideInInspector]
    public PickUpController playerPickUpController;

    [HideInInspector]
    public TPSRotation camTpsRotation;
    [HideInInspector]
    public PlayerMovement playerMovement;

    [HideInInspector]
    public NetworkPlayer networkPlayer;
    [HideInInspector]
    public GameObject playerAvatar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        
    }
    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            PhotonRoom.room.SetGameScene(2);
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.level2Scene);
        }
    }
    */
    public void NextLevel()
    {
        PhotonRoom.room.SetGameScene(2);
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.level2Scene);
    }


}
