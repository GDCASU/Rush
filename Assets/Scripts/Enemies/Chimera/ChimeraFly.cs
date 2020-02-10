using System.Collections;
using UnityEngine;

public class ChimeraFly : BossAction
{
    private Transform chimera;
    private System.Random rng = new System.Random();
    private bool flying = false;
    private int action;
    public float flySpeed = 0.1f;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    public GameObject shadowPrefab;
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

        action = rng.Next(2);
        print(action);
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
            float x =(float)rng.Next(minX,maxX);
            float y = (float)rng.Next(minY,maxY);
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
        GameObject shadow=new GameObject();
        if (action == 0)shadow = GameObject.Instantiate(shadowPrefab, new Vector3(transform.position.x, transform.position.y, -.1f), Quaternion.identity);
        shadow.transform.localScale = new Vector3(.1f,.1f,.1f);
        while (transform.position.z!=z)
        {
            if (action == 0)
            {
                shadow.GetComponent<SpriteRenderer>().color = new Color(shadow.GetComponent<SpriteRenderer>().color.r, shadow.GetComponent<SpriteRenderer>().color.g, shadow.GetComponent<SpriteRenderer>().color.b, shadow.GetComponent<SpriteRenderer>().color.a + .017f);
                shadow.transform.localScale += new Vector3(.008f, .004f, 0);
            }
            chimera.position = Vector3.MoveTowards(transform.position,new Vector3(transform.position.x, transform.position.y, z),.4f);
            yield return new WaitForEndOfFrame();
        }
        Destroy(shadow);
        flying = false;
    }
}
