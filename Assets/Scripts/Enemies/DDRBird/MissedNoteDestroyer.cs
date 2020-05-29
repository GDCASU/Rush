using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedNoteDestroyer : MonoBehaviour
{
    public PlayerHealth Player;
    public DDRBirdManager ddrBirdManager;
    public Conductor conductor;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        print("Player lose health");
        Player.takeDamage();
        ddrBirdManager.ResetCombo();

        if (Player.lives <= 0)
        {
            conductor.Speaker.Stop();
        }
    }
}
