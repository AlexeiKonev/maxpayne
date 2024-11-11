using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
 public   Transform targetAI;
    public NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); agent.SetDestination(targetAI.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
