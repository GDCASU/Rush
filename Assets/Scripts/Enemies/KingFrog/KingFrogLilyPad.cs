using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogLilyPad : MonoBehaviour
{
    public bool collided = false;

    private float scaleAmount = 0.5f;
    //[SerializeField]
    private float maxSize = 4.0f;
    private float startSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KingFrogLilyPad") //checks for collision with other pads
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
        startSize = transform.localScale.x; //save start scale
        while (transform.localScale.x < maxSize) //while not max size
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale += new Vector3(step, step, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SinkCR()
    {
        while(transform.localScale.x > startSize) //while not start size
        {
            float step = scaleAmount * Time.deltaTime;
            transform.localScale -= new Vector3(step, step, 0);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
