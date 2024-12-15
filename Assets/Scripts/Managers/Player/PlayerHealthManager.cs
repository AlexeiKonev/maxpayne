using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{

    public int PlayerMaxHealth = 100;
    public int CurentHealth;
    public GameObject DeadSignObj;

    void Start()
    {
        DeadSignObj.SetActive(false);
        CurentHealth = PlayerMaxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (CurentHealth <= 0)
        {
            Die();
        }
    }



    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

        TakeDamage();

        }

    }


    private void Die()
    {
        Debug.Log("помер");
        DeadSignObj.SetActive(true);
    }
    public void TakeDamage()
    {
        CurentHealth -= 10;
    }
}
