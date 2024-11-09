using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ����
    public Transform shootingPoint; // �����, �� ������� ����� ������������� �������
    public float shootingDelay = 1f; // �������� ����� ����������
    public float bulletSpeed = 40f; // �������� ����
    public float range = 100f; // ��������� ��������
    public int damageAmount = 10; // ����, ��������� �����

    private float nextShootTime; // ����� ���������� ��������
    private Transform playerTransform; // ��������� ������
    private bool canShoot = true;

    private void Start()
    {
        nextShootTime = Time.time; // ������������� ������� ���������� ��������
        playerTransform = FindObjectOfType<PlayerController>().transform; // �������� ��������� ������
    }

    private void Update()
    {
        if (canShoot)
        {
        // �������� �� ����������� �������� � ����������� �� ������
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

        // ���������, ��� ���� ����� �������� ������� ����� � ������������ �� ������ ������������� (� �������� 90 ��������)
        return dotProduct > 0;
    }

    private void Shoot()
    {
        // �������� ����
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // ����������� �������� (� ������� ������)
        Vector3 direction = (playerTransform.position - shootingPoint.position).normalized;
        bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse); // ���������� ���� � ����

        // ��������� ������� ���������� ��������
        nextShootTime = Time.time + shootingDelay;

        // ����������� ���� ����� 5 ������ ��� ������� �����
        Destroy(bullet, 5f);
    }
    public void StopShoot()
    {
        canShoot = false;
    }
}