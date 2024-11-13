using System.Collections;
using System.Collections.Generic;
  
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
  [SerializeField]
    private int currentHealth;

  public  EnemyShooting enemyShooting;

    void Start()
    {
        enemyShooting = GetComponent<EnemyShooting>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
            enemyShooting.StopShoot();
        }
    }

 public   void Die()
    {
        CursorManager.Instance.ShowDeadCursor(1f);
        // ������ �������� ������ � ������ ��������, ��������� � ������� �����
        GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 20f); // ���������� ������ ����� ����� 20 ������  ����� ������
    }
}
