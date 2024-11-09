using UnityEngine;
using UnityEngine.UI;

//public class Reloader : MonoBehaviour
//{
//    public enum GunsStates { pistol, shootgun }
//    public GunsStates GunState;

//    public Transform[] TransformGun;
//    public GameObject bulletObject;

//    public int ammoPistol = 5;
//    public int ammoPistolAll = 200;
//    private int ammoPistolReload = 15;

//    private int ammoShootgunReload = 7;

//    public int ammoShootgun = 7;
//    public int ammoShootgunlAll = 102;

//    public Text ammoCurentUI;
//    public Text ammoAllUI;

//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {


//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            ChoseWeapon(GunsStates.pistol);

//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            ChoseWeapon(GunsStates.shootgun);

//        }


//        if (Input.GetKeyDown(KeyCode.LeftAlt))
//        {
//            Shoot();
//        }

//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Reload();
//        }
//    }

//    private void Reload()
//    {
//        if (GunState == GunsStates.pistol)
//        {
//            ReloadPistol();
//            Debug.Log("reload pistol");

//        }

//        if (GunState == GunsStates.shootgun)
//        {
//            ReloadShootgun();
//            Debug.Log("reload   shotgun");


//        }

//    }

//    private void Shoot()
//    {
//        if (GunState == GunsStates.pistol)
//        {

//            ShootPistol(bulletObject);
//        }

//        if (GunState == GunsStates.shootgun)
//        {

//            ShootShotgun(bulletObject);


//        }
//    }

//    private void ChoseWeapon(GunsStates weapon)
//    {
//        if (weapon == GunsStates.pistol)
//        {
//            GunState = GunsStates.pistol;

//        }
//        if (weapon == GunsStates.shootgun)
//        {
//            GunState = GunsStates.shootgun;

//        }
//    }

//    public void ShootShotgun(GameObject bulletObject)
//    {
//        if (ammoShootgun > 0)
//        {
//            ammoShootgun -= 1;
//            Debug.Log("shoot shotgun");
//            foreach (Transform t in TransformGun)
//            {
//                GameObject.Instantiate(bulletObject, t.position, t.rotation);
//                UiUpdateAmmo(ammoShootgunlAll, ammoShootgun);
//            }
//        }

//    }
//    public void ShootPistol(GameObject bulletObject)
//    {
//        if (ammoPistol > 0)
//        {
//            ammoPistol -= 1;
//            Debug.Log("shoot");
//            GameObject.Instantiate(bulletObject, TransformGun[0].position, TransformGun[0].rotation);
//            UiUpdateAmmo(ammoPistolAll, ammoPistol);
//        }
//    }
//    public void ShootUzi(GameObject bulletObject)
//    {

//    }
//    public void ShootAk47(GameObject bulletObject)
//    {

//    }

//    public void ReloadPistol()
//    {
//        if (ammoPistolAll > 0)
//        {
//            ammoPistol =+ ammoPistolReload;
//            ammoPistolAll -= ammoShootgunReload;
//            UiUpdateAmmo(ammoPistolAll, ammoPistol);
//        }
//    }
//    public void ReloadShootgun()
//    {
//        if (ammoShootgunlAll > 0 )
//        {
//            ammoShootgun =+ ammoShootgunReload;
//            ammoShootgunlAll -= ammoShootgunReload;
//            UiUpdateAmmo(ammoShootgunlAll, ammoShootgun);
//        }
//    }

//    private void UiUpdateAmmo(int allAmmo, int curentAmmo)
//    {
//        ammoAllUI.text = allAmmo.ToString();
//        ammoCurentUI.text = curentAmmo.ToString();
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class Reloader : MonoBehaviour
{
    public enum GunsStates { pistol, shotgun }
    public GunsStates GunState;

    public Transform[] TransformGun;
    public GameObject bulletObject;

    public int ammoPistol = 5;
    public int ammoPistolAll = 200;
    private int maxAmmoPistol = 5;
    private int ammoPistolReload = 15;

    private int ammoShotgunReload = 7;

    public int ammoShotgun = 7;
    public int ammoShotgunAll = 102;
    private int maxAmmoShotgun = 7;

    public Text ammoCurrentUI;
    public Text ammoAllUI;

    void Start()
    {
        UiUpdateAmmo(ammoPistolAll, ammoPistol);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChoseWeapon(GunsStates.pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChoseWeapon(GunsStates.shotgun);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (GunState == GunsStates.pistol)
        {
            ReloadPistol();
            Debug.Log("reload pistol");
        }
        else if (GunState == GunsStates.shotgun)
        {
            ReloadShotgun();
            Debug.Log("reload shotgun");
        }
    }

    private void Shoot()
    {
        if (GunState == GunsStates.pistol)
        {
            ShootPistol(bulletObject);
        }
        else if (GunState == GunsStates.shotgun)
        {
            ShootShotgun(bulletObject);
        }
    }

    private void ChoseWeapon(GunsStates weapon)
    {
        GunState = weapon;
        if(GunsStates.pistol == GunState)
        {
            UiUpdateAmmo(ammoPistolAll, ammoPistol); // Обновляем UI при смене оружия
        }  
        if(GunsStates.shotgun == GunState)
        {
            UiUpdateAmmo(ammoShotgunAll, ammoShotgun); // Обновляем UI при смене оружия
        }
        
    }

    public void ShootShotgun(GameObject bulletObject)
    {
        if (ammoShotgun > 0)
        {
            ammoShotgun -= 1;
            Debug.Log("shoot shotgun");
            foreach (Transform t in TransformGun)
            {
                Instantiate(bulletObject, t.position, t.rotation);
                UiUpdateAmmo(ammoShotgunAll, ammoShotgun);
            }
        }
    }

    public void ShootPistol(GameObject bulletObject)
    {
        if (ammoPistol > 0)
        {
            ammoPistol -= 1;
            Debug.Log("shoot pistol");
            Instantiate(bulletObject, TransformGun[0].position, TransformGun[0].rotation);
            UiUpdateAmmo(ammoPistolAll, ammoPistol);
        }
    }

    public void ReloadPistol()
    {
        int neededAmmo = maxAmmoPistol - ammoPistol;

        if (ammoPistolAll > 0 && neededAmmo > 0)
        {
            int toReload = Mathf.Min(neededAmmo, Mathf.Min(ammoPistolReload, ammoPistolAll));

            ammoPistol += toReload;
            ammoPistolAll -= toReload;

            UiUpdateAmmo(ammoPistolAll, ammoPistol);
        }
    }

    public void ReloadShotgun()
    {
        int neededAmmo = maxAmmoShotgun - ammoShotgun;

        if (ammoShotgunAll > 0 && neededAmmo > 0)
        {
            int toReload = Mathf.Min(neededAmmo, Mathf.Min(ammoShotgunReload, ammoShotgunAll));

            ammoShotgun += toReload;
            ammoShotgunAll -= toReload;

            UiUpdateAmmo(ammoShotgunAll, ammoShotgun);
        }
    }

    private void UiUpdateAmmo(int allAmmo, int currentAmmo)
    {
        ammoAllUI.text = allAmmo.ToString();
        ammoCurrentUI.text = currentAmmo.ToString();
    }
}