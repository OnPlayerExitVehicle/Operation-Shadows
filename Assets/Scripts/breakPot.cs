using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakPot : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionStrength = 100f;
    public void OnHitBreakPot(GameObject pot)
    {
        //Script ve gameObject alýndý, gameObject fonksiyona iletildi. Kýrýk olmayan hali fonksiyona parametre olarak geldi.
        //<>

        GameObject parent = transform.parent.gameObject;
        GameObject child = parent.transform.GetChild(0).gameObject;
        child.SetActive(true);
        rb = child.GetComponent<Rigidbody>();
        for(int i= 0; i < child.transform.childCount; i++)
        {
            rb = child.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(explosionStrength, transform.position, explosionRadius);
        }
        gameObject.SetActive(false);
    }
}
