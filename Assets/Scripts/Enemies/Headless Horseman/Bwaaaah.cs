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
        flip();
        for (int i = 0; i < aimFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        Vector3 line = transform.position - player.position;
        float angle = Mathf.Atan2(line.x, line.y) * Mathf.Rad2Deg;
        transform.GetChild(2).transform.RotateAround(transform.GetChild(3).transform.position,Vector3.forward, -angle);
        for (int i = 0; i < laserFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(2).transform.RotateAround(transform.GetChild(3).transform.position, Vector3.forward, angle);
        transform.GetChild(2).gameObject.SetActive(false);
        actionRunning = false;
    }

    void flip()
    {
        if (transform.position.x > PlayerHealth.singleton.transform.position.x)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }
}
