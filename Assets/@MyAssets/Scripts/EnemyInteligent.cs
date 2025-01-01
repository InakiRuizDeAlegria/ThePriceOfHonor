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
    private GameObject objetivoActual;

    public AudioSource audioSource;
    public AudioClip sonidoGolpe;

    private static List<EnemyInteligent> enemigosAtacandoPorton = new List<EnemyInteligent>();
    public float distanciaEspera = 5f;
    private bool estaEsperando = false;

    void Start()
    {
        porton = Porton.instancia;
        if (jugador == null)
        {
            GameObject xrOriginObject = GameObject.FindObjectOfType<XROrigin>()?.gameObject;
            if (xrOriginObject != null)
            {
                jugador = xrOriginObject.GetComponent<XROrigin>();
                target = jugador.transform;
            }
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

    }

    void Update()
    {
        if (CompareTag("medico"))
        {
             DetectarJugador();
             GestionarMedico();

             IA.speed = escondido || haSidoVisto ? Velocity : velocidadMedico;
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
            if (!enemigosAtacandoPorton.Contains(this) && enemigosAtacandoPorton.Count < 3)
            {
                enemigosAtacandoPorton.Add(this);
                estaEsperando = false;
            }

            if (enemigosAtacandoPorton.Contains(this) && enemigosAtacandoPorton.IndexOf(this) < 3)
            {
                IA.SetDestination(portonObject.transform.position);
                objetivoActual = portonObject;

                // Si está quieto, ataca
                bool estaQuieto = IA.velocity.sqrMagnitude < 0.01f;
                anim.SetBool("estaAtacando", estaQuieto);
                anim.SetBool("esperando", false);
            }
            else
            {
                if (!estaEsperando)
                {
                    Vector3 direccionEspera = (transform.position - portonObject.transform.position).normalized;
                    Vector3 posicionEspera = portonObject.transform.position + direccionEspera * distanciaEspera;
                    IA.SetDestination(posicionEspera);
                    objetivoActual = null;
                    estaEsperando = true;
                }

                bool estaQuieto = IA.velocity.sqrMagnitude < 0.01f;
                anim.SetBool("esperando", estaQuieto);
                anim.SetBool("estaAtacando", false);
            }
        }
        else
        {
            IA.SetDestination(target.position);
            objetivoActual = target.gameObject;

            // Comprobar si el aldeano está cerca del jugador
            float distanciaAlJugador = Vector3.Distance(transform.position, target.position);
            bool estaCerca = distanciaAlJugador < 2f; // Ajusta la distancia según sea necesario

            if (estaCerca)
            {
                // Si está cerca y quieto, activar la animación de atacar
                bool estaQuieto = IA.velocity.sqrMagnitude < 0.01f;
                anim.SetBool("estaAtacando", estaQuieto);
                anim.SetBool("esperando", false);
            }
            else
            {
                // Si está lejos, asegurarse de que esté corriendo
                anim.SetBool("estaAtacando", false);
                anim.SetBool("esperando", false);
            }
        }
    }

    void GestionarMedico()
    {
        if (!haSidoVisto)
        {
            anim.SetBool("aSidoVisto", false);
            IA.SetDestination(target.position);
            objetivoActual = target.gameObject;
        }
        else
        {
            Vector3 direccionAlJugador = transform.position - Camera.main.transform.position;
            if (direccionAlJugador.magnitude <= 5f)
            {
                IA.SetDestination(target.position);
                anim.SetBool("aSidoVisto", true);
                escondido = false;
                objetivoActual = target.gameObject;
            }
            else
            {
                BuscarEscondite();
            }
        }

        anim.SetBool("estaAtacando", IA.velocity == Vector3.zero);
    }


    public void Hit()
    {

        if (audioSource != null && sonidoGolpe != null)
        {
            audioSource.PlayOneShot(sonidoGolpe);
        }

        if (objetivoActual != null)
        {
            if (objetivoActual.CompareTag("Player"))
            {
                BarraVida barraVidaJugador = objetivoActual.GetComponent<BarraVida>();
                if (barraVidaJugador != null)
                {
                    barraVidaJugador.RecibirDanio(10);
                }
            }
            else if (objetivoActual.CompareTag("Porton"))
            {
                Porton portonComponent = objetivoActual.GetComponent<Porton>();
                if (portonComponent != null)
                {
                    portonComponent.RecibirDanio(20);
                }
            }
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
                        IA.SetDestination(target.position);
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

    private void OnDestroy()
    {
        enemigosAtacandoPorton.Remove(this);
    }

}


