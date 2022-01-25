using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClips : MonoBehaviour
{
    public static SoundClips soundClips;

    public AudioClip[] characterSound;
    public AudioClip[] gunSound;
    public AudioClip[] someSFX;

    private void Awake()
    {
        soundClips = this;
    }
}
