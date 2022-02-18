using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    chase,
    attack,
    knockback
}

abstract public class AbstractEnemy : MonoBehaviour
{
    abstract protected void Awake();
    abstract protected void Start();
    abstract protected void Update();
    abstract protected void MoveToTarget();
    abstract protected void ReturnToHome();
    abstract protected void AttackTarget();
    abstract public void ReceiveKnockbackMessage(object[] args);
    abstract public void ReceiveDamageMessage(object[] args);
}
