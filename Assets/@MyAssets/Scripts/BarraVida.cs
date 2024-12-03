using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public float vidadMax;

    private float vidaActual;

    void Start()
    {
        vidaActual = vidadMax;
        //barraVida.GetComponent<Image>().color = new Color(70, 210, 50);
    }

    void Update()
    {
        barraVida.fillAmount = vidaActual / vidadMax;
        //cambiarColor();
    }

    /*private void cambiarColor()
    {
        if (vidaActual >= 60)
        {
            barraVida.GetComponent<Image>().color = new Color(70, 210, 50);
        }
        else if (vidaActual <= 30)
        {
            barraVida.GetComponent<Image>().color = new Color(210, 40, 50);
        }
        else
        {
            barraVida.GetComponent<Image>().color = new Color(220, 120, 30);
        }
    }*/

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
