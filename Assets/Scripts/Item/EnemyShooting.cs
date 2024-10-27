using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public Transform shootingPoint; // Точка, из которой будет производиться выстрел
    public float shootingDelay = 1f; // Задержка между выстрелами
    public float bulletSpeed = 20f; // Скорость пули
    public float range = 100f; // Дальность стрельбы
    public int damageAmount = 10; // Урон, наносимый пулей

    private float nextShootTime; // Время следующего выстрела

    private void Start()
    {
        nextShootTime = Time.time; // Инициализация времени следующего выстрела
    }

    private void Update()
    {
        // Проверка на возможность стрельбы
        if (Time.time >= nextShootTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Создание пули
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Направление стрельбы (в сторону игрока или цели)
        Vector3 direction = (FindObjectOfType<PlayerController>().transform.position - shootingPoint.position).normalized;
        bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse); // Применение силы к пуле

        // Установка времени следующего выстрела
        nextShootTime = Time.time + shootingDelay;

        // Уничтожение пули через 5 секунд для очистки сцены
        Destroy(bullet, 5f);
    }
}