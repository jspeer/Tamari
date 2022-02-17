using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEnemy : BaseEnemy
{
    [Header("Log Specific")]
    [SerializeField] float wakeUpRange;

    new private void Awake()
    {
        base.Awake();
    }

    new private void Start()
    {
        base.Start();

        attackTarget = GameObject.FindGameObjectWithTag("Player");
    }

    new private void Update()
    {
        base.Update();

        switch (enemyState) {
            case EnemyState.idle:
                // go to sleep
                break;
            case EnemyState.walk:
                ReturnToHome();
                break;
            case EnemyState.chase:
                MoveToTarget();
                break;
            case EnemyState.attack:
                // attack player
                break;
        }
    }


    sealed override protected void AttackTarget()
    {

    }
}
