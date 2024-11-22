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

    private bool haSidoVisto = false;

    void Start()
    {
        porton = Porton.instancia;
    }

    void Update()
    {
        if (CompareTag("aldeano"))
        {
            IA.speed = Velocity;
            GestionarAldeano();
        }
        else if (CompareTag("medico"))
        {
            IA.speed = velocidadMedico;
            GestionarMedico();
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
            DetectarJugador();
        }
        else
        {
            anim.SetBool("aSidoVisto", true);
            IA.SetDestination(target.position);
        }

        if (IA.velocity == Vector3.zero && haSidoVisto)
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
        Vector3 direccionAlJugador = Camera.main.transform.position - transform.position;
        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

        if (angulo < 60f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direccionAlJugador.normalized, out hit, 20f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    haSidoVisto = true;
                    anim.SetBool("aSidoVisto", true);
                }
            }
        }
    }
}
