using System;
using UnityEngine;
using static Reloader;

public class Reloader : MonoBehaviour
{
    public enum GunsStates { pistol,shootgun}
    public GunsStates GunState;

    public Transform[] TransformGun;
    public GameObject bulletObject;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
        ChoseWeapon(GunsStates.pistol);

        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
        ChoseWeapon(GunsStates.shootgun);

        }


        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

           if( GunState== GunsStates.pistol) 
            {
            
            Debug.Log("shoot");
                GameObject.Instantiate(bulletObject, TransformGun[0].position,TransformGun[0].rotation);
            }

            if (GunState == GunsStates.shootgun)
            {

                Debug.Log("shoot shotgun");
                foreach(Transform t in TransformGun)
                {
                    GameObject.Instantiate(bulletObject, t.position, t.rotation);
                }

                
            }
        }
    }

    private void ChoseWeapon(GunsStates weapon)
    {
        if(weapon == GunsStates.pistol)
        {
        GunState = GunsStates.pistol;

        }
        if(weapon == GunsStates.shootgun)
        {
        GunState = GunsStates.shootgun;

        }
    }

    public void ShootShotgun(GameObject bulletObject)
    {

    } 
    public void ShootPistol(GameObject bulletObject)
    {

    } 
    public void ShootUzi(GameObject bulletObject)
    {

    } 
    public void ShootAk47(GameObject bulletObject)
    {

    }
}
