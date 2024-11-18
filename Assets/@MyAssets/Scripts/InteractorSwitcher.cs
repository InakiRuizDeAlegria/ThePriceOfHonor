using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    private XRDirectInteractor directInteractor;
    private XRRayInteractor rayInteractor;

    private void Awake()
    {
        directInteractor = gameObject.GetComponent<XRDirectInteractor>();
        rayInteractor = gameObject.GetComponent<XRRayInteractor>();

        if (directInteractor == null)
        {
            Debug.Log("XRDirectInteractor no encontrado, se agrega uno nuevo.");
            directInteractor = gameObject.AddComponent<XRDirectInteractor>();
        }

        if (rayInteractor == null)
        {
            Debug.Log("XRRayInteractor no encontrado, se agrega uno nuevo.");
            rayInteractor = gameObject.AddComponent<XRRayInteractor>();
        }

        directInteractor.enabled = false;
        rayInteractor.enabled = false;
    }

    private void Start()
    {
        directInteractor.enabled = true;
        rayInteractor.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchInteractor();
        }
    }

    private void SwitchInteractor()
    {
        if (directInteractor == null || rayInteractor == null)
        {
            Debug.LogError("Los interactores no están correctamente inicializados.");
            return;
        }

        if (directInteractor.enabled)
        {
            directInteractor.enabled = false;
            rayInteractor.enabled = true;
        }
        else
        {
            directInteractor.enabled = true;
            rayInteractor.enabled = false;
        }
    }
}
