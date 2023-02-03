using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableArma : MonoBehaviour
{
    private PoolaArma m_Owner;

    public void SetPool(PoolaArma owner)
    {
        m_Owner = owner;
    }
}
