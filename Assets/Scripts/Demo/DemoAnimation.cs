using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DemoAnimation : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D playerRb;
    public Transform visualTransform;
    public float xMovementPadding;

    private Vector2 newScale;
    private float xScale;

    private void Start()
    {
        xScale = visualTransform.localScale.x;
        newScale = new Vector2(xScale, visualTransform.localScale.y);
    }

    void Update()
    {
        if(playerRb.velocity.x < -xMovementPadding)
        {
            animator.SetBool("XMovement", true);
            newScale.x = -xScale;
        }
        else if(playerRb.velocity.x > xMovementPadding)
        {
            animator.SetBool("XMovement", true);
            newScale.x = xScale;
        }
        else
        {
            animator.SetBool("XMovement", false);
        }

        visualTransform.localScale = newScale;

        animator.SetFloat("YMovement", playerRb.velocity.y);
        animator.SetBool("Grounded", Player.INSTANCE.isGrounded);
    }
}
