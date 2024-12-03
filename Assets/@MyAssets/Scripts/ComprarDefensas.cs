using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComprarDefensas : MonoBehaviour
{
    public TextMeshProUGUI portonText;
    public TextMeshProUGUI barricada;
    public TextMeshProUGUI textoDinero;
    public Button botonComprar;
    
    public Porton porton;
    public GameObject barricadaPrefab;

    private string opcionSeleccionada = "";

    public int precioReparacion = 200;
    public int preciobarricada = 100;

    public MenuManagerTexto gestorMenu;

    void Start()
    {
        portonText.text = $"1. Reparar Portón - ${precioReparacion}";
        barricada.text = $"2. Barricada Improvisada - ${preciobarricada}";

        portonText.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("portonText"));
        barricada.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("barricada"));
        botonComprar.onClick.AddListener(ComprarObjeto);

        ActualizarDineroUI();
    }

    void SeleccionarOpcion(string opcion)
    {
        opcionSeleccionada = opcion;

        ResetearColores();
        if (opcion == "portonText")
            portonText.color = Color.green;
        else if (opcion == "barricada")
            barricada.color = Color.green;
    }

    void ResetearColores()
    {
        portonText.color = Color.white;
        barricada.color = Color.white;
    }

    void ComprarObjeto()
    {
        int precio = 0;

        if (opcionSeleccionada == "portonText")
            precio = precioReparacion;
        else if (opcionSeleccionada == "barricada")
            precio = preciobarricada;

        if (gestorMenu.dineroTotal >= precio)
        {
            gestorMenu.dineroTotal -= precio;

            if (opcionSeleccionada == "porton")
                porton.Reparar(100);
            else if (opcionSeleccionada == "barricada")
                barricadaPrefab.SetActive(true);

            ActualizarDineroUI();
        }
    }

    void ActualizarDineroUI()
    {
        textoDinero.text = $"Dinero: ${gestorMenu.dineroTotal}";
    }

}
