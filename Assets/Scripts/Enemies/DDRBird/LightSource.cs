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

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_maxRotation * Mathf.Sin(Time.time * _speed) * _direction, 0f, 0);
    }
}
