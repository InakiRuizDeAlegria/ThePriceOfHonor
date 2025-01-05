using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaOleadas : MonoBehaviour
{
    [System.Serializable]
    public class Zona
    {
        public string nombreZona;
        public Transform[] puntosSpawn;
    }

    [System.Serializable]
    public class ConfiguracionEnemigo
    {
        public GameObject prefabEnemigo;
        public bool esInterior;
        public int cantidadInicial;
        public float probabilidad;
        public int nivelRequerido;
    }

    public List<Zona> zonasExteriores;
    public List<Zona> zonasInteriores;
    public List<ConfiguracionEnemigo> configuracionesEnemigos;

    public float tiempoEntreOleadas = 5f;
    public int dineroPorOleada = 100;

    private int oleadaActual = 0;
    private int nivelActual = 1;
    private int enemigosRestantes = 0;
    private bool estaIniciandoOleada = false;

    public MenuManagerTexto tienda;
    public comprarArmas tiendaArmas;
    public ComprarArmadura tiendaArmaduras;
    public ComprarDefensas tiendaDefensas;

    public AudioSource audioSource;
    public AudioClip audioInicioOleada;

    void Start()
    {
        IniciarOleada();
    }

    void Update()
    {
        if (enemigosRestantes <= 0 && !estaIniciandoOleada)
        {
            estaIniciandoOleada = true;
            StartCoroutine(IniciarSiguienteOleada());
        }
    }

    void IniciarOleada()
    {
        oleadaActual++;
        if (oleadaActual % 5 == 1 && oleadaActual > 1)
        {
            nivelActual++;
        }

        estaIniciandoOleada = false;

        ReproducirAudioInicioOleada();

        foreach (var configuracion in configuracionesEnemigos)
        {
            if (configuracion.nivelRequerido > nivelActual)
                continue;

            int cantidadDeEnemigos = (nivelActual >= configuracion.nivelRequerido)
                ? configuracion.cantidadInicial + (oleadaActual - configuracion.nivelRequerido)
                : configuracion.cantidadInicial;

            Zona zonaSeleccionada = SeleccionarZona(configuracion.esInterior);

            if (zonaSeleccionada != null)
            {
                GenerarEnemigosEnZona(configuracion.prefabEnemigo, zonaSeleccionada, cantidadDeEnemigos, configuracion.probabilidad);
                enemigosRestantes += cantidadDeEnemigos;
            }
        }

        int dineroGanado = dineroPorOleada * oleadaActual;
        tienda.dineroTotal += dineroGanado;
        tiendaArmas.ActualizarDineroUI();
        tiendaArmaduras.ActualizarDineroUI();
        tiendaDefensas.ActualizarDineroUI();
    }

    void ReproducirAudioInicioOleada()
    {
        if (audioSource != null && audioInicioOleada != null)
        {
            audioSource.PlayOneShot(audioInicioOleada);
        }
    }

    Zona SeleccionarZona(bool esInterior)
    {
        List<Zona> zonasDisponibles = esInterior && zonasInteriores.Count > 0 
            ? zonasInteriores 
            : zonasExteriores;

        if (zonasDisponibles.Count == 0)
        {
            return null;
        }

        int indiceAleatorio = Random.Range(0, zonasDisponibles.Count);
        return zonasDisponibles[indiceAleatorio];
    }

    void GenerarEnemigosEnZona(GameObject prefabEnemigo, Zona zona, int cantidad, float probabilidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            float aleatorio = Random.value;
            if (aleatorio > probabilidad) continue;

            Transform puntoSpawn = zona.puntosSpawn[Random.Range(0, zona.puntosSpawn.Length)];
            Vector3 posicionAleatoria = puntoSpawn.position + new Vector3(
                Random.Range(-1f, 1f), 
                0, 
                Random.Range(-1f, 1f));

            GameObject enemigo = Instantiate(prefabEnemigo, posicionAleatoria, Quaternion.identity);

            EnemyInteligent enemigoScript = enemigo.GetComponent<EnemyInteligent>();
            if (enemigoScript != null)
            {
                enemigoScript.portonObject = GameObject.FindWithTag("Porton");
                enemigoScript.porton = enemigoScript.portonObject?.GetComponent<Porton>();
                enemigoScript.target = GameObject.FindWithTag("Player").transform;
                enemigoScript.IA = enemigo.GetComponent<UnityEngine.AI.NavMeshAgent>();
                enemigoScript.anim = enemigo.GetComponentInChildren<Animator>();
            }

            enemigo.GetComponent<Enemigo>()?.AsociarSistemaOleadas(this);
        }
    }

    public void EnemigoEliminado()
    {
        enemigosRestantes--;
    }

    IEnumerator IniciarSiguienteOleada()
    {
        yield return new WaitForSeconds(tiempoEntreOleadas);
        IniciarOleada();
    }
}
