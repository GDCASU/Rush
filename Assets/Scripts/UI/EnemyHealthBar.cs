using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour {
    public static EnemyHealthBar singleton;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    NamedEnemy currentEnemy = null;
    public RectTransform HealthMask;

    public Text nameText;
    
    void Start () {
        if(currentEnemy == null) gameObject.SetActive(false);
    }
    public void setCurrentEnemy(NamedEnemy e) {
        // unsub current enemy 
        if(currentEnemy != null) currentEnemy.enemyHealth.OnDeath -= BossDeath;

        currentEnemy = e;
        e.enemyHealth.OnDeath += BossDeath;
        gameObject.SetActive(true);
        nameText.text = e.enemyName;
    }
	
    public void LateUpdate() {
        HealthMask.sizeDelta = new Vector2 (currentEnemy.enemyHealth.healthPercent, 100);
    }

    public void BossDeath () => gameObject.SetActive(false); // this is where an animation for the health bar could go
}
