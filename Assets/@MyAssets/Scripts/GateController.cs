using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class GateController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("La palanca que controla el portón")]
    private XRLever lever;

    [SerializeField]
    [Tooltip("El objeto que representa el portón")]
    private GameObject gate;

    [SerializeField]
    [Tooltip("La altura a la que debe subir el portón cuando está activo")]
    private float openHeight = 5.0f;

    [SerializeField]
    [Tooltip("La velocidad a la que se mueve el portón")]
    private float moveSpeed = 2.0f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isMoving = false;

    void Start()
    {
        if (gate == null)
        {
            enabled = false;
            return;
        }

        closedPosition = gate.transform.position;
        openPosition = new Vector3(closedPosition.x, closedPosition.y + openHeight, closedPosition.z);

        if (lever != null)
        {
            lever.onLeverActivate.AddListener(OpenGate);
            lever.onLeverDeactivate.AddListener(CloseGate);

            if (lever.value)
            {
                gate.transform.position = closedPosition;
            }
            else
            {
                gate.transform.position = openPosition;
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveGate();
        }
    }

    private void OpenGate()
    {
        ActivateGate();
        isMoving = true;
    }

    private void CloseGate()
    {
        ActivateGate();
        isMoving = true;
    }

    private void MoveGate()
    {
        Vector3 targetPosition = lever.value ? openPosition : closedPosition;

        gate.transform.position = Vector3.MoveTowards(gate.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(gate.transform.position, targetPosition) < 0.01f)
        {
            gate.transform.position = targetPosition;
            isMoving = false;

            if (lever.value && gate.transform.position == openPosition)
            {
                DeactivateGate();
            }
        }
    }

    private void ActivateGate()
    {
        if (gate != null)
        {
            gate.SetActive(true);
        }
    }

    private void DeactivateGate()
    {
        if (gate != null)
        {
            gate.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (lever != null)
        {
            lever.onLeverActivate.RemoveListener(OpenGate);
            lever.onLeverDeactivate.RemoveListener(CloseGate);
        }
    }
}
