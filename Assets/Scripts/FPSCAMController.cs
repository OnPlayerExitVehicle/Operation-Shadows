using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCAMController : MonoBehaviour
{
    public float mouseSpeed = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        GameSetup.GS.CursorDeactive();
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        if (playerBody)
        {
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
        else
        {
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
