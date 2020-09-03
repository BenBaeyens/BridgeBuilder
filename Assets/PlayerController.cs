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
        destination = GameObject.Find("Point").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Debug Start
            StartPathFind();
        }
    }

    public void StartPathFind()
    {
        agent.destination = destination.position;
        hasStarted = true;
    }

}
