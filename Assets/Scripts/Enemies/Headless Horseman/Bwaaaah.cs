using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bwaaaah : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable() => StartCoroutine(ImFiringMahLaser());

    IEnumerator ImFiringMahLaser()
    {
        //actionRunning = true;
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = true;

        for (int i = 0; i < 300; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
        //actionRunning = false;
    }
}
