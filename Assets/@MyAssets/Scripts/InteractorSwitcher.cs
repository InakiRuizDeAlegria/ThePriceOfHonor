using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    public XRDirectInteractor directInteractor;
    public XRRayInteractor rayInteractor;

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
        directInteractor.enabled = !directInteractor.enabled;
        rayInteractor.enabled = !rayInteractor.enabled;
    }
}
