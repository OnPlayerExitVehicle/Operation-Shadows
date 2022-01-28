using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    private PhotonView PV;
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
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            PV.RPC("SetMPSettings", RpcTarget.All);
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.level2Scene);
        }
    }


    [PunRPC]
    private void SetMPSettings()
    {
        MultiplayerSettings.multiplayerSettings.gameScene++;
    }

}
