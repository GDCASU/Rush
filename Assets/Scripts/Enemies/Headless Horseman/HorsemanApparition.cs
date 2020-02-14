using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanApparition : MonoBehaviour
{
    private void Start() => GetComponent<BoxCollider2D>().enabled = true;
    private void Update()
    {
        transform.position += new Vector3(0, -.5f, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag=="Player")
        {
            if (gameObject.name == "Horseman") enabled = false;
            else Destroy(transform.gameObject);
        }

    }
}
