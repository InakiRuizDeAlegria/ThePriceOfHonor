using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class sliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public Material crossSectionMaterial;
    public float cutForce = 2000;

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        SkinnedMeshRenderer skinnedRenderer = target.GetComponentInChildren<SkinnedMeshRenderer>();
        Mesh bakedMesh = new Mesh();

        if (skinnedRenderer != null)
        {
            skinnedRenderer.BakeMesh(bakedMesh);
        }
        else
        {
            Debug.LogWarning("El objeto no tiene un SkinnedMeshRenderer.");
            return;
        }

        GameObject tempObject = new GameObject("TempMeshObject");
        tempObject.AddComponent<MeshFilter>().mesh = bakedMesh;
        tempObject.AddComponent<MeshRenderer>().sharedMaterials = skinnedRenderer.sharedMaterials;
        tempObject.transform.position = target.transform.position;
        tempObject.transform.rotation = target.transform.rotation;

        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = tempObject.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(tempObject, crossSectionMaterial);
            SetupSlicedComponent(upperHull, target);

            GameObject lowerHull = hull.CreateLowerHull(tempObject, crossSectionMaterial);
            SetupSlicedComponent(lowerHull, target);

            Destroy(target);
        }

        Destroy(tempObject);
    }

    public void SetupSlicedComponent(GameObject slicedObject, GameObject originalObject)
    {
        slicedObject.transform.position = originalObject.transform.position;
        slicedObject.transform.rotation = originalObject.transform.rotation;

        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
