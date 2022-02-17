using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseEnemy : AbstractEnemy
{
    [Header("Base")]
    [SerializeField] protected string enemyName;
    [SerializeField] protected float health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected bool returnToHomeposition;

    [Header("Attack")]
    [SerializeField] protected Collider2D damageTakenCollider;
    [SerializeField] protected float agroRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float baseDamage;

    [SerializeField] protected Vector3 homePosition;
    protected GameObject attackTarget;
    protected bool isChaseTarget;
    protected bool isAttackTarget;
    protected float attackDistance;
    protected EnemyState enemyState;

    sealed override protected void Awake()
    {
        
    }

    sealed override protected void Start()
    {
        enemyState = EnemyState.idle;
        homePosition = transform.position;
    }

    sealed override protected void Update()
    {
        // Reset the distance and flags
        CheckDistance();

        // Reset the state every frame
        enemyState = EnemyState.idle;
        if (transform.position != homePosition) enemyState = EnemyState.walk;
        if (isChaseTarget) enemyState = EnemyState.chase;
        if (isAttackTarget) enemyState = EnemyState.attack;
    }

    override protected void MoveToTarget()
    {
        transform.position = CalculateMoveVector(attackTarget.transform.position);
    }

    override protected void ReturnToHome()
    {
        if (returnToHomeposition && transform.position != homePosition) {
            transform.position = CalculateMoveVector(homePosition);
        }
    }

    public override void ReceiveKnockback(Vector3 direction)
    {
        throw new NotImplementedException();
    }

    protected void CheckDistance()
    {
        attackDistance = Vector3.Distance(attackTarget.transform.position, transform.position);
        isChaseTarget = attackDistance <= agroRange;
        isAttackTarget = attackDistance <= attackRange;
    }

    protected Vector3 CalculateMoveVector(Vector3 target)
    {
        float maxDistanceDelta = moveSpeed * Time.deltaTime;
        return Vector3.MoveTowards(transform.position, target, maxDistanceDelta);
    }
}
