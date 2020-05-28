using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AnimationController : MonoBehaviour {

	public bool animationLocked;
    private Animator anim;
    public void Start() {
        anim = GetComponentInChildren<Animator>();
    }
    private bool breakCallback;
    public void tryNewAnimation (string animationName, bool loop, float frames = 0, bool force = false, Action onComplete = null){
        if(animationLocked && !force)return;
        if(force)breakCallback = true;
        animationLocked = !loop;
        anim.Play(animationName);
        if(!loop) {
            StartCoroutine(animationEnd(frames, onComplete));
        }
    }

    public IEnumerator animationEnd (float wait, Action onComplete){
        for(int i = 0; i < wait; i++) {
                if(breakCallback) { breakCallback = false; yield break;} 
                else yield return new WaitForEndOfFrame(); 
            }
        animationLocked = false;
        onComplete?.Invoke();
    }
}
