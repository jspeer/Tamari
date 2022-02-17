using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D myRigidBody;
    private Animator animator;
    private Vector3 change = new Vector3();

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Reset change and reassign to input values
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        // If we've inputted movement, move the character
        if (change != Vector3.zero)
        {
            MoveCharacter();
            // Set the animator move values
            animator.SetBool("moving", true);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
        } else {
            //animator.StopPlayback();
            animator.SetBool("moving", false);
        }
    }

    private void MoveCharacter()
    {
        // Apply transform change
        // Multiply by speed and delta time
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }
}
