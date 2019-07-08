using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour {
    public static PlayerHealth singleton;
    public int lives;

    public bool inv = false;

    //singleton
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
     private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if ( (!inv) && ((other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>().hostile ) || other.gameObject.CompareTag("Enemy")) )
        {
            inv = true;
            StartCoroutine(flashingSprite());
            lives--;
            // Hurt the player
            if (lives > 0) Debug.Log("Update the player health in the UI here"); //example: HUDManager.singleton.setLiveCount(lives);
            else Debug.Log("Go to gameover"); //example: SceneManager.LoadScene("GameOver"); 

            if(other.gameObject.CompareTag("Bullet")) other.GetComponent<Bullet>().BulletDestroy ();
        }
    }
    // changes the amount of frames the player is flashing for
    const int iframes = 20;
    public IEnumerator flashingSprite () {
        for(int i = iframes; i>0; i--){
            sp.enabled = i%2 == 0;
            yield return new WaitForEndOfFrame();
        }
        sp.enabled = true;
        inv = false;
    }
}
