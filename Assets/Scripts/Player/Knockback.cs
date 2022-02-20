using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] float knockbackTimeout = 2f;
    new private Rigidbody2D rigidbody2D; // overload deprecated definition

    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy") {
            // TODO: add knockback effect if player walks into enemy
            Vector3 otherPosition = other.transform.position;

            Vector2 appliedForce = gameObject.transform.position - otherPosition;
            appliedForce = appliedForce.normalized * gameObject.GetComponentInParent<Player>().KnockbackForce;
            StartCoroutine(ApplyKnockback(appliedForce));

        }
    }

    private IEnumerator ApplyKnockback(Vector2 appliedForce)
    {
        gameObject.GetComponentInParent<Rigidbody2D>().AddForce(appliedForce);
        gameObject.GetComponentInParent<PlayerMovement>().CurrentState = PlayerState.stagger;
        yield return new WaitForSeconds(knockbackTimeout);

        gameObject.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponentInParent<PlayerMovement>().CurrentState = PlayerState.walk;
        yield return null;
    }
}
