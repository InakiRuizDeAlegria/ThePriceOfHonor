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

    public List<Zona> zonasExteriores;
    public List<Zona> zonasInteriores;
    public GameObject prefabEnemigo;
    public int numeroInicialDeEnemigos = 3;
    public float tiempoEntreOleadas = 5f;
    public bool usarZonaInterior;

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
        int cantidadDeEnemigos = numeroInicialDeEnemigos + oleadaActual - 1;
        enemigosRestantes = cantidadDeEnemigos;
        estaIniciandoOleada = false;

        Debug.Log($"Iniciando oleada {oleadaActual} con {cantidadDeEnemigos} enemigos.");

        Zona zonaSeleccionada = SeleccionarZona();
        if (zonaSeleccionada != null)
        {
            GenerarEnemigosEnZona(zonaSeleccionada, cantidadDeEnemigos);
        }
    }

    Zona SeleccionarZona()
    {
        List<Zona> zonasDisponibles = usarZonaInterior && zonasInteriores.Count > 0 
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

    void GenerarEnemigosEnZona(Zona zona, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
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
