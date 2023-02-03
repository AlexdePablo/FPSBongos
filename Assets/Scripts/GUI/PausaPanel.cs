using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausaPanel : MonoBehaviour
{
    [SerializeField]
    public GameObject panel;
    [SerializeField]
    public GameObject botonInvertir;
    [SerializeField]
    Player m_Player;
    [SerializeField]
    GameEvent reloadInput;
    public void InvertirY()
    {
        if(m_Player.invertY)
            m_Player.invertY = false;
        else
            m_Player.invertY = true;
    }
    public void Resume()
    {
        reloadInput.Raise();
    }
}
