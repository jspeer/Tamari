using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] float knockbackTimeout = .2f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy") {
            Vector2 appliedForce = transform.position - other.transform.position;
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
