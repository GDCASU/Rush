using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the bouncing logic of the magic
/// missile by the rage mage boss
/// </summary>
public class RageMageMagicBullet : MonoBehaviour
{
    private Bullet _bullet;
    public int Bounces; //How many bounces until this obj is destroyed

    private void Awake()
    {
        _bullet = GetComponent<Bullet>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (Bounces > 0)
                Bounces--;
            else
                Destroy(gameObject);
            
            _bullet.MoveVector = Vector2.Reflect(_bullet.MoveVector, collision.transform.right);
        }
    }
}
