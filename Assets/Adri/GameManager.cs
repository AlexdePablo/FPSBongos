using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Pool m_Pool;
    void Start()
    {
        m_Pool = GetComponent<Pool>();
    }
    public static void returnEnemy(GameObject o)
    {
        m_Pool.ReturnElement(o);
    }
    public static void SpawnEnemy(Vector3 t)
    {
        GameObject o = m_Pool.GetElement();
        o.transform.position = t;
    }
}
