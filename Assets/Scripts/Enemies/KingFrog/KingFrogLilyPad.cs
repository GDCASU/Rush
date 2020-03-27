using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogLilyPad : MonoBehaviour
{
    public bool collided = false;

    private float scaleAmount = 0.5f;
    private float maxSize = 2.0f;
    private float startSize;

    private void Start()
    {
        startSize = transform.localScale.x; //save start scale
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KingFrogLilyPad")
        {
            collided = true;
        }
    }

    public void Rise()
    {
        StartCoroutine(RiseCR());
    }

    public void Sink()
    {
        StartCoroutine(SinkCR());
    }

    IEnumerator RiseCR()
    {
        while(transform.localScale.x < maxSize)
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale += new Vector3(step, step, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SinkCR()
    {
        while(transform.localScale.x > startSize)
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale -= new Vector3(step, step, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}
