using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porton : MonoBehaviour
{
    public int vidaMaxima = 5000;
    public static Porton instancia;
    private int vidaActual;

    private void Awake()
    {
        instancia = this;
    }

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
        if (vidaActual > 0)
        {
            ReactivarPorton();
        }
    }

    private void DesactivarPorton()
    {
        gameObject.SetActive(false);
    }

    private void ReactivarPorton()
    {
        gameObject.SetActive(true);
    }

    public int ObtenerVidaActual()
    {
        return vidaActual;
    }

    public bool EstaActivo()
    {
        return gameObject.activeInHierarchy;
    }

}
