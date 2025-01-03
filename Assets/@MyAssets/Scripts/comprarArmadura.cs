using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComprarArmadura : MonoBehaviour
{
    public TextMeshProUGUI curarText;
    public TextMeshProUGUI armaduraLigeraText;
    public TextMeshProUGUI armaduraPesadaText;
    public TextMeshProUGUI textoDinero;
    public Button botonComprar;
    public BarraVida vida;

    private string opcionSeleccionada = "";

    public int precioCurar = 100;
    public int precioArmaduraLigera = 200;
    public int precioArmaduraPesada = 300;

    public MenuManagerTexto gestorMenu;

    void Start()
    {
        curarText.text = $"1. Curar - ${precioCurar}";
        armaduraLigeraText.text = $"2. Armadura Ligera - ${precioArmaduraLigera}";
        armaduraPesadaText.text = $"3. Armadura Pesada - ${precioArmaduraPesada}";

        curarText.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("curarText"));
        armaduraLigeraText.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("armaduraLigeraText"));
        armaduraPesadaText.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("armaduraPesadaText"));
        botonComprar.onClick.AddListener(ComprarObjeto);

        ActualizarDineroUI();
    }

    void OnEnable()
    {
        ActualizarDineroUI();
    }

    void SeleccionarOpcion(string opcion)
    {
        opcionSeleccionada = opcion;

        ResetearColores();
        if (opcion == "curarText")
            curarText.color = Color.green;
        else if (opcion == "armaduraLigeraText")
            armaduraLigeraText.color = Color.green;
        else if (opcion == "armaduraPesadaText")
            armaduraPesadaText.color = Color.green;
    }

    void ResetearColores()
    {
        curarText.color = Color.white;
        armaduraLigeraText.color = Color.white;
        armaduraPesadaText.color = Color.white;
    }

    void ComprarObjeto()
    {
        int precio = 0;

        if (opcionSeleccionada == "curarText")
            precio = precioCurar;
        else if (opcionSeleccionada == "armaduraLigeraText")
            precio = precioArmaduraLigera;
        else if (opcionSeleccionada == "armaduraPesadaText")
            precio = precioArmaduraPesada;

        if (gestorMenu.dineroTotal >= precio)
        {
            gestorMenu.dineroTotal -= precio;

            if (opcionSeleccionada == "curarText")
                vida.curar();
            else if (opcionSeleccionada == "armaduraLigeraText")
                vida.actualizarVidaMax("media");
            else if (opcionSeleccionada == "armaduraPesadaText")
                vida.actualizarVidaMax("alta");

            ActualizarDineroUI();
        }
    }

    void ActualizarDineroUI()
    {
        textoDinero.text = $"Dinero: ${gestorMenu.dineroTotal}";
    }

}
