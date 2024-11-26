using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    public GameObject directInteractorObject;
    public GameObject rayInteractorObject;

    private XRDirectInteractor directInteractor;
    private XRRayInteractor rayInteractor;

    public InputActionReference switchInteractorAction;

    private void Awake()
    {
        if (directInteractorObject != null)
        {
            directInteractor = directInteractorObject.GetComponent<XRDirectInteractor>();
        }

        if (rayInteractorObject != null)
        {
            rayInteractor = rayInteractorObject.GetComponent<XRRayInteractor>();
        }

        if (directInteractor != null)
        {
            directInteractor.enabled = true;
        }

        if (rayInteractor != null)
        {
            rayInteractor.enabled = false;
        }


        if (switchInteractorAction != null)
        {
            switchInteractorAction.action.performed += OnSwitchInteractor;
        }
    }

    private void OnDestroy()
    {
        if (switchInteractorAction != null)
        {
            switchInteractorAction.action.performed -= OnSwitchInteractor;
        }
    }

    private void OnSwitchInteractor(InputAction.CallbackContext context)
    {
        SwitchInteractor();
    }

    private void SwitchInteractor()
    {
        if (directInteractor != null && rayInteractor != null)
        {
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
}
