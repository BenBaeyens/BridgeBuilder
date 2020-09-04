using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Transform destination;
    public NavMeshAgent agent;
    public bool hasStarted;

    private void Start()

    {
        destination = GameObject.FindObjectOfType<LocalNavMeshBuilder>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Debug Start
            StartPathFind();
        }

        if (Vector3.Distance(transform.position, destination.position) < 0.1f)
        {
            Debug.LogWarning("YOU WON");
        }
    }

    public void StartPathFind()
    {
        agent.destination = destination.position;
        hasStarted = true;
    }

}
