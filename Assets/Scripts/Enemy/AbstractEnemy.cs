using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    chase,
    attack
}

abstract public class AbstractEnemy : MonoBehaviour
{
    abstract protected void Awake();
    abstract protected void Start();
    abstract protected void Update();
    abstract protected void MoveToTarget();
    abstract protected void ReturnToHome();
    abstract protected void AttackTarget();
    abstract public void ReceiveKnockback(Vector3 direction);
}
