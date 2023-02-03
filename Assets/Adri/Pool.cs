using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PoolableElement;
    [SerializeField]
    private int m_Size;
    private List<GameObject> m_Pool;
    
    void Awake()
    {
        if (!m_PoolableElement.GetComponent<Poolable>())
        {
            Debug.LogError(gameObject + ": Poolable element without a Poolable component");
            Destroy(this);
        }
        m_Pool = new List<GameObject>();
        for (int i = 0; i < m_Size; ++i)
        {
            GameObject element = Instantiate(m_PoolableElement, transform);
            element.GetComponent<Poolable>().SetPool(this);
            element.SetActive(false);
            m_Pool.Add(element);
        }
    }

    public bool ReturnElement(GameObject element)
    {
        if (m_Pool.Contains(element))
        {
            element.SetActive(false);
            return true;
        }
        return false;
    }

    public GameObject GetElement()
    {
        foreach(GameObject element in m_Pool)
        {
            if (!element.activeInHierarchy)
            {
                element.SetActive(true);
                return element;
            }
        }
        return null;
    }
}
