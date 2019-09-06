using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RatbossA2 : MonoBehaviour
{
    public GameObject tailPrefab;
    public Sprite chargedTail;
    public Sprite extendedTail;
    public Sprite currentSprite;
    public Sprite backSprite;
    public Sprite frontSprite;
    public int lookingFrames;
    public int curlFrames;
    public int coolDownFrames;
    private SpriteRenderer srTail;
    private SpriteRenderer srBoss;
    private Transform player;
    private GameObject tail;
    private float distance;
    private Vector2 origin;

    private RatbossA0 A0;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        A0 = GetComponent<RatbossA0>();
    }

    private void OnEnable() => StartCoroutine(TailStab());

    IEnumerator TailStab()
    {
        yield return A0.shakedoors();

        srBoss = A0.location.GetComponentInChildren<SpriteRenderer>();
        if (srBoss.sprite.name == "rat_king_sprites_front") srBoss.sprite = backSprite;
        else srBoss.flipX = true;
        if(tail==null) tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
        srTail = tail.GetComponent<SpriteRenderer>();
        srTail.enabled = true;
        tail.transform.parent = A0.location.transform;
        tail.transform.localPosition = (srBoss.sprite.name != "rat_king_sprites_front" ? new Vector3(0, 0, 1f) : new Vector3(0, 0, 2f));
        int attacksPerformed = 0;
        while (attacksPerformed < 3)
        {


            for (int i = 0; i < lookingFrames; i++)
            {
                var VectorToPlayer = (Vector2)(PlayerHealth.singleton.transform.position - tail.transform.position).normalized;
                tail.transform.rotation = Quaternion.identity;
                tail.transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, VectorToPlayer)));
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = chargedTail;

            Vector3 midVector = Vector3.Lerp(player.transform.position, A0.location.transform.position, 0.5f);
            distance = Vector2.Distance(player.transform.position, A0.location.transform.position);
            origin = new Vector2(midVector.x, midVector.y);

            var hits = Physics2D.BoxCastAll(origin, new Vector2(distance, .1f), 0, player.transform.position - midVector);
            for (int i = 0; i < curlFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = extendedTail;

            var players = hits?.Where(x => x.transform.tag == "Player")?.Select(e => e.transform.GetComponent<PlayerHealth>());
            foreach (PlayerHealth playerH in players) if (!playerH.inv) playerH.takeDamage();

            for (int i = 0; i < coolDownFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            attacksPerformed++;
        }
        if (srBoss.sprite.name == "rat_king_sprites_back") srBoss.sprite = frontSprite; srBoss.flipX = false;
        srTail.enabled = false;
        yield return A0.returnIntoDoor();
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying && tail != null) Gizmos.DrawLine(tail.transform.position, player.transform.position);
    }
}

