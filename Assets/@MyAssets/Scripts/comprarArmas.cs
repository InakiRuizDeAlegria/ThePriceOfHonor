using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class comprarArmas : MonoBehaviour
{
    public TextMeshProUGUI Espada;
    public TextMeshProUGUI EspadaInicial;
    public TextMeshProUGUI Arco;
    public TextMeshProUGUI Flecha;
    public Button botonComprar;
    
    public GameObject espadaPrefab;
    public GameObject espadaInicialPrefab;
    public GameObject arcoPrefab;
    public GameObject flechaPrefab;

    private string opcionSeleccionada = "";
    public Transform mesaTransform;

    void Start()
    {
        Espada.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Espada"));
        EspadaInicial.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("EspadaInicial"));
        Arco.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Arco"));
        Flecha.GetComponent<Button>().onClick.AddListener(() => SeleccionarOpcion("Flecha"));
        botonComprar.onClick.AddListener(ComprarObjeto);
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
        Vector3 posicionMesa = mesaTransform.position + new Vector3(0, 1, 0);

        if (opcionSeleccionada == "Espada")
        {
            Instantiate(espadaPrefab, posicionMesa, Quaternion.identity);
        }
        else if (opcionSeleccionada == "EspadaInicial")
        {
            Instantiate(espadaInicialPrefab, posicionMesa, Quaternion.identity);
        }
        else if (opcionSeleccionada == "Arco")
        {
            Instantiate(arcoPrefab, posicionMesa, Quaternion.identity);
        }
        else if (opcionSeleccionada == "Flecha")
        {
            Instantiate(flechaPrefab, posicionMesa, Quaternion.identity);
        }
    }
}
