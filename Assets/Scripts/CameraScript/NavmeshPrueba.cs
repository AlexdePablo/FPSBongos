using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshPrueba : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            irAlPlayer();
        }
    }

    private void irAlPlayer()
    {
       
        agent.destination = goal.position;
    }
}
