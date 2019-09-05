using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(EnemyHealth))]
public class BossBehaviorController : MonoBehaviour {
    [Serializable]
    public struct Phase {
        public float health;
        public MonoBehaviour startBehaviour; // a behavior to enable on start
        public MonoBehaviour endBehaviour; // a behavior to enable on end
        public List<PhaseAction> backgroundActions; // behavours to be run on loop until the phase ends
        public List<PhaseAction> PossibleActions; 
    }
    [Serializable]
    public struct PhaseAction {
        public float selectionChance; // chance that this is chosen from possible actions
        //public bool background;
        public int runFrames; // framesa to run before finding next action
        public int cooldownFrames; // frames after running before nfinding next action
        public MonoBehaviour behavior;
    }
    public List<Phase> bossPhases;
    private EnemyHealth health;
    public void Awake() {
        foreach(Phase b in bossPhases){
            foreach(PhaseAction p in b.PossibleActions) p.behavior.enabled = false;
            foreach(PhaseAction p in b.backgroundActions) p.behavior.enabled = false;
            if(b.startBehaviour!=null) b.startBehaviour.enabled = false;
            if(b.endBehaviour!=null) b.endBehaviour.enabled = false;
        }
        health = GetComponent<EnemyHealth>();
        health.maxHealth = bossPhases.Select(x=> x.health).Sum();
        health.onEnemyDamage+=checkHealth;
    }
    public void Start () => StartPhase ();
    public int currentPhase;
    private float phaseHealth;
    private PhaseAction currentAction;
    private System.Random rand = new System.Random();

    [SerializeField]
    public List<PhaseAction> actionQueue = new List<PhaseAction>();
    public void checkHealth (float damageDealt, float healthAfterDamage) {
        while(damageDealt > 0 && phaseHealth > 0) {
            float temp = damageDealt;
            damageDealt -= phaseHealth;
            phaseHealth -= temp;
            if(phaseHealth<=0) { // Change Phases
                ChangePhase();
            }
        }
    }
    public void ChangePhase () {
        if( bossPhases[currentPhase].endBehaviour != null) bossPhases[currentPhase].endBehaviour.enabled = true;
        foreach(PhaseAction p in bossPhases[currentPhase].backgroundActions) p.behavior.enabled = false;
        if(currentAction.behavior != null) currentAction.behavior.enabled = false;
        // if there are phases left
        if(currentPhase<bossPhases.Count-1) {
            currentPhase++;
            StartPhase();
        }
    }

    public void StartPhase () {
        phaseHealth = bossPhases[currentPhase].health;
        if( bossPhases[currentPhase].startBehaviour != null) bossPhases[currentPhase].startBehaviour.enabled = true;
        foreach(PhaseAction p in bossPhases[currentPhase].backgroundActions) StartCoroutine( startActionBackground(p) );
        ChangeAction();
    }
    public void ChangeAction () {
        if(currentAction.behavior != null) currentAction.behavior.enabled = false;
        if(actionQueue.Any()) { currentAction =  actionQueue[0]; actionQueue.RemoveAt(0); }
        else if(bossPhases[currentPhase].PossibleActions.Count > 1) 
            currentAction = bossPhases[currentPhase].PossibleActions.Where(x=> !x.Equals(currentAction)).ElementAt( rand.Next(bossPhases[currentPhase].PossibleActions.Count-2) );
        else currentAction = bossPhases[currentPhase].PossibleActions.FirstOrDefault();
        
        if(currentAction.behavior != null) StartCoroutine(startAction(currentAction, true));
        else Debug.Log("BBC Error: Cannot start null behavior!");
    }
    public IEnumerator startAction (PhaseAction action, bool startNewOnFinish) {
        action.behavior.enabled = true;
        int startedPhase = currentPhase;
        for(int i = 0; i < action.runFrames; i++) {
            if(currentPhase != startedPhase) {
                action.behavior.enabled = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        } 
        action.behavior.enabled = false;
        for(int i = 0; i < action.cooldownFrames; i++) {
            if(currentPhase != startedPhase) yield break;
            yield return new WaitForEndOfFrame();
        }
        if(startNewOnFinish) ChangeAction ();
    }
    public IEnumerator startActionBackground (PhaseAction action) {
        int startedPhase = currentPhase;
        while(currentPhase == startedPhase) {
            action.behavior.enabled = true;
            if(action.cooldownFrames > 0) yield return StartCoroutine( startAction(action, false) );
            else yield return new WaitForEndOfFrame();
        }
    }
}
