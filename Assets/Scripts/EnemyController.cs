using System.Collections;
using System.Collections.Generic;
  
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
  [SerializeField]
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ������ �������� ������ � ������ ��������, ��������� � ������� �����
        GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 20f); // ���������� ������ ����� ����� 20 ������  ����� ������
    }
}
