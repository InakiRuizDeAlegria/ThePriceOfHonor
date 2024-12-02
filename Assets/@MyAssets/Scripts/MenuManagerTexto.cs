using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManagerTexto : MonoBehaviour
{
    public TextMeshProUGUI textoOpcion1;
    public TextMeshProUGUI textoOpcion2;
    public TextMeshProUGUI textoOpcion3;
    public Button botonSeleccionar;
    public GameObject panelCompras;
    public GameObject panelOpcion1;
    public GameObject panelOpcion2;
    public GameObject panelOpcion3;
    public int dineroTotal = 1000;

    private string opcionSeleccionada = "";

    void Start()
    {
        textoOpcion1.GetComponentInParent<Button>().onClick.AddListener(() => SeleccionarOpcion("Opcion1"));
        textoOpcion2.GetComponentInParent<Button>().onClick.AddListener(() => SeleccionarOpcion("Opcion2"));
        textoOpcion3.GetComponentInParent<Button>().onClick.AddListener(() => SeleccionarOpcion("Opcion3"));
        botonSeleccionar.onClick.AddListener(ActivarPanelSeleccionado);

        panelOpcion1.SetActive(false);
        panelOpcion2.SetActive(false);
        panelOpcion3.SetActive(false);
    }

    void SeleccionarOpcion(string opcion)
    {
        opcionSeleccionada = opcion;

        ResetearColores();

        if (opcion == "Opcion1")
            textoOpcion1.color = Color.green;
        else if (opcion == "Opcion2")
            textoOpcion2.color = Color.green;
        else if (opcion == "Opcion3")
            textoOpcion3.color = Color.green;
    }

    void ResetearColores()
    {
        textoOpcion1.color = Color.white;
        textoOpcion2.color = Color.white;
        textoOpcion3.color = Color.white;
    }

    void ActivarPanelSeleccionado()
    {
        panelOpcion1.SetActive(false);
        panelOpcion2.SetActive(false);
        panelOpcion3.SetActive(false);

        if (opcionSeleccionada == "Opcion1")
            panelOpcion1.SetActive(true);
        else if (opcionSeleccionada == "Opcion2")
            panelOpcion2.SetActive(true);
        else if (opcionSeleccionada == "Opcion3")
            panelOpcion3.SetActive(true);

        panelCompras.SetActive(false);
    }
}
