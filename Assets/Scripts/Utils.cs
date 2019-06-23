using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static bool IsOnScreen(GameObject g) {
        var vP = Camera.main.WorldToViewportPoint(g.transform.position);
        return vP.x > 0 && vP.x < 1 && vP.y > 0 && vP.y < 1;
    }
}
