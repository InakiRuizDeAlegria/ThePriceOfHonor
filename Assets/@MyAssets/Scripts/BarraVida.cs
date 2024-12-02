using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public float vidaActual;
    public float vidadMax;

    void Update()
    {
        barraVida.fillAmount = vidaActual / vidadMax;
        
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0)
        {
            vidaActual = 0;
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("El jugador ha muerto");
    }
}
