using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    public float RotationMaxAngle;
    public float RotationSpeed;

    [SerializeField]
    private float _rotationAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _rotationAngle = Mathf.Clamp(_rotationAngle + RotationSpeed * Time.deltaTime, -RotationMaxAngle, RotationMaxAngle);

        //transform.Rotate(0, 0, _rotationAngle, Space.Self);
        transform.localScale += new Vector3(_rotationAngle, 0, 0);

        if (RotationMaxAngle == Mathf.Abs(_rotationAngle))
        {
            RotationSpeed *= -1;
            print("swap direction");
        }
    }
}
