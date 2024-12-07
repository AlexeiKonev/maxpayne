using UnityEngine;

public class DenEnemyController : MonoBehaviour
{
    public int Health = 100;
    public GameObject enemyObj;
    
    void Start()
    {

    }


    void Update()
    {
        if (Health <= 0)
        {
            Destroy(enemyObj);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Health = Health - 20;
            Debug.Log($"health {Health}");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Health = Health - 20;
            Debug.Log($"health {Health}");
        }
    }
}
