using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    public EnemyAI enemyAi;
    public EnemyShooting enemyShooting;

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

    public void Die()
    {
        CursorManager.Instance.ShowDeadCursor(1f);
        // Запуск анимации смерти и другие действия, связанные с смертью врага
        GetComponent<Animator>().SetTrigger("Die");
        enemyAi.enabled = false;
        Destroy(gameObject, 20f); // Уничтожаем объект врага через 20 секунд  после смерти
    }
}
