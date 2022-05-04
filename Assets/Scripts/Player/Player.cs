using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 2;
    public float runSpeed = 8;
    public float jumpHeight = 2;

    private Animator animator;
    private CharacterController controller;

    private const float gravity = -9.8f;
    private bool isGrounded;
    private float jumpSpeed;

    private Vector3 playerVelocity;

    //recieved from controller
    private float vertical;
    private bool isRunning;
    private bool spacePressed;

    public PlayerController pc;
    public PlayerCloth cloth;

    [NonSerialized] public int number;
    [NonSerialized] public bool isControlled;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //using physics formula
        jumpSpeed = (float)Math.Sqrt(jumpHeight * -2 * gravity);
    }

    void Update()
    {
        Move();

        HandleAnimations();
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        Vector3 move = new Vector3(0, 0, vertical);
        move = transform.TransformDirection(move);
        move = (isRunning ? runSpeed : moveSpeed) * move;
        if (isGrounded)
        {
            playerVelocity.x = move.x;
            playerVelocity.z = move.z;
        }
        VerticalMove();

        controller.Move(playerVelocity * Time.deltaTime);
    }

    void VerticalMove()
    {
        playerVelocity.y = playerVelocity.y + gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -0.1f;

        bool isJumping = isGrounded && spacePressed;
        if (isJumping)
            playerVelocity.y = jumpSpeed;

    }

    public void receiveData(float vertical, bool isRunning, bool spacePressed)
    {
        this.vertical = vertical;
        this.isRunning = isRunning;
        this.spacePressed = spacePressed;
    }

    void HandleAnimations()
    {
        animator.SetFloat("walkSpeed", (vertical + 1) / 2);
        animator.SetFloat("ySpeed", playerVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
    }
}


