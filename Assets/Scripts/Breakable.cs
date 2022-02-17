using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Break Animation")]
    [SerializeField] private bool hasAnimation;
    [SerializeField] private float lengthOfFrame;
    [SerializeField] private float numberOfFrames;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator Break()
    {
        if (hasAnimation) {
            animator.SetBool("broken", true);
            float waitDelay = lengthOfFrame * numberOfFrames;
            yield return new WaitForSeconds(waitDelay);
        }
        Destroy(gameObject);
    }
}
