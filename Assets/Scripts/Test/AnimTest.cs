using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class AnimTest : MonoBehaviour
{
    public float speed;
    public bool crouch;

    public Animator anim;

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = Input.GetAxis("Vertical") * 5;

        }
        else
            speed = Input.GetAxis("Vertical") * 2;

        anim.SetFloat("speed", speed);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouch = true;

        }
        else
            crouch = false;
        anim.SetBool("crouch", crouch);

        if (Input.GetMouseButtonDown(0))
        {
            //anim.SetLayerWeight(1, 1);
            anim.SetBool("fire", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("fire", false);
            //anim.SetLayerWeight(1, 0);
        }

        
    }

    
}
