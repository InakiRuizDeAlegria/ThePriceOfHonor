using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;
    public float danio = 15f;

    private Rigidbody arrowRigidbody ;
    private bool inAir = false;
    private Vector3 lastPosition = Vector3.zero;
    private ParticleSystem arrowParticleSystem;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        arrowRigidbody  = GetComponent<Rigidbody>();
        arrowParticleSystem = GetComponentInChildren<ParticleSystem>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        PullInteraction.PullActionReleased += Release;

        Stop();
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * speed;
        arrowRigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        lastPosition = tip.position;
        arrowParticleSystem.Play();
        trailRenderer.emitting = true;
    }

    IEnumerator RotateWithVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        while (inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(arrowRigidbody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    void FixedUpdate()
    {
        if (inAir)
        {
            CheckCollision();
            lastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer != 3)
            {
                if (hitInfo.transform.TryGetComponent(out Rigidbody body))
                {
                    arrowRigidbody .interpolation = RigidbodyInterpolation.None;
                    transform.parent = hitInfo.transform;
                    body.AddForce(arrowRigidbody .velocity, ForceMode.Impulse);
                }
                Stop();
            }
        }
    }

    private void Stop()
    {
        inAir = false;
        SetPhysics(false);
        arrowParticleSystem.Stop();
        trailRenderer.emitting = false;
    }

    private void SetPhysics(bool usePhysics)
    {
        arrowRigidbody .useGravity = usePhysics;
        arrowRigidbody .isKinematic = !usePhysics;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemigo enemigo = other.GetComponent<Enemigo>();
        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);
            Destroy(gameObject);
        }
    }

}
