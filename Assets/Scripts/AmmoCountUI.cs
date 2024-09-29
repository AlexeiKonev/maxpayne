using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AmmoCountUI : MonoBehaviour
{
   public  TMP_Text ammoUI;
    public Shooting shooting;
    void Start()
    {
        ammoUI.text = shooting.ammo.ToString() ;
    }

    // Update is called once per frame
    void Update()
    {
        ammoUI.text = shooting.ammo.ToString();
    }
}
