using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vine : MonoBehaviour
{
    private VineSegment currentSegment = null;

    [SerializeField]
    float speedBoost;

    HingeJoint2D playerJoint = null;

    public void EnterVine(Player player, VineSegment segment)
    {
        if (playerJoint != null)
            return;

        currentSegment = segment;

        currentSegment.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;

        Rigidbody2D[] children = GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D child in children)
        {
            child.angularVelocity = currentSegment.GetComponent<Rigidbody2D>().angularVelocity;
        }

        player.GetComponent<Animator>().SetBool("Swinging", true);

        playerJoint = player.AddComponent<HingeJoint2D>();
        playerJoint.connectedBody = currentSegment.GetComponent<Rigidbody2D>();
        player.transform.parent = transform;
        player.swinging = true;
    }

    public void ExitVine(Player player)
    {
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        Destroy(playerJoint);
        player.transform.parent = null;
        playerRB.velocity = new(Mathf.Min(playerRB.velocity.x * speedBoost + currentSegment.GetComponent<Rigidbody2D>().velocity.x, player.maxSpeed * speedBoost), 
                                Mathf.Min(playerRB.velocity.y + currentSegment.GetComponent<Rigidbody2D>().velocity.y, player.maxSpeed * speedBoost));
        player.swinging = false;
        currentSegment = null;
        StartCoroutine(CollisionDisableTimer());
    }

    IEnumerator CollisionDisableTimer()
    {
        BoxCollider2D[] children = GetComponentsInChildren<BoxCollider2D>();

        foreach (BoxCollider2D child in children)
        {
            child.enabled = false;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (BoxCollider2D child in children)
        {
            child.enabled = true;
        }

        yield return null;
    }
}
