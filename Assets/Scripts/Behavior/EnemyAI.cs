using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float distanceOfStop = 4f;
    public Transform targetAI;
    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //Error handling: Check if the target and agent are assigned.
        if (targetAI == null)
        {
            Debug.LogError("Target AI not assigned to Enemy AI!");
            enabled = false; //Disable the script if target is missing.
            return;
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on Enemy AI!");
            enabled = false; //Disable the script if agent is missing.
            return;
        }
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetAI.position);

        if (distanceToTarget <= distanceOfStop)
        {
            agent.isStopped = true; 
            transform.LookAt(targetAI);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetAI.position);
            
        }
    }
}