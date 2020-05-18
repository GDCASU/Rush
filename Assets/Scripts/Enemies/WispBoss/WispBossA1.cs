using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the seconf phase for the wisp boss. This involves controlling
/// the movement of the boss as well as the bullets that he will spawn
/// </summary>
public class WispBossA1 : BossAction
{
    public GameObject locationsParent;  //Parent obj that has children that will be used as referen
    public Transform[] BossLocations    //All the locations the boss can move to
    {
        get
        {
            return locationsParent.GetComponentsInChildren<Transform>();
        }
    }
    public float movementSpeed;
    public float damageToMove;

    public WispAttackModel[] bossAttacks;
    private WispAttackModel currentAttack;

    private Vector3 _targetPos;
    private bool _isMoving;
    private float _tmpDamageTaken = 0;
    private float _stopAdjuster = 1;  //This is the max distance to the target location to cause the boss to stop moving

    private void Start()
    {
        //Initial movement
        _targetPos = BossLocations[Random.Range(1, BossLocations.Length)].position; //I start at 1 because index 0 is the parent transform and that should not be used
        _isMoving = true;
        GetComponent<WispHealth>().canTakeDamage = false;
    }

    private void FixedUpdate()
    {
        if(_isMoving)
        {
            Vector3 distVector = _targetPos - transform.position;

            if(distVector.magnitude < _stopAdjuster)
            {
                _isMoving = false;
                currentAttack = bossAttacks[Random.Range(0, bossAttacks.Length)];
                currentAttack.StartAttacking();

                GetComponent<WispHealth>().canTakeDamage = true;
            }
            else
            {
                transform.position += distVector.normalized * movementSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Method called when damage is taken to the boss. 
    /// Determines whether the boss changes location or not
    /// </summary>
    /// <param name="damage">The amount of damage taken</param>
    public void OnTakenDamage(float damage)
    {
        _tmpDamageTaken += damage;

        if(_tmpDamageTaken >= damageToMove)
        {
            GetComponent<WispHealth>().canTakeDamage = false;

            if (currentAttack != null)
                currentAttack.StopAttacking();

            _tmpDamageTaken = 0;
            _targetPos = BossLocations[Random.Range(1, BossLocations.Length)].position; //I start at 1 because index 0 is the parent transform and that should not be used
            _isMoving = true;
        }
    }
}
