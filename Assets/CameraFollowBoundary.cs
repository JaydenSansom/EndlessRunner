using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBoundary : MonoBehaviour
{
    [SerializeField]
    CameraFollow followCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            followCam.UpdateCurrentTrackPoint(gameObject);

    }
}
