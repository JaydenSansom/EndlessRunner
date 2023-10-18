using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] 
    GameObject player;

    bool playerMaxDist = false;
    Rigidbody2D rb;
    Rigidbody2D playerRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMaxDist)
            rb.velocity = new(playerRb.velocity.x, rb.velocity.y);
        else
            rb.velocity = new(Mathf.Lerp(rb.velocity.x, playerRb.velocity.x, Time.fixedDeltaTime), rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerMaxDist = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerMaxDist = false;
    }
}
