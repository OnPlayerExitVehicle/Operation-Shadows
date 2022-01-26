using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TPSRotation : MonoBehaviour
{
    [SerializeField] public Transform followObject; //inspector
    [SerializeField] private float lerp;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float time;
    [SerializeField] private float duration;
    [SerializeField] private float fireRotationSpeed;
    [SerializeField] private float fireDuration;
    private bool started = false;

    private Vector3 angles;
    private Tween? tween;

    private Vector2 input;


    private void Start()
    {
        GameManager.instance.camTpsRotation = this;
        Debug_Start();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Debug_Update();
        RotateCamera();
        
    }

    private void RotateCamera()
    {
        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;

        if (input != Vector2.zero)
        {
            float _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(followObject.eulerAngles.y, _targetRotation, ref rotationSpeed, duration);

            followObject.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    public void RotateCameraWithFire()
    {
        float _targetRotation = transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(followObject.eulerAngles.y, _targetRotation, ref fireRotationSpeed, fireDuration);

        followObject.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    private void Debug_Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Debug_Update()
    {
        Debug.Log(Input.GetAxis("Fire"));
    }
    /*
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
    */
}
