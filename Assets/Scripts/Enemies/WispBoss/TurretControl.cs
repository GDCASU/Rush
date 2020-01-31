using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will control the WispBoss movement and fire.
/// 
/// Created by Nicholas Nguyen
/// </summary>
public class TurretControl : MonoBehaviour
{
    private BulletSpawner bulletSpawner
    {
        get
        {
            return GetComponent<BulletSpawner>();
        }
    }

    [Header("Turret Travel")]
    public GameObject targetLocationObj;
    public float movementSpeed;
    private Vector3 NormalizedTargetVector  //Normalized vector that points from this obj to the target obj
    {
        get
        {
            Vector3 rval = targetLocationObj.transform.position - transform.position;
            rval = rval.normalized;

            return rval;
        }
    }
    public Vector3 startingDirection;   //This is the initial direction for the arch feature
    private Vector3 _currentDirection;
    public float archModifier;  //Decrease to create less arch and Increase for the opposite. Neutral value is 1
    public bool isMoving;

    private void Awake()
    {
        _currentDirection = startingDirection;
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            Vector3 newPos = NormalizedTargetVector + _currentDirection;
            _currentDirection = newPos * archModifier;

            transform.position += newPos.normalized * movementSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Once the turret has reached it's target location stop it from moving
        if (collision.gameObject.name.Equals(targetLocationObj.name))
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Public method that tells the bullet spawner to start
    /// shooting
    /// </summary>
    public void StartShooting()
    {
        //This exact line can be changed. The turrets just need to start shooting at this point
        bulletSpawner.bulletAmount = 1;
    }
}
