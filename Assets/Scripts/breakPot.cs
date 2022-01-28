using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class breakPot : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionRadius; //Patlama iþlendiðinde daðýldýðý alan.
    [SerializeField] float explosionStrength = 100f; //Patlama gücü
    public GameObject audio;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    public void OnHitBreakPot(Transform plyr)
    {
        PV.RPC("RPC_OnHitBreakPot", RpcTarget.All, plyr.position);
    }

    [PunRPC]
    private void RPC_OnHitBreakPot(Vector3 player)
    {
        //Merminin çarptýðý noktada çalýþtýrýlýr. Çarpan obje GUNSYSTEM.cs de kontrol edilir.
        //Kýrýlabilir eþyanýn çalýþma þekli Parent: EmptyGameObject / Child: Objenin kýrýlmamýþ hali ve Objenin kýrýlmýþ hali.
        //Objenin kýrýlmamýþ halinden parenta gidilir, parenttan objenin kýrýlmýþ haline gidilir.
        //Objenin kýrýlmamýþ hali scene'den kaldýrýlýr ve kýrýlmýþ hali yüklenir.
        //Player'ýn transformunun alýnma sebebi merminin ne taraftan geldiðine baðlý olarak objeye FORCE uygulanmasýdýr.
        //<>
        Instantiate(audio, transform.position, Quaternion.identity);

        GameObject parent = transform.parent.gameObject;
        GameObject child = parent.transform.GetChild(0).gameObject;
        child.SetActive(true);
        rb = child.GetComponent<Rigidbody>();

        //Objenin kýrýlmýþ hali aktive olur içerisine patlama olaylarý eklenir.
        for (int i = 0; i < child.transform.childCount; i++)
        {
            rb = child.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(explosionStrength, transform.position, explosionRadius);
            Vector3 direction = (this.transform.position - player) + new Vector3(RandomFloat(-5, 5), RandomFloat(-5, 5), RandomFloat(-5, 5));
            rb.AddForce(direction.normalized * RandomFloat(5, 10), ForceMode.Impulse);
        }
        gameObject.SetActive(false);
    }

    private float RandomFloat(float min, float max)
    {
        float rndm = Random.Range(min, max);
        return rndm;
    }
}
