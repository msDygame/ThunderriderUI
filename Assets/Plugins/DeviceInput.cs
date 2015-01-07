using UnityEngine;
using System.Collections;

public enum DeviceKey
{
    UP = 19,
    DOWN = 20,
    LEFT = 21,
    RIGHT = 22,
    BACK = 4,
    HOME = 82,
    MENU = 82,
    ENTER = 66,
    ENTER2 = 23
};

public class DeviceInput 
{
    public static string KeyStateString = "";
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
#else
    public static AndroidJavaObject m_pluginObject;
    private static float CheckTime = 0;

    //遊戲啟動時呼叫大廳
    private static bool m_islaunch = false;

	public static void LaunchGameZone()
	{
        if (m_islaunch) return;
        m_islaunch = true;
		using( var pluginClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			m_pluginObject = pluginClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		
		//m_pluginObject.Call("startGameZoneIfNotRunning");
	}
#endif

    
    public static string DetectKey()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
#else
        if (CheckTime == 0)
        {
            using (var pluginClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                m_pluginObject = pluginClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

        }
        if (Time.time < CheckTime + 1)
        {
        }
        KeyStateString = m_pluginObject.CallStatic<string>("getKeyState");
        CheckTime = Time.time;
#endif

        return KeyStateString;
    }

    public static bool GetKeyDown(int DeviceID, DeviceKey Key)
    {
        string strKey = DeviceID.ToString() + ":" + ((int)Key).ToString() + ":1,";
        if (DeviceID < 0) strKey =  ":" + ((int)Key).ToString() + ":1,";
        bool IsGet = (KeyStateString.IndexOf(strKey) >= 0);
        return IsGet;
    }

    public static bool GetKeyUp(int DeviceID, DeviceKey Key)
    {
        string strKey = DeviceID.ToString() + ":" + ((int)Key).ToString() + ":2,";
        if (DeviceID < 0) strKey = ":" + ((int)Key).ToString() + ":2,";
        bool IsGet = (KeyStateString.IndexOf(strKey) >= 0);
        return IsGet;
    }
}

 
 