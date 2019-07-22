using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AnimationController : MonoBehaviour {

	public bool animationLocked;
    private Animator anim;
    public void Start() {
        anim = GetComponent<Animator>();
    }
    public void tryNewAnimation (string animationName, bool loop, float frames = 0, bool force = false, Action onComplete = null){
        if(animationLocked && !force)return;
        animationLocked = !loop;
        anim.Play(animationName);
        if(!loop) {
            StartCoroutine(animationEnd(frames, onComplete));
        }
    }

    public IEnumerator animationEnd (float wait, Action onComplete){
        for(int i = 0; i < wait; i++) yield return new WaitForEndOfFrame();
        onComplete?.Invoke();
        animationLocked = false;
    }
}
