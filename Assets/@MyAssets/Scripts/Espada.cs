using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espada : MonoBehaviour
{
    public float danio = 25f;

    private void OnTriggerEnter(Collider other)
    {
        Enemigo enemigo = other.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);
        }
    }
}