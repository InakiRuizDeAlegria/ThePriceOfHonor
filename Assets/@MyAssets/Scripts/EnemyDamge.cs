using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamge : MonoBehaviour
{
    public EnemyInteligent enemy;

    public void Damage()
    {
        enemy.Hit();
    }
}
