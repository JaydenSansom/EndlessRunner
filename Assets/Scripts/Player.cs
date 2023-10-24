using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float speedUpLerpValue;
    [SerializeField] float jumpHeight;
    
    InputAction movementAction;
    Rigidbody2D rb;
    
    bool canJump = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementAction = GetComponent<PlayerInput>().actions["move"];
    }

    public void FixedUpdate()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.1f))
            canJump = true;
        else
            canJump = false;

        float dir = movementAction.ReadValue<float>();

        rb.velocity = Vector2.Lerp(rb.velocity, new(dir * maxSpeed, rb.velocity.y), speedUpLerpValue);
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        if (canJump && context.action.ReadValue<float>() != 0)
        {
            rb.AddForce(new(0, jumpHeight), ForceMode2D.Impulse);
            canJump = false;
        }
    }
}
