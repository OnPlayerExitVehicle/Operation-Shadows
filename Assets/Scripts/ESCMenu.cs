using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    private GameObject ayarUi;
    private GameObject escUI;
    public bool escControl;
    public NetworkPlayer networkPlayer;
    private GameObject mainCam;
    public BuyMenu buyMenu;
    private void Awake()
    {
        escUI = transform.GetChild(0).gameObject;
        ayarUi = transform.GetChild(1).gameObject;
        escUI.SetActive(false);
        ayarUi.SetActive(false);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Start()
    {
        foreach (GameObject nPObject in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            if (nPObject.GetComponent<PhotonView>().IsMine)
            {
                networkPlayer = nPObject.GetComponent<NetworkPlayer>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputKeys();
    }

    public void MenuyeDon()
    {
        GameSetup.GS.DisconnectPlayer();
    }


    public void SettingsButton()
    {
        escUI.SetActive(false);
        ayarUi.SetActive(true);
    }
    public void QualitySetting(int index)
    {
        int newIndex = 5;
        newIndex -= index;
        QualitySettings.SetQualityLevel(newIndex);
    }


    
    private void InputKeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buyMenu.buyControl == false)
            {
                escControl = !escControl;
                ESC();
            }
            if (buyMenu.buyControl == true)
            {
                buyMenu.CloseBuy();
            }
        }
    }

    private void ESC ()
    {
        if (escControl == true)
        {
            if (mainCam)
            {
                if (mainCam.GetComponent<FPSCAMController>())
                {
                    mainCam.GetComponent<FPSCAMController>().enabled = false;
                }
            }
            if (networkPlayer.playerAvatar)
            {
                if (networkPlayer.playerAvatar.GetComponent<PlayerMovement>())
                    networkPlayer.playerAvatar.GetComponent<PlayerMovement>().esc = true;
                if (mainCam.transform.GetChild(0).childCount > 0)
                    mainCam.transform.GetChild(0).GetChild(0).GetComponent<GunSystem>().enabled = false;
            }
            GameSetup.GS.CursorActive();
            escUI.SetActive(true);
        }
        if (escControl == false)
        {
            escUI.SetActive(false);
            ayarUi.SetActive(false);
            GameSetup.GS.CursorDeactive();
            if (mainCam)
            {
                if (mainCam.GetComponent<FPSCAMController>())
                {
                    mainCam.GetComponent<FPSCAMController>().enabled = true;
                }
            }
            if (networkPlayer.playerAvatar)
            {
                if (networkPlayer.playerAvatar.GetComponent<PlayerMovement>())
                    networkPlayer.playerAvatar.GetComponent<PlayerMovement>().esc = false;
                if(mainCam.transform.GetChild(0).childCount > 0)
                    mainCam.transform.GetChild(0).GetChild(0).GetComponent<GunSystem>().enabled = true;
                    
            }
        }
    }
}
