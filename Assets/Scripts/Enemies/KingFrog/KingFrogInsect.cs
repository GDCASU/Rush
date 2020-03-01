using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsect : MonoBehaviour
{
    [SerializeField]
    private float chaseSeconds = 5.0f;

    [SerializeField]
    private float maxSpeed = 10.0f;

    private float acceleration = 10.0f;

    private Vector3 vectorToTarget;
    private GameObject myPlayer;
    private Rigidbody2D flyRB;

    private Quaternion angle;

    private float timer;
    private float rotateAngle;

    private void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        flyRB = gameObject.GetComponent<Rigidbody2D>();

        timer = 0;

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(timer < chaseSeconds)
        {
            AngleInsect();

            ChasePlayer();

            yield return new WaitForEndOfFrame();
        }
    }

    void ChasePlayer()
    {
        if (flyRB.velocity.magnitude < maxSpeed)
            flyRB.velocity = transform.right * acceleration;
        else
            flyRB.velocity = transform.right;
    }

    void AngleInsect()
    {
        vectorToTarget = myPlayer.transform.position - transform.position;
        rotateAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, 100);
    }
}
