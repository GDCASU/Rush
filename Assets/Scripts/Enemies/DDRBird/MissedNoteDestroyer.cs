using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedNoteDestroyer : MonoBehaviour
{
    public PlayerHealth Player;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        print("Player lose health");
        Player.takeDamage();
    }
}
