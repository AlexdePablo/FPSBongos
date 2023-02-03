using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables del Player")]
    [SerializeField]
    private Player m_Player;
    private bool puedeDodgear;

    [Header("Variables de la cámara")]
    [SerializeField]
    private Transform m_Camera;
    [SerializeField]
    private float m_Sensitivity = 5;
    private float angle;
    private float mousePositionX;
    private float mousePositionY;
    private bool isShooting;
    private float timerRecoil;
    private float pepe = 0;

    [Header("Variables de gravedad")]
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;

    [Header("Variables del nitroso")]
    private bool m_UsingNitro;
    float contadorP;
    private bool devolviendoNitro;

    [Header("Variables de los inputs")]
    Vector2 input;
    PLayerControls m_PlayerControls;

    [Header("Variables de estados")]
    private bool m_Corriendo;
    private bool m_RecoveringNitro;

    [Header("Variables de wallRun")]
    [SerializeField]
    private LayerMask whatIsWall;
    [SerializeField]
    private float wallCheckDistance;
    public bool wallRunning;

    [Header("GUI")]
    [SerializeField]
    public Slider m_Slider;
    [SerializeField]
    public GameObject panel;

    [Header("El RigidBody xd")]
    Rigidbody m_RigidBody;
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_PlayerControls = new PLayerControls();
        m_PlayerControls.Movement.Sprint.started += ACorrerLosLakers;
        m_PlayerControls.Movement.Sprint.canceled += QuietoParao;
        m_PlayerControls.Movement.Jump.started += YoDigoSaltaConmigo;
        m_PlayerControls.Movement.Jump.performed += aVolar;
        m_PlayerControls.Movement.Jump.canceled += seAcaboElSalto;
        m_PlayerControls.Movement.Crunch.started += ComoDodgea;
        m_PlayerControls.Arma.Arma.started += Dispar;
        m_PlayerControls.Arma.Arma.performed += Recover;
        m_PlayerControls.Arma.Arma.canceled += NoDispar;
        m_PlayerControls.Enable();
    }

    private void Dispar(InputAction.CallbackContext obj)
    {
        isShooting = true;
    }
    private void NoDispar(InputAction.CallbackContext obj)
    {
        isShooting = false;
    }
    private void Recover(InputAction.CallbackContext obj)
    {
        StartCoroutine("RecoverY");
    }

    // Start is called before the first frame update
    void Start()
    {
        puedeDodgear = true;
        m_RecoveringNitro = false;
        m_UsingNitro = false;
        m_RigidBody.useGravity = false;
        m_Player.nitroso = 100;
        m_Corriendo = false;
        //  wallRunning = false;
    }
    private void FixedUpdate()
    {

        //print(wallMovementDirection);
        if (m_UsingNitro && m_Player.nitroso > 0)
        {
            if (m_RigidBody.velocity.y == 0)
            {
                m_RigidBody.AddForce(0, m_Player.jumpForce, 0, ForceMode.Impulse);
            }
            else
            {
                m_Player.nitroso--;
            }
        }
        else
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            m_RigidBody.AddForce(gravity, ForceMode.Acceleration);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (m_Player.nitroso < 100)
            StartCoroutine("devolverNitro");
        m_Slider.value = m_Player.nitroso;
        //print("WallRunning: " + wallRunning);
        // CheckForWall();
        //doingWallRun();
        mousePositionX = m_PlayerControls.Movement.MouseX.ReadValue<float>() / 20;
        mousePositionY = m_PlayerControls.Movement.MouseY.ReadValue<float>() / 20;
        if (isShooting)
        {

            timerRecoil += Time.deltaTime;
            pepe += m_Player.recoil / 100;
            if (!m_Player.invertY)
                mousePositionY += m_Player.recoil / 100;
            else
                mousePositionY -= m_Player.recoil / 100;


        }
        if (mousePositionX != 0)
            transform.Rotate(Vector3.up * mousePositionX * m_Sensitivity);

        if (mousePositionY != 0)
        {
            if (!m_Player.invertY)
            {
                angle = (m_Camera.localEulerAngles.x - mousePositionY * m_Sensitivity + 360) % 360;
            }
            else
            {
                angle = (m_Camera.localEulerAngles.x - -mousePositionY * m_Sensitivity + 360) % 360;
            }
            if (angle > 180)
                angle -= 360;
            angle = Mathf.Clamp(angle, -80, 80);
            m_Camera.localEulerAngles = Vector3.right * angle;
        }
        input = m_PlayerControls.Movement.WASD.ReadValue<Vector2>();
        m_RigidBody.MovePosition(transform.position
            + input.x * transform.right * m_Player.velocity * Time.deltaTime
            + input.y * transform.forward * m_Player.velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();

        }
    }

    private void OnPause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        panel.SetActive(true);
        m_PlayerControls.Movement.Sprint.started -= ACorrerLosLakers;
        m_PlayerControls.Movement.Sprint.canceled -= QuietoParao;
        m_PlayerControls.Movement.Jump.started -= YoDigoSaltaConmigo;
        m_PlayerControls.Movement.Jump.performed -= aVolar;
        m_PlayerControls.Movement.Jump.canceled -= seAcaboElSalto;
        m_PlayerControls.Movement.Crunch.started -= ComoDodgea;
        m_PlayerControls.Disable();

    }
    public void OnResume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        panel.SetActive(false);
        m_PlayerControls.Movement.Sprint.started += ACorrerLosLakers;
        m_PlayerControls.Movement.Sprint.canceled += QuietoParao;
        m_PlayerControls.Movement.Jump.started += YoDigoSaltaConmigo;
        m_PlayerControls.Movement.Jump.performed += aVolar;
        m_PlayerControls.Movement.Jump.canceled += seAcaboElSalto;
        m_PlayerControls.Movement.Crunch.started += ComoDodgea;
        m_PlayerControls.Enable();

    }



    private void ComoDodgea(InputAction.CallbackContext obj)
    {
        if (m_Corriendo && m_Player.nitroso > 0 && puedeDodgear)
        {
            m_Player.nitroso -= 25;
            if (m_Player.nitroso < 0)
                m_Player.nitroso = 0;
            Vector3 DashForce = (m_RigidBody.transform.forward * m_Player.dashForce);
            m_RigidBody.AddForce(DashForce, ForceMode.Impulse);
            StartCoroutine("cooldownDodge");
            m_RecoveringNitro = true;
            m_UsingNitro = false;
        }
    }
    /* private void stopGravity()
     {
         this.gravityScale = 0;
     }*/
    private void seAcaboElSalto(InputAction.CallbackContext obj)
    {
        m_RecoveringNitro = true;
        m_UsingNitro = false;
    }
    private void aVolar(InputAction.CallbackContext obj)
    {
        m_UsingNitro = true;
        /*while (!m_Aterra && m_Nitroso > 0)
        {
            if (!m_UsingNitro)
            {
                StartCoroutine("GestionNitroso");
                m_RigidBody.AddForce(0, m_JumpForce / 10, 0, ForceMode.Acceleration);
            }
        }*/
    }
    private void QuietoParao(InputAction.CallbackContext obj)
    {
        m_Player.velocity -= 10;
        m_Corriendo = false;
    }
    private void ACorrerLosLakers(InputAction.CallbackContext obj)
    {
        m_Player.velocity += 10;
        m_Corriendo = true;
    }
    private void YoDigoSaltaConmigo(InputAction.CallbackContext obj)
    {
        m_RecoveringNitro = false;
        m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0, m_RigidBody.velocity.z);
    }
    /*private void doingWallRun()
    {
        if (wallRunning && (wallLeft || wallRight))
        {
            stopGravity();
            if (input.y != 0)
            {
                if (m_UsingNitro)
                    snapBackToReality2();
                m_RigidBody.velocity = new Vector3(0, 0, 0);
                if (wallLeft)
                {
                    wallMovementDirection = Vector3.Cross(m_RigidBody.transform.up, leftWallHit.normal).normalized;
                    m_RigidBody.MovePosition(transform.position + input.y * wallMovementDirection * m_Player.velocity * Time.deltaTime);
                }
                if (wallRight)
                {
                    wallMovementDirection = Vector3.Cross(m_RigidBody.transform.up, rightWallHit.normal).normalized;
                    m_RigidBody.MovePosition(transform.position + input.y * -wallMovementDirection * m_Player.velocity * Time.deltaTime);
                }
            }
        }
        if (!wallRunning)
            snapBackToReality();
    }
    private void snapBackToReality()
    {
        this.gravityScale = 1;
    }
    private void snapBackToReality2()
    {
        this.gravityScale = 1;
        if (wallRight)
        {
            m_RigidBody.AddForce(rightWallHit.normal.normalized, ForceMode.Impulse);
            m_RigidBody.AddForce(0, m_Player.jumpForce / 20, 0, ForceMode.Impulse);
        }
        if (wallLeft)
        {
            m_RigidBody.AddForce(leftWallHit.normal, ForceMode.Impulse);
            m_RigidBody.AddForce(0, m_Player.jumpForce / 20, 0, ForceMode.Impulse);
        }
    }
       private void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, m_RigidBody.transform.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallRight = Physics.Raycast(transform.position, -m_RigidBody.transform.right, out rightWallHit, wallCheckDistance, whatIsWall);
        //print(wallLeft + " " + wallRight);
    }*/
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            //wallRunning = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            //wallRunning = true;
        }
    }
    IEnumerator devolverNitro()
    {

        if (!devolviendoNitro)
        {
            devolviendoNitro = true;
            yield return new WaitForSeconds(.4f);
            contadorP = 0;
            while (m_RecoveringNitro && contadorP < 1)
            {
                contadorP += 0.15f;
                yield return new WaitForSeconds(.1f);
            }
            while (m_RecoveringNitro && m_Player.nitroso < 100)
            {
                m_Player.nitroso += .7f;
                yield return new WaitForFixedUpdate();
            }
            devolviendoNitro = false;
        }

    }
    IEnumerator RecoverY()
    {
        yield return new WaitForSeconds(0.1f);
        if (!m_Player.invertY)
            mousePositionY -= m_Player.recoil / 100;
        else
            mousePositionY += m_Player.recoil / 100;
    }

    IEnumerator cooldownDodge()
    {
        puedeDodgear = false;
        yield return new WaitForSeconds(1.5f);
        puedeDodgear = true;
      
    }

    internal void QuitarVida(int damage)
    {
        m_Player.hp -= damage;
    }
}
