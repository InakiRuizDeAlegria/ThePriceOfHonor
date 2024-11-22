using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyInteligent : MonoBehaviour
{
    public Transform target;
    public float Velocity;
    public NavMeshAgent IA;
    public Animator anim;
    public GameObject portonObject;
    public Porton porton;

    void Start()
    {
        porton = Porton.instancia;
    }

    void Update()
    {
        IA.speed = Velocity;
        if (porton != null && porton.EstaActivo())
        {
            IA.SetDestination(portonObject.transform.position);
        }
        else
        {
            IA.SetDestination(target.position);
        }
        
        if(IA.velocity == Vector3.zero)
        {
            anim.SetBool("estaAtacando", true);
        }
        else
        {
            anim.SetBool("estaAtacando", false);
        }
    }

    public void hit()
    {
        porton.RecibirDanio(20);
    }
}