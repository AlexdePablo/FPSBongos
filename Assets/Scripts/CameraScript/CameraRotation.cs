using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    Camera m_Camera;
    private float rotation;
    private bool crechendo;
    // Start is called before the first frame update
    void Start()
    {
        rotation = m_Camera.transform.localEulerAngles.y;
        crechendo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(crechendo)
        {
            rotation+=0.1f;
        }
        else
        {
            rotation-=0.1f;
        }
        if(rotation > 110)
        {
            crechendo = false;
        }
        if(rotation < 50)
        {
            crechendo= true;
        }
        Vector3 dalenen = Vector3.up * rotation * Time.timeScale;
        float y = dalenen.y;
        m_Camera.transform.localEulerAngles = new Vector3(48,y,0);

    }
}
