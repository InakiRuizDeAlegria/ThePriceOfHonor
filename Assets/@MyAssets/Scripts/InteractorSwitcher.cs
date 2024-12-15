using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    public GameObject grabInteractor;
    public GameObject rayInteractor;
    public InputActionReference switchInteractorAction;

    private bool isDirectInteractorActive = true;

    private void Awake()
    {
        if (grabInteractor == null || rayInteractor == null)
        {
            return;
        }

        SetInteractorState(true);

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
        isDirectInteractorActive = !isDirectInteractorActive;
        SetInteractorState(isDirectInteractorActive);
    }

    private void SetInteractorState(bool activateDirectInteractor)
    {
        grabInteractor.SetActive(activateDirectInteractor);
        rayInteractor.SetActive(!activateDirectInteractor);
    }
}
