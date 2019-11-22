using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that follows a given objects transform
/// </summary>
public class FollowObject : MonoBehaviour
{
    public float xOffSet;
    public float yOffSet = -1f;
    public float zOffSet;
    public Transform objTransform;

    private void Update()
    {
        Vector3 newPosition = this.transform.position;
        Vector3 objPosition = objTransform.position;

        newPosition.x = objPosition.x + xOffSet;
        newPosition.y = objPosition.y + yOffSet;
        newPosition.z = objPosition.z + zOffSet;

        this.transform.position = newPosition;
    }
}
