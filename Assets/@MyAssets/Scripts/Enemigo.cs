using UnityEngine;
using System.Collections;

public class Enemigo : MonoBehaviour
{
    public SistemaOleadas sistemaOleadas;
    public float vidaMaxima = 100f;
    public string capaCorte = "Sliceable";
    public Animator animador;

    private float vidaActual;
    private bool estaMuerto = false;
    private bool habilitarCorte = false;
    private bool haSidoCortado = false;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void Cortar()
    {
        if (estaMuerto) return;

        haSidoCortado = true;
        estaMuerto = true;

        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }

        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        EnemyInteligent enemigoInteligente = GetComponent<EnemyInteligent>();
        if (enemigoInteligente != null)
        {
            enemigoInteligente.enabled = false;
        }
    }

    public void RecibirDanio(float cantidad)
    {
        if (estaMuerto || haSidoCortado) return;

        vidaActual -= cantidad;
        Debug.Log(vidaActual);

        if (vidaActual <= 0 && !habilitarCorte)
        {
            CambiarACapaSliceable();
            habilitarCorte = true;
            Morir();
        }
    }

    void Morir()
    {
        if (estaMuerto || haSidoCortado) return;

        estaMuerto = true;
        
        if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }
    
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        EnemyInteligent enemigoInteligente = GetComponent<EnemyInteligent>();
        if (enemigoInteligente != null)
        {
            enemigoInteligente.enabled = false;
        }

        animador.SetBool("estaMuriendo", true);
        StartCoroutine(ProcesarMuerte());
    }

    IEnumerator ProcesarMuerte()
    {
        yield return new WaitForSeconds(3f);

        float duracion = 2f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            float escala = Mathf.Lerp(1f, 0f, tiempo / duracion);
            transform.localScale = Vector3.one * escala;

            yield return null;
        }
        Destroy(gameObject);
    }

    void CambiarACapaSliceable()
    {
        int sliceableLayer = LayerMask.NameToLayer(capaCorte);

        if (sliceableLayer == -1)
        {
            return;
        }

        CambiarLayerRecursivamente(gameObject, sliceableLayer);
    }

    void CambiarLayerRecursivamente(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            CambiarLayerRecursivamente(child.gameObject, layer);
        }
    }

    public void AsociarSistemaOleadas(SistemaOleadas sistema)
    {
        sistemaOleadas = sistema;
    }

    void OnDestroy()
    {
        if (sistemaOleadas != null)
        {
            sistemaOleadas.EnemigoEliminado();
        }
    }
}
