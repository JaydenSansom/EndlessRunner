using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D playerRB;

    [SerializeField]
    float percentOfPlayerSpeed;

    [SerializeField]
    float maxDistanceSlower;

    Rigidbody2D wallRB;
    bool playerDamaged;
    bool headStart;

    private void Start()
    {
        wallRB = GetComponent<Rigidbody2D>();
        StartCoroutine(GiveHeadStart());
    }

    private void Update()
    {
        if (headStart)
            return;

        if (playerRB.velocity.x > 0)
        {
            if (Vector2.Distance(playerRB.position, wallRB.position) >= maxDistanceSlower)
                wallRB.position = new(playerRB.position.x - maxDistanceSlower, wallRB.position.y);
            else
                wallRB.velocity = new(playerRB.velocity.x * percentOfPlayerSpeed, wallRB.velocity.y);
        }
        else if (playerDamaged || (int) playerRB.velocity.x == 0)
            wallRB.velocity = new(playerRB.gameObject.GetComponent<Player>().maxSpeed * percentOfPlayerSpeed, wallRB.velocity.y);
    }

    public void DamageGainOnPlayer()
    {
        wallRB.position = new(playerRB.position.x - maxDistanceSlower/2, wallRB.position.y);
        StartCoroutine(PlayerDamaged());
    }

    IEnumerator PlayerDamaged()
    {
        playerDamaged = true;
        yield return new WaitForSeconds(1);
        playerDamaged = false;
        yield return null;
    }

    IEnumerator GiveHeadStart()
    {
        headStart = true;
        yield return new WaitForSeconds(1);
        headStart = false;
        yield return null;
    }
}
