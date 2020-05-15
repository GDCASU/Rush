using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsect : KingFrogParent
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private float chaseSeconds = 5.0f;

    [SerializeField]
    private float maxSpeed = 5.0f;

    [SerializeField]
    private float rotateSpeed = 12.0f;

    private Vector3 flySpeed;

    private float timer;
    private float rotateAngle;

    //new stuff
    private Vector3 accelX;
    private Vector3 accelY;
    private Vector3 deccelX;
    private Vector3 deccelY;

    [SerializeField]
    private float accel = 1f;
    [SerializeField]
    private float deccel = 2f;
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
        accel *= 0.001f;
        deccel *= 0.001f;
        maxSpeed = 0.75f / maxSpeed;
        
        
        //

        myPlayer = GameObject.FindGameObjectWithTag("Player");

        GameObject.Find("KingFrog").GetComponent<KingFrogInsectAttack>().totalCount++;

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
        SetFlyStraight();

        while (timer < (chaseSeconds / 2))
        {
            //move until fly hits wall
            MoveForward();

            timer += Time.deltaTime;

            yield return ws;
        }

        Despawn();
        yield return ws;
    }

    void TrackPlayer()
    {
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

        if(transform.position.y < myPlayer.transform.position.y) //if fly below player
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
        else //fly is above player
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

    //check fly speed when tracking is stopped
    //if slow speed up the fly
    void SetFlyStraight()
    {
        //if fly is slower than HALF max speed
        if (flySpeed.x < (maxSpeed / 2) && flySpeed.x > -(maxSpeed / 2))
        {
            //if fly is moving positive or negative make speed = maxSpeed / 2
            if (flySpeed.x > 0)
            {
                flySpeed.x = (maxSpeed / 2);
            }
            else //fly x speed is < 0
            {
                flySpeed.x = -(maxSpeed / 2);
            }
        }

        //if fly is slower than HALF max speed
        if (flySpeed.y < (maxSpeed / 2) && flySpeed.x > -(maxSpeed / 2))
        {
            //if fly is moving positive or negative make speed = maxSpeed / 2
            if (flySpeed.y > 0)
            {
                flySpeed.y = (maxSpeed / 2);
            }
            else //if fly y speed < 0
            {
                flySpeed.y = -(maxSpeed / 2);
            }
        }
    }

    void Despawn()
    {
        try
        {
            GameObject.Find("KingFrog").GetComponent<KingFrogInsectAttack>().totalCount--;
        }
        catch { }
        
        Destroy(this.gameObject);
    }
}
