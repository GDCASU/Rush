using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public Transform targetTransform;
    public float speed;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate ()
    {
        chase();
	}
    void chase()
    {
        Vector3 transformDistance = targetTransform.position - transform.position;
        Vector3 directionToTarget = transformDistance.normalized;
        Vector3 velocity = directionToTarget * speed;
        float distanceToTarget = transformDistance.magnitude;

        if (distanceToTarget > 2f)
        {
            rb.velocity = velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("wtf");
    }
}
