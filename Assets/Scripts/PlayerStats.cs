using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public PhotonView PV;
    public Text healthText;
    public NetworkPlayer networkPlayer;
    public string whoKilledMe;

    private GameObject sapka;

    public int health = 100;
    public int Health
    {
        get { return health; }
        set
        {
            if(value <= 10)
            {
                if (PV.IsMine)
                    SFXPlay(0);
            }
            if (value <= 0)
            {
                health = 0;
                if(PV.IsMine)
                    SFXPlay(Random.Range(1, 4));
                Died();
            }
            else
                health = value;
        }
    }
    public bool slotFull;


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        healthText = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<Text>();
        sapka = transform.GetChild(1).gameObject;
        if(PV.IsMine)
            sapka.SetActive(false);
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;
        healthText.text = Health.ToString();
    }

    private void SFXPlay(int index)
    {
        AudioSource sfx = SoundClips.soundClips.GetComponent<AudioSource>();
        sfx.clip = SoundClips.soundClips.someSFX[index];
        sfx.Play();
    }

    private void Died()
    {
        Debug.Log(this.gameObject + " öldü");
        healthText.text = Health.ToString();
        SetDeathOnNetworkPlayer();
        SetScore();
        GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(7).GetComponent<BuyMenu>().CloseBuy();
        if (PV.IsMine)
            SilahAt();
        ActiveRebirth();
        YokEtVeRagDoll();
    }

    private void SetDeathOnNetworkPlayer()
    {
        if (PV.IsMine)
        {
            networkPlayer.PV.RPC("RPC_SetDeath", RpcTarget.All);
        }
    }


    public void SilahAt()
    {
        if(this.transform.GetChild(2).GetChild(0).GetChild(0).childCount > 0)
            this.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<PickUpController>().Drop();
    }
    private void ActiveRebirth()
    {
        if (PV.IsMine)
        {
            networkPlayer.avatarSpawned = false;
            networkPlayer.SetPressRtoSpawnText();
        }
    }

    private void YokEtVeRagDoll()
    {
        if (PV.IsMine)
        {
            sapka.SetActive(true);
            this.transform.GetChild(2).GetChild(0).GetComponent<FPSCAMController>().playerBody = null;
            this.transform.GetChild(2).parent = null;
        }
        Destroy(this.gameObject.GetComponent<PlayerMovement>());
        Destroy(this.gameObject.GetComponent<CharacterController>());
        this.gameObject.AddComponent<CapsuleCollider>();
        Rigidbody rg = this.gameObject.AddComponent<Rigidbody>();
        rg.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        this.gameObject.tag = "DeadBody";
        this.gameObject.layer = 0;
        Destroy(this.gameObject.GetComponent<PlayerStats>());
    }

    public void DamageDealer(int damage, Vector3 enemypos)
    {
        PV.RPC("TakeDamage", RpcTarget.All, damage, enemypos);
    }
    private void SetScore()
    {
        if ((int)PV.Owner.CustomProperties["Team"] == 0)
        {
            //PV.RPC("SetTeam1Score", RpcTarget.All);
            SetTeam1Score();
        }
        if ((int)PV.Owner.CustomProperties["Team"] == 1)
        {
            //PV.RPC("SetTeam0Score", RpcTarget.All);
            SetTeam0Score();
        }
    }

    public void BoosterHealth(int heal)
    {
        PV.RPC("SetHealth", RpcTarget.All, heal);
    }

    [PunRPC]
    private void TakeDamage(int damage, Vector3 enemy)
    {
        Health -= damage;
        Vector3 knockbackVector = this.transform.position - enemy;
        this.GetComponent<CharacterController>().Move(knockbackVector * Time.deltaTime);
    }
    [PunRPC]
    public void RPC_SetWhoKilledMe(string name)
    {
        whoKilledMe = name;
    }

    [PunRPC]
    private void SetHealth(int healths)
    {
        Health = healths;
    }
    //[PunRPC]
    private void SetTeam1Score()
    {
        GameObject.FindGameObjectWithTag("TeamStatistics").GetComponent<TeamStatistics>().team1score += 1;
    }
    //[PunRPC]
    private void SetTeam0Score()
    {
        GameObject.FindGameObjectWithTag("TeamStatistics").GetComponent<TeamStatistics>().team0score += 1;
    }

}
