using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool hasStarted;

    public Transform destination;
    public List<Transform> navCheckPoints; // Checkpoints the player has to pass through to reach the end goal

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool destinationIsEndPoint;
    int currentPoint = 0;

    private void Start()

    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Debug Start
            StartPathFind();
        }

        if (HasReachedDestination() && hasStarted)
        {
            if (currentPoint + 1 <= navCheckPoints.Count)
            {
                agent.destination = navCheckPoints[currentPoint].position;
                currentPoint++;
            }
            else
                agent.destination = destination.position;
        }
    }

    public void StartPathFind()
    {
        hasStarted = true;
        if (navCheckPoints.Count >= 1)
        {
            agent.destination = navCheckPoints[currentPoint].position;
        }
        else
        {
            destinationIsEndPoint = true;
            agent.destination = destination.position;
        }
    }


    // Path debugger
    private void OnDrawGizmosSelected()
    {
        if (navCheckPoints.Count >= 1)
        {
            for (int i = 0; i < navCheckPoints.Count; i++)
            {
                Gizmos.color = Color.red;

                if (i == 0)
                {
                    Gizmos.DrawLine(transform.position, navCheckPoints[i].position);
                    if (navCheckPoints.Count > 1)
                        Gizmos.DrawLine(navCheckPoints[i].position, navCheckPoints[i + 1].position);
                }
                if (navCheckPoints.Count == 1)
                    Gizmos.DrawLine(navCheckPoints[i].position, destination.position);
                else if (i == navCheckPoints.Count - 1 && navCheckPoints.Count > 1)
                    Gizmos.DrawLine(navCheckPoints[i].position, destination.position);
                else
                    Gizmos.DrawLine(navCheckPoints[i].position, navCheckPoints[i + 1].position);

                Gizmos.DrawSphere(navCheckPoints[i].position, 0.2f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(destination.position, 0.2f);
            }
        }
    }

    public bool HasReachedDestination()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 0.6f)
            return true;
        return false;
    }

}
