using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointSet : MonoBehaviour
{
    public GameObject leftWall;
    public GameObject rightWall;
    public int phase;
    public int spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        float dist1 = Mathf.Abs(leftWall.transform.position.x);
        float dist2 = Mathf.Abs(rightWall.transform.position.x);
        float dist = dist1 + dist2;

        if (phase == 1)
        {
            float point = dist / 9;
            transform.position = new Vector3(leftWall.transform.position.x + point*(spawnPoint + 1), transform.position.y,transform.position.z);
        }
        else
        {
            if (phase == 1)
            {
                float point = dist / 15;
                transform.position = new Vector3(leftWall.transform.position.x + point * (spawnPoint + 1), transform.position.y, transform.position.z);
            }
        }
    }
}
