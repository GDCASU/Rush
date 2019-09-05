using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class RatbossA2 : MonoBehaviour
{
    private Transform player;
    private GameObject location;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        location = GetComponent<RatbossA0>().location;
        StartCoroutine(TailStab());
        
    }

    IEnumerator TailStab()
    {
        Vector3 midVector = Vector3.Lerp(player.transform.position, location.transform.position, 0.5f);
        float distance = Vector2.Distance(player.transform.position, location.transform.position);
        Vector2 origin = new Vector2(midVector.x, midVector.y);

        var hits = Physics2D.BoxCastAll(origin, new Vector2(distance,3), 0 ,player.transform.position);

        var players = hits?.Where(x => x.transform.tag == "Player")?.Select(e => e.transform.GetComponent<PlayerHealth>());
        foreach (PlayerHealth enemy in players) print("Sup");

        yield return new WaitForEndOfFrame();
    }

}
