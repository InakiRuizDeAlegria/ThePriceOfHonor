using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public float vidaMax;

    private float vidaActual;
    private float vidadMaxActual;
    private string vidadMaxMedia = "media";
    private string vidadMaxAlta = "alta";

    [Header("Ventana de Muerte")]
    public GameObject ventanaMuerte;
    public Transform vrCamera;

    void Start()
    {
        vidaActual = vidaMax;
        vidadMaxActual = vidaMax;

        if (ventanaMuerte != null)
            ventanaMuerte.SetActive(false);
    }

    void Update()
    {
        barraVida.fillAmount = vidaActual / vidaMax;
    }

    void LateUpdate()
{
    if (ventanaMuerte.activeSelf && vrCamera != null)
    {
        Vector3 posicionFrente = vrCamera.position + vrCamera.forward * 2.0f; // Ajusta la distancia si es necesario
        ventanaMuerte.transform.position = posicionFrente;

        ventanaMuerte.transform.rotation = Quaternion.LookRotation(vrCamera.forward);
    }
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
    }

    void Morir()
    {
        MostrarVentanaMuerte();
        DetenerJuego();
    }

    void MostrarVentanaMuerte()
    {
        if (ventanaMuerte != null && vrCamera != null)
        {
            ventanaMuerte.SetActive(true);

            Vector3 posicionFrente = vrCamera.position + vrCamera.forward * 2.0f;
            ventanaMuerte.transform.position = posicionFrente;

            ventanaMuerte.transform.rotation = Quaternion.LookRotation(vrCamera.forward);
        }
    }

    void DetenerJuego()
    {
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1f;
    }
}
