using UnityEngine;

public class Shooting : MonoBehaviour
{

    public bool canShoot = true;

    public int damageAmount = 20;

    public float range = 100f;
    public float shootingDelay = 10f;
    public float hitForce = 10f; // Сила импульса при попадании

    public ParticleSystem[] shootParticles;
    public ParticleSystem bulletCollisionParticles;
    public LayerMask layerMask;

    private Transform mainCameraTransform;
    private float currentTimeDelay;
    //public bool bulletEmpty = false;


    public AudioSource shootSound;
    public AudioSource reloadGunSound;
    public float ammo = 20;
    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    /// <summary>
    /// Функция, осуществляющая выстрел
    /// </summary>
    /// <param name="targetPosition"> Конечная точка стрельбы </param>
    public void Shoot(Vector3 targetPosition)
    {
        if (!canShoot) { shootSound.Stop(); return; }
        shootSound.Play();
        //ammo -= 1;
        if (Time.time >= currentTimeDelay)
        {
            currentTimeDelay = Time.time + 1 / shootingDelay;
            ammo -= 1;
            foreach (var particle in shootParticles)
            {
                if (ammo >= 0)
                {

                    particle.Play();

                }
                else
                {
                    ReloadGun();
                }
            }

            Vector3 direction = (targetPosition - mainCameraTransform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(mainCameraTransform.position, direction, out hit, range, layerMask))
            {
                // Проверяем, попали ли во врага
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    // Если попали во врага, наносим ему урон
                    enemy.TakeDamage(damageAmount);
                }

                bulletCollisionParticles.transform.position = hit.point;
                bulletCollisionParticles.transform.LookAt(mainCameraTransform);

                bulletCollisionParticles.Play();
                // Применяем импульс к объекту, по которому попал выстрел, чтобы он отталкивался от героя
                Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
                if (hitRigidbody != null)
                {
                    // Направление импульса - от героя к объекту попадания
                    Vector3 impulseDirection = hit.point - mainCameraTransform.position;
                    // Придаем импульс объекту
                    hitRigidbody.AddForce(impulseDirection.normalized * hitForce, ForceMode.Impulse);
                }

            }
        }
    }

    public void ReloadGun()
    {
        reloadGunSound.Play();
        ammo = 20;
        //bulletEmpty =false;
    }
}
