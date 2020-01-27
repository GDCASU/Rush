using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    [Header("Turret Travel")]
    public GameObject targetLocationObj;
    public float movementSpeed;
    private Vector3 NormalizedTargetVector
    {
        get
        {
            Vector3 rval = targetLocationObj.transform.position - transform.position;
            rval = rval.normalized;

            return rval;
        }
    }
    private bool _isMoving = false;

    private void Start()
    {
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if(_isMoving)
        {
            transform.position += NormalizedTargetVector * movementSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Location"))
            _isMoving = false;
    }
}
