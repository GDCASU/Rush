using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : MonoBehaviour
{
    //This is going to be the Main boss AI script.
    //It will handle health and phase changes.
    //The attacks and behaviors of each phase will be subdivided into other scripts, which will be activated/deactivated from this script.

    //START OF BATTLE
    //Boss will play entry animation, camera will be locked on boss. (//Player control locked, here or another script?)

    //PHASE 1
    //Boss will enter Phase 1 pattern, creating 1 turret while remaining itself invincible.
        //Vulnerability: Once the chain is broken, the player's sword is empowered.
        //The boss is vulnerable for a set amount of time.
        //Attacks: Basic, arcs, homing, helix
    //Phase 1.1
        //Repeat of Phase 1 with two turrets.
    //Phase 1.2
        //Repeat of Phase 1 with three turrets.

    //PHASE 2
    //Boss will enter Phase 2 pattern, becoming permanently vunerable.
        //Boss will move to edge of arena, and move player to opposite edge.
        //Vulnerability: The wisp is vulnerable to 1 attack from the sword.
        //Attacks: Big Emitter, Beam, Sine Wave Triple
    //Phase 2.1
        //Repeat of Phase 2
    //Phase 2.2
        //Repeat of Phase 2

    //END OF BATTLE
    //Boss will play death animation. 


}