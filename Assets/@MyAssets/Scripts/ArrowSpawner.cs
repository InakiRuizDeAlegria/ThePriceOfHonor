using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;
    public Quiver quiver;
    private XRGrabInteractable bow;
    private bool arrowNotched = false;
    private GameObject currentArrow = null;

    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();

        quiver = FindObjectOfType<Quiver>();
        GameObject arrowObject = GameObject.FindGameObjectWithTag("flechaFuncional");
        if (arrowObject != null)
        {
            arrow = arrowObject;
        }

        PullInteraction.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    void Update()
    {
        if (bow.isSelected && !arrowNotched && quiver.UseArrow())
        {
            arrowNotched = true;
            StartCoroutine("DelayedSpawn");
        }

        if (!bow.isSelected && currentArrow != null)
        {
            Destroy(currentArrow);
            quiver.AddArrow();
            NotchEmpty(1f);
        }
    }

    private void NotchEmpty(float value)
    {
        arrowNotched = false;
        currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f);
    
        currentArrow = Instantiate(arrow, notch.transform);
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
    
        ResetPosition resetPosition = currentArrow.GetComponent<ResetPosition>();
        if (resetPosition != null)
        {
            Destroy(resetPosition);
        }
    }

}
