using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour {
    public static PlayerHealth singleton;
    public int lives;
    private IInputPlayer player;

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
        player = GetComponent<IInputPlayer>();
        sp = GetComponent<SpriteRenderer>();
        HUDManager.singleton.setLiveCount(lives);
    }
     void Update()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Potion, player))
        {
            lives++;
        }
        if (lives > 0) HUDManager.singleton.setLiveCount(lives);
        else Debug.Log("Go to gameover"); //example: SceneManager.LoadScene("GameOver"); 
    }

    void OnTriggerEnter2D (Collider2D other) {
        if ( (!inv) && ((other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>().hostile ) || other.gameObject.CompareTag("Enemy")) )
        {
            inv = true;
            StartCoroutine(flashingSprite());
            lives--;
            // Hurt the player
            

            if(other.gameObject.CompareTag("Bullet")) other.GetComponent<Bullet>().BulletDestroy ();
        }
    }
    // changes the amount of frames the player is flashing for
    const int iframes = 40;
    public IEnumerator flashingSprite () {
        for(int i = iframes; i>0; i--){
            sp.enabled = i%2 == 0 ? !sp.enabled : sp.enabled;
            yield return new WaitForEndOfFrame();
        }
        sp.enabled = true;
        inv = false;
    }
}
