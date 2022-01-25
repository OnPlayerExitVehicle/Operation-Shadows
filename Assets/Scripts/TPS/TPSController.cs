using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour
{
    private Camera cam;
    private float mouseX;
    private float mouseY;

    [SerializeField] private float mouseSensitivity;

    private void Awake()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //ProcessInput();
        //ProcessCameraRotation();

    }

    private void ProcessInput()
    {
        float temp = mouseSensitivity * Time.deltaTime;
        mouseX = Input.GetAxis("Mouse X") * temp;
        mouseY = Input.GetAxis("Mouse Y") * temp;
    }

    private void ProcessCameraRotation()
    {
        cam.transform.RotateAround(transform.position, transform.up, mouseX);
        cam.transform.RotateAround(transform.position, transform.right, mouseY);
    }
}
