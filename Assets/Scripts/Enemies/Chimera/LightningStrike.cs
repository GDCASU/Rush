using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public SpriteRenderer lightningStrike;
    public CircleCollider2D col;
    public int delay;
    public int on;
    void Start()
    {
        StartCoroutine("strike");
    }

    IEnumerator strike()
    {
        for (int x = 0; x < delay; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        lightningStrike.enabled=true;
        col.enabled = true;
        for (int x = 0; x < on; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(transform.gameObject);

    }
}
