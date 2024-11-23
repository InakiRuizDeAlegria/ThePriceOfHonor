using UnityEngine;
using System.Collections;

public class Enemigo : MonoBehaviour
{
    public SistemaOleadas sistemaOleadas;
    public float vidaMaxima = 100f;

    private float vidaActual;
    private bool estaMuerto = false;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        if (estaMuerto) return;

        estaMuerto = true;
        Debug.Log($"{gameObject.name} ha muerto.");

        StartCoroutine(DesactivarDespuesDeTiempo(2f));
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
