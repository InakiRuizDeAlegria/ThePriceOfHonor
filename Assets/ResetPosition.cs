using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    void Update()
    {
        ResetToOrigin();
    }

    public void ResetToOrigin()
    {
        transform.position = Vector3.zero;
    }

}