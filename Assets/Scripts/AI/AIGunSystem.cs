using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.IO;
using System.Threading.Tasks;

public class AIGunSystem : MonoBehaviour
{
    private PhotonView PV;
    //private PhotonView pvPlayer;
    private AudioSource audioSource;
    private AudioSource shootAudioSource;
    public GameObject player;
    private GameObject tempBulletHoleObject;
    public GameObject bulletHolePrefab;
    public float knockBackObjectForce = 500f;
    private bool debug = true; // sunu silin amk

    //Gun stats
    public int damage;
    public int totalAmmo;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsShootable;

    //Graphics
    public GameObject bulletHoleGraphic;
    public ParticleSystem muzzleFlash;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private int debugDamageMultiplier = 0; // sil
    private void Start()
    {
        //fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //text = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //camShake = fpsCam.GetComponent<CameraShake>();



        audioSource = GetComponent<AudioSource>();
        shootAudioSource = attackPoint.GetComponent<AudioSource>();
        shootAudioSource.clip = SoundClips.soundClips.gunSound[0];
        PV = GetComponent<PhotonView>();
        //foreach (GameObject player in GameObject.FindGameObjectsWithTag("NetworkPlayer"))
        //{
        //    if (player.GetComponent<PhotonView>().IsMine)
        //    {
        //        pvPlayer = player.GetComponent<PhotonView>();
        //        break;
        //    }
        //}
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        /*if (PV.IsMine)
        {
            MyInput();
            //SetText
            text.SetText(bulletsLeft + " / " + totalAmmo.ToString());
        }*/
    }
    public void MyInput(GameObject player)
    {
        /*if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);*/

        //if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && totalAmmo > 0) Reload();

        //Shoot
        if (readyToShoot /*&& shooting */&& !reloading && (bulletsLeft > 0 || debug))
        {
            //GameManager.instance.camTpsRotation.RotateCameraWithFire();//?
            bulletsShot = bulletsPerTap;
            Shoot(player);
        }
    }

    
    private void Shoot(GameObject plyr)
    {
        readyToShoot = false;

        //Spread
        /*float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);*/

        //Calculate Direction with Spread
        //Vector3 direction = fpsCam.transform.forward;

        //RayCast
        //if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsShootable))
        //{
            //Debug.Log(rayHit.collider.name);

            //if (rayHit.collider.CompareTag("Player"))
            //{
                PlayerStats pStats = plyr.GetComponent<PlayerStats>();
                pStats.DamageDealer(damage * debugDamageMultiplier, this.transform.position);
                //this.GetComponent<PickUpController>().PV.GetComponent<Economy>().Para += damage * 2;
                if (pStats.Health <= 0) //öldürünce yapýlacaklar
                {
                    string myName = "AI";
                    string enemyName = pStats.PV.Owner.NickName;
                    //pStats.PV.RPC("RPC_SetWhoKilledMe", RpcTarget.All, myName);
                    //this.GetComponent<PickUpController>().PV.GetComponent<NetworkPlayer>().PV.RPC("RPC_SetKills", RpcTarget.All);
                    //this.GetComponent<PickUpController>().PV.GetComponent<Economy>().Para += 100;
                    KillTable.KT.CreateKillTablePart(myName, enemyName);
                    //SFXPlay(Random.Range(9, 11));
                }
            //}
            /*if (rayHit.collider.GetComponent<Rigidbody>())
            {
                if (!rayHit.collider.CompareTag("Gun"))
                {
                    PhotonView pView = rayHit.collider.GetComponent<PhotonView>();
                    //if (!pView.IsMine)
                        pView.RequestOwnership();
                    Vector3 knockback = rayHit.collider.transform.position - this.transform.position;
                    rayHit.collider.GetComponent<Rigidbody>().AddForce(knockback.normalized * knockBackObjectForce);
                }
            }*/
            //Graphics
            //tempBulletHoleObject = PhotonNetwork.Instantiate(Path.Combine("SceneSpawn", "BulletImpactMetalEffect"), rayHit.point, Quaternion.LookRotation(rayHit.normal), 0);
            /*if (rayHit.collider.GetComponent<PhotonView>())
            {
                int tempBulletHoleId = tempBulletHoleObject.GetComponent<PhotonView>().ViewID;
                int parentObjectId = rayHit.collider.GetComponent<PhotonView>().ViewID;
                PV.RPC("RPC_SetBulletHoleParent", RpcTarget.All, tempBulletHoleId, parentObjectId);

            }*/
        //}

        //ShakeCamera
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));




        PV.RPC("AIRPC_Shooting", RpcTarget.All);


        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }


    private void SFXPlay(int index)
    {
        AudioSource sfx = SoundClips.soundClips.GetComponent<AudioSource>();
        sfx.clip = SoundClips.soundClips.someSFX[index];
        sfx.Play();
    }



    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        if (audioSource.clip != SoundClips.soundClips.gunSound[1])
            audioSource.clip = SoundClips.soundClips.gunSound[1];
        audioSource.Play();
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        //bulletsLeft = magazineSize;
        PV.RPC("AIRPC_TotalAmmo", RpcTarget.All);
        reloading = false;
    }

    [PunRPC]
    private void AIRPC_TotalAmmo()
    {
        int totalAmmoAndMagazine = bulletsLeft + totalAmmo;
        if (totalAmmoAndMagazine > magazineSize)
        {
            totalAmmo = totalAmmoAndMagazine - magazineSize;
            bulletsLeft = magazineSize;
        }
        else
        {
            bulletsLeft += totalAmmo;
            totalAmmo = 0;
        }
    }

    public void SetTotalAmmoToEveryOne(int mermi)
    {
        PV.RPC("AIRPC_SetTotalAmmo", RpcTarget.All, mermi);
    }

    [PunRPC]
    private void AIRPC_Shooting()
    {
        muzzleFlash.Play();
        shootAudioSource.Play();

        bulletsLeft--;
    }

    [PunRPC]
    private void AIRPC_SetTotalAmmo(int ammo)
    {
        totalAmmo = ammo;
    }

    [PunRPC]
    private void AIRPC_SetBulletHoleParent(int viewId1, int viewId2)
    {
        PhotonView.Find(viewId1).gameObject.transform.parent = PhotonView.Find(viewId2).gameObject.transform;
    }
}