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
    float percentOfSpeedDamaged;

    [SerializeField]
    float maxDistanceSlower;

    [SerializeField]
    float distWhileDamaged;

    [SerializeField]
    int headStartSeconds;

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
        if (!GameManager.Instance.isPlaying())
            return;

        if (headStart)
            return;

        if (!playerDamaged && playerRB.velocity.x > 0)
        {
            if (Vector2.Distance(playerRB.position, wallRB.position) >= maxDistanceSlower)
                wallRB.position = new(playerRB.position.x - maxDistanceSlower, wallRB.position.y);
            else
                wallRB.velocity = new(playerRB.velocity.x * percentOfPlayerSpeed, wallRB.velocity.y);
        }
        else if (playerDamaged || (int) playerRB.velocity.x < .1f * playerRB.gameObject.GetComponent<Player>().maxSpeed)
            wallRB.velocity = new(playerRB.gameObject.GetComponent<Player>().maxSpeed * percentOfSpeedDamaged, wallRB.velocity.y);
    }

    public void DamageGainOnPlayer(float waitSeconds)
    {
        if (playerRB.position.x - wallRB.position.x > distWhileDamaged)
            wallRB.position = new(playerRB.position.x - distWhileDamaged, wallRB.position.y);

        //StopAllCoroutines();
        StartCoroutine(PlayerDamaged(waitSeconds));
    }

    IEnumerator PlayerDamaged(float waitSeconds)
    {
        playerDamaged = true;
        yield return new WaitForSeconds(waitSeconds);
        playerDamaged = false;
        yield return null;
    }

    IEnumerator GiveHeadStart()
    {
        headStart = true;
        yield return new WaitForSeconds(headStartSeconds);
        headStart = false;
        yield return null;
    }
}
