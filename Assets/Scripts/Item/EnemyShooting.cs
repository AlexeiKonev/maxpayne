using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ����
    public Transform shootingPoint; // �����, �� ������� ����� ������������� �������
    public float shootingDelay = 1f; // �������� ����� ����������
    public float bulletSpeed = 20f; // �������� ����
    public float range = 100f; // ��������� ��������
    public int damageAmount = 10; // ����, ��������� �����

    private float nextShootTime; // ����� ���������� ��������

    private void Start()
    {
        nextShootTime = Time.time; // ������������� ������� ���������� ��������
    }

    private void Update()
    {
        // �������� �� ����������� ��������
        if (Time.time >= nextShootTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // �������� ����
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // ����������� �������� (� ������� ������ ��� ����)
        Vector3 direction = (FindObjectOfType<PlayerController>().transform.position - shootingPoint.position).normalized;
        bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse); // ���������� ���� � ����

        // ��������� ������� ���������� ��������
        nextShootTime = Time.time + shootingDelay;

        // ����������� ���� ����� 5 ������ ��� ������� �����
        Destroy(bullet, 5f);
    }
}