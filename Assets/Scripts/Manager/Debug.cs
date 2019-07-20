using UnityEngine;

/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 19, 2018
 * 
 * Author:      Zachary Schmalz
 * Version:     1.1.0
 * Date:        September 28, 2018
 *              Updated static fields and removed extra copies of variables
 */

/// <summary>
/// This class extends functionality of the default Debug.Log to categorize debugging statements
/// </summary>
public class Debug : MonoBehaviour
{
    public enum LogType
    {
        Normal,
        Warning,
        Error
    };
    
    public bool generalLog;
    public bool audioLog;
    public bool inputLog;
    public Color generalColor;
    public Color audioColor;
    public Color inputColor;

    private static Debug singleton;

    public void Awake()
    {
        // Delete any extra copies of script not attached to the GameObject with the GameManager
        if (singleton == null && gameObject.GetComponent<GameManager>())
            singleton = this;
        else
        {
            Destroy(this);
            return;
        }
    }

    public static void Log(object o, LogType type = LogType.Normal)
    {
        switch(type)
        {
            case LogType.Normal:
                UnityEngine.Debug.Log(o);
                break;

            case LogType.Warning:
                UnityEngine.Debug.LogWarning(o);
                break;

            case LogType.Error:
                UnityEngine.Debug.LogError(o);
                break;
        }
    }

    public static void GeneralLog(object o, LogType type = LogType.Normal)
    {
        if (singleton.generalLog)
            Log(FormatString("(General Log)\n", singleton.generalColor) + o, type);
    }

    public static void AudioLog(object o, LogType type = LogType.Normal)
    {
        if (singleton.audioLog)
            Log(FormatString("(Audio Log)\n", singleton.audioColor) + o, type);
    }

    public static void InputLog(object o, LogType type = LogType.Normal)
    {
        if (singleton.inputLog)
            Log(FormatString("(Input Log)\n", singleton.inputColor) + o, type);
    }

    public static void LogWarning(object o)
    {
        Log(o, LogType.Warning);
    }

    public static void LogError(object o)
    {
        Log(o, LogType.Error);
    }
    
    private static string FormatString(object o, Color color)
    {
        return "<b><color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + o + "</color></b>";
    }
}