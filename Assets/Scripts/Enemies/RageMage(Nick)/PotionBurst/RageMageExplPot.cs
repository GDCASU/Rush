using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the explosive potion for the
/// rage mage boss. For more info look at the class
/// comments in RageMageA3.cs
/// </summary>
public class RageMageExplPot : RageMagePotion
{
    public override void PotionAction()
    {
        base.PotionAction();

        //Damages player
        if(PlayerInRange())
        {
            PlayerHealth.singleton.takeDamage();
        }
    }
}
