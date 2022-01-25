using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunsPrice : MonoBehaviour
{
    public int gunPrice;

    private void Awake()
    {
        transform.GetChild(1).GetComponent<Text>().text = gunPrice + "TL";
    }
}
