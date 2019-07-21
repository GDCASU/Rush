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

        currentEnemy = e;
        gameObject.SetActive(true);
        nameText.name = e.name;
    }
	
    public void LateUpdate() {
        HealthMask.sizeDelta = new Vector2 (currentEnemy.enemyHealth.healthPercent, 100);
    }

    public void BossDeath () => gameObject.SetActive(false); // this is where an animation for the health bar could go
}
