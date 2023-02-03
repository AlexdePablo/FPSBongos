using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    [SerializeField]
    private int fuerza;
    Vector3 a;
    Rigidbody rb;
    void Start()
    {
        a = (transform.forward ) * fuerza;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(a, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pepe = transform.up * -9.81f * 3;
        rb.AddForce (pepe, ForceMode.Acceleration);
    }
}
