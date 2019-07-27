using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(EnemyHealth))]
public class BossBehaviorController : MonoBehaviour {
    [Serializable]
    public struct subBehaviour {
        public MonoBehaviour controlledBehaviour;
        public float activeHealthRangeMin;
        public float activeHealthRangeMax;
        public bool disableAfterTime;
        public int activeFramesBeforeDisable;
    }
    public List<subBehaviour> controlledBehaviours;
    private List<subBehaviour> activeBehaviours;
    private List<subBehaviour> inactiveBehaviours;
    public void Awake() {
        foreach(subBehaviour b in controlledBehaviours){
            b.controlledBehaviour.enabled = false;
        }
    }
	void Update () {
		
	}
}
