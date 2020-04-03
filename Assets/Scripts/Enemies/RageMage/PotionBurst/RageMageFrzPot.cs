using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the Freeze potion for the
/// rage mage boss. For more info look at the class
/// comments in RageMageA3.cs
/// </summary>
public class RageMageFrzPot : RageMagePotion
{
    public float FreezeSpeed;
    private bool _canFreeze;
    private float _originalSpeed = -1;

    public override void PotionAction()
    {
        base.PotionAction();

        _originalSpeed = PlayerHealth.singleton.GetComponent<PlayerMovement>().speed;   //Saves original speed of player
        _canFreeze = true;
    }

    private void OnDestroy()
    {
        //Just in case whenever this is destroyed it'll set the speed back to normal
        PlayerHealth.singleton.GetComponent<PlayerMovement>().speed = _originalSpeed;
    }

    protected override void Update()
    {
        base.Update();

        //Freezes player if in range
        if(_canFreeze && PlayerInRange())
        {
            PlayerHealth.singleton.GetComponent<PlayerMovement>().speed = FreezeSpeed;
        }
        else if(_canFreeze)
        {
            PlayerHealth.singleton.GetComponent<PlayerMovement>().speed = _originalSpeed;
        }
    }
}
