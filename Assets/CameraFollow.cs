using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D playerRB;

    [SerializeField]
    Transform leftTrackPoint;

    [SerializeField]
    Transform rightTrackPoint;

    [SerializeField]
    GameObject rightMax;

    Transform currentTrackPoint;

    private void Start()
    {
        currentTrackPoint = rightTrackPoint;
    }

    private void FixedUpdate()
    {
        if (currentTrackPoint == rightTrackPoint && playerRB.velocity.x > 0) 
            transform.position = Vector3.MoveTowards(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), Time.fixedDeltaTime * 20);
        else if (currentTrackPoint == leftTrackPoint && playerRB.velocity.x < 0)
            transform.position = Vector3.MoveTowards(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), Time.fixedDeltaTime * 20);
    }

    public void UpdateCurrentTrackPoint(GameObject side)
    {
        currentTrackPoint = side == rightMax ? rightTrackPoint : leftTrackPoint;
    }
}
