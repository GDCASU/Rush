using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossAction : MonoBehaviour
{
    public bool actionRunning = false;
}
[RequireComponent(typeof(EnemyHealth))]
public class BossBehaviorController : MonoBehaviour
{
    [Serializable]
    public struct Phase
    {
        public float health;
        public BossAction repeatingBehaviour;//  behavior that gets called everytime b4 the other possible behaviors
        public BossAction startBehaviour; // a behavior to enable on start
        public BossAction endBehaviour; // a behavior to enable on end
        public List<PhaseAction> backgroundActions; // behavours to be run on loop until the phase ends
        public List<PhaseAction> PossibleActions;
    }
    [Serializable]
    public struct PhaseAction
    {
        public bool independent;
        public float selectionChance; // chance that this is chosen from possible actions
        //public bool background;
        public int runFrames; // framesa to run before finding next action
        public int cooldownFrames; // frames after running before nfinding next action
        public BossAction behavior;
    }
    public List<Phase> bossPhases;
    private EnemyHealth health;
    public void Awake()
    {
        foreach (Phase b in bossPhases)
        {
            foreach (PhaseAction p in b.PossibleActions) p.behavior.enabled = false;
            foreach (PhaseAction p in b.backgroundActions) p.behavior.enabled = false;
            if (b.startBehaviour != null) b.startBehaviour.enabled = false;
            if (b.endBehaviour != null) b.endBehaviour.enabled = false;
        }
        health = GetComponent<EnemyHealth>();
        health.maxHealth = bossPhases.Select(x => x.health).Sum();
        health.onEnemyDamage += checkHealth;
    }
    public void Start() => StartCoroutine(StartPhase());
    public int currentPhase;
    private float phaseHealth;
    private PhaseAction currentAction;
    private System.Random rand = new System.Random();

    [SerializeField]
    public List<PhaseAction> actionQueue = new List<PhaseAction>();
    public void checkHealth(float damageDealt, float healthAfterDamage)
    {
        while (damageDealt > 0 && phaseHealth > 0)
        {
            float temp = damageDealt;
            damageDealt -= phaseHealth;
            phaseHealth -= temp;
            if (phaseHealth <= 0)
            { // Change Phases
                ChangePhase();
            }
        }
    }
    public void ChangePhase()
    {
        if (bossPhases[currentPhase].endBehaviour != null) bossPhases[currentPhase].endBehaviour.enabled = true;
        foreach (PhaseAction p in bossPhases[currentPhase].backgroundActions) p.behavior.enabled = false;
        foreach (PhaseAction p in bossPhases[currentPhase].PossibleActions)
        {
            p.behavior.StopAllCoroutines();
            p.behavior.enabled = false;
        }

        // if there are phases left
        if (currentPhase < bossPhases.Count - 1)
        {
            currentPhase++;
            StartCoroutine(StartPhase());
        }
    }

    public IEnumerator StartPhase()
    {
        phaseHealth = bossPhases[currentPhase].health;
        if (bossPhases[currentPhase].startBehaviour != null)
        {
            bossPhases[currentPhase].startBehaviour.enabled = true;
            while (bossPhases[currentPhase].startBehaviour.actionRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            bossPhases[currentPhase].startBehaviour.enabled = false;
        }
        else ChangeAction();
        foreach (PhaseAction p in bossPhases[currentPhase].backgroundActions) StartCoroutine(startActionBackground(p));

    }
    public void ChangeAction()
    {
        foreach(PhaseAction p in actionQueue) print(p.behavior);
        if (currentAction.behavior != null) currentAction.behavior.enabled = false;
        if (bossPhases[currentPhase].repeatingBehaviour != null && currentAction.behavior != bossPhases[currentPhase].repeatingBehaviour) currentAction.behavior = bossPhases[currentPhase].repeatingBehaviour;
        else if (actionQueue.Any()) { currentAction = actionQueue[0]; actionQueue.RemoveAt(0); }
        else if (bossPhases[currentPhase].PossibleActions.Count > 1)
            currentAction = bossPhases[currentPhase].PossibleActions.Where(x => !x.Equals(currentAction)).ElementAt(rand.Next(bossPhases[currentPhase].PossibleActions.Count));
        else currentAction = bossPhases[currentPhase].PossibleActions.FirstOrDefault();
        if (currentAction.behavior != null) StartCoroutine(startAction(currentAction, true));
        else Debug.Log("BBC Error: Cannot start null behavior!");
    }
    public IEnumerator startAction(PhaseAction action, bool startNewOnFinish)
    {
        action.behavior.enabled = true;
        action.behavior.actionRunning = true;
        int startedPhase = currentPhase;
        if (action.independent || action.behavior == bossPhases[currentPhase].repeatingBehaviour)
        {
            while (action.behavior.actionRunning)
            {
                yield return new WaitForEndOfFrame();
            }
            action.behavior.enabled = false;

        }
        else
        {
            for (int i = 0; i < action.runFrames; i++)
            {
                if (currentPhase != startedPhase)
                {
                    action.behavior.enabled = false;
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
            action.behavior.enabled = false;
        }
        for (int i = 0; i < ((action.behavior == bossPhases[currentPhase].repeatingBehaviour) ? 0 : action.cooldownFrames); i++)
        {
            if (currentPhase != startedPhase) yield break;
            yield return new WaitForEndOfFrame();
        }
        if (startNewOnFinish) ChangeAction();
    }
    public IEnumerator startActionBackground(PhaseAction action)
    {
        int startedPhase = currentPhase;
        while (currentPhase == startedPhase)
        {
            action.behavior.enabled = true;
            if (action.cooldownFrames > 0) yield return StartCoroutine(startAction(action, false));
            else yield return new WaitForEndOfFrame();
        }
    }
}
