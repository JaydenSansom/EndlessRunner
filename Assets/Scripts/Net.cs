using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField, Tooltip("the direction this net is moving")]private Vector3 heading;
    [SerializeField, Tooltip("the speed this net moves at")]private float speed = 3;
    [SerializeField, Tooltip("wall of death tag")]private string WallOfDeath = "WallOfDeath";
    [SerializeField, Tooltip("vine tag")]private string Vine = "Vine";
    // Start is called before the first frame update
    void Start()
    {
        heading = heading.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += heading.normalized * speed * Time.fixedDeltaTime;
    }

    public void SetHeading(Vector3 newHeading){
        heading = newHeading.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag != WallOfDeath && other.gameObject.tag != Vine){
            Destroy(gameObject);
        }
    }
}
