﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 0.3f)] private float m_MovementSmoothing = 0.02f;
    private Rigidbody2D rigidBody;
    private Vector3 m_velocity = Vector3.zero;

    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 40f;

    public float jumpForce;
    private bool isGrounded = false;
    private bool isFacingRight = true;
    private bool isSlowDown = false;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float inputMovement = Input.GetAxisRaw("Horizontal");
        horizontalMove = inputMovement * runSpeed;

        if (Input.GetButton("SlowDown"))
        {
            isSlowDown = true;
            runSpeed = 10f;
        }

        if (!Input.GetButton("SlowDown"))
        {
            isSlowDown = false;
            runSpeed = 20f;
        }

        if (inputMovement > 0 && !isFacingRight && !isSlowDown)
        {
            flip();
        }
        else if (inputMovement < 0 && isFacingRight && !isSlowDown)
        {
            flip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce));

        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        move(horizontalMove * Time.fixedDeltaTime);
    }

    public void setIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;

        animator.SetBool("IsJumping", !isGrounded);
    }

    public void move(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rigidBody.velocity.y);
        rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref m_velocity, m_MovementSmoothing);
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
