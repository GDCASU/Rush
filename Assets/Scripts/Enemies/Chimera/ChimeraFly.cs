using System.Collections;
using UnityEngine;

public class ChimeraFly : BossAction
{
    private Transform chimera;
    private System.Random rng = new System.Random();
    public float flySpeed = 0.1f;
    private bool flying = false;

    public void Awake()
    {
        chimera = GetComponent<Transform>();
    }

    public void OnEnable() => StartCoroutine(Fly());

    IEnumerator Fly()
    {
        actionRunning = true;
        float z = transform.position.z;
        for (int i = 0; i < 120; i++)
        {
            chimera.position += Vector3.back * flySpeed;
            yield return new WaitForEndOfFrame();
        }

        // Wait a few seconds
        for (int i = 0; i < 120; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        int action = rng.Next(0, 2);
        if (action == 0)
        {
            chimera.position = PlayerHealth.singleton.transform.position + new Vector3(0, 0, chimera.position.z);
            StartCoroutine(flyback(z));
            while (flying)
            {
                yield return new WaitForEndOfFrame();
            }
            GetComponent<ChimeraA4>().enabled = true;
            while (GetComponent<ChimeraA4>().actionRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            GetComponent<ChimeraA4>().enabled = false;
        }
        else
        {
            float x =(float)rng.Next(-24,24);
            float y =(float)rng.Next(-28,18);
            chimera.position = new Vector3(x, y, chimera.position.z);
            StartCoroutine(flyback(z));
            while (flying)
            {
                yield return new WaitForEndOfFrame();
            }
            GetComponent<ChimeraA1>().enabled = true;
            while (GetComponent<ChimeraA1>().actionRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            for (int i = 0; x < 60; x++)
            {
                yield return new WaitForEndOfFrame();
            }
            GetComponent<ChimeraA1>().enabled = false;
        }
        actionRunning = false;
    }
    IEnumerator flyback(float z)
    {
        flying = true;
        while(transform.position.z!=z)
        {
            chimera.position = Vector3.MoveTowards(transform.position,new Vector3(transform.position.x, transform.position.y, z),.5f);
            yield return new WaitForEndOfFrame();
        }
        flying = false;
    }
}
