using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfGeyser : BossAction
{
    WaitForSeconds ws = new WaitForSeconds(1f / 60f);

    GameObject dwarf;
    private float secondsPerWave;
    public GameObject geyserPrefab;
    public int windUpAnimation;
    public int numberOfGeysers;
    public int lengthOfWaves;
    public int waveLength;
    public int timer;
    public int waveCount;

    void Start()
    {
        dwarf = transform.gameObject;
        StartCoroutine("shockwave");
        actionRunning = true;
    }

    IEnumerator shockwave()
    {
        timer = 0;
        waveCount = 0;
        for (int x = 0; x < windUpAnimation; x++)
        {
            yield return ws;
        }
        for (int x = 0; x < lengthOfWaves; x++)
        {
            if (timer % waveLength == 0)
            {
                spawnGeysers(numberOfGeysers,waveCount);
                waveCount++;
            }
            timer++;
            yield return ws;
        }
        actionRunning = false;
    }

    void spawnGeysers(int n, int wave)
    {
        Vector3 leftDistance=Vector3.zero;
        Vector3 rightDistance=Vector3.zero;

        Vector3 height = new Vector3(0, geyserPrefab.GetComponent<BoxCollider2D>().size.y + 1 * wave, 0);
        //Vector3 height = new Vector3(0, geyserPrefab.transform.lossyScale.y * wave, 0);                   //For when the scale is not 1


        if (n % 2 == 0)
        {

            leftDistance = new Vector3(-geyserPrefab.GetComponent<BoxCollider2D>().size.x/2, 0, 0);
            rightDistance = new Vector3(geyserPrefab.GetComponent<BoxCollider2D>().size.x/2, 0, 0);

            //leftDistance = new Vector3(-geyserPrefab.transform.lossyScale.x / 2, 0, 0);                   //For when the scale is not 1
            //rightDistance = new Vector3(geyserPrefab.transform.lossyScale.x / 2, 0, 0);                   //For when the scale is not 1

            Instantiate(geyserPrefab, dwarf.transform.position + leftDistance - height, Quaternion.identity);
            Instantiate(geyserPrefab, dwarf.transform.position + rightDistance - height, Quaternion.identity);
            n += -2;
        }
        else 
        {
            Instantiate(geyserPrefab, dwarf.transform.position - height, Quaternion.identity);
            n--;
        }
        for (int x = 0; x < n; x += 2)
        {
            leftDistance = leftDistance + new Vector3(-geyserPrefab.GetComponent<BoxCollider2D>().size.x, 0, 0);
            rightDistance = rightDistance + new Vector3(geyserPrefab.GetComponent<BoxCollider2D>().size.x, 0, 0);

            //leftDistance = leftDistance + new Vector3(-geyserPrefab.transform.lossyScale.x / 2, 0, 0);    //For when the scale is not 1
            //rightDistance = leftDistance + new Vector3(geyserPrefab.transform.lossyScale.x / 2, 0, 0);    //For when the scale is not 1

            Instantiate(geyserPrefab, dwarf.transform.position + leftDistance - height, Quaternion.identity);
            Instantiate(geyserPrefab, dwarf.transform.position + rightDistance - height, Quaternion.identity);
        }
    }
}
