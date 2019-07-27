using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    const float bulletLiveSize=0.5f;
    const float bn = 0 - bulletLiveSize;
    const float bp = 1 + bulletLiveSize;
	public static bool IsOnScreen(GameObject g) {
        var vP = Camera.main.WorldToViewportPoint(g.transform.position);
        return vP.x > bn && vP.x < bp && vP.y > bn && vP.y < bp;
    }
}
