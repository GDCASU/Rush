using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string soundEffect;
    FMOD.Studio.EventInstance EffectAudio;
    void Awake()
    {
       
        EffectAudio = FMODUnity.RuntimeManager.CreateInstance(soundEffect);
        EffectAudio.start();
        FMODUnity.RuntimeManager.PlayOneShotAttached(soundEffect, transform.gameObject);
    }
   
        
    }

