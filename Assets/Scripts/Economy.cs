using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Economy : MonoBehaviour
{
    private PhotonView PV;

    public int para = 100;
    public int Para
    {
        get { return para; }
        set
        {
            if (value <= 0)
                para = 0;
            else
                para = value;
        }
    }

    private TextMeshProUGUI paraText;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
            paraText = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
        paraText.text = Para.ToString() + " TL";
    }
}
