using System.Threading;
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
    [SerializeField] protected float knockbackTimeout;
    protected Vector3 homePosition;
    protected GameObject attackTarget;
    protected bool isChaseTarget;
    protected bool isAttackTarget;
    protected float attackDistance;
    protected EnemyState enemyState;
    new protected Rigidbody2D rigidbody2D;  // overloaded deprecated

    sealed override protected void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        if (enemyState != EnemyState.knockback) {
            enemyState = EnemyState.idle;
            if (transform.position != homePosition) enemyState = EnemyState.walk;
            if (isChaseTarget) enemyState = EnemyState.chase;
            if (isAttackTarget) enemyState = EnemyState.attack;
        }
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

    override public void ReceiveKnockbackMessage(object[] args)
    {
        // unpack args
        Vector3 otherPosition = (Vector3)args[0];
        float knockbackForce = (float)args[1];

        // calculate force
        Vector2 appliedForce = transform.position - otherPosition;
        appliedForce = appliedForce.normalized * knockbackForce;
        StartCoroutine(ApplyKnockback(appliedForce));
    }

    protected IEnumerator ApplyKnockback(Vector2 appliedForce)
    {
        // turn on ragdoll and apply force
        rigidbody2D.isKinematic = false;
        rigidbody2D.AddForce(appliedForce);
        enemyState = EnemyState.knockback;
        // wait
        yield return new WaitForSeconds(knockbackTimeout);

        // turn off ragdoll and remove force
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = true;
        enemyState = EnemyState.walk;
        yield return null;
    }

    override public void ReceiveDamageMessage(object[] args)
    {
        // unpack args
        GameObject other = (GameObject)args[0];
        float damageApplied = (float)args[1];

        // TODO: ...do something...
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
