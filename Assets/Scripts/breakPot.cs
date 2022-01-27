using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakPot : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionStrength = 100f;
    public void OnHitBreakPot(GameObject pot, Transform player)
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
            Vector3 direction = (this.transform.position - player.transform.position) + new Vector3(RandomFloat(-5,5), RandomFloat(-5, 5), RandomFloat(-5, 5));
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
