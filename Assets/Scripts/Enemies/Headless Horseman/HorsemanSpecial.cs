using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanSpecial : BossAction
{
    public GameObject lance;
    public GameObject ghosts;
    public int raisingFraames;
    public List<Transform> spawnpoints;
    private System.Random rng = new System.Random();

    private void OnEnable()
    {
        StartCoroutine("apparitions");
    }
    IEnumerator apparitions()
    {
        actionRunning = true;
        int remove = rng.Next(5);
        int real = rng.Next(4);
        for (int x = 0; x < raisingFraames; x++)
        {
            lance.transform.Rotate(new Vector3(0, 0, 155f / raisingFraames));
            yield return new WaitForEndOfFrame();
        }
        spawnpoints.RemoveAt(remove);
        for (int x = 0; x <spawnpoints.Count; x++)
        {
            GameObject temp;
            if (x == real)
            {
                transform.position = spawnpoints[x].position;
                GetComponent<HorsemanApparition>().enabled = true;
            }
            else temp = Instantiate(ghosts, new Vector3(spawnpoints[x].position.x, spawnpoints[x].position.y, spawnpoints[x].position.z + 0.1f), Quaternion.identity);
        }
        while (GameObject.FindGameObjectsWithTag("HorsemanApparition").Length != 0)
        {
            yield return new WaitForEndOfFrame();
        }
        actionRunning = false;
    }
}
