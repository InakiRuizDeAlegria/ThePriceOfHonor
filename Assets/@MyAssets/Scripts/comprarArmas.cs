using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class comprarArmas : MonoBehaviour
{
    public TextMeshProUGUI Espada;
    public TextMeshProUGUI EspadaInicial;
    public TextMeshProUGUI Arco;
    public TextMeshProUGUI Flecha;
    public TextMeshProUGUI textoDinero;
    public Button botonComprar;
    
    public GameObject espadaPrefab;
    public GameObject espadaInicialPrefab;
    public GameObject arcoPrefab;
    public GameObject flechaPrefab;

    private string opcionSeleccionada = "";
    public Transform mesaTransform;

    public int precioEspada = 300;
    public int precioEspadaInicial = 150;
    public int precioArco = 500;
    public int precioFlecha = 50;

    public MenuManagerTexto gestorMenu;

    void Start()
    {
        Espada.text = $"1. Espada Superior - ${precioEspada}";
        EspadaInicial.text = $"2. Espada Inicial - ${precioEspadaInicial}";
        Arco.text = $"3. Arco - ${precioArco}";
        Flecha.text = $"4. Flecha - ${precioFlecha}";

        Espada.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Espada"));
        EspadaInicial.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("EspadaInicial"));
        Arco.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Arco"));
        Flecha.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Flecha"));
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
        if (opcion == "Espada")
            Espada.color = Color.green;
        else if (opcion == "EspadaInicial")
            EspadaInicial.color = Color.green;
        else if (opcion == "Arco")
            Arco.color = Color.green;
        else if (opcion == "Flecha")
            Flecha.color = Color.green;
    }

    void ResetearColores()
    {
        Espada.color = Color.white;
        EspadaInicial.color = Color.white;
        Arco.color = Color.white;
        Flecha.color = Color.white;
    }

    void ComprarObjeto()
    {
        int precio = 0;

        if (opcionSeleccionada == "Espada")
            precio = precioEspada;
        else if (opcionSeleccionada == "EspadaInicial")
            precio = precioEspadaInicial;
        else if (opcionSeleccionada == "Arco")
            precio = precioArco;
        else if (opcionSeleccionada == "Flecha")
            precio = precioFlecha;

        if (gestorMenu.dineroTotal >= precio)
        {
            gestorMenu.dineroTotal -= precio;
            Vector3 posicionMesa = mesaTransform.position + new Vector3(0, 1, 0);

            if (opcionSeleccionada == "Espada")
                Instantiate(espadaPrefab, posicionMesa, Quaternion.identity);
            else if (opcionSeleccionada == "EspadaInicial")
                Instantiate(espadaInicialPrefab, posicionMesa, Quaternion.identity);
            else if (opcionSeleccionada == "Arco")
                Instantiate(arcoPrefab, posicionMesa, Quaternion.identity);
            else if (opcionSeleccionada == "Flecha")
                Instantiate(flechaPrefab, posicionMesa, Quaternion.identity);

            ActualizarDineroUI();
        }
    }

    void ActualizarDineroUI()
    {
        textoDinero.text = $"Dinero: ${gestorMenu.dineroTotal}";
    }

}
