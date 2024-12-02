using UnityEngine;
using System.Collections;

public class Enemigo : MonoBehaviour
{
    public SistemaOleadas sistemaOleadas;
    public float vidaMaxima = 100f;
    public string capaCorte = "Sliceable";

    private float vidaActual;
    private bool estaMuerto = false;
    private bool habilitarCorte = false;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;

        if (vidaActual <= 0 && !habilitarCorte)
        {
            CambiarACapaSliceable();
            habilitarCorte = true;
            Morir();
        }
    }

    void Morir()
    {
        if (estaMuerto) return;

        estaMuerto = true;
        
        StartCoroutine(DesactivarDespuesDeTiempo(0.1f));
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

    IEnumerator DesactivarDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        Destroy(gameObject);
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
