using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject plyr;
    private IInputPlayer player;
    private BoxCollider2D col;
    public int swordSwingFrames;
    private int combo;
    private bool window;
    // Use this for initialization
    void Start ()
    {
        combo = 1;
        col=GetComponent<BoxCollider2D>();
        col.enabled=false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //print(combo);
        //print(window+"w");
        followPlayer();
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player))
        {
            col.enabled = true;
            window = true;
            StartCoroutine("swing");
            
        }
        if (!window) combo = 1;
	}
    void followPlayer()
    {
        transform.localPosition = new Vector3(plyr.transform.position.x+.75f, plyr.transform.position.y+.33f, plyr.transform.position.z);
    }
    public IEnumerator swing()
    {
        for (int i = swordSwingFrames; i > 0; i--)
        {
            yield return new WaitForEndOfFrame();
        }
        window = false;
        col.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("entered"+combo);
            other.GetComponent<EnemyHealth>().takeDamage(combo);
            combo++;
            if (combo >3) combo = 1;
        }

    }
}
