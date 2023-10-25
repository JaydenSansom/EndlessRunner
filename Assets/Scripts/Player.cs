using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public float maxSpeed;
    [SerializeField] float speedUpLerpValue;
    [SerializeField] float jumpHeight;
    [SerializeField] float slowDownForce;
    [SerializeField] WallOfDeath wallOfDeath;
    
    InputAction movementAction;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    
    bool canJump = false;
    bool damageSlowDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementAction = GetComponent<PlayerInput>().actions["move"];
        sprite = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.1f))
            canJump = true;
        else
            canJump = false;

        float dir = movementAction.ReadValue<float>();
        Debug.Log(dir);

        if (!damageSlowDown)
            rb.velocity = Vector2.Lerp(rb.velocity, new(dir * maxSpeed, rb.velocity.y), Time.fixedDeltaTime * speedUpLerpValue);
        else
            rb.velocity = Vector2.Lerp(rb.velocity, new((dir * maxSpeed)/2.0f, rb.velocity.y), Time.fixedDeltaTime * speedUpLerpValue);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            damageSlowDown = true;
            rb.AddForce(new(-(rb.velocity.x)/2.0f, 0), ForceMode2D.Impulse);
            Camera.main.GetComponent<CameraFollow>().SetDamageCameraTimer();
            wallOfDeath.DamageGainOnPlayer();
            StartCoroutine(DamageBlink());
        }

        if (collision.CompareTag("WallOfDeath"))
        {
            Debug.Log("Game Over >:(");
        }
    }

    IEnumerator DamageBlink()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.25f);
            sprite.color = new(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            yield return new WaitForSeconds(0.25f);
            sprite.color = new(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        damageSlowDown = false;
        yield return null;
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
