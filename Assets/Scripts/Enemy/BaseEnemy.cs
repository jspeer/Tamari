using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        List<EnemyState> validStates = new List<EnemyState>{
            EnemyState.idle,
            EnemyState.walk,
            EnemyState.chase,
            EnemyState.attack
        };

        if (validStates.Contains(enemyState)) {
            bool rth = returnToHomeposition && transform.position != homePosition;
            enemyState = (isAttackTarget) ? EnemyState.attack : (isChaseTarget) ? EnemyState.chase : (rth) ? EnemyState.walk : EnemyState.idle;
        }
    }

    virtual protected void MoveToTarget()
    {
        List<EnemyState> validStates = new List<EnemyState>{
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
        List<EnemyState> validStates = new List<EnemyState>{
            EnemyState.walk
        };

        if (validStates.Contains(enemyState)) {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.MovePosition(CalculateMoveVector(homePosition));
        }
    }

    virtual public void ReceiveKnockbackMessage(object[] args)
    {
        // If the args aren't what we're expecting, return early
        if (args.Length != 2 || args[0].GetType() != typeof(Vector3) || args[1].GetType() != typeof(float)) { return; }

        // unpack args
        Vector3 otherPosition = (Vector3)args[0];
        float knockbackForce = (float)args[1];

        List<EnemyState> validStates = new List<EnemyState>{
            EnemyState.idle,
            EnemyState.walk,
            EnemyState.chase,
            EnemyState.attack
        };

        if (validStates.Contains(enemyState)) {
            // calculate force
            Vector2 appliedForce = transform.position - otherPosition;
            appliedForce = appliedForce.normalized * knockbackForce;
            StartCoroutine(ApplyKnockback(appliedForce));
        }
    }

    protected IEnumerator ApplyKnockback(Vector2 appliedForce)
    {
        rigidbody2D.AddForce(appliedForce);
        EnemyState previousState = enemyState;
        enemyState = EnemyState.stagger;
        yield return new WaitForSeconds(knockbackTimeout);

        rigidbody2D.velocity = Vector2.zero;
        enemyState = previousState;
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
