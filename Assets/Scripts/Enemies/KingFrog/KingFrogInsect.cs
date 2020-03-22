using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsect : MonoBehaviour
{
    [SerializeField]
    private float chaseSeconds = 5.0f;

    //[SerializeField]
    private float maxSpeed = 5.0f;

    //[SerializeField]
    private float rotateSpeed = 3.0f;

    private float acceleration =  2.0f;

    private GameObject myPlayer;
    private Rigidbody2D flyRB;

    private Vector3 flySpeed;

    private float timer;
    private float rotateAngle;

    private void OnEnable()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        flyRB = gameObject.GetComponent<Rigidbody2D>();

        flySpeed = Vector3.zero;

        timer = 0;

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(timer < chaseSeconds)
        {
            AngleInsect();

            ChasePlayer();

            //timer += timer + Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    void ChasePlayer()
    {
        Debug.Log(flyRB.velocity.magnitude);
        if (flySpeed.magnitude < maxSpeed)
        {
            flySpeed += transform.right * acceleration;
            flyRB.velocity = flySpeed;
        }
        else
        {
            //flyRB.velocity = transform.right;
        }
    }

    void AngleInsect()
    {
        float rotateX = myPlayer.transform.position.x - transform.position.x;
        float rotateY = myPlayer.transform.position.y - transform.position.y;
        rotateAngle = Mathf.Rad2Deg * Mathf.Atan2(rotateY, rotateX); //get angle in radians then turn to degrees

        //rotate along z-axis to face the x-axis towards the player
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
