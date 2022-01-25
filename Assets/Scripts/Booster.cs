using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private PhotonView thisPV;

    public bool saglik;
    public int saglikOrani = 500;

    public bool ziplama;
    public float ziplamaOrani = 15f;
    public float ziplamaSuresi = 15f;

    public bool hiz;
    public float hizOrani = 24f;
    public float hizSuresi = 20f;

    public bool kucultme;
    public float kucultmeOrani = 0.5f;

    public bool mermi;
    public int mermiOrani = 1000;

    public float deploymentHeight;
    public float parachuteEffectiveness;
    private RaycastHit hit;
    private bool deployed;
    // Start is called before the first frame update
    private void Awake()
    {
        thisPV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        Ray landingRay = new Ray(transform.position, Vector3.down);
        if (!deployed)
        {
            if (Physics.Raycast(landingRay, out hit, deploymentHeight))
                DeployParachut();
        }
    }

    private void DeployParachut()
    {
        deployed = true;
        GetComponent<Rigidbody>().drag = parachuteEffectiveness;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                DoMagic(other.gameObject);
            }
        }
    }
    private void DoMagic(GameObject player)
    {
        if (saglik)
            SaglikArttir(player);
        if (ziplama)
            ZiplamaArttir(player);
        if (hiz)
            HizArttir(player);
        if (kucultme)
            Kucult(player);
        if (mermi)
            MermiArttir(player);
    }

    private void Kucult(GameObject oyuncu)
    {
        oyuncu.transform.localScale = new Vector3(kucultmeOrani, kucultmeOrani, kucultmeOrani);
        oyuncu.transform.GetChild(1).localScale = new Vector3(1 / kucultmeOrani, 1 / kucultmeOrani, 1 / kucultmeOrani);
        YokEt();
    }

    private void HizArttir(GameObject oyuncu)
    {
        oyuncu.GetComponent<PlayerMovement>().BoosterSpeed(hizOrani, hizSuresi);
        SFXPlay(7);
        YokEt();
    }

    private void ZiplamaArttir(GameObject oyuncu)
    {
        oyuncu.GetComponent<PlayerMovement>().BoosterJump(ziplamaOrani, ziplamaSuresi);
        SFXPlay(5);
        YokEt();
    }

    private void SaglikArttir(GameObject oyuncu)
    {
        oyuncu.GetComponent<PlayerStats>().BoosterHealth(saglikOrani);
        SFXPlay(4);
        YokEt();
    }
    private void MermiArttir(GameObject oyuncu)
    {
        if(oyuncu.transform.GetChild(2).GetChild(0).GetChild(0).childCount > 0)
        {
            oyuncu.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<GunSystem>().SetTotalAmmoToEveryOne(mermiOrani);
            SFXPlay(8);
            YokEt();
        }
    }


    private void SFXPlay(int index)
    {
        /*AudioSource sfx = SoundClips.soundClips.GetComponent<AudioSource>();
        sfx.clip = SoundClips.soundClips.someSFX[index];
        sfx.Play();*/
    }


    private void YokEt()
    {
        thisPV.RequestOwnership();
        //PhotonNetwork.Destroy(this.gameObject);
        thisPV.RPC("RPC_Destroy", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Destroy()
    {
        Destroy(this.gameObject);
    }
}
