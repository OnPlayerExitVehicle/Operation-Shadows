using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private PhotonView PV;
    private CharacterController controller;
    private AudioSource audioSource;

    public float speed = 12f;
    private float speedWithoutBoost;
    public float jumpHeight = 3f;
    private float jumpWithoutBoost;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float moveSoundVelocity = 1f;

    public Vector3 move;
    private Vector3 velocity;
    bool isGrounded;

    public bool esc;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        speedWithoutBoost = speed;
        jumpWithoutBoost = jumpHeight;
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = 0;
        float z = 0;

        if (!esc)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && move.sqrMagnitude > moveSoundVelocity && !audioSource.isPlaying)
        {
            float pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            PV.RPC("PlayFootStep", RpcTarget.All, pitch);
        }
    }

    public void BoosterSpeed(float yenihiz, float time)
    {
        speed = yenihiz;
        Invoke("RemoveBoosterSpeed", time);
    }

    private void RemoveBoosterSpeed()
    {
        speed = speedWithoutBoost;
    }

    public void BoosterJump(float yenijump, float time)
    {
        jumpHeight = yenijump;
        Invoke("RemoveBoosterJump", time);
    }

    private void RemoveBoosterJump()
    {
        jumpHeight = jumpWithoutBoost;
    }

    [PunRPC]
    private void PlayFootStep(float rndmPitch)
    {
        if (audioSource.clip != SoundClips.soundClips.characterSound[0])
            audioSource.clip = SoundClips.soundClips.characterSound[0];
        audioSource.pitch = rndmPitch;
        audioSource.Play();
    }

}
