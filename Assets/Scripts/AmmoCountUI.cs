using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class AmmoCountUI : MonoBehaviour
{
   public  TMP_Text ammoUIMagazine;
   public  TMP_Text ammoUIAll;
    public Shooting shooting;
    public WeaponManager weaponManager;
    void Start()
    {
        weaponManager= GameObject.Find("WeaponManager").GetComponent<WeaponManager>();   

      
       
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponManager.weaponState == WeaponManager.WeaponState.pistol)
        {
            ammoUIMagazine.text = shooting.ammoPistolCurent.ToString();

            ammoUIAll.text = shooting.ammo.ToString();

        }
        if (weaponManager.weaponState == WeaponManager.WeaponState.uzi)
        {
            ammoUIMagazine.text = shooting.ammoUziCurent.ToString();

            ammoUIAll.text = shooting.ammoUziAll.ToString();
        }

        //ammoUIMagazine.text = shooting.ammo.ToString();

        //ammoUIAll.text = shooting.ammo.ToString();
    }
}
