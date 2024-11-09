using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Transform shootingPoint; // Точка, из которой будет производиться выстрел
    public float shootingDelay = 1f; // Задержка между выстрелами
    public float bulletSpeed = 40f; // Скорость пули
    public float range = 100f; // Дальность стрельбы
    public int damageAmount = 10; // Урон, наносимый пулей

    private float nextShootTime; // Время следующего выстрела
    private Transform playerTransform; // Трансформ игрока
    private bool canShoot = true;

    private void Start()
    {
        nextShootTime = Time.time; // Инициализация времени следующего выстрела
        playerTransform = FindObjectOfType<PlayerController>().transform; // Получаем трансформ игрока
    }

    private void Update()
    {
        if (canShoot)
        {
        // Проверка на возможность стрельбы и направление на игрока
        if (Time.time >= nextShootTime && CanSeePlayer())
        {
            Shoot();
        }

        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // Проверяем, что угол между вектором взгляда врага и направлением на игрока положительный (в пределах 90 градусов)
        return dotProduct > 0;
    }

    private void Shoot()
    {
        // Создание пули
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Направление стрельбы (в сторону игрока)
        Vector3 direction = (playerTransform.position - shootingPoint.position).normalized;
        bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse); // Применение силы к пуле

        // Установка времени следующего выстрела
        nextShootTime = Time.time + shootingDelay;

        // Уничтожение пули через 5 секунд для очистки сцены
        Destroy(bullet, 5f);
    }
    public void StopShoot()
    {
        canShoot = false;
    }
}