using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool canShoot = true;

    public int damageAmount = 20;
    public float range = 100f;
    public float shootingDelay = 0.5f; // Set to a reasonable delay (e.g., 0.5 seconds)
    public float hitForce = 10f; // Force applied on hit

    public ParticleSystem[] shootParticles;
    public ParticleSystem bulletCollisionParticles;
    public LayerMask layerMask;

    private Transform mainCameraTransform;
    private float nextShootTime; // Time when the player can shoot again

    public AudioSource shootSound;
    public AudioSource reloadGunSound;
    public float ammoPistol = 20;
    public float ammoPistolCurent;
    public float ammoUzi = 40;
    public float ammoUziCurent;
    public float ammo;

    public WeaponManager weaponManager;
    private void Start()
    {
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        mainCameraTransform = Camera.main.transform;
        nextShootTime = Time.time; // Initialize next shoot time
    }

    /// <summary>
    /// Function to perform shooting
    /// </summary>
    /// <param name="targetPosition">Target position for shooting</param>
    public void Shoot(Vector3 targetPosition)
    {
        if (weaponManager.weaponState == WeaponManager.WeaponState.pistol)
        { 
            ammoPistolCurent = ammo;
        
        }
        if (weaponManager.weaponState == WeaponManager.WeaponState.uzi) 
        {
            ammoUziCurent = ammo;
        }
            

        if (!canShoot || Time.time < nextShootTime) // Check if the player can shoot
            return;

        shootSound.Play();

        if (ammo > 0)
        {
            nextShootTime = Time.time + shootingDelay; // Set the next shoot time
            ammo--;

            foreach (var particle in shootParticles)
            {
                particle.Play();
            }

            Vector3 direction = (targetPosition - mainCameraTransform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(mainCameraTransform.position, direction, out hit, range, layerMask))
            {
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                }

                bulletCollisionParticles.transform.position = hit.point;
                bulletCollisionParticles.transform.LookAt(mainCameraTransform);
                bulletCollisionParticles.Play();

                Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
                if (hitRigidbody != null)
                {
                    Vector3 impulseDirection = hit.point - mainCameraTransform.position;
                    hitRigidbody.AddForce(impulseDirection.normalized * hitForce, ForceMode.Impulse);
                }
            }
        }
        else
        {
            ReloadGun(); // Reload if ammo is empty
        }
    }

    public void ReloadGun()
    {
        if (weaponManager.weaponState == WeaponManager.WeaponState.pistol)
        {
            reloadGunSound.Play();
            ammo = ammoPistol; // Reset ammo count
                               // Optionally add a delay for reloading

        }
        if (weaponManager.weaponState == WeaponManager.WeaponState.uzi)
        {
            reloadGunSound.Play();
            ammo = ammoUzi; // Reset ammo count
                            // Optionally add a delay for reloading

        }
    }
}