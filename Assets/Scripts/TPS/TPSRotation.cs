using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSRotation : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private float lerp;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float time;
    private bool started = false;

    private Vector3 angles;

    private void Update()
    {
       if(Input.GetKey(KeyCode.W) && !started)
        {
            //Debug.Log(transform.rotation.y - followObject.rotation.y);
            if(Mathf.Abs(transform.rotation.eulerAngles.y - followObject.rotation.eulerAngles.y) > 1f ||true)
            {
                started = true;
                Debug.Log(transform.rotation.eulerAngles.y);
                float rotation = Mathf.Lerp(transform.rotation.eulerAngles.y, followObject.rotation.eulerAngles.y, lerp);
                //followObject.rotation = Quaternion.Euler(followObject.rotation.eulerAngles.x, rotation, followObject.rotation.eulerAngles.z);

                //followObject.Rotate(followObject.up, followObject.rotation.eulerAngles.y - rotation);
                angles = followObject.rotation.eulerAngles;
                followObject.rotation = Quaternion.Lerp(followObject.rotation, transform.rotation, lerp);
                StartCoroutine("Rotate", followObject.rotation.eulerAngles - transform.rotation.eulerAngles);
                Fix();
            }
        }
    }

    private void Fix()
    {
        followObject.rotation = Quaternion.Euler(angles.x, followObject.rotation.eulerAngles.y, angles.z);
    }

    IEnumerator Rotate(float rotation)
    {
        Vector3 euler = followObject.rotation.eulerAngles;
        float startRotation = euler.y;
        float deltaRotation = rotation - startRotation;
        float deltaTime = 0;
        bool a = false;
        while(followObject.rotation.y != rotation)
        {
            if (deltaTime >= time)
            {
                deltaTime = time;
                a = true;
            }
            euler.y = startRotation + (deltaRotation / (deltaTime / time));
            

            deltaTime += Time.deltaTime;
            if (a) break;
            yield return null;
            
        }
    }
}
