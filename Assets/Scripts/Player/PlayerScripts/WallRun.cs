using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Variables")]
    public LayerMask whatIsWall;
    private bool wallRight;
    private bool wallLeft;
    public float wallCheckDistance;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public Transform orientation;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        doingWallRun();
    }

    private void doingWallRun()
    {
        if(wallLeft)
        {

        }
        if(wallRight)
        {
           
        }
    }

    private void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallRight = Physics.Raycast(transform.position, -orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        print(wallLeft+" "+ wallRight);
    }
}
