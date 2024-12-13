using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public float vidaMax;

    private float vidaActual;
    private float vidaMaxActual;
    private string vidadMaxMedia = "media";
    private string vidadMaxAlta = "alta";

    [Header("Ventana de Muerte")]
    public GameObject ventanaMuerte;
    public Transform VRCamera;

    void Start()
    {
        vidaActual = vidaMax;
        vidaMaxActual = vidaMax;

        if (ventanaMuerte != null)
            ventanaMuerte.SetActive(false);
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
            Morir();
        }
    }

    public void curar()
    {
        vidaActual = vidaMax;
    }

    public void actualizarVidaMax(string vida)
    {
        if (vida == vidadMaxMedia)
        {
            vidaMax += 50;
        }
        else if (vida == vidadMaxAlta)
        {
            vidaMax += 100;
        }
        vidaActual = vidaMax;
        Debug.Log(vidaActual);
    }

    void Morir()
    {
        MostrarVentanaMuerte();
    }

    void MostrarVentanaMuerte()
    {
        if (ventanaMuerte != null && VRCamera != null)
        {
            ventanaMuerte.SetActive(true);

            ventanaMuerte.transform.position = VRCamera.position + VRCamera.forward * 2.0f;
            ventanaMuerte.transform.rotation = Quaternion.LookRotation(VRCamera.forward);
        }
    }
}
