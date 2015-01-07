using UnityEngine;
using System.Collections;
using System;

public class LogManager
{
	public static bool SHOW_DEBUG_LOG = true;
	public static bool SHOW_ENGINE_LOG = true;
	
	public static void DebugLog(Type sender, string message)
	{
		if (SHOW_DEBUG_LOG) Debug.Log(string.Format("ⓓ {0} - {1}", sender.ToString(), message));
	}
	public static void DebugLogError(Type sender, string message)
	{
		if (SHOW_DEBUG_LOG) Debug.LogError(string.Format("ⓓ {0} - {1}", sender.ToString(), message));
	}
	public static void DebugLogWarning(Type sender, string message)
	{
		if (SHOW_DEBUG_LOG) Debug.LogWarning(string.Format("ⓓ {0} - {1}", sender.ToString(), message));
	}
	
	public static void EngineLog(Type sender, string message)
	{
		if (SHOW_ENGINE_LOG) Debug.Log(string.Format("██ {0} - {1}", sender.ToString(), message));
	}
	public static void EngineLogError(Type sender, string message)
	{
		if (SHOW_ENGINE_LOG) Debug.LogError(string.Format("██ {0} - {1}", sender.ToString(), message));
	}
	public static void EngineLogWarning(Type sender, string message)
	{
		if (SHOW_ENGINE_LOG) Debug.LogWarning(string.Format("██ {0} - {1}", sender.ToString(), message));
	}
	
}
