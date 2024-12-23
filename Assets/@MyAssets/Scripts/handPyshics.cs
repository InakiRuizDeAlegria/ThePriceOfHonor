using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysics : MonoBehaviour
{
    public Transform target;
    public Renderer nonPhysicalHnad;
    public float showNonPhysicalHandDistance = 0.05f;

    private Rigidbody rb;
    private Collider[] handColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    public void EnableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = true;
        }
    }

    public void EnableHandColliderDelay(float delay)
    {
        Invoke("EnableHandCollider", delay);
    }

    public void DisableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > showNonPhysicalHandDistance)
        {
            nonPhysicalHnad.enabled = true;
        }
        else 
            nonPhysicalHnad.enabled = false;
    }

    void FixedUpdate()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        float zRotationCorrection = (gameObject.layer == LayerMask.NameToLayer("LeftHandPhisics")) ? 90f : -90f;
        Quaternion targetRotation = target.rotation * Quaternion.Euler(0, 0, zRotationCorrection);
        Quaternion rotationDifference = targetRotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
