using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInteligent : MonoBehaviour
{
    public Transform target;
    public float Velocity;
    public float velocidadMedico;
    public NavMeshAgent IA;
    public Animator anim;
    public GameObject portonObject;
    public XROrigin jugador;
    public Porton porton;
    public float rangoDeteccion = 80f;
    public float anguloDeteccion = 60f;
    public float distanciaEscondite = 800f;

    private bool haSidoVisto = false;
    private bool escondido = false;
    private Transform esconditeActual;

    void Start()
    {
        porton = Porton.instancia;
    }

    void Update()
    {
        if (CompareTag("medico"))
        {
            DetectarJugador();
                GestionarMedico();
            if (!escondido)
            {
                IA.speed = velocidadMedico;
            }
            else if (escondido)
            {
                IA.speed = Velocity;
            }
            else if (haSidoVisto)
            {
                IA.speed = Velocity;
            }
            else if (!haSidoVisto)
            {
                IA.speed = velocidadMedico;
            }
            else
            {
                IA.speed = Velocity;
            }
        }
        else if (CompareTag("aldeano"))
        {
            IA.speed = Velocity;
            GestionarAldeano();
        }
    }

    void GestionarAldeano()
    {
        if (porton != null && porton.EstaActivo())
        {
            IA.SetDestination(portonObject.transform.position);
        }
        else
        {
            IA.SetDestination(target.position);
        }

        if (IA.velocity == Vector3.zero)
        {
            anim.SetBool("estaAtacando", true);
        }
        else
        {
            anim.SetBool("estaAtacando", false);
        }
    }

    void GestionarMedico()
    {
        if (!haSidoVisto)
        {
            anim.SetBool("aSidoVisto", false);
            IA.SetDestination(target.position);
        }
        else
        {
            Vector3 direccionAlJugador = transform.position - Camera.main.transform.position;
            if (direccionAlJugador.magnitude <= 5f)
            {
                IA.SetDestination(Camera.main.transform.position);
                anim.SetBool("aSidoVisto", true);
            }
            else
            {
                BuscarEscondite();
            }
        }

        if (IA.velocity == Vector3.zero)
        {
            anim.SetBool("estaAtacando", true);
        }
        else
        {
            anim.SetBool("estaAtacando", false);
        }
    }


    public void Hit()
    {
        if (jugador.CompareTag("Player"))
        {
            BarraVida barraVidaJugador = jugador.GetComponent<BarraVida>();
            if (barraVidaJugador != null)
            {
                Debug.Log("Daño");
                barraVidaJugador.RecibirDanio(10);
            }
        }
        else if (porton != null)
        {
            porton.RecibirDanio(20);
        }
    }


    void DetectarJugador()
    {
        if (Camera.main == null) return;

        Vector3 direccionAlMedico = transform.position - Camera.main.transform.position;
        float angulo = Vector3.Angle(Camera.main.transform.forward, direccionAlMedico);

        if (angulo < anguloDeteccion && direccionAlMedico.magnitude < rangoDeteccion)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, direccionAlMedico.normalized, out hit, rangoDeteccion))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    haSidoVisto = true;
                    anim.SetBool("aSidoVisto", true);

                    if (direccionAlMedico.magnitude <= 5f)
                    {
                        IA.SetDestination(Camera.main.transform.position);
                        anim.SetBool("aSidoVisto", true);
                        escondido = false;
                    }
                }
            }
        }
    }


    void BuscarEscondite()
    {
        Collider[] posiblesEscondites = Physics.OverlapSphere(transform.position, distanciaEscondite);
        Transform mejorEscondite = null;
        float distanciaMinima = Mathf.Infinity;
        Vector3 puntoOculto = Vector3.zero;

        foreach (Collider col in posiblesEscondites)
        {
            if (col.CompareTag("Refugio"))
            {
                Vector3 direccionEscondite = col.transform.position - Camera.main.transform.position;
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.transform.position, direccionEscondite.normalized, out hit, direccionEscondite.magnitude))
                {
                    if (hit.collider.transform != col.transform)
                    {
                        Vector3 direccionOpuesta = col.transform.position - Camera.main.transform.position;
                        puntoOculto = col.transform.position + direccionOpuesta.normalized * 2f;

                        float distancia = Vector3.Distance(transform.position, puntoOculto);
                        if (distancia < distanciaMinima)
                        {
                            distanciaMinima = distancia;
                            mejorEscondite = col.transform;
                        }
                    }
                }
            }
        }

        if (mejorEscondite != null)
        {
            escondido = true;
            esconditeActual = mejorEscondite;
            anim.SetBool("aSidoVisto", true);
            IA.SetDestination(puntoOculto);
        }
        else
        {
            escondido = false;
            Vector3 direccionAleatoria = transform.position + Random.insideUnitSphere * 10f;
            anim.SetBool("aSidoVisto", true);
            IA.SetDestination(direccionAleatoria);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (escondido && other.CompareTag("Refugio") && other.transform == esconditeActual)
        {
            haSidoVisto = false;
            escondido = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Hit();
        }
    }
}


