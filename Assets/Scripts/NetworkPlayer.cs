using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviour
{
    public PhotonView PV;
    public GameObject playerAvatar;
    public PickUpController equippedGun;

    public bool avatarReadyToSpawn;
    public bool avatarSpawned ;
    public GameObject test;
    private Vector3 camHolderOffset = new Vector3(0.006f, 0.52f, 0.13f);

    public int _kills;
    public int _deaths;

    private GameObject pressRtoSpawnText;

    public int kills
    {
        get { return _kills; }
        set
        {
            if (value <= 0)
                _kills = 0;
            else
                _kills = value;
        }
    }
    public int deaths
    {
        get { return _deaths; }
        set
        {
            if (value <= 0)
                _deaths = 0;
            else
                _deaths = value;
        }
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        pressRtoSpawnText = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(4).gameObject;
        //pressRtoSpawnText.SetActive(true);
        if(PV.IsMine)
            GameManager.instance.networkPlayer = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnChar();
        }
    }

    public void SpawnChar() // Sets On SpawnCharacter
    {
        if (!avatarSpawned && GameSetup.GS.gameOver == false)
        {
            if(PV.IsMine)
                SpawnPlayerAvatarAndCamSet();
        }
    }

    private void SpawnPlayerAvatarAndCamSet()
    {
        GameObject avatar;
        //if ((int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == 0)
            avatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainCharacter"), 
                GameSetup.GS.team0SpawnPoints[Random.Range(0, GameSetup.GS.team0SpawnPoints.Length)].position, Quaternion.identity, 0);
        GameManager.instance.playerAvatar = avatar;
        /*else
            avatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player1"), 
                GameSetup.GS.team1SpawnPoints[Random.Range(0, GameSetup.GS.team1SpawnPoints.Length)].position, Quaternion.identity, 0);
        */
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        mainCam.GetComponent<TPSRotation>().followObject = avatar.transform;

        /*Transform camHolder = mainCam.transform.parent;
        camHolder.parent = avatar.transform;
        camHolder.localPosition = camHolderOffset;
        camHolder.localRotation = Quaternion.Euler(Vector3.zero);
        mainCam.GetComponent<FPSCAMController>().playerBody = avatar.transform;*/


        playerAvatar = avatar;
        PlayerStats playerStats = playerAvatar.GetComponent<PlayerStats>();

        CinemachineFreeLook cm = GameObject.FindGameObjectWithTag("CMFreeLook").GetComponent<CinemachineFreeLook>();
        cm.Follow = avatar.transform;
        cm.LookAt = playerStats.lookAtObject;

        //avatar.layer = 9;
        playerStats.networkPlayer = this;
        if(PhotonNetwork.IsMasterClient) Debug.Log("Owner");
        avatarSpawned = true;

        pressRtoSpawnText.SetActive(false);

        SpawnGun("m4a4");
    }
    public void SpawnGun(string whichGun)
    {
        if (GameManager.instance.playerPickUpController)
            GameManager.instance.playerPickUpController.Drop();
        GameObject gun = PhotonNetwork.Instantiate(Path.Combine("SceneSpawn", whichGun), transform.position, Quaternion.identity, 0);
        ImplementAvatarToGuns();
        gun.GetComponent<PickUpController>().PickUp();
    }   

    private void ImplementAvatarToGuns()
    {
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun.GetComponent<PickUpController>().SetNewAvatarProperties();
        }
    }

    public void SetPressRtoSpawnText()
    {
        pressRtoSpawnText.SetActive(true);
    }

    [PunRPC]
    public void RPC_SetDeath()
    {
        deaths += 1;
    }

    [PunRPC]
    public void RPC_SetKills()
    {
        kills += 1;
    }
}
