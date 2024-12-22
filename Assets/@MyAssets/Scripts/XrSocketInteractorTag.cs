using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XrSocketInteractorTag : XRSocketInteractor
{
	public string targetTag;

	public override bool CanSelect(IXRSelectInteractable interactable)
	{
		return base.CanSelect(interactable) && (interactable as MonoBehaviour)?.CompareTag(targetTag) == true;
	}
}
