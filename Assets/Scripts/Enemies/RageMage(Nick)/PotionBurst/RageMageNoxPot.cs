using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the noxious potion for the
/// rage mage boss. For more info look at the class
/// comments in RageMageA3.cs
/// </summary>
public class RageMageNoxPot : RageMagePotion
{
    public float NoxDmgDelay;   //noxious damage delay.
    private float _noxDmgDelayCount;
    private bool _canDamage;

    public override void PotionAction()
    {
        base.PotionAction();

        _canDamage = true;
    }

    protected override void Update()
    {
        base.Update();

        //Damages player over time
        if (_canDamage && _noxDmgDelayCount < 0 && PlayerInRange())
        {
            PlayerHealth.singleton.takeDamage();
            _noxDmgDelayCount = NoxDmgDelay;
        }
        else if (_canDamage)
            _noxDmgDelayCount -= Time.deltaTime;
    }
}
