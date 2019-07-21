using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedEnemy : MonoBehaviour {

    public string enemyName = "";
    [HideInInspector]
    public EnemyHealth enemyHealth;
	void Start () {
        enemyHealth = GetComponent<EnemyHealth>();
		enemyHealth.onEnemyDamage+=setName;
	}
	
	void setName (float damage, float health) {
        if(enemyHealth != EnemyHealthBar.singleton) EnemyHealthBar.singleton.setCurrentEnemy(this);
    }
}
