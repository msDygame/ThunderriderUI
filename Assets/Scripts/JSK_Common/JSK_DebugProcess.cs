using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
	
public class JSK_DebugProcess : MonoBehaviour
{
	public 	bool 	isShowFPS		= false;	//是否显示帧数.
	public  bool 	isShowLOG		= false;	//是否显示日志.
	public  bool 	isResetRecord	= false;	//是否显示重设记录按钮.
	
	private float 	updateInterval	= 1.0f;
	private float 	lastInterval	= 0;
	private int 	frames			= 0;
	private float 	fps				= 0;
	
	private static 	ArrayList LogList = new ArrayList();

	public static void AddLog( string sMsg )
	{
		Debug.Log(sMsg);
		if( LogList.Count >= 20 )
			LogList.RemoveAt(0);
		LogList.Add(sMsg);
	}

	void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	void OnGUI()
	{
		GUI.depth = 0;
		
		if( isShowLOG )
    	{
			int nLog = 0;
        	foreach( string sLog in LogList )
        	{
				GUI.Box(new Rect(Screen.width - 310, Screen.height - 20 - LogList.Count * 20 + nLog * 20, 300, 20), sLog);
				nLog++;
			}
		}
		
    	if( isShowFPS )
		{
        	GUI.Box(new Rect(5, Screen.height - 40, 200, 30), "@" + Screen.width + "x" + Screen.height + " , FPS : " + fps.ToString("f2"));
    	}
		
		if( isResetRecord )
		{
			if( GUI.Button(new Rect(5, Screen.height - 70, 200, 30), "ResetRecord") )
				JSK_GlobalProcess.DebugResetPlayerData();
		}
	}

	void Update()
	{
		//if( Input.GetKeyUp(KeyCode.F11) )
			//isShowLOG = !isShowLOG;
		//if( Input.GetKeyUp(KeyCode.F12))
			//isShowFPS = !isShowFPS;
		
    	if( !isShowFPS )
			return;
		
    	++frames;
		float timeNow = Time.realtimeSinceStartup;
		if( timeNow > lastInterval + updateInterval )
		{
			fps = frames / (timeNow - lastInterval);
			frames = 0;
			lastInterval = timeNow;
		}
	}
	
}
