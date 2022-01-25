using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    private GameObject buyMenuPanel;
    private GameObject mainCam;
    private NetworkPlayer networkPlayer;

    public ESCMenu escMenu;
    public bool buyControl;
    // Start is called before the first frame update
    private void Awake()
    {
        buyMenuPanel = transform.GetChild(0).gameObject;
        buyMenuPanel.SetActive(false);
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
    private void Update()
    {
        InputKeys();
    }

    private void InputKeys()
    {
        if (Input.GetKeyDown(KeyCode.B) && networkPlayer.avatarSpawned && GameSetup.GS.gameOver == false)
        {
            if(escMenu.escControl == false)
            {
                buyControl = !buyControl;
                Buy();
            }
        }
    }

    public void CloseBuy()
    {
        buyControl = false;
        Buy();
    }

    private void Buy()
    {
        if (buyControl == true)
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
                if(networkPlayer.playerAvatar.GetComponent<PlayerMovement>())
                    networkPlayer.playerAvatar.GetComponent<PlayerMovement>().esc = true;
                if (mainCam.transform.GetChild(0).childCount > 0)
                    mainCam.transform.GetChild(0).GetChild(0).GetComponent<GunSystem>().enabled = false;
            }
            //Cursor.lockState = CursorLockMode.None;
            GameSetup.GS.CursorActive();
            buyMenuPanel.SetActive(true);
            SetButtonsProperties();
        }
        if (buyControl == false)
        {
            buyMenuPanel.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
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
                if (mainCam.transform.GetChild(0).childCount > 0)
                    mainCam.transform.GetChild(0).GetChild(0).GetComponent<GunSystem>().enabled = true;
            }
        }
    }

    private void SetButtonsProperties()
    {
        for (int i = 0; i < buyMenuPanel.transform.childCount; i++)
        {
            if (networkPlayer.GetComponent<Economy>().Para >= buyMenuPanel.transform.GetChild(i).GetComponent<GunsPrice>().gunPrice)
                buyMenuPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            else
                buyMenuPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    public void BuyItem(int fiyat)
    {
        networkPlayer.GetComponent<Economy>().Para -= fiyat;
        SetButtonsProperties();
    }

    public void SpawnItem(string gunName)
    {
        networkPlayer.SpawnGun(gunName);
    }
}
