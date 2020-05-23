using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" && GetComponentInParent<Dash>().thisAction)
        {
            GetComponentInParent<Dash>().move = false;
        }
        if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player") && GetComponentInParent<HorsemanSpecial>().enabled)
        {
            if (transform.parent.name == "Horseman") GetComponentInParent<HorsemanApparition>().enabled = false;
        }
    }
}
