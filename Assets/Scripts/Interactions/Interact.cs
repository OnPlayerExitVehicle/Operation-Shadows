using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interact : MonoBehaviour
{
    private Transform camera;
    private bool foundInteract;
    private Outline? lastOutline;
    //private bool isLooking = false;

    //[SerializeField] private CinemachineFreeLook cmFL;

    private float maxDistance = 15;

    private Transform backupLookAt;
    private Transform backupFollow;
    private CinemachineCore.AxisInputDelegate backupDel;
    private void Awake() // START'a tasinabilir
    {
        camera = Camera.main.transform;
        //cmFL = InteractionManager.instance.cmFL;
    }
    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance) && hit.transform.CompareTag("Interaction"))
        {
            lastOutline = hit.transform.GetComponent<Outline>();
            lastOutline.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.NextLevel();
            }
        }
        else
        {
            if (lastOutline)
            {
                lastOutline.enabled = false;
                lastOutline = null;
            }
        }
        /*
        if(isLooking)
        {
            
            if(Input.GetKeyDown(KeyCode.E))
            {
                isLooking = false;
                RestoreCinemachineLook();
            }
            
        }
        else if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance) && hit.transform.CompareTag("Interaction"))
        {
            lastOutline = hit.transform.GetComponent<Outline>();
            lastOutline.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                isLooking = true;
                //BackupCinemachineLook();
                //cmFL.LookAt = lastOutline.gameObject.transform;
                //cmFL.Follow = lastOutline.gameObject.transform;
                //CinemachineCore.GetInputAxis = FakeInputAxis;
                //InteractionManager.instance.playerMovement.canMove = false;
            }
        }
        else
        {
            if(lastOutline)
            {
                lastOutline.enabled = false;
            }
        }
        */
    }
    /*
    private void BackupCinemachineLook()
    {
        //backupFollow = cmFL.Follow;
        //backupLookAt = cmFL.LookAt;
        backupDel = CinemachineCore.GetInputAxis;
    }

    private void RestoreCinemachineLook()
    {
        //cmFL.Follow = backupFollow;
        //cmFL.LookAt = backupLookAt;
        CinemachineCore.GetInputAxis = backupDel;
        InteractionManager.instance.playerMovement.canMove = true;
    }

    private float FakeInputAxis(string axisName)
    {
        return 0;
    }
    */
}
