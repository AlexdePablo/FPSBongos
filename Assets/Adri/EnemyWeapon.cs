using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using Random = UnityEngine.Random;

public class EnemyWeapon : MonoBehaviour
{
    private Vector3 Position;
    [SerializeField]
    private Pool m_pool;
    private ArrayList m_Balas;
    public bool m_Shooting;
    private void Start()
    {
        m_Shooting = false;
        m_Balas = new ArrayList();
        Position = transform.localPosition;
    }
    private void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        transform.localPosition = Position;
    }
    public void Mirar(Vector3 point, float m_angularVelocity, int damage)
    {
        Double D = -(transform.up.x * transform.position.x + transform.up.y * transform.position.y + transform.up.z * transform.position.z);
        Double Point = transform.up.x * point.x + transform.up.y * point.y + transform.up.z * point.z + D;
        int num;
        if (Point > 0)
            num = -1;
        else if (Point < 0)
            num = 1;
        else
            num = 0;
        transform.Rotate(m_angularVelocity * num, 0, 0);
        if (!m_Shooting)
            StartCoroutine(Shooting(damage));
    }
    public void AnularVelocity()
    {
        transform.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    }
    private void Shoot(int damage)
    {
        RaycastHit hit;
        float rightDispersion = Random.Range(-0.1f, 0.1f);
        float upDispersion = Random.Range(-0.1f, 0.1f);
        Vector3 direction = transform.forward + rightDispersion * transform.right + upDispersion * transform.up;

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            hit.transform.GetComponent<PlayerMovement>().QuitarVida(damage);
        }
        else
        {
            GameObject go = m_pool.GetElement();
            if (go == null)
            {
                m_pool.ReturnElement((GameObject)m_Balas[0]);
                go = m_pool.GetElement();
                m_Balas.Remove(go);
            }
            go.transform.position = hit.point + hit.normal;
            go.transform.up = hit.normal;
            m_Balas.Add(go);
        }
    }
    private IEnumerator Shooting(int damage)
    {
        while (true)
        {
            Shoot(damage);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
