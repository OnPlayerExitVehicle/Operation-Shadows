using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class breakPot : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionRadius; //Patlama i�lendi�inde da��ld��� alan.
    [SerializeField] float explosionStrength = 100f; //Patlama g�c�
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
        //Merminin �arpt��� noktada �al��t�r�l�r. �arpan obje GUNSYSTEM.cs de kontrol edilir.
        //K�r�labilir e�yan�n �al��ma �ekli Parent: EmptyGameObject / Child: Objenin k�r�lmam�� hali ve Objenin k�r�lm�� hali.
        //Objenin k�r�lmam�� halinden parenta gidilir, parenttan objenin k�r�lm�� haline gidilir.
        //Objenin k�r�lmam�� hali scene'den kald�r�l�r ve k�r�lm�� hali y�klenir.
        //Player'�n transformunun al�nma sebebi merminin ne taraftan geldi�ine ba�l� olarak objeye FORCE uygulanmas�d�r.
        //<>
        Instantiate(audio, transform.position, Quaternion.identity);

        GameObject parent = transform.parent.gameObject;
        GameObject child = parent.transform.GetChild(0).gameObject;
        child.SetActive(true);
        rb = child.GetComponent<Rigidbody>();

        //Objenin k�r�lm�� hali aktive olur i�erisine patlama olaylar� eklenir.
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
