using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public float danio = 25f;
    public float danioRecibidoPorAtaque = 5f;

    private BarraVidaArma barraVidaArma;

    private void Start()
    {
        barraVidaArma = GetComponent<BarraVidaArma>();
        if (barraVidaArma == null)
        {
            Debug.LogWarning("No se encontró el script BarraVidaArma en la espada.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemigo enemigo = other.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);

            if (barraVidaArma != null)
            {
                barraVidaArma.RecibirDanio(danioRecibidoPorAtaque);
            }

        }
    }
}