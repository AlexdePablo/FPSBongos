using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private bool m_InvertY = false;
    PLayerControls m_PlayerControls;
    float inputX;
    float inputY;
    [SerializeField]
    private float m_Sensitivity = 5;
    private Vector2 angle = new Vector2(90 * Mathf.Deg2Rad, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
       
        m_PlayerControls = new PLayerControls();
       
        m_PlayerControls.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        float mousePositionX = (m_PlayerControls.Movement.MouseX.ReadValue<float>()/20);

        // float hor = mousePosition.x;

        if (mousePositionX != 0)
        {
            angle.x += mousePositionX * Mathf.Deg2Rad * m_Sensitivity;
        }

        float mousePositionY = (m_PlayerControls.Movement.MouseY.ReadValue<float>()/20);

        if (mousePositionY != 0)
        {
            if(m_InvertY)
                angle.y += mousePositionY * Mathf.Deg2Rad * m_Sensitivity;
            else
                angle.y -= mousePositionY * Mathf.Deg2Rad * m_Sensitivity;
            angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);

        }

        /*
        inputX = m_PlayerControls.Movement.MouseX.ReadValue<float>();
        inputY = m_PlayerControls.Movement.MouseY.ReadValue<float>();
        float deltaY = (m_InvertY ? 1 : -1) * inputY * m_Sensitivity * Time.deltaTime;
        float a = transform.localEulerAngles.x + deltaY;
        a = Mathf.Clamp(a, 0, 90);
        print(a);
        transform.Rotate(Vector3.right * a);*/
    }
    private void FixedUpdate()
    {
        Vector3 orbit = new Vector3(
            Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
            -Mathf.Sin(angle.y),
            -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
        );
        transform.rotation = Quaternion.LookRotation(orbit);
    }
}
