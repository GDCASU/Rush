using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinWalk : MonoBehaviour
{
    private PlayerMovement pm;
    private void Start()
    {
        pm=GetComponent<PlayerMovement>();
        pm.winWalk = true;
    }
    private void Update()
    {
        if(pm.winWalk)pm.CheckMovementInput(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerPedestal")
        {
            pm.winWalk = false;
            pm.mo.won = true;
            pm.StopMovement();
        }
    }
}
