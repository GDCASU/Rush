using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogLilyPad : MonoBehaviour
{
    public bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckSpace());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "KingFrogLilyPad" || collision.gameObject.tag == "Player")
        {
            collided = true;
        }
    }

    IEnumerator CheckSpace()
    {
        yield return null;

        if(collided)
        {
            Destroy(this, 0.01f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
