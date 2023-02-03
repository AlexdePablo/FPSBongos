using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SimpleAgent : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    private Rigidbody m_rigidbody;
    private float m_Vida;
    private float m_rangeVision;
    private float m_rageRandom;
    private float m_angularVelocity;
    private int Damage;
    [SerializeField] //KK test *
    private EnemyScriptable m_Test; //KK test *

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_Agent.SetDestination(RandomNavmeshLocation(m_rageRandom));
        LoadScriptable(m_Test); //KK test *
    }
    private void Update()
    {
        RaycastHit hit;
        bool Sphere = Physics.SphereCast(transform.position, m_rangeVision, transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Player"), QueryTriggerInteraction.UseGlobal);
        if (Sphere)
        {
            bool line = Physics.Raycast(transform.position, (hit.point - transform.position).normalized, out hit, Mathf.Infinity);
            if (line && hit.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            {
                m_Agent.SetDestination(hit.point);
                m_Agent.isStopped = true;
                Double D = -(transform.right.x * transform.position.x + transform.right.y * transform.position.y + transform.right.z * transform.position.z);
                Double Point = transform.right.x * hit.point.x + transform.right.y * hit.point.y + transform.right.z * hit.point.z + D;
                int num;
                if (Point > 0)
                    num = 1;
                else if (Point < 0)
                    num = -1;
                else
                    num = 0;
                transform.Rotate(0, m_angularVelocity * num, 0);
                transform.GetChild(0).GetComponent<EnemyWeapon>().Mirar(hit.point, m_angularVelocity, Damage);
                transform.GetChild(0).GetComponent<EnemyWeapon>().m_Shooting = true;
            }
            else
            {
                m_rigidbody.angularVelocity = Vector3.zero;
                m_Agent.isStopped = false;
                transform.GetChild(0).GetComponent<EnemyWeapon>().AnularVelocity();
                transform.GetChild(0).GetComponent<EnemyWeapon>().m_Shooting = false;
                transform.GetChild(0).GetComponent<EnemyWeapon>().StopAllCoroutines();
            }
        }
        else
        {
            m_rigidbody.angularVelocity = Vector3.zero;
            m_Agent.isStopped = false;
            transform.GetChild(0).GetComponent<EnemyWeapon>().AnularVelocity();
            transform.GetChild(0).GetComponent<EnemyWeapon>().m_Shooting = false;
            transform.GetChild(0).GetComponent<EnemyWeapon>().StopAllCoroutines();
        }
        if (ReachedDestinationOrGaveUp(m_Agent))
            m_Agent.SetDestination(RandomNavmeshLocation(15f));
    }
    public void LoadScriptable(EnemyScriptable enemy)
    {
        m_Vida = enemy.vida;
        m_rageRandom = enemy.rangeRandom;
        m_rangeVision = enemy.rangeVision;
        m_angularVelocity = enemy.angularVelocity;
        Damage = enemy.Damage;
    }
    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    private bool ReachedDestinationOrGaveUp(NavMeshAgent Agent)
    {

        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void hit(float f)
    {
        m_Vida -= f;
        if (m_Vida <= 0)
        {
            GameManager.returnEnemy(gameObject);
            GameManager.SpawnEnemy(RandomNavmeshLocation(m_rageRandom));
        }
    }
}