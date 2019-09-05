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
    public int lookingFrames;
    public int curlFrames;
    public int coolDownFrames;
    private SpriteRenderer srTail;
    private SpriteRenderer srBoss;
    private Transform player;
    private GameObject location;
    private GameObject tail;
    private int attacksPerformed=0;
    private float distance;
    private Vector2 origin;



    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        location = GetComponent<RatbossA0>().location;
        StartCoroutine(TailStab());
        
    }

    IEnumerator TailStab()
    {
        srBoss = location.GetComponentInChildren<SpriteRenderer>();
        if (srBoss.sprite.name == "rat_king_sprites_front") srBoss.sprite = backSprite;
        else srBoss.flipX = true;
        tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
        srTail = tail.GetComponent<SpriteRenderer>();
        tail.transform.parent = location.transform;
        tail.transform.localPosition= (srBoss.sprite.name != "rat_king_sprites_front"? new Vector3(0,0,1f): new Vector3(0, 0, 2f));
        while (attacksPerformed < 3)
        {
            
            srTail.sprite = extendedTail;
            Vector3 midVector = Vector3.Lerp(player.transform.position, location.transform.position, 0.5f);

            for (int i = 0; i < lookingFrames; i++)
            {
                tail.transform.LookAt(player.transform.position, Vector3.up);
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = chargedTail;
            for (int i = 0; i < curlFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = extendedTail;


            
            distance = Vector2.Distance(player.transform.position, location.transform.position);
            origin = new Vector2(midVector.x, midVector.y);

            var hits = Physics2D.BoxCastAll(origin, new Vector2(distance, .5f), 0, player.transform.position-midVector);

            var players = hits?.Where(x => x.transform.tag == "Player")?.Select(e => e.transform.GetComponent<PlayerHealth>());
            foreach (PlayerHealth player in players) player.takeDamage();

            for (int i = 0; i < coolDownFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            attacksPerformed++;
        }
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying) Gizmos.DrawLine(tail.transform.position,player.transform.position);
    }

}
