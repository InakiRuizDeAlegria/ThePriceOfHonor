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
    private bool haAtacado = false;

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
        if (haSidoVisto || haAtacado)
        {
            IA.speed = Velocity;
        }
        else
        {
            IA.speed = velocidadMedico;
        }
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
        float distanciaAlJugador = Vector3.Distance(transform.position, target.position);
        IA.SetDestination(target.position);
        if (!haSidoVisto)
        {
            anim.SetBool("aSidoVisto", false);
            DetectarJugador();
        }
        if (distanciaAlJugador <= 2.0f)
        {
            anim.SetBool("estaAtacando", true);
            haAtacado = true;
            haSidoVisto = true;
        }
        else if (distanciaAlJugador > 2.0f)
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

    public void hitMedico()
    {
        //golpea siempre al jugador
    }

    void DetectarJugador()
    {
        if (Camera.main == null)
        {
            return;
        }

        Vector3 direccionAlMedico = transform.position - Camera.main.transform.position;

        float angulo = Vector3.Angle(Camera.main.transform.forward, direccionAlMedico);

        if (angulo < 60f)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, direccionAlMedico.normalized, out hit, 50f))
            {
                Debug.Log($"El raycast impactó en: {hit.collider.gameObject.name}");

                if (hit.collider.gameObject == this.gameObject)
                {
                    haSidoVisto = true;
                    anim.SetBool("aSidoVisto", true);
                    Debug.Log("El médico ha sido visto por el jugador");
                }
                else
                {
                    Debug.Log("El raycast impactó en otro objeto, no en el médico.");
                }
            }
        }
        else
        {
            Debug.Log("El médico está fuera del campo de visión del jugador.");
        }
    }



}
