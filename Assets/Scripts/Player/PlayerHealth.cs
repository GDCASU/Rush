using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public int lives;
    private PlayerMovement pm;
    private IInputPlayer player;
    private int _maxHealth;
    public SpriteRenderer[] sprites;
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
        pm = GetComponent<PlayerMovement>();
        player = GetComponent<IInputPlayer>();
        //sp = GetComponent<SpriteRenderer>();
        HUDManager.singleton.setLiveCount(lives);
        _maxHealth = lives;
    }
    void Update()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Potion, player))
        {
            GainHealth();
        }
        if (lives > 0) HUDManager.singleton.setLiveCount(lives);
        else
        {
            pm.StopMovement();
            pm.winWalk = false;
            pm.mo.dead=true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if ((!inv) && ((other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>().hostile))
            || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("HorsemanApparition"))
        {
            takeDamage();
            // Hurt the player
          
            if(other.gameObject.CompareTag("Bullet")) other.GetComponent<Bullet>().BulletDestroy ();
        }
    }
    // changes the amount of frames the player is flashing for
    const int iframes = 40;
    public IEnumerator flashingSprite () {
        for(int i = iframes; i>0; i--){
            if (sprites.Length != 0) foreach (SpriteRenderer sprite in sprites)sprite.enabled = i % 2 == 0 ? !sprite.enabled : sprite.enabled;
            //sp.enabled = i%2 == 0 ? !sp.enabled : sp.enabled;
            yield return GameManager.singleton.ws;
        }
        if (sprites.Length != 0) foreach (SpriteRenderer sprite in sprites)sprite.enabled = true;
        //sp.enabled = true;
        inv = false;
    }
    public void takeDamage()
    {
        inv = true;
        StartCoroutine(flashingSprite());
        lives--;
    }
    public void GainHealth()
    {
        if (lives < _maxHealth) lives++;
    }
    
}
