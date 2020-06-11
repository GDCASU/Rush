using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// This class defines a moment behavior which can be used to interpolate an object between several nodes 
/// In the future we may want to use a reorderable list for the nodes, I believe 2020 introduces a native ones but there are librarys for it already
/// </summary>
[System.Serializable]
public struct MovementKeyframe {
    public Vector3 pos;
    public Vector3 rot;
    public float timeToReachMe;
    public Curves curve;
    
    public MovementKeyframe(Vector3 p, Vector3 r, float time = 5f, Curves c = Curves.Lerp) {
        pos = p; rot = r; timeToReachMe = time; curve = c;
    }
    // this override will have the camera face the same direction as the transform at that location
    public MovementKeyframe (Transform transform, float time)
        : this(transform.position, transform.forward, time) { }
    public void InterpTransform(MovementKeyframe next, float t, Transform transform) {
        t = Interpolation.GetCurve(curve)(t);
        transform.position = Vector3.Lerp(pos, next.pos, t);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot),Quaternion.Euler(next.rot),t);
    }
}

public class TimedMovement : MonoBehaviour {
    public bool loop = false;
    public float totalMovementScaling = 1f;
    private bool moving;
    public List<MovementKeyframe> keyframes = new List<MovementKeyframe>();
    void Start () { 
        StartCoroutine( loop ? LoopingMovement() : Movement() );
        moving = true; 
    }
    IEnumerator Movement() {
        keyframes.Insert(0, new MovementKeyframe(transform, 0));
        for(int i = 0; i < keyframes.Count-1; i++) {
            var prev = keyframes[i]; 
            var next = keyframes[i + 1];
            for (float t=0; t<1; t += next.timeToReachMe / totalMovementScaling) {
                prev.InterpTransform(next, t, transform);
                yield return null;
            }
        }
    }
    IEnumerator LoopingMovement() {
        // We want to return to this exact rotation and position
        keyframes.Insert(0, new MovementKeyframe(transform.position, transform.rotation.eulerAngles, keyframes.Last().timeToReachMe,Curves.Lerp));
        while(true) {
            for(int i = 0; i < keyframes.Count; i++) {
                var prev = keyframes[i % (keyframes.Count)]; 
                var next = keyframes[ (i + 1) % (keyframes.Count) ];
                for (float t=0; t<1; t += next.timeToReachMe / totalMovementScaling) {
                    prev.InterpTransform(next, t, transform);
                    yield return null;
                }
            }
        }
    }
	void OnDrawGizmosSelected () => DrawPath();
	void DrawPath (float sphereRadius = 1) {
        if (keyframes == null || !(keyframes.Count > 1)) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, sphereRadius*2);
            return;
        }
        Gizmos.color = Color.blue;
        if(!moving) DrawSegment(transform.position,keyframes.First().pos, sphereRadius);
        if(loop) DrawSegment(keyframes.Last().pos, moving ? keyframes.First().pos : transform.position, sphereRadius);

        for(int i = 0; i < keyframes.Count-1; i++) {
            var prev = keyframes[i]; var next = keyframes[i + 1];
            DrawSegment(prev.pos, next.pos, sphereRadius);
        }
        Gizmos.DrawSphere(keyframes.Last().pos, sphereRadius);
	}

    void DrawSegment (Vector3 pos1, Vector3 pos2, float radius) { 
        Gizmos.DrawSphere(pos1, radius);  
        Gizmos.DrawLine(pos1,pos2);
    }
}


public enum Curves {
    Lerp, EaseIn, EaseOut, SmootherStep, StickToLowerBound, StickToUpperBound 
}

/// <summary>
/// Defines a standard type for interpolation, to allow easy use of nonlinear functions
/// </summary>
public static class Interpolation {
    public delegate float Curve (float tIn);
    public static float Interpolate (float a, float b, float t, Curve c) {
        return Mathf.Lerp(a, b, c(t));
    }
    public static Curve GetCurve (Curves c) {
        switch (c) {
            case Curves.Lerp: return t => t;
            case Curves.EaseIn: return t => t * Mathf.PI * 0.5f;
            case Curves.EaseOut: return t => Mathf.Sin(t * Mathf.PI * 0.5f);
            case Curves.SmootherStep: return t => (float)Math.Pow(t, 4f) * (t * 6 - 15) + 10;
            case Curves.StickToLowerBound: return t => 0;
            case Curves.StickToUpperBound: return t => 1;
            default: throw new ArgumentException();
        }
    }
}