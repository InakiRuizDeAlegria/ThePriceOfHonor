using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaOleadas : MonoBehaviour
{
    [System.Serializable]
    public class Zona
    {
        public string nombreZona;
        public Transform puntoDeGeneracion;
        public GameObject prefabEnemigo;
    }

    public List<Zona> zonas;
    public int numeroInicialDeEnemigos = 3;
    public float tiempoEntreOleadas = 5f;

    private int oleadaActual = 0;
    private int enemigosRestantes = 0;

    void Start()
    {
        IniciarOleada();
    }

    void Update()
    {
        if (enemigosRestantes <= 0)
        {
            StartCoroutine(IniciarSiguienteOleada());
        }
    }

    void IniciarOleada()
    {
        oleadaActual++;
        int cantidadDeEnemigos = numeroInicialDeEnemigos + oleadaActual - 1;
        enemigosRestantes = cantidadDeEnemigos;

        Debug.Log($"Iniciando oleada {oleadaActual} con {cantidadDeEnemigos} enemigos.");

        foreach (Zona zona in zonas)
        {
            GenerarEnemigosEnZona(zona, cantidadDeEnemigos);
        }
    }

    void GenerarEnemigosEnZona(Zona zona, int cantidad)
{
    if (zona.puntoDeGeneracion == null || zona.prefabEnemigo == null)
    {
        return;
    }

    for (int i = 0; i < cantidad; i++)
    {
        Vector3 posicionAleatoria = zona.puntoDeGeneracion.position + new Vector3(
            Random.Range(-2f, 2f), 
            0, 
            Random.Range(-2f, 2f));
        
        GameObject enemigo = Instantiate(zona.prefabEnemigo, posicionAleatoria, Quaternion.identity);

        EnemyInteligent enemigoScript = enemigo.GetComponent<EnemyInteligent>();
        if (enemigoScript != null)
        {
            enemigoScript.portonObject = GameObject.FindWithTag("Porton");
            enemigoScript.porton = enemigoScript.portonObject?.GetComponent<Porton>();
            enemigoScript.target = GameObject.FindWithTag("Player").transform;
            enemigoScript.IA = enemigo.GetComponent<UnityEngine.AI.NavMeshAgent>();
            enemigoScript.anim = enemigo.GetComponentInChildren<Animator>();
            enemigo.GetComponent<Enemigo>().AsociarSistemaOleadas(this);
        }
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
