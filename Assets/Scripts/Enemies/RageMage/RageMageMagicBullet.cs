using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the bouncing logic of the magic
/// missile by the rage mage boss
/// 
/// Note: There is also fireball logic (A2) in here as their destroy
/// call is dependent on magic missiles
/// </summary>
public class RageMageMagicBullet : MonoBehaviour
{
    private Bullet _bullet;
    public int Bounces; //How many bounces until this obj is destroyed

    public GameObject PairedFireBall;

    private void Awake()
    {
        _bullet = GetComponent<Bullet>();
    }

    /// <summary>
    /// If a fireball reference was set then destroy that fireball
    /// when this magic missile is destroyed
    /// </summary>
    private void OnDestroy()
    {
        if (PairedFireBall != null)
        {
            Destroy(PairedFireBall);
        }
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
