using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{

    public float bombRadius;
    public float bombForce;
    private bool exploaded;
    private void OnCollisionEnter(Collision collision)
    {
        print("bomba triggered");
        if(!exploaded)
        Explode();
    }

    private void Explode()
    {
        exploaded = true;
        Collider[] coll = Physics.OverlapSphere(transform.position, bombRadius);

        

        foreach (Collider col in coll)
        {
            if (col.CompareTag("Exploadable"))
            {
                if (!CheckIfTwoMethodBreakable(col.transform))
                {
                    Rigidbody rbforsingle = col.GetComponent<Rigidbody>();
                    rbforsingle.isKinematic = false;
                    rbforsingle.AddExplosionForce(bombForce, transform.position, bombRadius);
                }
                else
                {
                    Transform childObject = col.transform.GetChild(0);
                    childObject.gameObject.SetActive(true);
                    childObject.transform.parent = null;
                    col.gameObject.SetActive(false);
                    for (int i = 0; i < childObject.childCount; i++)
                    {
                        Rigidbody rb = childObject.GetChild(i).GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.AddExplosionForce(bombForce, transform.position, bombRadius);
                    }
                }
            }
        }
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
