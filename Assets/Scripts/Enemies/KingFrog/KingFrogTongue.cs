using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogTongue : MonoBehaviour
{
    [SerializeField]
    private GameObject kingFrogObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            kingFrogObject.GetComponent<KingFrogTongueAttack>().hitPlayer = true;
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage();
        }
    }
}
