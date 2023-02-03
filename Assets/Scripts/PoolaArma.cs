using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PoolaArma : MonoBehaviour
{
    public GameObject PoolableObject;
    public int Capacity;


    private List<GameObject> m_Pool;

    public void Rellenar()
    {

        //en aquest exemple exigim que els elements siguin poolables
        if (!PoolableObject.GetComponent<PoolableArma>())
        {
            Debug.LogError(gameObject + ": Poolable element without a Poolable component");
            Destroy(this);
        }
        m_Pool = new List<GameObject>();
        for (int i = 0; i < Capacity; ++i)
        {
            GameObject element = Instantiate(PoolableObject, transform);
            element.GetComponent<PoolableArma>().SetPool(this);
            element.SetActive(false);
            m_Pool.Add(element);
        }

    }

    public void Vaciar() {

        for (int i = 0; i < Capacity; ++i)
        {
            Destroy(m_Pool[i]);
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
        foreach (GameObject element in m_Pool)
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
