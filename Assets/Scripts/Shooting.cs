using UnityEngine;

public class Shooting : MonoBehaviour
{
    public static Shooting Instance;
    public Transform gunTransform;

    public bool canShoot = true;
    public int damageAmount = 20;
    public float range = 100f;
    public float shootingDelay = 0.5f;
    public float hitForce = 10f;

    public ParticleSystem[] shootParticles;
    public ParticleSystem bulletCollisionParticles;
    public LayerMask layerMask;

    public Transform mainCameraTransform;
    private float nextShootTime;

    public AudioSource shootSound;
    public AudioSource reloadGunSound;

    public float ammoPistol = 12;
    public float ammoPistolCurent;

    public float ammoUzi = 40;
    public float ammoUziAll = 400;
    public float ammoUziCurent;

    public float ammo;
    public float bulletSpeed =2000f;

    public WeaponManager weaponManager;

    // Add this line for the bullet prefab
    public GameObject bulletPrefab; // Assign this in the Inspector

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        ammo = ammoPistol;
        ammoUziCurent = ammoUzi;
        weaponManager = GameObject.Find("WeaponManager").GetComponent<WeaponManager>();
        mainCameraTransform = Camera.main.transform;
        nextShootTime = Time.time;
    }

    /// <summary>
    /// Function to perform shooting
    /// </summary>
    /// <param name="targetPosition">Target position for shooting</param>
    public void Shoot()
    {
        if (!canShoot || Time.time < nextShootTime)
            return;

        if (weaponManager.weaponState == WeaponManager.WeaponState.pistol && ammo > 0)
        {
            ShootBullet();
            ammo--;
        }
        else if (weaponManager.weaponState == WeaponManager.WeaponState.uzi && ammoUziCurent > 0)
        {
            ShootBullet();
            ammoUziCurent--;
        }
        else
        {
            ReloadGun();
        }
    }

    private void ShootBullet()
    {
        shootSound.Play();
        nextShootTime = Time.time + shootingDelay;

        foreach (var particle in shootParticles)
        {
            particle.Play();
        }

        // Calculate direction using Raycast
        RaycastHit hit;
        Vector3 direction = mainCameraTransform.forward; // Направление взгляда камеры

        // Check if we hit something within range
        if (Physics.Raycast(mainCameraTransform.position, direction, out hit, range, layerMask))
        {
            // If we hit something, set the direction to that point
            direction = (hit.point - gunTransform.position).normalized; // Направление к точке попадания

            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
             
        
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            //выключен рейкаст
            ////bulletCollisionParticles.transform.position = hit.point;
            ////bulletCollisionParticles.transform.LookAt(mainCameraTransform);
            ////bulletCollisionParticles.Play();

            ////Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
            ////if (hitRigidbody != null)
            ////{
            ////    Vector3 impulseDirection = hit.point - mainCameraTransform.position;
            ////    hitRigidbody.AddForce(impulseDirection.normalized * hitForce, ForceMode.Impulse);
            ////}
        }

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Apply force to the bullet in the calculated direction
        bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse +100); // Adjust speed as necessary

        Destroy(bullet, 5f); // Destroy the bullet after 5 seconds to clean up
    }

    public void ReloadGun()
    {
        reloadGunSound.Play();

        if (weaponManager.weaponState == WeaponManager.WeaponState.pistol)
        {
            ammo = ammoPistol;
        }

        if (weaponManager.weaponState == WeaponManager.WeaponState.uzi && ammoUziAll > 0)
        {
            ammoUziAll -= ammoUzi;
            ammoUziCurent = ammoUzi;
        }
    }
}