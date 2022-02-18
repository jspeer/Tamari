using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PlayerState currentState;
    public PlayerState CurrentState { get { return currentState; } }
    [SerializeField] private float speed;
    [Header("Attack Animation")]
    [SerializeField] private float lengthOfFrame;
    [SerializeField] private float numberOfFrames;

    private Rigidbody2D myRigidBody;
    private Animator animator;
    private Vector3 change = new Vector3();

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // set initial values so animations work as intended
        currentState = PlayerState.walk;
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    private void Update()
    {
        // Reset change and reassign to input values
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Fire1") && currentState != PlayerState.attack) {
            StartCoroutine(Attacking());
        } else if (currentState == PlayerState.walk) {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator Attacking()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return new WaitForEndOfFrame();
        animator.SetBool("attacking", false);
        float waitDelay = lengthOfFrame * numberOfFrames;
        yield return new WaitForSeconds(waitDelay);
        currentState = PlayerState.walk;
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
            animator.SetBool("moving", false);
        }
    }

    private void MoveCharacter()
    {
        change.Normalize();
        // Apply transform change
        // Multiply by speed and delta time
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }
}
