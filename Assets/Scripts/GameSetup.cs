using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public Transform[] team0SpawnPoints;
    public Transform[] team1SpawnPoints;
    public bool gameOver;
    public bool gameStarted;

    private void Awake()
    {
        GS = this;
    }
    private void Start()
    {
        gameStarted = true;
    }
    public void DisconnectPlayer()
    {
        Destroy(PhotonRoom.room.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }

    public void CursorActive()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void CursorDeactive()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DisconnectAndLoad()
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        CursorActive();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }
}
