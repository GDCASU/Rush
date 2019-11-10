using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that follows a given objects transform in
/// terms of x and y ONLY
/// </summary>
public class FollowObject : MonoBehaviour
{
    public float xOffSet;
    public float yOffSet = -1f;
    public Transform objTransform;

    private void Update()
    {
        Vector3 newPosition = this.transform.position;
        Vector3 objPosition = objTransform.position;

        newPosition.x = objPosition.x + xOffSet;
        newPosition.y = objPosition.y + yOffSet;

        this.transform.position = newPosition;
    }
}
