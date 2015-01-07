using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum JSK_LogoState
{
	None,
	LogoPlay,
	UseStatute,
	Warning
}
public class LogoProcess : MonoBehaviour
{
    public static bool IsShow = false;
    private bool IsPlaySound = false;
    private float StartTime = 0;
    private int fps = 24;
	
	#if UNITY_STANDALONE_WIN
	private JSK_LogoState logoState = JSK_LogoState.LogoPlay;
	
	//电影纹理
	public MovieTexture movTexture;
	private Texture2D pic = null;
	private Texture2D ok_pic = null;
	private Texture2D ok_click_pic = null;
	private Texture2D use_statute_pic = null;
	private Texture2D use_statute_esc = null;
	private Texture2D use_statute_ok = null;
	private Texture2D use_statute_esc_f = null;
	private Texture2D use_statute_ok_f = null;
	
	private float showTime = 0;
	private int	  firstStart = -1;
	
	private int curIndex = 0;
	private bool IsLoadLevelOnce = false ;
	#elif UNITY_ANDROID
	private Texture2D pic = null;//20141029Delete.
//      public MovieTexture movTexture;//20141030Delete.Androud not support!
	#endif
	
   void Awake()
   {
   //     UnityEngine.Debug.Log("#####logo Process Awake.");
   //     #if UNITY_STANDALONE_WIN
   //     if( Application.platform == RuntimePlatform.Android && JSK_PlatformManager.g_LaunchAiwi )
   //         this.GetComponent<JSK_PlatformManager>().LaunchAIWI();
		
   //     string[] arguments = Environment.GetCommandLineArgs();
		
   //     if( JSK_GlobalProcess.g_IsNTTVersion )
   //     {
   //         //JSK_GlobalProcess.g_NTTCommand = arguments;
   //         JSK_GlobalProcess.ParseNTTParameter(arguments);
   //     }
		
   //     JSK_GlobalProcess.InitGlobal();
   //     JSK_GlobalProcess.g_IsWaitInfo = true;
   //     firstStart = ConfigManager.GetDataInt("FirstStart");
   //     #elif UNITY_ANDROID
   //         if( Application.platform == RuntimePlatform.Android && JSK_PlatformManager.g_LaunchAiwi )
   //         this.GetComponent<JSK_PlatformManager>().LaunchAIWI();
   ////      20141029 20141030
   //         Debug.Log("#######Awake()");
          pic = (Texture2D)Resources.Load("Photos/logo_anm/dylogo_anm_003");
   //         JSK_GlobalProcess.InitGlobal();
   //         JSK_GlobalProcess.g_IsWaitInfo = true;			
   //     #endif
		
   }

    void Start()
    {
        //IsShow = true;
        //IsPlaySound = false;
        //GlobalSetting.Init();
        //StartTime = Time.time + 0.5f;
		
		//20130704

//		Invoke("checkGetDevList", 1.4f);
//		Invoke("setPlay", 1.8f);
//		Invoke("checkUserInfo", 2.2f);
//		Invoke("checkQRString1", 2.6f);
		
		//if(!movTexture.isPlaying)
		//{
		#if UNITY_STANDALONE_WIN
		movTexture.Play();
		
		AudioSource SoundAudio = gameObject.GetComponent<AudioSource>();
    	SoundAudio.Play();
		
		pic = (Texture2D)Resources.Load("JSK/language/control_content");
		ok_pic = (Texture2D)Resources.Load("JSK/language/Button_ok_cn");
		ok_click_pic = (Texture2D)Resources.Load("JSK/language/Button_ok_click_cn");
		
		use_statute_pic = (Texture2D)Resources.Load("JSK/language/UseStatute_cn");
		use_statute_esc = (Texture2D)Resources.Load("JSK/language/btn_notagree_cn");
		use_statute_ok = (Texture2D)Resources.Load("JSK/language/btn_agree_cn");
		use_statute_esc_f = (Texture2D)Resources.Load("JSK/language/btn_notagree_focus_cn");
		use_statute_ok_f = (Texture2D)Resources.Load("JSK/language/btn_agree_focus_cn");
		//}
		#elif UNITY_ANDROID
 /*     movTexture.Play();
        Handheld.PlayFullScreenMovie("dyLogo1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
 */
        AudioSource SoundAudio = gameObject.GetComponent<AudioSource>();
        SoundAudio.Play();   

			IsShow = true;
	        IsPlaySound = false;
	        //GlobalSetting.Init();
	        StartTime = Time.time + 0.5f;
        	Debug.Log("#######Start()");
 
		//20130704
            //Invoke("checkCopyRight", 2.0f);
            //Invoke("checkGetDevList", 1.4f);
            //Invoke("setPlay", 1.8f);
            //Invoke("checkUserInfo", 2.2f);
            //Invoke("checkQRString1", 2.0f);
#endif

    }

    void OnGUI()
    {
//        float t = Time.time - StartTime;
//        if (t < 0) return;
//        if (!IsPlaySound)
//        {
//            IsPlaySound = true;
//            //JSK_SoundProcess.PlaySound("dygame_logo");
//			//AudioSource SoundAudio = gameObject.GetComponent<AudioSource>();
//            //SoundAudio.Play();
//        }
//        int index = (int)(t * fps);
//        Texture2D pic = (Texture2D)Resources.Load("Photos/logo_anm/dylogo_anm_" + index.ToString("000"));
//        //if (pic != null) GUI.DrawTexture(new Rect(Screen.width / 2 - pic.width / 2, Screen.height / 2 - pic.height / 2, pic.width, pic.height), pic);
//		float PicWith = Screen.height * 0.60f;
//        float PicHeight = Screen.height * 0.30f;
//        if (pic != null) GUI.DrawTexture(new Rect(Screen.width / 2 - PicWith / 2, Screen.height / 2 - PicHeight / 2, PicWith, PicHeight), pic);
//
//        if (index > 0 && pic == null)
//        {
//            IsShow = false;
//            Destroy(this.gameObject);
//            //Application.LoadLevel("JSK_LogoMenu");
//			//20130704
//			if( JSK_GlobalProcess.g_IsWaitInfo )
//			{
//				JSK_DebugProcess.AddLog("No Aiwi UserInfo");
//				//JSK_GlobalProcess.g_ModuleVerson = 0;
//				JSK_GlobalProcess.g_IsWaitInfo = false;
//				JSK_GlobalProcess.g_IsConncet1P = false;
//				JSK_GlobalProcess.g_IsConncet2P = false;
//			}
//			JSK_GlobalProcess.ReturnMainMenu();
//			//JSK_GlobalProcess.ReturnMainMenuGC();
//        }
		#if UNITY_STANDALONE_WIN
		int   width     = Screen.width;
		int   height    = Screen.height;
		float offWidth	= width / 100.0f;
		float offHeight	= height / 100.0f;
		
		if( logoState == JSK_LogoState.LogoPlay )
		{
			GUI.DrawTexture (new Rect (0,0, Screen.width, Screen.height),movTexture,ScaleMode.StretchToFill);
			
			if(!movTexture.isPlaying)
			{
				//if( JSK_GlobalProcess.g_IsWaitInfo )
				//{
				//	JSK_DebugProcess.AddLog("No Aiwi UserInfo");
					
				//	JSK_GlobalProcess.g_IsWaitInfo = false;
				//	JSK_GlobalProcess.g_IsConncet1P = false;
				//	JSK_GlobalProcess.g_IsConncet2P = false;
				//}
				//ConfigManager.SaveConfig();
				if (IsLoadLevelOnce == false)
				{
					JSK_GlobalProcess.ReturnMainMenu();
					IsLoadLevelOnce = true ;
				//logoState = JSK_LogoState.Warning;
//				if( firstStart == 0 )
//				{
//					logoState = JSK_LogoState.UseStatute;
//				}
//				else if ( firstStart == 1)
//				{
//					logoState = JSK_LogoState.Warning;
//				}
				}
			}
		}
		else if( logoState == JSK_LogoState.UseStatute )
		{
			GUI.DrawTexture (new Rect (0,0, Screen.width, Screen.height),use_statute_pic,ScaleMode.StretchToFill);
			if( curIndex == 0 )
			{
				GUI.DrawTexture (new Rect (offWidth * 23,offHeight *90, use_statute_esc_f.width/2.5f, use_statute_esc_f.height/2.5f),use_statute_esc_f,ScaleMode.StretchToFill);
				GUI.DrawTexture (new Rect (offWidth * 43,offHeight *90, use_statute_ok.width/2.5f, use_statute_ok.height/2.5f),use_statute_ok,ScaleMode.StretchToFill);
			}
			else if( curIndex == 1 )
			{
				GUI.DrawTexture (new Rect (offWidth * 23,offHeight *90, use_statute_esc.width/2.5f, use_statute_esc.height/2.5f),use_statute_esc,ScaleMode.StretchToFill);
				GUI.DrawTexture (new Rect (offWidth * 43,offHeight *90, use_statute_ok_f.width/2.5f, use_statute_ok_f.height/2.5f),use_statute_ok_f,ScaleMode.StretchToFill);
			}
		}
		else if( logoState == JSK_LogoState.Warning )
		{
			
			
			if (pic != null) //GUI.DrawTexture(new Rect(Screen.width / 2 - pic.width / 2, Screen.height / 2 - pic.height / 2, pic.width, pic.height), pic);
			{
				GUI.DrawTexture (new Rect (0,0, Screen.width, Screen.height),pic,ScaleMode.StretchToFill);
				GUI.DrawTexture (new Rect (offWidth * 43,offHeight *90, ok_pic.width/3.5f, ok_pic.height/3.5f),ok_pic,ScaleMode.StretchToFill);
				
			}
			//if( showTime >= 5 )
			//	JSK_GlobalProcess.ReturnMainMenu();
		}
		#elif UNITY_ANDROID
		
			float t = Time.time - StartTime;
	        if (t < 0) return;
/*
	        if (!IsPlaySound)
	        {
	            IsPlaySound = true;
	            JSK_SoundProcess.PlaySound("dygame_logo");
				//AudioSource SoundAudio = gameObject.GetComponent<AudioSource>();
	            //SoundAudio.Play();
	        }
 */
	        //int index = (int)(t * fps);
	        //Texture2D pic = (Texture2D)Resources.Load("Photos/logo_anm/dylogo_anm_" + index.ToString("000"));
	        //if (pic != null) GUI.DrawTexture(new Rect(Screen.width / 2 - pic.width / 2, Screen.height / 2 - pic.height / 2, pic.width, pic.height), pic);
			float PicWith = Screen.height * 0.60f;
	        float PicHeight = Screen.height * 0.30f;
	        if (pic != null) GUI.DrawTexture(new Rect(Screen.width / 2 - PicWith / 2, Screen.height / 2 - PicHeight / 2, PicWith, PicHeight), pic);
	
	        //if (index > 0 && pic == null)

	        if ( t > 3.0f )
			{	    
	            //Application.LoadLevel("JSK_LogoMenu");
				//20130704
                //if( JSK_GlobalProcess.g_IsWaitInfo )
                //{
                //    Debug.Log("#######JSK_GlobalProcess.g_IsWaitInfo:" + JSK_GlobalProcess.g_IsWaitInfo);	
                //    JSK_DebugProcess.AddLog("No Aiwi UserInfo");
                //    JSK_GlobalProcess.g_ModuleVerson = 0;
                //    JSK_GlobalProcess.g_IsWaitInfo = false;
                //    JSK_GlobalProcess.g_IsConncet1P = false;
                //    JSK_GlobalProcess.g_IsConncet2P = false;
                //}
                //20141027 Fix.
				//JSK_GlobalProcess.ReturnMainMenu();
                int iButtonWidth = 300;
                int iButtonHeight = 50;
                int iButtonSpace = (int)(iButtonHeight * 0.5);
                int iLocationX = 10;
                int iLocationY = 10;

                if (GUI.Button(new Rect(iLocationX, 10 + (iButtonHeight + iButtonSpace) * 0, iButtonWidth, iButtonHeight), "Version 1 Original MainMenu"))
                {
                    JSK_GlobalProcess.g_iDemoUIVersion = 1;
                    IsShow = false;
                    Destroy(this.gameObject);
                    Application.LoadLevel("JSK_MainMenu");//主選單介面
                }
                if (GUI.Button(new Rect(iLocationX, 10 + (iButtonHeight + iButtonSpace) * 1, iButtonWidth, iButtonHeight), "Version 2 Playmake"))
                {
                    JSK_GlobalProcess.g_iDemoUIVersion = 2;
                    IsShow = false;
                    Destroy(this.gameObject);
                    Application.LoadLevel("UI_chara");//選角介面          
                }
                iLocationY = 10 + (iButtonHeight + 10) * 2;
                if (GUI.Button(new Rect(iLocationX, 10 + (iButtonHeight + iButtonSpace) * 2, iButtonWidth, iButtonHeight), "Version 3 newUI"))
                {
                    JSK_GlobalProcess.g_iDemoUIVersion = 3;
                    IsShow = false;
                    Destroy(this.gameObject);
                    Application.LoadLevel("UI_Chara");//選角介面          
                }
                if (GUI.Button(new Rect(iLocationX, 10 + (iButtonHeight + iButtonSpace) * 3, iButtonWidth, iButtonHeight), "Version 4 nGUI"))
                {
                    JSK_GlobalProcess.g_iDemoUIVersion = 4;
                    IsShow = false;
                    Destroy(this.gameObject);
                    Application.LoadLevel("UI_Chara");//選角介面          
                }                
	        }

  /*      	GUI.DrawTexture (new Rect (0,0, Screen.width, Screen.height),movTexture,ScaleMode.StretchToFill);
            if (!movTexture.isPlaying)
            {
                StartCoroutine(InvokeLoadLevel());                
            }
*/
#endif
    }


    void Update()
    {
        //GameFifo.GetMsg();
		//if( logoState == JSK_LogoState.Warning )
		//	showTime+=Time.deltaTime;
		 #if UNITY_STANDALONE_WIN
		if( logoState == JSK_LogoState.UseStatute )
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				JSK_SoundProcess.PlaySound("MenuSelect");
				
				if( curIndex == 1 )
					logoState = JSK_LogoState.Warning;
				else if( curIndex == 0)
					JSK_GlobalProcess.ExitGame();
			}
			else if( Input.GetKeyDown(KeyCode.LeftArrow))
			{
				JSK_SoundProcess.PlaySound("MenuMove");
				curIndex--;
				if( curIndex < 0 )
					curIndex = 0;
			}
			else if( Input.GetKeyDown(KeyCode.RightArrow))
			{
				JSK_SoundProcess.PlaySound("MenuMove");
				curIndex++;
				if( curIndex > 1 )
					curIndex = 1;
			}
			
			//Debug.Log("curIndex:" + curIndex);
		}
		else if( logoState == JSK_LogoState.Warning )
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				JSK_SoundProcess.PlaySound("MenuSelect");
				if( firstStart == 0 )
				{
					ConfigManager.SetDataInt("FirstStart",1);
					ConfigManager.SaveConfig();
				}
				JSK_GlobalProcess.ReturnMainMenu();
			}
		}
		#endif
    }
	
	IEnumerator sendFifoScene( float sceond )
	{
		yield return new WaitForSeconds(sceond);
		JSK_DebugProcess.AddLog("SetFifoScene");
		JSK_GlobalProcess.SetFifoScene(1,1);
		JSK_GlobalProcess.SetFifoScene(2,1);
	}

    IEnumerator InvokeLoadLevel()
    {
        yield return new WaitForSeconds(0f);
        Destroy(this.gameObject);
        //if (JSK_GlobalProcess.g_IsWaitInfo)
        //{
        //    JSK_GlobalProcess.g_ModuleVerson = 0;
        //    JSK_GlobalProcess.g_IsWaitInfo = false;
        //    JSK_GlobalProcess.g_IsConncet1P = false;
        //    JSK_GlobalProcess.g_IsConncet2P = false;
        //}
        //20141027 Fix.Change LoadLevel Logo-Tachimu                
        Application.LoadLevel("UI_Chara");//選角介面
    }
	
    //void checkGetDevList()
    //{
    //    Debug.Log("#######checkGetDevList");
    //    JSK_GameFifo.SendMsgNew("GetDevList", 1);
    //}
		
    //void checkConnectState()
    //{
    //    Debug.Log("#######checkConnectState");
    //    JSK_GameFifo.GetDeviceState();
    //}
	
    //void checkCopyRight()
    //{
    //    Debug.Log("#######checkCopyRight");
    //    //JSK_GlobalProcess.SendFifoMsg("CopyRight", "1P");
    //    JSK_GlobalProcess.SendFifoMsg("License", "1P");//20130809
    //}
	
    //void setPlay()
    //{
    //    Debug.Log("#######setPlay");
    //    JSK_GameFifo.SendMsgNew("SetAsPlayDev", 1);
    //    JSK_GameFifo.SendMsgNew("SetAsPlayDev", 2);
    //}
	
    //void checkUserInfo()
    //{
    //    Debug.Log("#######checkUserInfo");
    //    JSK_GameFifo.SendMsg("UserInfo", "1P");
    //}
	
    //void checkQRString1()
    //{
    //    Debug.Log("#######checkQRString1");
    //    JSK_GameFifo.SendMsgNew("GetQRString", 1);
    //    JSK_GameFifo.SendMsgNew("GetQRString", 2);
    //}
	
    //void checkQRString2()
    //{
		
    //}
}
