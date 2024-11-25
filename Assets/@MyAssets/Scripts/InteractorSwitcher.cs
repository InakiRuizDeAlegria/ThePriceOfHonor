using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    public GameObject directInteractorObject;
    public GameObject rayInteractorObject;

    private XRDirectInteractor directInteractor;
    private XRRayInteractor rayInteractor;

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
