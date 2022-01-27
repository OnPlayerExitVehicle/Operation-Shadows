using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }

    private CinemachineImpulseSource source;
    private void Awake()
    {
        instance = this;
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera()
    {
        source.GenerateImpulse();
    }
}
