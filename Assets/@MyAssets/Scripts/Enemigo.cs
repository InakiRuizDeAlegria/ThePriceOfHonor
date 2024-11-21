using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public SistemaOleadas sistemaOleadas;

    public void AsociarSistemaOleadas(SistemaOleadas sistema)
    {
        sistemaOleadas = sistema;
    }

    void OnDestroy()
    {
        if (sistemaOleadas != null)
        {
            sistemaOleadas.EnemigoEliminado();
        }
    }
}
