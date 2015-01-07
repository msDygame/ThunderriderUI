using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class JSK_LogoMenuProcess : MonoBehaviour
{
	private int		fadeDir     = 0;
	private float	fadeSpeed	= 0.2f;
	private float	alpha       = 0.0f;
	
	void Awake()
	{
		//if( Application.platform == RuntimePlatform.Android && JSK_PlatformManager.g_LaunchAiwi )
			//this.GetComponent<JSK_PlatformManager>().LaunchAIWI();
		
		JSK_GlobalProcess.InitGlobal();
		//JSK_GlobalProcess.g_IsWaitInfo = true;
	}
	
	void Start()
	{
		fadeIn();
		//StartCoroutine(sendFifoScene(1.0f));
        //if( JSK_GlobalProcess.g_LenovoVerson )
        //{
        //    Invoke("checkGetDevList", 1.4f);
        //    Invoke("setPlay", 1.8f);
        //    Invoke("checkUserInfo", 2.2f);
        //    Invoke("checkQRString1", 2.6f);
        //}
        //else
        //{
        //    Invoke("checkConnectState", 1.5f);
        //    Invoke("checkCopyRight", 2.0f);
        //    Invoke("checkQRString2", 2.5f);
        //}
	}
	
    //IEnumerator sendFifoScene( float sceond )
    //{
    //    yield return new WaitForSeconds(sceond);
    //    JSK_DebugProcess.AddLog("SetFifoScene");
    //    JSK_GlobalProcess.SetFifoScene(1,1);
    //    JSK_GlobalProcess.SetFifoScene(2,1);
    //}
	
    //void checkGetDevList()
    //{
    //    JSK_GameFifo.SendMsgNew("GetDevList", 1);
    //}
		
    //void checkConnectState()
    //{
    //    JSK_GameFifo.GetDeviceState();
    //}
	
    //void checkCopyRight()
    //{
    //    JSK_GlobalProcess.SendFifoMsg("CopyRight", "1P");
    //}
	
    //void setPlay()
    //{
    //    JSK_GameFifo.SendMsgNew("SetAsPlayDev", 1);
    //    JSK_GameFifo.SendMsgNew("SetAsPlayDev", 2);
    //}
	
    //void checkUserInfo()
    //{
    //    JSK_GameFifo.SendMsg("UserInfo", "1P");
    //}
	
    //void checkQRString1()
    //{
    //    JSK_GameFifo.SendMsgNew("GetQRString", 1);
    //    JSK_GameFifo.SendMsgNew("GetQRString", 2);
    //}
	
    //void checkQRString2()
    //{
		
    //}
	
	void OnGUI()
	{
		GUI.depth = 2;
		
		float delta = Time.deltaTime;
		if( delta > 0.05f )
			delta = 0.05f;
		
		if( fadeDir != 0 )
		{
			float val = fadeDir * fadeSpeed * delta;
			alpha += val; 
			alpha = Mathf.Clamp01(alpha);
			//GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), JSK_GUIProcess.GetPicture("DYGameStart2013"));
		}
		else
			return;
			
		if( alpha >= 1.0f )
		{
			fadeOut();
		}
		else if( alpha <= 0.0f )
		{
			fadeDir = 0;
            //if( JSK_GlobalProcess.g_IsWaitInfo )
            //{
            //    JSK_DebugProcess.AddLog("No Aiwi UserInfo");
            //    JSK_GlobalProcess.g_ModuleVerson = 0;
            //    JSK_GlobalProcess.g_IsWaitInfo = false;
            //    JSK_GlobalProcess.g_IsConncet1P = false;
            //    JSK_GlobalProcess.g_IsConncet2P = false;
            //}
			JSK_GlobalProcess.g_ModuleVerson = 1;//hack
			JSK_GlobalProcess.ReturnMainMenu();
		}
	}
	
	void fadeIn()
	{
	    fadeDir = 1; 
		alpha = 0.0f;
	}
	
	void fadeOut()
	{
	    fadeDir = -1;
		alpha = 1.0f;
	}
	
}
