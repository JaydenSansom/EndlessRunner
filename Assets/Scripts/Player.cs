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
    [SerializeField] float damageWaitForSeconds;
    [SerializeField] WallOfDeath wallOfDeath;
    [SerializeField] Canvas gameOver;
    
    InputAction movementAction;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    
    bool canJump = false;
    bool damageSlowDown = false;
    public bool swinging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementAction = GetComponent<PlayerInput>().actions["move"];
        sprite = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 2f))
            canJump = true;
        else
            canJump = false;

        float dir = movementAction.ReadValue<float>();
        //Debug.Log(dir);

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
            Camera.main.GetComponent<CameraFollow>().SetDamageCameraTimer(damageWaitForSeconds);
            wallOfDeath.DamageGainOnPlayer(damageWaitForSeconds);
            //StopAllCoroutines();
            StartCoroutine(DamageBlink());
        }

        if (collision.CompareTag("WallOfDeath"))
        {
            Debug.Log("Game Over >:(");
            gameOver.enabled = true;
        }
    }

    IEnumerator DamageBlink()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(damageWaitForSeconds / 8.0f);
            sprite.color = new(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            yield return new WaitForSeconds(damageWaitForSeconds / 8.0f);
            sprite.color = new(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        damageSlowDown = false;
        yield return null;
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        if (swinging && context.action.ReadValue<float>() != 0)
        {
            GetComponentInParent<Vine>().ExitVine(this);
            return;
        }

        if (canJump && context.action.ReadValue<float>() != 0)
        {
            rb.AddForce(new(0, jumpHeight), ForceMode2D.Impulse);
            canJump = false;
        }
    }
}
