using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DenEnemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform Target;
    void Start()
    {
        Agent.destination = Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
