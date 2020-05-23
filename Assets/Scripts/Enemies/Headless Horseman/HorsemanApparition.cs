using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanApparition : MonoBehaviour
{
    [HideInInspector]
    public float speed = 1;

    private void Start() => GetComponent<BoxCollider2D>().enabled = true;
    private void Update()
    {
        transform.position += new Vector3(0, speed * -.25f, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag=="Player")
        {
            if (gameObject.name != "Horseman") Destroy(transform.gameObject);
        }

    }
}
