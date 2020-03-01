using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogTongueAttack : KingFrogParent
{
    [SerializeField]
    private GameObject tongueObject; //gameobject TongueContainer
    [SerializeField]
    private float maxTongueDistance = 15.0f; //max distance tongue can reach
    [SerializeField]
    private float tongueSpeed = 15.0f; //speed of tongue

    public bool hitPlayer; //checks if player was hit

    private float scaleX; //gets tongue's X scale
    private float amountToChange; //math of tongueSpeed * Time.deltaTime;
    private Vector3 tempAngle; //gets angle of tongue when facing player

    private void OnEnable()
    {
        actionRunning = true;

        hitPlayer = false;

        AngleTongue();

        StartCoroutine(TongueAttack());
    }

    IEnumerator TongueAttack()
    {
        SetVars();

        while(scaleX < maxTongueDistance && !hitPlayer)
        {
            Grow();
            yield return new WaitForEndOfFrame();
        }

        while(scaleX > 1)
        {
            Shrink();
            yield return new WaitForEndOfFrame();
        }

        actionRunning = false;
    }

    void Grow()
    {
        SetVars();
        tongueObject.transform.localScale = new Vector3(scaleX + amountToChange, 1, 0); //increeased tongue X scale
    }

    void Shrink()
    {
        SetVars();
        tongueObject.transform.localScale = new Vector3(scaleX - amountToChange, 1, 0); //decreases tongue Y scale
    }

    void SetVars()
    {
        scaleX = tongueObject.transform.localScale.x;
        amountToChange = tongueSpeed * Time.deltaTime;
    }

    void AngleTongue()
    {
        tongueObject.transform.LookAt(myPlayer.transform, Vector3.left);
        tempAngle = tongueObject.transform.rotation.eulerAngles;
        tempAngle.x = 0;
        tempAngle.y = 0;
        tongueObject.transform.rotation = Quaternion.Euler(tempAngle); //sets tongue X and Y rotation to 0 but keeps Z the same
    }
}
