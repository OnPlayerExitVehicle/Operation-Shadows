using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamStatistics : MonoBehaviour
{
    public Text team0ScoreText;
    public Text team1ScoreText;
    public Text wonText;

    private  int _team0score;
    public  int team0score
    {
        get { return _team0score; }
        set 
        {
            _team0score = value;
            SetScoreText();
            CheckGameOver();
        }

    }
    private  int _team1score;
    public  int team1score
    {
        get { return _team1score; }
        set 
        {
            _team1score = value;
            SetScoreText();
            CheckGameOver();
        }
    }

    private void Start()
    {
        wonText.gameObject.SetActive(false);
        SetScoreText();
    }

    private void SetScoreText()
    {
        team0ScoreText.text = "Kırmızı Takım: " + team0score.ToString();
        team1ScoreText.text = "Mavi Takım: " + team1score.ToString();
    }

    private void CheckGameOver()
    {
        if (team0score >= PhotonRoom.room.roundsToPlay || team1score >= PhotonRoom.room.roundsToPlay)
        {
            if (team0score > team1score)
            {
                wonText.text = "Kırmızı Takım Kazandı!";
                wonText.gameObject.SetActive(true);
                GameOver();
            }
            else
            {
                wonText.text = "Mavi Takım Kazandı!";
                wonText.gameObject.SetActive(true);
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        GameSetup.GS.gameOver = true;
        Destroy(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FPSCAMController>());
        foreach (GameObject oyuncu in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (oyuncu.GetComponent<PhotonView>().IsMine)
            {
                oyuncu.GetComponent<PlayerStats>().SilahAt();
                oyuncu.GetComponent<PlayerStats>().slotFull = true;
                oyuncu.GetComponent<PlayerMovement>().enabled = false;
            }
        }
        SFXPlay(6);
    }

    private void SFXPlay(int index)
    {
        AudioSource sfx = SoundClips.soundClips.GetComponent<AudioSource>();
        sfx.clip = SoundClips.soundClips.someSFX[index];
        sfx.Play();
    }
}
