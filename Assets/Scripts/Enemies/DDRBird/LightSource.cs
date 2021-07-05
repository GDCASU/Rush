using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private int _maxRotation = 45;

    [SerializeField]
    private int _direction = 1;

    [SerializeField]
    private bool rotateX;

    [SerializeField]
    private bool rotateY;

    [SerializeField]
    private bool rotateZ;

    private void Update()
    {
        float xRotation = 0;
        float yRotation = 0;
        float zRotation = 0;

        if (rotateX) xRotation = _maxRotation * Mathf.Sin(Time.time * _speed) * _direction;
        if (rotateY) yRotation = _maxRotation * Mathf.Sin(Time.time * _speed) * _direction;
        if (rotateZ) zRotation = _maxRotation * Mathf.Sin(Time.time * _speed) * _direction;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }
}
