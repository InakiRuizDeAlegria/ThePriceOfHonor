using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiver : MonoBehaviour
{
    public int arrowCount = 0;
    public TextMeshProUGUI arrowCountText;

    private void Start()
    {
        UpdateArrowCountUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            Debug.Log("he recogido una flecha");
            AddArrow();
            Destroy(other.gameObject);
        }
    }

    private void UpdateArrowCountUI()
    {
        if (arrowCountText != null)
        {
            arrowCountText.text = "" + arrowCount;
        }
    }

    public bool UseArrow()
    {
        if (arrowCount > 0)
        {
            arrowCount--;
            UpdateArrowCountUI();
            return true;
        }
        return false;   
    }

    public void AddArrow()
    {
        arrowCount++;
        UpdateArrowCountUI();
    }

}
