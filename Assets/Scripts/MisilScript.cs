using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilScript : MonoBehaviour
{
    [SerializeField]
    float m_Velocity = 3;
    private Rigidbody m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.velocity = transform.forward * m_Velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Agafar els punts de contacte
        List<ContactPoint> contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        foreach (ContactPoint contact in contacts)
        {
            Debug.DrawLine(contact.point, contact.point + contact.normal, Color.red, 5f);
        }

        //Agafem el primer punt de contacte
        ContactPoint contacte = collision.GetContact(0);
        Vector3 nouForward = Vector3.Reflect(transform.forward, contacte.normal);
        transform.forward = nouForward;
        m_Rigidbody.velocity = transform.forward * m_Velocity;
    }

}
