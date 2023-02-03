using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    Pool m_Pool;
    public void SetPool(Pool pool)
    {
        m_Pool = pool;
    }

    public void ReturnToPool()
    {
        if (!m_Pool.ReturnElement(gameObject))
            Debug.LogError(gameObject + ": Poolable component not properly configured.");
    }
}
