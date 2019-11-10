using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class meant for the lady of the lake boss that
/// controls the camera creating the player and other objects reflections
/// </summary>
public class CameraReflection : MonoBehaviour
{
    public float yOffSet = 2f;
    public Transform playerCamera;

    private void FixedUpdate()
    {
        Debug.Log(string.Format("This y = {0}, Player y = {1}", this.transform.position.y, playerCamera.position.y));

        Vector3 thisPosition = this.transform.position;
        Vector3 playerCamPosition = playerCamera.localPosition;

        thisPosition.y = playerCamPosition.y + yOffSet;

        this.transform.position = thisPosition;
    }
}
