using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class WeaponCanvasHandler : MonoBehaviour
{
    public Dictionary<XRGrabInteractable, CanvasGroup> weaponCanvasPairs = new Dictionary<XRGrabInteractable, CanvasGroup>();
    public FloatAffordanceReceiver affordanceReceiver;

    private void Update()
    {
        XRGrabInteractable[] grabInteractables = FindObjectsOfType<XRGrabInteractable>();

        foreach (var grab in grabInteractables)
        {
            if (weaponCanvasPairs.ContainsKey(grab))
                continue;

            CanvasGroup canvasGroup = grab.GetComponentInChildren<CanvasGroup>();
            if (canvasGroup != null)
            {
                weaponCanvasPairs.Add(grab, canvasGroup);
                canvasGroup.alpha = 0;
                canvasGroup.gameObject.SetActive(false);

                grab.selectEntered.AddListener(args => OnGrabbed(grab));
                grab.selectExited.AddListener(args => OnReleased(grab));
            }
        }
    }

    private void OnGrabbed(XRGrabInteractable grabInteractable)
    {
        if (weaponCanvasPairs.TryGetValue(grabInteractable, out CanvasGroup canvasGroup))
        {
            canvasGroup.gameObject.SetActive(true);
            if (affordanceReceiver != null)
            {
                affordanceReceiver.valueUpdated.AddListener(alpha => UpdateCanvasAlpha(canvasGroup, alpha));
            }
        }
    }

    private void OnReleased(XRGrabInteractable grabInteractable)
    {
        if (weaponCanvasPairs.TryGetValue(grabInteractable, out CanvasGroup canvasGroup))
        {
            if (affordanceReceiver != null)
            {
                affordanceReceiver.valueUpdated.RemoveListener(alpha => UpdateCanvasAlpha(canvasGroup, alpha));
            }

            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
        }
    }

    private void UpdateCanvasAlpha(CanvasGroup canvasGroup, float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    private void OnDisable()
    {
        foreach (var pair in weaponCanvasPairs)
        {
            pair.Key.selectEntered.RemoveListener(args => OnGrabbed(pair.Key));
            pair.Key.selectExited.RemoveListener(args => OnReleased(pair.Key));
        }

        weaponCanvasPairs.Clear();
    }
}
