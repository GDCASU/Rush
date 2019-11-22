using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerWall : MonoBehaviour
{
    public GameObject wall;
    public bool moveInX;

    private void Update()
    {
        if (moveInX) gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(wall.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), 0.1f);
        else gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, wall.transform.position.y, gameObject.transform.position.z), 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().takeDamage();
            Destroy(gameObject);
        }
    }
}

