using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porton : MonoBehaviour
{
    public int vidaMaxima = 5000;
    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            DesactivarPorton();
        }
    }

    public void Reparar(int cantidad)
    {
        vidaActual += cantidad;
        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
    }

    private void DesactivarPorton()
    {
        gameObject.SetActive(false);
    }

    public int ObtenerVidaActual()
    {
        return vidaActual;
    }
}
