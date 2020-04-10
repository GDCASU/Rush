using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfPhaseSwitch : BossAction
{
    [SerializeField]
    private Transform Phase2POS;

    private void OnEnable()
    {
        actionRunning = true;

        gameObject.transform.position = Phase2POS.position;

        actionRunning = false;
    }
}
