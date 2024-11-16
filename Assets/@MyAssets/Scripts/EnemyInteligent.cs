using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInteligent : MonoBehaviour
{
    public Transform target;
    public float Velocity;
    public float velocityDecrese;
    public NavMeshAgent IA;
    public Animator anim;
    public float timeToHit = 5f;

    void Update()
    {
        IA.speed = Velocity;
        IA.SetDestination(target.position);
        updateAnimations();
    }

    void updateAnimations()
    {
        float speed1;
        speed1 = IA.velocity.magnitude;
        speed1 = Mathf.Clamp01(speed1);
        anim.SetFloat("Speed", speed1, 0.1f, Time.deltaTime);
    }

}