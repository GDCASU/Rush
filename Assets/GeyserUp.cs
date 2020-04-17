using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserUp : MonoBehaviour
{
    public int riseFrames;
    public int despawnFrames;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("rise");
    }

    IEnumerator rise()
    {
        for (int x = 0; x < riseFrames; x++)
        {
            transform.position = transform.position + new Vector3(0, 0, -.1f);
            yield return new WaitForEndOfFrame();
        }

        for (int x = 0; x < despawnFrames; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(transform.gameObject);
    }


}
