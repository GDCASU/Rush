using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChimeraA3 : BossAction
{
    public GameObject tailPrefab;
    public Sprite chargedTail;
    public Sprite extendedTail;
    public int lookingFrames;
    public int curlFrames;
    private SpriteRenderer srTail;
    private GameObject tail;
    private float distance;
    private Vector2 origin;
    private void OnEnable()
    {
        StartCoroutine("stab");
    }

    IEnumerator stab()
    {
        if (tail == null) tail = Instantiate(tailPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
        tail.transform.localPosition = tail.transform.localPosition + new Vector3(2, 0,0);
        tail.transform.localScale = new Vector3(.75f, .75f, .75f);
        srTail = tail.GetComponent<SpriteRenderer>();
        tail.transform.parent =transform;
        srTail.sprite = extendedTail;
        for (int i = 0; i < lookingFrames; i++)
        {
            var VectorToPlayer = (Vector2)(PlayerHealth.singleton.transform.position - tail.transform.position).normalized;
            tail.transform.rotation = Quaternion.identity;
            tail.transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(-Vector2.right, VectorToPlayer)));
            yield return new WaitForEndOfFrame();
        }
        srTail.sprite = chargedTail;
        tail.transform.localPosition = tail.transform.localPosition + new Vector3(-2, 0, 0);
        tail.transform.localScale=new Vector3(.5f,.5f,.5f);
        tail.transform.Rotate(new Vector3(0, 180, 0),Space.Self);
        Vector3 midVector = Vector3.Lerp(PlayerHealth.singleton.transform.position, transform.position, 0.5f);
        distance = Vector2.Distance(PlayerHealth.singleton.transform.position, transform.position);
        origin = new Vector2(midVector.x, midVector.y);

        var hits = Physics2D.BoxCastAll(origin, new Vector2(distance, .1f), 0, PlayerHealth.singleton.transform.position - midVector);
        for (int i = 0; i < curlFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        tail.transform.localPosition = tail.transform.localPosition + new Vector3(2, 0, 0);
        tail.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
        srTail.sprite = extendedTail;
        tail.transform.localScale = new Vector3(.75f, .75f, .75f);

        var players = hits?.Where(x => x.transform.tag == "Player")?.Select(e => e.transform.GetComponent<PlayerHealth>());
        foreach (PlayerHealth playerH in players) if (!playerH.inv && distance < 4) playerH.takeDamage();
        Destroy(tail);
        actionRunning = false;
    }
}


