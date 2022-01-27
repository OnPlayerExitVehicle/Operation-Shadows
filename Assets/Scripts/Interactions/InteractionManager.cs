using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    public CinemachineFreeLook cmFL;
    public PlayerMovement playerMovement;
    private InteractionManager()
    {
        instance = this;
    }

    private void Awake()
    {
        cmFL = FindObjectOfType<CinemachineFreeLook>();
    }
}
