using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleKillPanel : MonoBehaviour
{
    public float destroyTime = 1f;
    void Start()
    {
        Invoke("DestroyMeReis", destroyTime);
    }

    private void DestroyMeReis()
    {
        Destroy(this.gameObject);
    }
}
