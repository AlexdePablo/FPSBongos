using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gancho : MonoBehaviour
{

    [SerializeField]
    private LayerMask layerMask;

    private LineRenderer lr;

    private SpringJoint gancho;

    PLayerControls m_PlayerControls;

    private Vector3 grap;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        m_PlayerControls = new PLayerControls();
        m_PlayerControls.Arma.Gancho.performed += LanzarGancho;
        m_PlayerControls.Arma.Arma.started += LanzarGanchoAcercador;
        m_PlayerControls.Arma.Arma.started += DesactivarGancho;
        m_PlayerControls.Arma.Gancho.canceled += DesactivarGancho;
        m_PlayerControls.Enable();
    }
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void LanzarGancho(InputAction.CallbackContext obj)
    {
        //trobar un raig des de la cmera en direcci al punt que hem clicat d'aquesta
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);

        //llancem un raig segons la direcci trobada anteriorment per a saber qu toquem
        //des del punt de vista de l'objecte cmera
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, layerMask))
        {
            Debug.DrawLine(ray.origin, raycastHit.point, Color.blue, 5f);
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
            {
                //Un cop veiem que el que hem tocat des la cmera s un objecte del tipus walkable
                //fem que el player llenci un raig en direcci al PUNT on la cmera ha tocat
                Vector3 direction = (raycastHit.point - transform.position).normalized;
                Debug.DrawLine(transform.position, transform.position + direction * 20f, Color.red, 5f);
                if (Physics.Raycast(transform.position, direction, out raycastHit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawLine(transform.position, raycastHit.point, Color.green, 5f);
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
                    {
                        grap = raycastHit.point;
                        gancho = this.gameObject.AddComponent<SpringJoint>();

                        gancho.autoConfigureConnectedAnchor = false;

                        gancho.connectedAnchor = grap;

                        float distancia = Vector3.Distance(transform.position, grap);

                        gancho.maxDistance = distancia * 0.8f;
                        gancho.minDistance = distancia * 0.2f;

                        gancho.spring = 4.5f;
                        gancho.damper = 8f;
                        gancho.massScale = 4.5f;


                    }
                }
            }
        }

    }
    public void LanzarGanchoAcercador(InputAction.CallbackContext obj)
    {
        //trobar un raig des de la cmera en direcci al punt que hem clicat d'aquesta
        Vector2 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);

        //llancem un raig segons la direcci trobada anteriorment per a saber qu toquem
        //des del punt de vista de l'objecte cmera
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, layerMask))
        {
            Debug.DrawLine(ray.origin, raycastHit.point, Color.blue, 5f);
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
            {
                //Un cop veiem que el que hem tocat des la cmera s un objecte del tipus walkable
                //fem que el player llenci un raig en direcci al PUNT on la cmera ha tocat
                Vector3 direction = (raycastHit.point - transform.position).normalized;
                Debug.DrawLine(transform.position, transform.position + direction * 20f, Color.red, 5f);
                if (Physics.Raycast(transform.position, direction, out raycastHit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawLine(transform.position, raycastHit.point, Color.green, 5f);
                    if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("wall"))
                    {
                        grap = raycastHit.point;
                        gancho = this.gameObject.AddComponent<SpringJoint>();

                        gancho.autoConfigureConnectedAnchor = false;

                        gancho.connectedAnchor = grap;
                        rb.velocity = (grap - rb.position).normalized * 50 * Time.timeScale;
                        float distancia = Vector3.Distance(transform.position, grap);

                        gancho.maxDistance = distancia * 0.8f;
                        gancho.minDistance = distancia * 0.2f;

                        gancho.spring = 4.5f;
                        gancho.damper = 8f;
                        gancho.massScale = 4.5f;


                    }
                }
            }
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();

        }
    }
    private void FixedUpdate()
    {
        DibujarGancho();
    }
    public void DibujarGancho()
    {
        if (!gancho) return;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, grap);
    }
    public void DesactivarGancho(InputAction.CallbackContext obj)
    {
        Destroy(gancho);
    }
    private void OnPause()
    {
        m_PlayerControls.Arma.Gancho.performed -= LanzarGancho;
        m_PlayerControls.Arma.Gancho.canceled -= DesactivarGancho;
        m_PlayerControls.Disable();

    }
    public void OnResume()
    {
        m_PlayerControls = new PLayerControls();
        m_PlayerControls.Arma.Gancho.performed += LanzarGancho;
        m_PlayerControls.Arma.Gancho.canceled += DesactivarGancho;
        m_PlayerControls.Enable();

    }
}
