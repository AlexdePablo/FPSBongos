using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlideValue : MonoBehaviour
{
    [SerializeField]
    Player m_Player;
    [SerializeField]
    public Slider m_Slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Slider.value = m_Player.nitroso;
        print(m_Slider.value);
    }
}
