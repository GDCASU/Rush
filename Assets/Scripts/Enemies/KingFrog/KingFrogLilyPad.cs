using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogLilyPad : MonoBehaviour
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    private bool collided = false;

    private float maxPadSize = 4.0f;
    private float scaleAmount = 0.5f;
    private float startSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KingFrogLilyPad") //checks for collision with other pads
        {
            SetCollided(true);
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
        startSize = transform.localScale.x; //save start scale
        while (transform.localScale.x < GetMaxPadSize()) //while not max size
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale += new Vector3(step, step, 0);
            yield return ws;
        }
    }

    IEnumerator SinkCR()
    {
        while(transform.localScale.x > startSize) //while not start size
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale -= new Vector3(step, step, 0);
            yield return ws;
        }
        Destroy(this.gameObject);
    }

    public void SetCollided(bool _input)
    {
        collided = _input;
    }

    public bool GetCollided()
    {
        return collided;
    }

    public void SetMaxPadSize(float _input)
    {
        maxPadSize = _input;
    }

    public float GetMaxPadSize()
    {
        return maxPadSize;
    }
}
