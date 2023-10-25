using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    [SerializeField]
    float moveSpeed;

    Transform currentTrackPoint;

    bool damageTimer = false;

    private void Start()
    {
        currentTrackPoint = rightTrackPoint;
    }

    private void FixedUpdate()
    {
        float cameraMoveSpeed = Time.fixedDeltaTime * moveSpeed * Mathf.Abs(playerRB.velocity.x);
        if (cameraMoveSpeed <= 0)
            cameraMoveSpeed = Time.fixedDeltaTime * moveSpeed;

        if (!playerRB.gameObject.GetComponent<SpriteRenderer>().isVisible)
            transform.position = Vector3.Lerp(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), Time.deltaTime * moveSpeed * 2);

        if (damageTimer)
        {
            transform.position = Vector3.Lerp(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), cameraMoveSpeed);
            return;
        }

        if (playerRB.velocity.x > 0)
        {
            if (currentTrackPoint == rightTrackPoint)
                transform.position = Vector3.Lerp(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), cameraMoveSpeed);
        }
        else if (playerRB.velocity.x < 0)
        {
            if (currentTrackPoint == leftTrackPoint)
                transform.position = Vector3.Lerp(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), cameraMoveSpeed);
        }
        else
            transform.position = Vector3.Lerp(transform.position, new(currentTrackPoint.position.x, transform.position.y, transform.position.z), cameraMoveSpeed);
    }

    public void UpdateCurrentTrackPoint(GameObject side)
    {
        if (!damageTimer)
            currentTrackPoint = side == rightMax ? rightTrackPoint : leftTrackPoint;
    }

    IEnumerator DamageTimer(float waitSeconds)
    {
        damageTimer = true;
        currentTrackPoint = leftTrackPoint;
        yield return new WaitForSeconds(waitSeconds);
        damageTimer = false;
        yield return null;
    }

    public void SetDamageCameraTimer(float waitSeconds)
    {
        currentTrackPoint = leftTrackPoint;
        //StopAllCoroutines();
        StartCoroutine(DamageTimer(waitSeconds));
    }
}
