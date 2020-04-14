using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogTongueAttack : KingFrogParent
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private GameObject tongueObject; //gameobject TongueContainer
    [SerializeField]
    private float maxTongueDistance = 15.0f; //max distance tongue can reach
    [SerializeField]
    private float tongueSpeed = 15.0f; //speed of tongue

    [SerializeField]
    private float CoolDown = 1.0f;

    public bool hitPlayer; //checks if player was hit

    private float scaleX; //gets tongue's X scale
    private float startScaleX;
    private float amountToChange; //math of tongueSpeed * Time.deltaTime;
    private float rotateAngle;

    private void OnEnable()
    {
        actionRunning = true;

        myPlayer = GameObject.FindGameObjectWithTag("Player");

        hitPlayer = false;
        startScaleX = transform.localScale.x;

        StartCoroutine(TongueAttack());
    }

    IEnumerator TongueAttack()
    {
        AngleTongue();

        while (scaleX < maxTongueDistance && !hitPlayer)
        {
            Grow();
            yield return ws;
        }

        while(scaleX > startScaleX)
        {
            Shrink();
            yield return ws;
        }

        Invoke("EndAction", CoolDown);
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
        scaleX = tongueObject.transform.localScale.x; //gets scale of tongue
        amountToChange = tongueSpeed * Time.deltaTime; //amount to change tongue scale by
    }

    void AngleTongue()
    {
        /*found here: https://answers.unity.com/questions/760900/how-can-i-rotate-a-gameobject-around-z-axis-to-fac.html then modified*/

        float rotateX = myPlayer.transform.position.x - tongueObject.transform.position.x;
        float rotateY = myPlayer.transform.position.y - tongueObject.transform.position.y;
        rotateAngle = Mathf.Rad2Deg * Mathf.Atan2(rotateY, rotateX); //get angle in radians then turn to degrees

        //rotate object
        tongueObject.transform.rotation = Quaternion.Euler(0, 0, rotateAngle);
    }

    void EndAction()
    {
        actionRunning = false;
    }
}
