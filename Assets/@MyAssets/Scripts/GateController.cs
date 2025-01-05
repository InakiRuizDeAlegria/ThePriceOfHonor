using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class GateController : MonoBehaviour
{
    [SerializeField]
    private XRLever lever;

    [SerializeField]
    private GameObject gate;

    [SerializeField]
    private float openHeight = 5.0f;

    [SerializeField]
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
            lever.value = false;
            lever.onLeverActivate.AddListener(OpenGate);
            lever.onLeverDeactivate.AddListener(CloseGate);

            gate.transform.position = closedPosition;
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
        if (!isMoving)
        {
            ActivateGate();
            isMoving = true;
        }
    }

    private void CloseGate()
    {
        if (!isMoving)
        {
            ActivateGate();
            isMoving = true;
        }
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
