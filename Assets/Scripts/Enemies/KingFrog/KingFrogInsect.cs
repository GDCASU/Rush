using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsect : KingFrogParent
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private float chaseSeconds = 5.0f;

    [SerializeField]
    private float maxSpeed = 15.0f;

    [SerializeField]
    private float rotateSpeed = 3.0f;

    private float acceleration;

    private Rigidbody2D rb;

    private Vector3 flySpeed;

    private float timer;
    private float rotateAngle;

    private void OnEnable()
    {

        myPlayer = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();

        flySpeed = Vector3.zero;
        acceleration = 10.0f;
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

            yield return ws;
        }
        timer = 0;

        while(timer < chaseSeconds)
        {
            //move until fly hits wall
            MoveForward();

            timer += Time.deltaTime;

            yield return ws;
        }

        Destroy(this);
        yield return ws;
    }

    void MoveForward()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity = transform.right * acceleration;
        }
        else
        {
            rb.velocity = transform.right;
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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            myPlayer.GetComponent<PlayerHealth>().takeDamage();
        }
    }*/
}
