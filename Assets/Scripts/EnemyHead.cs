using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
   public EnemyController enemyController;
    private void Awake()
    {
        enemyController = GameObject.Find("enemycontroller").GetComponent<EnemyController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
         
       
           if( collision.gameObject.tag == "Bullet")
            {
                Debug.Log("попал в голову");
                enemyController.Die();
                
            }
      
    }
}
