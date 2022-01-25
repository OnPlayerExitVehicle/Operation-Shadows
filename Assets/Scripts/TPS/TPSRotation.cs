using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TPSRotation : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private float lerp;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float time;
    [SerializeField] private float duration;
    private bool started = false;

    private Vector3 angles;
    private Tween? tween;

    private float inputX;
    private float inputY;

    private void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        Debug.Log(inputY);
        /*
       if(Input.GetKey(KeyCode.W))
        {
            TurnCharacter(1);
        }
       else if(Input.GetKey(KeyCode.S))
        {
            TurnCharacter(-1);
        }
        */
        Test();
    }
    private void Test()
    {
        Vector3 inputDirection = new Vector3(inputX, 0.0f, inputY).normalized;

        if (inputX != 0f || inputY != 0f)
        {
            float _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(followObject.eulerAngles.y, _targetRotation, ref rotationSpeed, duration);

            followObject.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
    private void TurnCharacter(int rotation) // rotation = 1 veya -1
    {
        if (tween.IsActive())
        {
            tween.Kill();
        }
        Vector3 euler = followObject.rotation.eulerAngles;
        
        if(rotation == 1)
        {
            euler.y = transform.rotation.eulerAngles.y;
        }
        else
        {
            euler.y =  - transform.rotation.eulerAngles.y;
        }
        tween = followObject.DORotate(euler, duration);
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
