using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class CamaraVigilancia : MonoBehaviour
{
    [SerializeField]
    Camera cam1;
    [SerializeField]
    Camera cam2;
    PLayerControls player_input;
    // Start is called before the first frame update
    void Start()
    {
        player_input = new PLayerControls();
        player_input.Enable();
        player_input.Arma.CanviCamara.started += ChangeCamera;
        cam1.enabled = true;
        cam2.enabled = false;
    }

    private void ChangeCamera(InputAction.CallbackContext obj)
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();

        }
    }
    private void OnPause()
    {
        player_input.Disable();
        player_input.Arma.CanviCamara.started -= ChangeCamera;

    }
    public void OnResume()
    {
        player_input = new PLayerControls();
        player_input.Enable();
        player_input.Arma.CanviCamara.started += ChangeCamera;

    }
}
