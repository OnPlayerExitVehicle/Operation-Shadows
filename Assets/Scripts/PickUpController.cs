using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public PhotonView PV;
    private PhotonView thisPV;

    private AudioSource audioSource;

    public GunSystem gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public PlayerStats playerStats;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public bool slotFull;
    public string ownerName;
    private string localPlayerName;

    private void Start()
    {
        SetNewAvatarProperties();

    }

    public void SetNewAvatarProperties()
    {
        /*foreach (GameObject playeR in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        {
            if (playeR.GetComponent<PhotonView>().IsMine)
            {
                PV = playeR.GetComponent<PhotonView>();
                break;
            }
        }*/


        PV = GameManager.instance.networkPlayer.GetComponent<PhotonView>();

        thisPV = GetComponent<PhotonView>();
        gunScript = GetComponent<GunSystem>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        if (PV.GetComponent<NetworkPlayer>().playerAvatar != null)
        {
            player = PV.GetComponent<NetworkPlayer>().playerAvatar.transform;
            playerStats = PV.GetComponent<NetworkPlayer>().playerAvatar.GetComponent<PlayerStats>();
        }

        //fpsCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //gunContainer = fpsCam.GetChild(0).transform;
        gunContainer = playerStats.gunHolderObject;
        localPlayerName = PhotonNetwork.LocalPlayer.NickName;
        CheckEquippedOrNot();

    }

    private void CheckEquippedOrNot()
    {
        //Setup
        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            if (PV.GetComponent<NetworkPlayer>().playerAvatar != null)
                playerStats.slotFull = true;
        }
    }

    private void Update()
    {
        if (player)
        {
            //Check if player is in range and "E" is pressed
            Vector3 distanceToPlayer = player.position - transform.position;//
            if (!equipped && !playerStats.slotFull && ownerName == "" && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E)) PickUp();

            //Drop if equipped and "Q" is pressed
            if (equipped && ownerName == localPlayerName && Input.GetKeyDown(KeyCode.Q)) Drop();
        }
    }

    public void PickUp()
    {
        thisPV.RPC("RPC_EquippedTrue", RpcTarget.All);
        thisPV.RequestOwnership();
        thisPV.RPC("RPC_OwnerName", RpcTarget.All, localPlayerName);
        playerStats.slotFull = true;
        //Make Rigidbody kinematic and BoxCollider a trigger

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        if (audioSource.clip != SoundClips.soundClips.gunSound[2])
            audioSource.clip = SoundClips.soundClips.gunSound[2];
        audioSource.Play();

        GameManager.instance.playerPickUpController = this;
        GameManager.instance.playerGunSystem = this.GetComponent<GunSystem>();


        //PV.GetComponent<NetworkPlayer>().equippedGun = this;

        //Enable script
    }

    public void Drop()
    {
        thisPV.RPC("RPC_EquippedFalse", RpcTarget.All);
        playerStats.slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<CharacterController>().velocity * 0.2f;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        //PV.GetComponent<NetworkPlayer>().equippedGun = null;

        GameManager.instance.playerPickUpController = null;
        GameManager.instance.playerGunSystem = null;

        //Disable script
    }


    [PunRPC]
    private void RPC_EquippedTrue()
    {
        equipped = true;
        //this.transform.parent
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<BoxCollider>().isTrigger = true;

        this.GetComponent<GunSystem>().enabled = true;
    }

    [PunRPC]
    private void RPC_EquippedFalse()
    {
        equipped = false;
        ownerName = "";
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().isTrigger = false;

        this.GetComponent<GunSystem>().enabled = false;
    }
    [PunRPC]
    private void RPC_OwnerName(string localplayer)
    {
        ownerName = localplayer;
    }
}
