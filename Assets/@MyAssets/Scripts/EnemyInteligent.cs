using System.Collections;
using System.Collections.Generic;
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
    public Porton porton;
    public float rangoDeteccion = 80f;
    public float anguloDeteccion = 60f;
    public float distanciaEscondite = 80f;

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
            if (!escondido)
            {
                IA.speed = velocidadMedico;
                DetectarJugador();
                GestionarMedico();
            }
            else
            {
                IA.speed = Velocity;
                DetectarJugador();
                GestionarMedico();
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
            BuscarEscondite();
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

    public void hit()
    {
        if (porton != null)
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
                }
            }
        }
    }

    void BuscarEscondite()
    {
        Collider[] posiblesEscondites = Physics.OverlapSphere(transform.position, distanciaEscondite);
        Transform mejorEscondite = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider col in posiblesEscondites)
        {
            if (col.CompareTag("Refugio"))
            {
                float distancia = Vector3.Distance(transform.position, col.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    mejorEscondite = col.transform;
                }
            }
        }

        if (mejorEscondite != null)
        {
            escondido = true;
            esconditeActual = mejorEscondite;
            IA.SetDestination(esconditeActual.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("El médico ha llegado al escondite.");
        if (escondido && collision.collider.CompareTag("Refugio") && collision.transform == esconditeActual)
        {
            haSidoVisto = false;
            escondido = false;
            Debug.Log("El médico ha llegado al escondite.");
        }
    }

}
