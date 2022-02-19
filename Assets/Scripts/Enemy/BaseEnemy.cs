using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    sleeping,
    idle,
    walk,
    chase,
    attack,
    stagger
}

abstract public class BaseEnemy : MonoBehaviour
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
    [SerializeField] protected EnemyState enemyState; // serialized so i can manipulate it during dev // TODO: REMOVE SERIALIZE FIELD
    new protected Rigidbody2D rigidbody2D;  // overloaded deprecated

    protected void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        enemyState = EnemyState.idle;
        homePosition = transform.position;

        attackTarget = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        // Check and adjust state every frame
        SetState();

        // Non-physics based states
        switch (enemyState) {
            case EnemyState.idle:
                // go to sleep
                break;
        }
    }

    protected void FixedUpdate()
    {
        // Change target vectors and flags during physics update
        CheckDistance();

        // Physics based states
        switch (enemyState) {
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

    private void SetState()
    {
        // Reset the state every frame
        // If we're staggering, wait until we're not
        if (enemyState != EnemyState.stagger)
        {
            enemyState = EnemyState.idle;
            // If we've are supposed to return to a home position, override the state to walk
            if (returnToHomeposition && transform.position != homePosition) enemyState = EnemyState.walk;
            // If we are supposed to chase, override the state to chase
            if (isChaseTarget) enemyState = EnemyState.chase;
            // If we are supposed to attack, override the state to attack
            if (isAttackTarget) enemyState = EnemyState.attack;
        }
    }

    virtual protected void MoveToTarget()
    {
        List<EnemyState> validStates = new List<EnemyState>{
            EnemyState.idle,
            EnemyState.walk,
            EnemyState.chase
        };

        if (validStates.Contains(enemyState)) {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.MovePosition(CalculateMoveVector(attackTarget.transform.position));
        }
    }

    virtual protected void ReturnToHome()
    {
        if (returnToHomeposition && transform.position != homePosition) {
            rigidbody2D.MovePosition(CalculateMoveVector(homePosition));
        }
    }

    virtual public void ReceiveKnockbackMessage(object[] args)
    {
        // TODO: test to ensure the arg types are what we're expecting
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
        rigidbody2D.AddForce(appliedForce);
        enemyState = EnemyState.stagger;
        yield return new WaitForSeconds(knockbackTimeout);

        rigidbody2D.velocity = Vector2.zero;
        enemyState = EnemyState.idle;
        yield return null;
    }

    virtual public void ReceiveDamageMessage(object[] args)
    {
        // TODO: test to ensure the arg types are what we're expecting
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
