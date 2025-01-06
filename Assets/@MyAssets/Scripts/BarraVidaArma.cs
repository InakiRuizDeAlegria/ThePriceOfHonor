using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaArma : MonoBehaviour
{
    public Image barraVida;
    public float vidaMax;

    private float vidaActual;

    public GameObject arma;

    void Start()
    {
        vidaActual = vidaMax;

        if (arma == null)
        {
            arma = gameObject;
        }
    }

    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMax;
    }

    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual <= 0)
        {
            vidaActual = 0;
            DestruirArma();
        }
    }

    public void RepararArma()
    {
        vidaActual = vidaMax;
    }

    void DestruirArma()
    {
        Destroy(arma);
    }
}
