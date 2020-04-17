using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsectTEST : KingFrogParent
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private float chaseSeconds = 5.0f;

    //[SerializeField]
    private float maxSpeed = 5.0f;

    //[SerializeField]
    private float rotateSpeed = 12.0f;

    private Vector3 flySpeed;

    private float timer;
    private float rotateAngle;

    //new stuff
    private Vector3 accelX;
    private Vector3 accelY;
    private Vector3 deccelX;
    private Vector3 deccelY;

    private float accel;
    private float deccel;
    private float jitterX;
    private float jitterY;
    //

    private void OnEnable()
    {
        //new
        /*accelX = new Vector3(0.001f, 0, 0);
        accelY = new Vector3(0, 0.001f, 0);

        deccelX = new Vector3(0.01f, 0, 0);
        deccelY = new Vector3(0, 0.01f, 0);*/

        accel = 0.001f;
        deccel = 0.0023f;
        //

        myPlayer = GameObject.FindGameObjectWithTag("Player");

        flySpeed = Vector3.zero;
        timer = 0;

        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        while (timer < chaseSeconds)
        {
            AngleInsect();

            TrackPlayer();

            timer += Time.deltaTime;

            yield return ws;
        }
        timer = 0;

        while (timer < chaseSeconds)
        {
            //move until fly hits wall
            MoveForward();

            timer += Time.deltaTime;

            yield return ws;
        }

        Destroy(this);
        yield return ws;
    }

    void TrackPlayer()
    {
        //NEW
        //add jitter to bug movement
        jitterX = Random.Range(-0.0025f, 0.0025f);
        jitterY = Random.Range(-0.0025f, 0.0025f);
        if (transform.position.x < myPlayer.transform.position.x) //if fly left of player
        {
            if (flySpeed.x < maxSpeed) //if below max 'x' speed
            {
                if(flySpeed.x < 0) //speed up fast if 'x' speed < 0
                {
                    flySpeed.x += deccel + jitterX;
                }
                else //speed up normally
                {
                    flySpeed.x += accel + jitterX;
                }
            }
        }
        else //fly right of player
        {
            if(flySpeed.x > -maxSpeed) //speed not max
            {
                if(flySpeed.x > 0) //slow down fast if 'x' speed > 0
                {
                    flySpeed.x -= deccel + jitterX;
                }
                else //slow down normally
                {
                    flySpeed.x -= accel + jitterX;
                }
            }
        }

        if(transform.position.y < myPlayer.transform.position.y) //fly below player
        {
            if(flySpeed.y < maxSpeed) //if below max 'y' speed
            {
                if(flySpeed.y < 0) //speed up fast if 'y' speed < 0
                {
                    flySpeed.y += deccel + jitterY;
                }
                else //speed up normally
                {
                    flySpeed.y += accel + jitterY;
                }
            }
        }
        else
        {
            if(flySpeed.y > -maxSpeed) //speed not max
            {
                if(flySpeed.y > 0) //slow down fast if 'y' speed > 0
                {
                    flySpeed.y -= deccel + jitterY;
                }
                else //slow down normally
                {
                    flySpeed.y -= accel + jitterY;
                }
            }
        }
        transform.position += flySpeed; //update speed
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

    //move straight
    void MoveForward()
    {
        transform.position += flySpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == myPlayer)
        {
            myPlayer.GetComponent<PlayerHealth>().takeDamage();
        }
    }
}
