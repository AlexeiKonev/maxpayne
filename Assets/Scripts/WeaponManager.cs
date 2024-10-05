using UnityEngine;

[System.Serializable]
public class WeaponArray
{
    public GameObject[] weapons; // Array for each "row" of your 2D array
}

public class WeaponManager : MonoBehaviour
{
    [Header("одноручное прицеливание")]
    [SerializeField] private PlayerController playerControllerSingleArmWeapon;
    [Header("двуручное прицеливание")]
    [SerializeField] private PlayerController playerControllerDualArmWeapon;

    public enum WeaponState { pistol, uzi }
    public WeaponState weaponState;

    public WeaponArray[] weaponsObjects; // Array of WeaponArray for each "column"

    void Start()
    {
        weaponState = WeaponState.pistol; // Start with the pistol
        Debug.Log("Current weapon: " + weaponState); // Log initial weapon state

        UpdateWeaponVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for input to switch weapons
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1' for pistol
        {
            weaponState = WeaponState.pistol;
            playerControllerSingleArmWeapon.enabled = true;
            playerControllerDualArmWeapon.enabled = false;
            UpdateWeaponVisibility();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press '2' for uzi
        {
            weaponState = WeaponState.uzi;
            playerControllerSingleArmWeapon.enabled = false;
            playerControllerDualArmWeapon.enabled = true;
            UpdateWeaponVisibility();
        }
    }

    private void UpdateWeaponVisibility()
    {
        // Deactivate all weapons first
        foreach (var row in weaponsObjects)
        {
            foreach (var weapon in row.weapons)
            {
                if (weapon != null)
                {
                    weapon.SetActive(false);
                }
            }
        }

        // Activate the current weapon based on its state
        switch (weaponState)
        {
            case WeaponState.pistol:
                if (weaponsObjects.Length > 0 && weaponsObjects[0].weapons.Length > 0)
                {
                    weaponsObjects[0].weapons[0].SetActive(true); // Assuming pistol is at [0][0]
                    weaponsObjects[0].weapons[1].SetActive(true); // Assuming pistol is at [0][1]
                    weaponsObjects[0].weapons[2].SetActive(true); // Assuming pistol is at [0][1]
                    Debug.Log("Switched to pistol");
                }
                break;
            case WeaponState.uzi:
                if (weaponsObjects.Length > 1 && weaponsObjects[1].weapons.Length > 0)
                {
                    weaponsObjects[1].weapons[0].SetActive(true); // Assuming uzi is at [1][0]
                    weaponsObjects[1].weapons[1].SetActive(true); // Assuming uzi is at [1][1]
                    weaponsObjects[1].weapons[2].SetActive(true); // Assuming uzi is at [1][1]
                    Debug.Log("Switched to uzi");
                }
                break;
            default:
                Debug.Log("Unknown weapon state");
                break;
        }
    }
}