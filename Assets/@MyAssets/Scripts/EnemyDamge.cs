using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamge : MonoBehaviour
{
    public EnemyInteligent enemy;

    public void Damage(GameObject objetivo)
    {
        enemy.Hit(objetivo);
    }
}
