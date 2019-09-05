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
    public int lookingFrames;
    public int curlFrames;
    public int coolDownFrames;
    private SpriteRenderer srTail;
    private SpriteRenderer srBoss;
    private Transform player;
    private GameObject location;
    private int attacksPerformed=0;




    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        location = GetComponent<RatbossA0>().location;
        StartCoroutine(TailStab());
        
    }

    IEnumerator TailStab()
    {
        while (attacksPerformed < 3)
        {
            srBoss = location.GetComponentInChildren<SpriteRenderer>();
            srBoss.flipX = true;
            GameObject tail = Instantiate(tailPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
            srTail = tail.GetComponent<SpriteRenderer>();
            srTail.sprite = extendedTail;
            tail.transform.SetParent(location.transform);
            tail.transform.position = location.transform.position + new Vector3(2f, 0, 0);
            //tail.transform.rotation= location.transform.rotation;


            for (int i = 0; i < lookingFrames; i++)
            {
                tail.transform.up = new Vector3(-player.position.x, player.position.y);
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = chargedTail;
            for (int i = 0; i < curlFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            srTail.sprite = extendedTail;


            Vector3 midVector = Vector3.Lerp(player.transform.position, location.transform.position, 0.5f);
            float distance = Vector2.Distance(player.transform.position, location.transform.position);
            Vector2 origin = new Vector2(midVector.x, midVector.y);

            var hits = Physics2D.BoxCastAll(origin, new Vector2(distance, 3), 0, player.transform.position);

            var players = hits?.Where(x => x.transform.tag == "Player")?.Select(e => e.transform.GetComponent<PlayerHealth>());
            foreach (PlayerHealth enemy in players) print("You got Hit");

            for (int i = 0; i < coolDownFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            attacksPerformed++;
        }
    }

}
