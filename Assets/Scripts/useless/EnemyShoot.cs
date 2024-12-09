using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform m_Target;
    public GameObject Ball;
    public float distanceShoot = 9f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z - m_Target.position.z    < distanceShoot)
        {
            Shoot();
        }
    }
    void Shoot()
    {

        if (Ball != null)
        {
            Ball.transform.Translate(Vector3.forward, m_Target);
        }
    }
}
