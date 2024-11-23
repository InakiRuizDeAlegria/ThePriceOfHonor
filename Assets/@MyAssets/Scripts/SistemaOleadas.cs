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
        public float probabilidad; // Para determinar la frecuencia de aparición
    }

    public List<Zona> zonasExteriores;
    public List<Zona> zonasInteriores;
    public List<ConfiguracionEnemigo> configuracionesEnemigos;

    public float tiempoEntreOleadas = 5f;

    private int oleadaActual = 0;
    private int enemigosRestantes = 0;
    private bool estaIniciandoOleada = false;

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
        estaIniciandoOleada = false;

        Debug.Log($"Iniciando oleada {oleadaActual}.");

        // Generar enemigos basados en configuraciones
        foreach (var configuracion in configuracionesEnemigos)
        {
            int cantidadDeEnemigos = configuracion.cantidadInicial + oleadaActual - 1;

            // Elegir la zona correspondiente (interior o exterior)
            Zona zonaSeleccionada = SeleccionarZona(configuracion.esInterior);

            if (zonaSeleccionada != null)
            {
                GenerarEnemigosEnZona(configuracion.prefabEnemigo, zonaSeleccionada, cantidadDeEnemigos, configuracion.probabilidad);
                enemigosRestantes += cantidadDeEnemigos;
            }
        }
    }

    Zona SeleccionarZona(bool esInterior)
    {
        List<Zona> zonasDisponibles = esInterior && zonasInteriores.Count > 0 
            ? zonasInteriores 
            : zonasExteriores;

        if (zonasDisponibles.Count == 0)
        {
            Debug.LogError("No hay zonas disponibles para generar enemigos.");
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
