using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;
    public XRSocketInteractor arrowSocket; // Referencia al socket del arco

    private XRGrabInteractable bow;
    private GameObject currentArrow = null;

    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();

        // Subscribir a los eventos del socket
        arrowSocket.selectEntered.AddListener(OnArrowPlaced);
        arrowSocket.selectExited.AddListener(OnArrowRemoved);
    }

    private void OnDestroy()
    {
        // Desubscribir de los eventos del socket
        arrowSocket.selectEntered.RemoveListener(OnArrowPlaced);
        arrowSocket.selectExited.RemoveListener(OnArrowRemoved);
    }

    private void OnArrowPlaced(SelectEnterEventArgs args)
    {
        if (currentArrow == null)
        {
            // Destruir la flecha colocada en el socket
            Destroy(args.interactableObject.transform.gameObject);

            // Instanciar una flecha nueva en el notch
            currentArrow = Instantiate(arrow, notch.transform);
        }
    }

    private void OnArrowRemoved(SelectExitEventArgs args)
    {
        // Limpiar la referencia a la flecha actual si se retira del notch
        currentArrow = null;
    }
}
