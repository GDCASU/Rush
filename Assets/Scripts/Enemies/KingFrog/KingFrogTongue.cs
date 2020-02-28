using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogTongue : KingFrogParent
{
    [SerializeField]
    private GameObject tongueObject;

    [SerializeField]
    private float maxTongueDistance = 15.0f;

    [SerializeField]
    private float tongueSpeed = 15.0f;

    private bool hitPlayer;

    private void Start()
    {
        hitPlayer = false;

        tongueObject.transform.Rotate(myPlayer.transform.position);

        StartCoroutine(TongueAttack());
    }

    IEnumerator TongueAttack()
    {
        while(tongueObject.transform.localScale.x < maxTongueDistance)
        {
            tongueObject.transform.localScale = new Vector3(tongueObject.transform.localScale.x + (tongueSpeed * Time.deltaTime), 1, 0);
            yield return new WaitForEndOfFrame();
        }

        while(tongueObject.transform.localScale.x > 1)
        {
            tongueObject.transform.localScale = new Vector3(tongueObject.transform.localScale.x - (tongueSpeed * Time.deltaTime), 1, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
