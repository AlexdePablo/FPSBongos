using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArma : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    private LineRenderer lr;

    private SpringJoint ganchou;


    private Vector3 grap;
    private Rigidbody rb;

    [SerializeField]
    private GameObject bala;
    [SerializeField]
    private GameObject balaNoRayCast;
    [SerializeField]
    private Transform cañon;
    PLayerControls p;
    private bool disparar = true;
    private bool disparando = false;
    [SerializeField]
    private PoolaArma m_pool;
    [SerializeField]
    private List<GameObject> balas;

    private int con = 0;

    private Arma armaActiva;

    [SerializeField]
    private Arma arma1;
    [SerializeField]
    private Arma arma2;
    [SerializeField]
    private Arma gancho;



    // Start is called before the first frame update
    private void Awake()
    {
        armaActiva = arma1;
        if (armaActiva is ArmaNoRayCastSO)
        {
            m_pool.PoolableObject = balaNoRayCast;
            m_pool.Capacity = armaActiva.capacidad;
            m_pool.Rellenar();
            print(armaActiva.nombre);
        }
        else
        {
            m_pool.Capacity = armaActiva.capacidad;
            m_pool.Rellenar();
            print(armaActiva.nombre);
        }
     

    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();  
        Cursor.lockState = CursorLockMode.Locked;
        p = new PLayerControls();
        p.Arma.Arma.started+= Shoot;
        p.Arma.Arma.canceled += terminateShoot;
        p.Arma.Recargar.started += Recargar;
        p.Arma.CambiarArma1.started += CambiarArma1;
        p.Arma.CambiarArma2.started += CambiarArma2;
        p.Arma.CambiarArma3.started += CambiarArma3;
        p.Enable();

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
                        ganchou = this.gameObject.AddComponent<SpringJoint>();

                        ganchou.autoConfigureConnectedAnchor = false;

                        ganchou.connectedAnchor = grap;

                        float distancia = Vector3.Distance(transform.position, grap);

                        ganchou.maxDistance = distancia * 0.8f;
                        ganchou.minDistance = distancia * 0.2f;

                        ganchou.spring = 4.5f;
                        ganchou.damper = 8f;
                        ganchou.massScale = 4.5f;


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
                        ganchou = this.gameObject.AddComponent<SpringJoint>();

                        ganchou.autoConfigureConnectedAnchor = false;

                        ganchou.connectedAnchor = grap;
                        rb.velocity = (grap - rb.position).normalized * 50 * Time.timeScale;
                        float distancia = Vector3.Distance(transform.position, grap);

                        ganchou.maxDistance = distancia * 0.8f;
                        ganchou.minDistance = distancia * 0.2f;

                        ganchou.spring = 4.5f;
                        ganchou.damper = 8f;
                        ganchou.massScale = 4.5f;


                    }
                }
            }
        }

    }
    void Update()
    {
        m_pool.Capacity = armaActiva.capacidad;
      
    }

    public void CambiarArma1(InputAction.CallbackContext obj) {
        if (armaActiva == arma1)
        {
            return;
        }
        else {
            m_pool.Vaciar();
            armaActiva = arma1;
            if (armaActiva is ArmaNoRayCastSO)
            {
                m_pool.PoolableObject = balaNoRayCast;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }
            else if (armaActiva == gancho)
            {
                p.Arma.Arma.started -= LanzarGancho;
                p.Arma.Arma.canceled -= DesactivarGancho;
                p.Arma.Gancho.performed -= LanzarGanchoAcercador;
                p.Arma.Gancho.canceled -= DesactivarGancho;

                m_pool.Vaciar();
                armaActiva = arma1;

                if (armaActiva is ArmaNoRayCastSO)
                {
                    m_pool.PoolableObject = balaNoRayCast;
                    m_pool.Capacity = armaActiva.capacidad;
                    m_pool.Rellenar();
                    print(armaActiva.nombre);
                }
                else
                {
                    m_pool.PoolableObject = bala;
                    m_pool.Capacity = armaActiva.capacidad;
                    m_pool.Rellenar();
                    print(armaActiva.nombre);
                }


                p.Arma.Arma.started += Shoot;
                p.Arma.Arma.canceled += terminateShoot;
            }
            else
                m_pool.Vaciar();
            armaActiva = arma1;
            if (armaActiva is ArmaNoRayCastSO)
            {
                m_pool.PoolableObject = balaNoRayCast;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }
            else
            {
                m_pool.PoolableObject = bala;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }

        }
    }
    public void CambiarArma3(InputAction.CallbackContext obj) {
        if (armaActiva == gancho)
        {
            return;
        }
        else
        {
            p.Arma.Arma.started -= Shoot;
            p.Arma.Arma.canceled -= terminateShoot;
            m_pool.Vaciar();
            armaActiva = gancho;
            p.Arma.Arma.started += LanzarGancho;
            p.Arma.Arma.canceled += DesactivarGancho;
            p.Arma.Gancho.performed += LanzarGanchoAcercador;
            p.Arma.Gancho.canceled += DesactivarGancho;
        }
        }
    public void CambiarArma2(InputAction.CallbackContext obj) {

        if (armaActiva == arma2)
        {
            return;
        }
        else if (armaActiva == gancho) {
            p.Arma.Arma.started -= LanzarGancho;
            p.Arma.Arma.canceled -= DesactivarGancho;
            p.Arma.Gancho.performed -= LanzarGanchoAcercador;
            p.Arma.Gancho.canceled -= DesactivarGancho;

            m_pool.Vaciar();
            armaActiva = arma2;

            if (armaActiva is ArmaNoRayCastSO)
            {
                m_pool.PoolableObject = balaNoRayCast;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }
            else
            {
                m_pool.PoolableObject = bala;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }


            p.Arma.Arma.started += Shoot;
            p.Arma.Arma.canceled += terminateShoot;
        }
        else {
            m_pool.Vaciar();
            armaActiva = arma2;
            if (armaActiva is ArmaNoRayCastSO)
            {
                m_pool.PoolableObject = balaNoRayCast;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }
            else
            {
                m_pool.PoolableObject = bala;
                m_pool.Capacity = armaActiva.capacidad;
                m_pool.Rellenar();
                print(armaActiva.nombre);
            }

        }


    }

    public void Shoot(InputAction.CallbackContext obj) {
        disparando = true;
        if (armaActiva is ArmaRayCastSo && disparar)
        {
            StartCoroutine(cadencia());
        }
        else if (armaActiva is ArmaNoRayCastSO && disparar) {

            ShootMissile();

        }
     
      
    
    }

    public void Recargar(InputAction.CallbackContext obj)
    {
        foreach (GameObject bala in balas)
        {
            m_pool.ReturnElement(bala);
        }
        con = 0;
    }

    public void RecargarAuto()
    {
        foreach (GameObject bala in balas)
        {
            m_pool.ReturnElement(bala);
        }
        con = 0;
    }
    public void terminateShoot(InputAction.CallbackContext obj)
    {
        disparando = false;
    }
    [SerializeField]
    private LayerMask layermask;
    IEnumerator cadencia()
    {


        print("hola");
        while (disparando)
        {
            disparar = false;
            RaycastHit hit;

            float rightDispersion = Random.Range(-0.1f, 0.1f);
            float upDispersion = Random.Range(-0.1f, 0.1f);
            Vector3 direction = cañon.transform.forward + rightDispersion * cañon.transform.right + upDispersion * cañon.transform.up;

            if (Physics.Raycast(cañon.position, direction, out hit, Mathf.Infinity, layermask) && con != armaActiva.capacidad)
            {
                GameObject go = m_pool.GetElement();
                go.transform.position = hit.point + hit.normal ;
                go.transform.up = hit.normal;
                balas.Add(go);
                con++;
                print(con);

            }





            yield return new WaitForSeconds(armaActiva.cadencia);
            disparar = true;
                
        }

    }
    private void ShootMissile()
    {
        if (con != armaActiva.capacidad) {
            GameObject go = m_pool.GetElement();
            go.transform.position = cañon.transform.position + cañon.transform.forward * 2;
            go.transform.forward = cañon.transform.forward;
            con++;
            balas.Add(go);
        }
     
    }
    private void FixedUpdate()
    {
        DibujarGancho();
    }
    public void DibujarGancho()
    {
        if (!ganchou) return;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, grap);
    }
    public void DesactivarGancho(InputAction.CallbackContext obj)
    {
        Destroy(ganchou);
    }


    IEnumerator cadenciaNoRayCast() {

        while (disparando) {

            yield return new WaitForSeconds(armaActiva.cadencia);

        }
    
    }

    private void OnPause()
    {
        p.Arma.Gancho.performed -= LanzarGancho;
        p.Arma.Gancho.canceled -= DesactivarGancho;
        p.Disable();

    }
    public void OnResume()
    {
        p = new PLayerControls();
        p.Arma.Gancho.performed += LanzarGancho;
        p.Arma.Gancho.canceled += DesactivarGancho;
        p.Enable();

    }

}
