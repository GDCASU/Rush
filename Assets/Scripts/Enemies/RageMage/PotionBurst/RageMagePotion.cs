using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the potions used by the rage mage boss
/// </summary>
public class RageMagePotion : MonoBehaviour
{
    public float Radius;
    public float PotionDuration;
    private bool _destroy;

    public GameObject PotionBullet; //This is the bullet obj that will activate the potion spot
    public GameObject SpotSpriteObj;    //This is activated to display that the potion spot has been activated

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Activates the potion spot if hit by the potion bullet
        if(collision.gameObject == PotionBullet)
        {
            SpotSpriteObj.SetActive(true);

            Destroy(PotionBullet);
            PotionBullet = null;

            _destroy = true;

            PotionAction();
        }
    }

    protected virtual void Update()
    {
        //Destroys potion spot after duration
        if(_destroy && PotionDuration < 0)
            Destroy(gameObject);
        else if(_destroy)
            PotionDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Method called whenever the potion spot has been activated.
    /// This is meant to be inherited by children and modified there
    /// </summary>
    public virtual void PotionAction() { }

    /// <summary>
    /// Simple bool that determines if the player is in range to be hit by the spot
    /// </summary>
    /// <returns></returns>
    public bool PlayerInRange()
    {
        float playerDist = Vector2.Distance(PlayerHealth.singleton.transform.position, transform.position);

        return playerDist < Radius;
    }
}
