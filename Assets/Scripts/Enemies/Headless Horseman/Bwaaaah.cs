using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bwaaaah : BossAction
{
    public int aimFrames;
    public int laserFrames;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable() => StartCoroutine(ImFiringMahLaser());

    IEnumerator ImFiringMahLaser()
    {
        actionRunning = true;
        Transform player = PlayerHealth.singleton.transform;
        //transform.GetChild(2).transform.LookAt(player, Vector3.right);
        for (int i = 0; i < aimFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;      
        for (int i = 0; i < laserFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
        actionRunning = false;
    }
}
