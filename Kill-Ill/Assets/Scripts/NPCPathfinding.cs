using UnityEngine;
using UnityEngine.AI;

public class NPCPathfinding : MonoBehaviour
{
    NavMeshAgent agent;
    
    int tick = 0;
    bool wander = true;
    public Transform destinationPoint;
    public int wanderRadius = 20;
    public int movementChance = 5;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        //wander around after a random interval, set movement chance to 1 to always wander
        if (!wander)
            return;

        tick++;
        if (tick == 100 && wander)
        {
            tick = 0;
            if (Random.Range(0, movementChance) == 0)
            {
                destinationPoint.localPosition = new Vector3(Random.Range(-wanderRadius, wanderRadius), 0, Random.Range(-wanderRadius, wanderRadius));
                agent.SetDestination(destinationPoint.position);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.DrawSphere(destinationPoint.position, 0.5f);
    }
    public void SetWander(bool state)
    {
        wander = state;
        agent.isStopped = !state;

    }

    public void StartInteraction()
    {
        SetWander(false);
    }

    public void EndInteraction()
    {
        SetWander(true);
    }
}
