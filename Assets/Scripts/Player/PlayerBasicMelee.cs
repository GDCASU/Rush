using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMelee : MonoBehaviour
{
    public GameObject plyr;
    private IInputPlayer player;
    private BoxCollider2D col;
    public int swordSwingFrames;
    private int combo;
    private bool window;
    
    void Start ()
    {
        combo = 1;
        col=GetComponent<BoxCollider2D>();
        col.enabled=false;
	}
	
	void Update ()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player))
        {
            col.enabled = true;
            window = true;
            StartCoroutine(swing1());
            
        }
        if (!window) combo = 1;
	}
    public IEnumerator swing1()
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
