using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float PercentHealth { get; private set; }

    void Update()
    {
        EnemyHealth enemyHealth;
        if (gameObject.TryGetComponent<EnemyHealth>(out enemyHealth))
        {
            PercentHealth = enemyHealth.Health_N / enemyHealth.Health_N;
        }
        else
        {
            // player health
        }
    }

}