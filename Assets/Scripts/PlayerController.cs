using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool hasStarted;

    public Transform destination;
    public List<Transform> navCheckPoints; // Checkpoints the player has to pass through to reach the end goal
    public string MoveableBlockTag = "Block"; // The tag the blocks use

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool destinationIsEndPoint;
    int currentPoint = 0;
    BlockScript[] moveableBlocks;

    private void Start()

    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        moveableBlocks = GameObject.FindObjectsOfType<BlockScript>();
        destination = GameObject.FindObjectOfType<LocalNavMeshBuilder>().transform;
        GameObject[] tempNavCheckPoints = GameObject.FindGameObjectsWithTag("NavMarker");
        navCheckPoints.Clear();
        for (int i = 0; i < tempNavCheckPoints.Length; i++)
        {
            navCheckPoints.Add(tempNavCheckPoints[i].transform);
        }
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
            {
                destinationIsEndPoint = true;
                agent.destination = destination.position;
            }
        }

        OnPlatformCheck();
    }

    public void StartPathFind()
    {
        agent.enabled = true;
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
            // Find navPoints
            navCheckPoints.Clear();
            destination = GameObject.FindObjectOfType<LocalNavMeshBuilder>().transform;

            GameObject[] tempNavCheckPoints = GameObject.FindGameObjectsWithTag("NavMarker");
            for (int i = 0; i < tempNavCheckPoints.Length; i++)
            {
                navCheckPoints.Add(tempNavCheckPoints[i].transform);
            }

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

    // Check if the player is on a platform.
    void OnPlatformCheck()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            for (int i = 0; i < moveableBlocks.Length; i++)
            {
                moveableBlocks[i].hasPlayerOnTop = false;
            }

            if (hit.transform.CompareTag(MoveableBlockTag))
            {
                hit.transform.GetComponent<BlockScript>().hasPlayerOnTop = true;
            }

        }
    }
}
