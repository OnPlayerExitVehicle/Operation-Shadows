using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    public float timeToExplode;
    public float bombRadius;
    public float bombForce;
    private bool exploaded;
    public float killRadius;
    public int bombDamage;

    public GameObject expEffect;
    private AudioSource booom;
    private void Start()
    {
        booom = GetComponent<AudioSource>();
        print("bomba triggered");
        if (!exploaded)
            Invoke("Explode", timeToExplode);
    }

    private void Explode()
    {
        exploaded = true;
        Instantiate(expEffect, transform.position, Quaternion.identity);
        booom.Play();
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            this.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }

        Collider[] ais = Physics.OverlapSphere(transform.position, killRadius);
        for (int i = 0;i < ais.Length;i++)
        {
            if (ais[i].CompareTag("AI"))
            {
                ais[i].GetComponent<AIHealth>().GiveMeDamage(bombDamage);
            }
        }
        for (int i = 0; i < ais.Length; i++)
        {
            if (ais[i].CompareTag("AIRagdoll"))
            {
                Vector3 direction = ais[i].transform.position - transform.position;
                ais[i].GetComponent<Rigidbody>().AddForce(direction.normalized * bombForce, ForceMode.Impulse);
            }
        }

        Collider[] coll = Physics.OverlapSphere(transform.position, bombRadius);



        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i].CompareTag("Exploadable"))
            {
                if (!CheckIfTwoMethodBreakable(coll[i].transform))
                {
                    Rigidbody rbforsingle = coll[i].GetComponent<Rigidbody>();
                    rbforsingle.isKinematic = false;
                    rbforsingle.AddExplosionForce(bombForce, transform.position, bombRadius);
                }
                else
                {
                    Transform childObject = coll[i].transform.GetChild(0);
                    childObject.gameObject.SetActive(true);
                    childObject.transform.parent = null;
                    coll[i].gameObject.SetActive(false);
                    for (int j= 0;j< childObject.childCount; j++)
                    {
                        Rigidbody rb = childObject.GetChild(j).GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.AddExplosionForce(bombForce, transform.position, bombRadius);
                    }
                }
            }
            if (coll[i].CompareTag("Pot"))
            {
                coll[i].GetComponent<breakPot>().OnHitBreakPot(transform);
            }
        }
        Destroy(this.gameObject, 10);
    }

    private bool CheckIfTwoMethodBreakable(Transform obj)
    {
        if (obj.childCount <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
