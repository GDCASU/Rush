using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsect : KingFrogParent
{
    [SerializeField]
    private float chaseSeconds = 5.0f;

    [SerializeField]
    private float maxSpeed = 5.0f;

    [SerializeField]
    private float rotateSpeed = 3.0f;

    private float acceleration;

    private Rigidbody2D flyRB;

    private Vector3 flySpeed;

    private float timer;
    private float rotateAngle;

    private void OnEnable()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        flyRB = gameObject.GetComponent<Rigidbody2D>();

        flySpeed = Vector3.zero;
        acceleration = 6.0f;
        timer = 0;

        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        while(timer < chaseSeconds)
        {
            AngleInsect();

            MoveForward();

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        bool temp = true;

        while(temp)
        {
            //move until fly hits wall
            MoveForward();

            yield return new WaitForEndOfFrame();
        }
    }

    void MoveForward()
    {
        if (flyRB.velocity.magnitude < maxSpeed)
        {
            flyRB.velocity = transform.right * acceleration;
        }
        else
        {
            flyRB.velocity = transform.right;
        }
    }

    //angles the insect towards the player
    void AngleInsect()
    {
        float rotateX = myPlayer.transform.position.x - transform.position.x;
        float rotateY = myPlayer.transform.position.y - transform.position.y;
        rotateAngle = Mathf.Rad2Deg * Mathf.Atan2(rotateY, rotateX); //get angle in radians then turn to degrees

        //rotate along z-axis to face towards the player
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), rotateSpeed * Time.deltaTime);

        //flip sprite depending on player position
        if (myPlayer.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
