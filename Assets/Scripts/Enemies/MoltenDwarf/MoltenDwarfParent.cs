﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfParent : BossAction
{
    protected Animator dwarfAnim;
    protected Transform dwarfTransform;

    [SerializeField]
    protected GameObject myPlayer;
    protected Vector2 playerPosition;
}
