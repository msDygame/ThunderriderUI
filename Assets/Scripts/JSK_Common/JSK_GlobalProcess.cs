using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Diagnostics;

public class JSK_GlobalProcess : MonoBehaviour
{
	public static bool		g_IsLogin				= false;
	public static string	g_NTTID					= "";
	public static string	g_PlayerNickName		= "";
	public static bool		g_IsMultiPlayer			= true;			// 多人遊戲
#if _DebugMode_
	public static bool		g_IsWebServer			= false;		// WEB Server使用
#else
    public static bool      g_IsWebServer           = false;    	// WEB Server使用 //"http: //183.129.244.122/thunderrider/Manager.aspx?data=" ;
#endif
	public static bool		g_IsMultipleRound		= true;
	public static bool		g_IsQuitGame 		= false;
	private static string	g_Version			= "JP0.0.3";
	public static int		g_NTTGameMode		= 1;				//NTT游戏模式 1:单人模式 2:双人模式
	public static bool		g_IsVerticalSplitScreen		= true;		//是否使用垂直分割畫面
	public static bool		g_IsNTTVersion		= true;				//是否NTT版本
	public static string 	g_LobbyID			= "";				//大廳ID
	public static bool	 	g_UseAchivement		= false;			//是否顯示成就
	public static bool 		g_IsReleaseDemo		= false;			//是否顯示內部測試版
	public static bool 		g_LenovoVerson 		= true;				//是否是联想的版本.
#if _DebugMode_
	public static bool 		g_UseTouchGame		= false;			//游戏是否使用触摸输入.
#else
	public static bool 		g_UseTouchGame		= false;			//游戏是否使用触摸输入.
#endif
	public static bool 		g_UseTouchMenu		= true;				//菜单界面是否使用触摸输入.
	public static bool		g_UseNetMode		= false;			//是否使用网络模式.
	public static bool		g_ShowBuyButton		= false;			//是否显示购买完整版按钮.
	public static bool		g_UseKeyBoardGame	= true;				//游戏中是否使用键盘控制.
	public static bool		g_UseNewActor		= true;				//是否使用新角色.
	public static bool		g_IsAndroidRing		= true;				//是否是安卓降规格的环形赛道.
	public static bool		g_UseNewAIPoint		= true;				//使用新路徑模式 iTweenPath
	
	public static int		g_LevelNum	  		= 5;				//5个关卡的存档记录.
	public static int[] 	g_LevelRecord  		= new int[5];		//5个关卡的存档记录.
	public static int[] 	g_LevelImpl  		= new int[5];		//5个关卡的内部最好记录.
#if _DebugMode_
	public static int[] 	g_LevelRound  		= {2,2,2,2,2};		//5个关卡最多跑幾圈.
#else
	public static int[] 	g_LevelRound  		= {3,3,3,3,2};		//5个关卡最多跑幾圈.
#endif
	public static float[] 	g_LevelDifficult	= {2,1.5f,1,3.5f,4};		//5个关卡難易度
	public static int 		g_ModuleVerson  	= 1;				//遊戲版本 0:Trial 1:Std.
	public static int 		g_Screen_Width		= 1280;  			//游戏的宽.
	public static int 		g_Screen_Height		= 720;   			//游戏的高.
	public static float 	g_ExitTime      	= 0;	  			//退出时间,COPY过来的.
	//public static bool		g_IsWaitInfo    	= false;			//是否正在等待传入的消息.
	public static int 		g_Language      	= 1;				//语言版本 1简中 2英语.
	public static bool 		g_IsAndroid     	= false;			//是否是安卓平台,暂时用来做触摸功能.
	public static bool		g_IsConncet1P		= true;				//游戏一启动的时候是否连接上手机.
	public static bool		g_IsConncet2P		= true;				//游戏一启动的时候是否连接上手机.
	
	public static ArrayList g_Characters 		= new ArrayList();	//所有的角色资料集合.
	public static ArrayList g_Actions			= new ArrayList();	//所有的动作资料集合.
	public static Hashtable g_ActionMap			= new Hashtable();	//根据名称找到动作.
	
	public static ArrayList	MotoPosList 		= new ArrayList();	//摩托车的位置偏移列表.
	
	public static int		g_SelectCharStep	= 0;				//双人游戏的选择步骤.
	public static int		g_SelectMotoStep	= 0;				//双人游戏的选择步骤.

	public static GameObject	g_GlobalObject	= null;
	
	public static DateTime		GameInitTime;			//遊戲啟動時間
	public static DateTime		GameEndTime;			//遊戲結束時間
    public static bool      g_IsShowFPS        = false;
    public static int       g_iDemoUIVersion = 1;
    public static int       Game1P = 1;//1:男,2:女
    public static int       GameMoto1P = 1;//1~9 MotoIndex
    public static int       GamePlace = 1;//1~5
    public static int       GameOil = 1;
	
	public static void InitGlobal()
	{
		Time.timeScale = 1.0f;//放在这吧.
		GameObject obj = GameObject.Find("JSK_GlobalProcess");
		if( !obj )
		{
			UnityEngine.Debug.Log("JSK_GlobalProcess init.");
			obj = new GameObject();
			obj.name = "JSK_GlobalProcess";
			obj.AddComponent<AudioListener>();
			obj.AddComponent<JSK_GlobalProcess>();
			obj.AddComponent<JSK_DebugProcess>();
			obj.AddComponent<JSK_SoundProcess>();
			//obj.AddComponent<JSK_NetProcess>();
			obj.AddComponent<JSK_GUIProcess>();
//			obj.AddComponent<JSK_LoadingProcess>();//20140911.Delete
			obj.AddComponent<JSK_GlobalLoadingState>();//20140915.Add.取代JSK_LoadingProcess;現在的JSK_LoadingProcess為JSK_LoadingMenu.unity的process		
			//obj.AddComponent<JSK_OffLineProcess>();
//          UIProcess.Init();
//          DeviceProcess.Init();

			if( g_IsMultiPlayer && g_IsWebServer )
			{
/* //20150105DemoUI
				obj.AddComponent<WebServerProcess>();
				//obj.AddComponent<UploadScoreTest>();
 */ 		}

			//if( g_IsNTTVersion )
			//	obj.AddComponent<JSK_GCAPI>();
/*			
			ConfigManager.Init();
*/			
			
			LoadPlayerData();
			
			//20131120  for GC mark
			//InitQuality();
			InitResolution();
			
            //if( g_IsUploadLog )
            //{
            //    GameInitTime = DateTime.Now;
            //    DYGameLog.SetUserInfo("dylobby_20", "user_456", "11111");
            //}

			//JSK_GameFifo.InitGameFifo();//初始化手柄输入.
			if( Application.platform == RuntimePlatform.WindowsPlayer )
			{
				//if( !JSK_GameProcess.IsShowConfig )
				//	Screen.showCursor = false;
				//if( Application.loadedLevelName == "JSK_LogoMenu" )
				//	g_UseKeyBoardGame = false;
			}
			//g_IsWaitInfo = false;
			if( Application.loadedLevelName != "JSK_LogoMenu" )
			{
				g_IsConncet1P = false;
				g_IsConncet2P = false;
			}
			//创建全局资源.
			JSK_EffectResource.g_EffectResourceObj = CreatGlobalObject("JSK/Common/EffectResource");
			JSK_EffectResource.g_EffectResourceObj.name = "JSK_GameEffectResource";
			
			JSK_PictureResource.g_PictureResourceObj = CreatGlobalObject("JSK/Common/PictureResource");
			JSK_PictureResource.g_PictureResourceObj.name = "JSK_GamePictureResource";

			//創建全局 數字sprite. 20140918.Add. 各UI介面的數字使用,Ex:金幣,Exp,Level,etc..
			JSK_SpriteRendererUtility.JSK_SpriteRendererObject = (GameObject)Instantiate(Resources.Load("JSK/Common/SpriteRendererUtility"));
/*
			//創建全局 WebServer Static Object. 20140923.Add.
			GameObject WebServerObject = (GameObject)Instantiate(Resources.Load("JSK/Common/WebServerStaticObject"));
			DontDestroyOnLoad(WebServerObject);
			WebServerObject.name = "WebServerUtility(DontDestroy)" ;
*/
			//創建全局 RivalList.20140926 Add. 由 配對頁面(Jsk_MatchMenu.unity)所產生的配對玩家RivalList,給予GamePlay使用.
			GameObject RivalListObject = (GameObject)Instantiate(Resources.Load("JSK/Common/MatchedRivalList"));
			DontDestroyOnLoad(RivalListObject);
			RivalListObject.name = "MatchedRivalList(DontDestroy)" ;

			//創建全局 FPS object.20141028Add.For Debug how many lag...
            if (g_IsShowFPS == true)
            {
                GameObject HUD_FPS_Object = (GameObject)Instantiate(Resources.Load("JSK/Common/HUDFps"));
                DontDestroyOnLoad(HUD_FPS_Object);
                HUD_FPS_Object.name = "HUD_FPS(globalObject)";
            }

			Physics.gravity = new Vector3(0, -15.0f, 0);
//			g_LevelImpl[0] = 7400;
//			g_LevelImpl[1] = 7000;
//			g_LevelImpl[2] = 6500;
//			g_LevelImpl[3] = 6500;
//			g_LevelImpl[4] = 13000;
			
			if( g_IsMultipleRound )
			{
//				g_LevelImpl[0] = 4000 * g_LevelRound[0];
//				g_LevelImpl[1] = 4000 * g_LevelRound[1];
//				g_LevelImpl[2] = 4000 * g_LevelRound[2];
//				g_LevelImpl[3] = 5000 * g_LevelRound[3];
//				g_LevelImpl[4] = 10000 * g_LevelRound[4];
				g_LevelImpl[0] = 4500 * g_LevelRound[0];
				g_LevelImpl[1] = 4500 * g_LevelRound[1];
				g_LevelImpl[2] = 4500 * g_LevelRound[2];
				g_LevelImpl[3] = 4500 * g_LevelRound[3];
				g_LevelImpl[4] = 10000 * g_LevelRound[4];
			}
			else
			{
				g_LevelImpl[0] = 4000;
				g_LevelImpl[1] = 4000;
				g_LevelImpl[2] = 4000;
				g_LevelImpl[3] = 5000;
				g_LevelImpl[4] = 10000;
			}
			
			
			//成就初始
	        AchievementManager.Init();
	        //變量歸零
	        AchievementManager.SetVarZero(AchievementVarType.Application);
		}
	}
	
	void Awake()
	{
		UnityEngine.Debug.Log("JSK_GlobalProcess Awake.");
		DontDestroyOnLoad(gameObject);
		//Screen.showCursor = false;
		JSK_DebugProcess.AddLog("SleepTimeout = " + Screen.sleepTimeout);
		if( Application.platform == RuntimePlatform.Android )
		{
			g_IsAndroid = true;
	   		Screen.sleepTimeout = 0;//让Android的屏幕不会休眠.
			//iPhoneSettings.screenCanDarken = false;
			//QualitySettings.maxQueuedFrames = 60;
		}
		Application.targetFrameRate = 30;
		g_ExitTime = 0;
		
		MotoPosList.Clear();
		MotoPosList.Add(Vector3.zero);
		MotoPosList.Add(new Vector3(0, 0, 0.03f));
		MotoPosList.Add(new Vector3(0, 0.02f, 0.035f));
		MotoPosList.Add(new Vector3(0, 0.07f, 0.01f));
		MotoPosList.Add(new Vector3(0, 0.09f, -0.35f));
		MotoPosList.Add(new Vector3(0, -0.12f, 0.04f));
		MotoPosList.Add(new Vector3(0, -0.06f, 0.2f));
		MotoPosList.Add(new Vector3(0, -0.55f, 0.09f));
		MotoPosList.Add(new Vector3(0, -0.55f, -0.11f));
		MotoPosList.Add(new Vector3(0, 0.1f, -0.09f));
		MotoPosList.Add(new Vector3(0, -0.08f, 0.14f));
		
		g_Characters.Clear();
		g_Actions.Clear();
		g_ActionMap.Clear();
		
		//创建人物资料集===========================================
		JSK_CharacterData ch;
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Jim";
		ch.AnimString	= "Am_Actor01_";
		ch.PrefabName 	= "JSK/Player/PlayerJim";
		ch.MassWater	= 0.13f;
		ch.MassFall		= 2.2f;
		ch.IdleTime		= 7.0f;
		ch.TurboPow		= 1.8f;
		g_Characters.Add(ch);
		
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Aiko";
		ch.AnimString	= "Am_Actor02_";
		ch.PrefabName 	= "JSK/Player/PlayerAiko";
		ch.MassWater	= 0.11f;
		ch.MassFall		= 2.0f;
		ch.IdleTime		= 6.0f;
		ch.TurboPow		= 1.7f;
		g_Characters.Add(ch);
		
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Charles";
		ch.AnimString	= "Am_Actor03_";
		ch.PrefabName 	= "JSK/Player/PlayerCharles";
		ch.MassWater	= 0.18f;
		ch.MassFall		= 2.8f;
		ch.IdleTime		= 7.0f;
		ch.TurboPow		= 2.0f;
		g_Characters.Add(ch);
		
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Giselie";
		ch.AnimString	= "Am_Actor04_";
		ch.PrefabName 	= "JSK/Player/PlayerGiselie";
		ch.MassWater	= 0.12f;
		ch.MassFall		= 2.1f;
		ch.IdleTime		= 7.5f;
		ch.TurboPow		= 1.7f;
		g_Characters.Add(ch);
		
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Karl";
		ch.AnimString	= "Am_Actor05_";
		ch.PrefabName 	= "JSK/Player/PlayerKarl";
		ch.MassWater	= 0.14f;
		ch.MassFall		= 2.3f;
		ch.IdleTime		= 8.0f;
		ch.TurboPow		= 1.9f;
		g_Characters.Add(ch);
		
		//2个新角色
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Male";
		ch.AnimString	= "";
		ch.SoundString	= "Boy";
		ch.PrefabName 	= "JSK/Player/PlayerMale";
		ch.MassWater	= 0.12f;
		ch.MassFall		= 2.0f;
		ch.IdleTime		= 8.0f;
		ch.TurboPow		= 1.5f;
		g_Characters.Add(ch);
		
		ch = gameObject.AddComponent<JSK_CharacterData>();
		ch.Name 		= "Female";
		ch.AnimString	= "";
		ch.SoundString	= "Girl";
		ch.PrefabName 	= "JSK/Player/PlayerFemale";
		ch.MassWater	= 0.12f;
		ch.MassFall		= 2.0f;
		ch.IdleTime		= 8.0f;
		ch.TurboPow		= 1.5f;
		g_Characters.Add(ch);
		
		//建立动作资料集=============================================
		JSK_PlayerAction act; 
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "ready";//原地IDLE状态.
		act.ActionLayer = 0;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "up_and_down";//前进状态.
		act.ActionLayer = 0;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "left";//左转弯状态.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "right";//右转弯状态.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "parking_left";//左倒退状态.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "parking_right";//右倒退状态.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "restart";//加速状态.
		act.ActionLayer = 2;
		act.ActionRestart = true;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "collide_front";//前部收到撞击动作(大).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "collision_front";//前部收到撞击动作(小).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "hit_rear";//后部受到撞击动作(大).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "hit_right";//左部收到撞击动作(大).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "hit_left";//右部收到撞击动作(大).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_1_maintain";//冲刺板动作1.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_2_maintain";//冲刺板动作2.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_3_maintain";//冲刺板动作3.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_1";//腾空1.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_2";//腾空2.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "jump_3";//腾空3.
		act.ActionLayer = 2;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "start";//开始动作.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "boost";//油门(空闲).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "ass";//伸手(空闲).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "move_shoulder";//活动肩膀(空闲).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "rejection_hand";//左右摆手(空闲).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "shoulder";//挠头(空闲).
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "end";//结束动作.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "winner_1";//胜利动作1.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "winner_2";//胜利动作1.
		act.ActionLayer = 1;
		g_Actions.Add(act);
			
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "LOSER_1";//失败动作1.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "LOSER_2";//失败动作2.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		act = gameObject.AddComponent<JSK_PlayerAction>();
		act.AnimName = "LOSER_3_maintain";//失败动作3.
		act.ActionLayer = 1;
		g_Actions.Add(act);
		
		foreach( JSK_PlayerAction action in g_Actions )
			g_ActionMap.Add(action.AnimName, action);
	}
	
	public static void InitQuality()
	{

	}
	
	public static void InitResolution()
	{
		//此函数在2个地方调用,一是读取完配置文件,二是在选项里面改变了分辨率.
		Resolution[] resolutions = Screen.resolutions;
		//JSK_DebugProcess.AddLog("Resolution count = " + resolutions.Length);
		foreach( Resolution res in resolutions )
		{
			float h = res.height;
			float w = res.width;
			if( h/w == 0.5625f || h/w == 0.625f )
				JSK_DebugProcess.AddLog("Resolution.X = " + res.width + " Resolution.Y = " + res.height);
		}
		//1.800*480
		//2.1280*720
		//3.1920*1080
		//预设-->1280*720
	    g_Screen_Width  = 1280;
		g_Screen_Height = 720;		
		Screen.SetResolution(g_Screen_Width, g_Screen_Height, true);
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F2))
			g_IsVerticalSplitScreen = !g_IsVerticalSplitScreen;
        //if( g_IsWaitInfo )
        //{
        //    if( g_LenovoVerson )
        //        updateWaitInfoLenovo();
        //    else
        //        updateWaitInfo();
        //}
		
		if( g_ExitTime == 0 )
			return;
		else if( Time.realtimeSinceStartup >= g_ExitTime )
		{
			g_ExitTime = 0;
			Application.Quit();
		}
	}

    void OnApplicationQuit()
    {
        //搖桿控制物件
        GameFifo.StopConnect();
       // ServerProcess.Instance.Logout();
    }

//    void updateWaitInfoLenovo()
//    {
//        string sInputMsg =  GameFifo.GetMsg();
//        if( sInputMsg.Length != 0 )
//        {
//            JSK_DebugProcess.AddLog(sInputMsg);
			
//            if( sInputMsg.IndexOf('\0') > 0 )
//                sInputMsg = sInputMsg.Substring(0, sInputMsg.IndexOf('\0'));
			
//            string[] Datas = sInputMsg.Split("\t"[0]);
//            if( Datas.Length > 0 )
//            {
//                if( Datas[0].IndexOf("Lang") >= 0 )
//                {
//                    if( Datas.Length <= 1 )
//                        return;
					
////					if( Datas[1] == "mgb" || Datas[1] == "all" )
////						g_Language = 1;
////					else
////						g_Language = 2;
//                }
//                else if( Datas[0].IndexOf("UserInfo") >= 0 )
//                {
//                    if( Datas.Length <= 1 )
//                        return;
					
//                    UnityEngine.Debug.Log("Rec UserInfo : " + Datas[1]);
					
//                    if( Datas[1] == "" )
//                        g_ModuleVerson = 0;
//                    else
//                        g_ModuleVerson = 1;
//                }
//                else if( Datas[0].IndexOf("DevList") >= 0 )
//                {
//                    //DevList	0	Type:Phone	Name:py	OS:Andr	OSVer	Model:HTC Sensation	ClientVer:1.9.11
//                    if( Datas.Length <= 2 )
//                        return;
					
//                    g_IsConncet1P = true;
//                    JSK_GameFifo.Devices[0].Status = enumDeviceStatus.Connect;
//                    if( Datas[2] == "Type:Phone" )
//                        JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
					
//                    UnityEngine.Debug.Log(JSK_GameFifo.Devices[0].Status);
//                    UnityEngine.Debug.Log(JSK_GameFifo.Devices[0].Type);

//                    Datas = sInputMsg.Split("\n\r"[0]);
//                    if( Datas.Length > 1 )
//                    {
//                        g_IsConncet2P = true;
//                        UnityEngine.Debug.Log("Datas[1]= " + Datas[1]);
//                        Datas = Datas[1].Split("\t"[0]);
						
//                        JSK_GameFifo.Devices[1].Status = enumDeviceStatus.Connect;
//                        if( Datas[1] == "Type:Phone" )
//                            JSK_GameFifo.Devices[1].Type = enumDeviceType.Phone;
//                    }
//                }
//                else if( Datas[0].IndexOf("Dev") >= 0 && Datas[0].IndexOf("DevList") < 0 )
//                {
//                    if( Datas.Length <= 2 )
//                        return;
					
//                    int id = -1;
//                    int.TryParse(Datas[1], out id);
//                    if( id < 0 )
//                        return;
					
//                    if( Datas[2] == "Stat" )
//                    {
//                        if( Datas[3] == "0" )
//                            JSK_GameFifo.Devices[id].Status = enumDeviceStatus.Disconnect;
//                        else if( Datas[3] == "1" )
//                        {
//                            JSK_GameFifo.Devices[id].Status = enumDeviceStatus.Connect;
//                            JSK_GameFifo.Devices[id].Ground = enumDeviceGround.Foreground;
//                        }
//                        else if( Datas[3] == "2" )
//                        {
//                            JSK_GameFifo.Devices[id].Status = enumDeviceStatus.Connect;
//                            JSK_GameFifo.Devices[id].Ground = enumDeviceGround.Background;
//                        }
//                    }
//                    else if( Datas[2] == "QRString" )
//                    {
//                        //UnityEngine.Debug.Log("QRString " + Datas[3]);
//                        JSK_GameFifo.Devices[id].QREncodeText = Datas[3];
						
//                        QRcodeManager._textToEncode = Datas[3];
//                        if( id == 1 )
//                            g_IsWaitInfo = false;
//                    }
//                }
//                else if( Datas[0].IndexOf("LobbyID") >= 0 )
//                {
//                    UnityEngine.Debug.Log("Rec LobbyID : " + Datas[1]);
//                    g_LobbyID = Datas[1];
//                }
//                else if( sInputMsg.IndexOf("License") >= 0 )//20130809
//                {
//                    UnityEngine.Debug.Log("▉WaveRider Get License:" + sInputMsg);
//                    if( sInputMsg.IndexOf("\tTrial") >= 0 )
//                        g_ModuleVerson = 0;
//                    else if( sInputMsg.IndexOf("\tStd") >= 0 )
//                        g_ModuleVerson = 1;
//                    else
//                        g_ModuleVerson = 0;
//                }
//            }
//        }
//    }
	
    //void updateWaitInfo()
    //{
    //    string sInputMsg = JSK_GameFifo.GetMsg(false);
    //    if( sInputMsg.Length != 0 )
    //    {
    //        JSK_DebugProcess.AddLog(sInputMsg);
    //        if( sInputMsg.IndexOf("1P_DeviceType") >= 0 )
    //        {
    //            if 	   ( sInputMsg.IndexOf("Joystick") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else if( sInputMsg.IndexOf("LenovoRC") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else if( sInputMsg.IndexOf("LenovoHS") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else 										  JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //        }
    //        else if( sInputMsg.IndexOf("2P_DeviceType") >= 0 )
    //        {
    //            if 	   ( sInputMsg.IndexOf("Joystick") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else if( sInputMsg.IndexOf("LenovoRC") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else if( sInputMsg.IndexOf("LenovoHS") >= 0 ) JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //            else 										  JSK_GameFifo.Devices[0].Type = enumDeviceType.Phone;
    //        }
    //        else if( sInputMsg.IndexOf("1P_DevStat:") >= 0 )
    //        {
    //            JSK_GameFifo.Devices[0].Status = enumDeviceStatus.Connect;
    //            g_IsConncet1P = true;
    //            if( sInputMsg.IndexOf("DevStat:0") >= 0 )
    //            {
    //                JSK_GameFifo.Devices[0].Status = enumDeviceStatus.Disconnect;
    //                g_IsConncet1P = false;
    //            }
    //            JSK_DebugProcess.AddLog("g_IsConncet1P = " + g_IsConncet1P.ToString());
    //        }
    //        else if( sInputMsg.IndexOf("2P_DevStat:") >= 0 )
    //        {
    //            JSK_GameFifo.Devices[1].Status = enumDeviceStatus.Connect;
    //            g_IsConncet2P = true;
    //            if( sInputMsg.IndexOf("DevStat:0") >= 0 )
    //            {
    //                JSK_GameFifo.Devices[1].Status = enumDeviceStatus.Disconnect;
    //                g_IsConncet2P = false;
    //            }
    //            JSK_DebugProcess.AddLog("g_IsConncet2P = " + g_IsConncet2P.ToString());
    //        }
    //        else if( sInputMsg.IndexOf("CopyRight") >= 0 )
    //        {
    //            if( sInputMsg.IndexOf(":Trial") >= 0 )
    //                g_ModuleVerson = 0;
    //            else if( sInputMsg.IndexOf(":Std") >= 0 )
    //                g_ModuleVerson = 1;
    //        }
    //        else if( sInputMsg.IndexOf("1P_QRString") >= 0 )//todo
    //        {
    //            JSK_GameFifo.Devices[0].QREncodeText = sInputMsg;
						
    //            QRcodeManager._textToEncode = sInputMsg;
				
    //            g_IsWaitInfo = false;
    //        }
    //    }
    //}
	
	void UploadGameStart()
	{
	//	StartCoroutine(g_UploadScore.GameStart());
	}
	
	void UploadGameEnd( float time )
	{
/*
		string scoreData = "Level=" + JSK_GameProcess.GamePlace + "\r\nRaceTime=" + time.ToString() + "\r\n";
		scoreData += "Rank=S";
		
		scoreData += "\r\nTurboCnt=" + "0";
		scoreData += "\r\nGetSpeedCnt=" + "0";
		scoreData += "\r\nSubTime=" + "0";
		scoreData += "\r\nAddTime=" + "0";
*/			
//		StartCoroutine(g_UploadScore.GameEnd(scoreData));
	}
	
	//通过动作名找到对应的动作.
	public static JSK_PlayerAction GetAction( string sKey )
	{
		if( g_ActionMap.Contains(sKey) )
			return (JSK_PlayerAction)g_ActionMap[sKey];
		else
			return null;
	}
	
    //public static void BuyFullVerson()
    //{
    //    JSK_GameFifo.SendMsg("GetFullVersion", "1P");
    //    ExitGame();
    //}

	public static void ExitGame()
	{
		JSK_GlobalProcess.g_IsQuitGame = true;
        if (g_ExitTime != 0) return;
        //IsExitTime = true;
        //GameObject camMain = GameObject.Find("GameCamera");
        //CameraProcess Cam = camMain.GetComponent<CameraProcess>();
        //Cam.FadeOut(1f);

        GameFifo.SendDevMsg(1, "CloseGame");
        GameFifo.SendDevMsg(2, "CloseGame");
        g_ExitTime = Time.realtimeSinceStartup + 1f;
        //Debug.Log("ExitGame  ExitTime=" + ExitTime);
	}
	
	public static void LoadGameLevel()
	{
		//Loading... WaitPlayer and WaitMatch..
		ChangeLevel("JSK_MatchMenu");
	}
	public static void LoadGameLevelEx()
	{
 /*
		if( JSK_GlobalProcess.g_IsMultiPlayer && JSK_GlobalProcess.g_IsWebServer )
		{
			UnityEngine.Debug.Log("#####WebServerProcess.GameStart");

            //JSK_GameProcess.GameMoto1P = 1~9
            int iType = ((JSK_GameProcess.GameMoto1P - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevel = ((JSK_GameProcess.GameMoto1P - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            string sOut = "";
            BoatData data = (BoatData)BoatManager.GetBoat(iType, iLevel, out sOut);			
			if (data != null) 
			{
				WebServerProcess.GameStart(JSK_GameProcess.GamePlace, 6,data.ID  , "OIL01");
			}
//			JSK_LoadingProcess.LoadingStartMulti();//20140915Fix.
		}
//		else
//			JSK_LoadingProcess.LoadingStart(true);//20140915Fix.
		//ChangeLevel("JSK_Race");
		if(g_IsAndroidRing)//20130826
			ChangeLevel("tr_sc0" + JSK_GameProcess.GamePlace.ToString());
		else
			ChangeLevel("JSK_Race" + JSK_GameProcess.GamePlace.ToString());
  */ 
	}
	
	public static void StartGameLevel()
	{
/*
		if( JSK_GameProcess.GameMode != 3 )
			JSK_GameProcess.StartGame();
		else
		{
			//JSK_LoadingProcess.g_LoadState = JSK_LoadState.LoadWait;
			//JSK_NetGameProcess.NetLoadOver();
		}
 */ 
	}
	
	public static void ReturnMainMenu()
	{
		g_ModuleVerson = 1;
	//	JSK_LoadingProcess.LoadingStart(false);//20140911.Fix
		ChangeLevel("UI_MainMenu");
	}
	
	public static void ChangeLevel( string levelName )
	{
		Application.LoadLevel(levelName);
	}
	
	public static void ReturnMainMenuGC()
	{
		g_ModuleVerson = 1;
	//  JSK_LoadingProcess.LoadingStart(false);//20140911.Fix
		ChangeLevel("JSK_Race1");
	}
	
	public static void LoadPlayerData()
	{
/*
		string str = "";
		for( int i = 0; i < g_LevelNum; i++ )
		{
			str = "JSK_Level_" + i.ToString() + "_Record";
			g_LevelRecord[i] = ConfigManager.GetDataInt(str);
			//g_LevelRecord[i] = PlayerPrefs.GetInt(str, 0);
			//UnityEngine.Debug.Log("g_LevelRecord[i]:" + g_LevelRecord[i]);
		}
		
		JSK_SoundProcess.LoadConfig();
		JSK_OptionMenuProcess.LoadOpition();
 */ 
	}
	
	public static void SavePlayerData()
	{
/*
		string str = "";
		for( int i = 0; i < g_LevelNum; i++ )
		{
			str = "JSK_Level_" + i.ToString() + "_Record";
			//PlayerPrefs.SetInt(str, g_LevelRecord[i]);
			ConfigManager.SetDataInt(str,g_LevelRecord[i]);
		}
		ConfigManager.SaveConfig();
		
		JSK_SoundProcess.SaveConfig();
		JSK_OptionMenuProcess.SaveOpition();
 */ 
	}
	
	public static void DebugResetPlayerData()
	{
		for( int i = 0; i < g_LevelNum; i++ )
			g_LevelRecord[i] = 0;
		
		JSK_SoundProcess.m_fSoundVolume 	= JSK_SoundProcess.m_fDefaultSoundVolume;
		JSK_SoundProcess.m_fMusicVolume 	= JSK_SoundProcess.m_fDefaultMusicVolume;
	
		SavePlayerData();
		ExitGame();
	}
	
	public static void SaveLevel( int level, int val )
	{
		if( g_LevelRecord[level] == 0 || val < g_LevelRecord[level] )
			g_LevelRecord[level] = val;
		
		SavePlayerData();
	}
	
	public static GameObject CreatGlobalObject( string fileName )
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load(fileName));
		DontDestroyOnLoad(obj);
		return obj;
	}
	
	public static GameObject CreatPrefabObject( string fileName )
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load(fileName));
		return obj;
	}
	
	public static GameObject CreatSceneObject( string fileName, Vector3 position, Quaternion rotation )
	{
		GameObject obj = (GameObject)Instantiate(Resources.Load(fileName), position, rotation);
		return obj;
	}
	
	public static GameObject CreatExistObject( GameObject prefab, Vector3 position, Quaternion rotation )
	{
		GameObject obj = (GameObject)Instantiate(prefab, position, rotation);
		return obj;
	}
		
	public static GameObject CreatEffectObject( string fileName, Vector3 position, Quaternion rotation )
	{
		GameObject par = JSK_EffectResource.g_EffectResourceObj.GetComponent<JSK_EffectResource>().getEffectByName(fileName);
		GameObject obj = (GameObject)Instantiate(par, position, rotation);
		//暂时不用资源载入的方式.
		//obj = (GameObject)Instantiate(Resources.Load("JSK/Effect/" + fileName), position, rotation);
		return obj;
	}
		
	public static GameObject CreatNetObject( string fileName, Vector3 position, Quaternion rotation, int netGroup )
	{
		//这个..网络物体比较特殊,必须在项目视图中是看得见的Prefab才能创建,看看以后怎么修改吧.
		GameObject obj = (GameObject)Network.Instantiate(Resources.Load(fileName), position, rotation, netGroup);
		return obj;
	}

    public static void SendFifoMsg(int DeviceID, String msg)
	{
        GameFifo.SendMsg(DeviceID, msg);
	}

    public static string CheckFifoMsg()
    {
        return GameFifo.GetMsg();
    }
		
	public static string GetFifoMsg()
	{
//      if (DeviceProcess.IsEnabled) return ""; //若是裝置連線頁面顯示中，則離開
        return GameFifo.GetMsg();
    }

    public static string GetFifoMsg1P()
    {
//      if (DeviceProcess.IsEnabled) return ""; //若是裝置連線頁面顯示中，則離開
        return GameFifo.Devices[1].FifoMessage;
    }

    public static string GetFifoMsg2P()
    {
//      if (DeviceProcess.IsEnabled) return ""; //若是裝置連線頁面顯示中，則離開
        return GameFifo.Devices[2].FifoMessage;
    }

	public static string GetFifoGameMsg()
	{
		return GameFifo.GetMsg();
	}
	
	public static void ClearFifoMessage()
	{
        GameFifo.ClearFifoData();
	}
	
	//<PlayID> <Scene 0:恢复原来 1:Menu 2:Play 3:Play>
	public static void SetFifoScene( int PlayID, int Scene )//这个函数的0(恢复原来),只在暂停时使用,其他地方慎用.
	{
        GameFifo.SetFifoScene(PlayID, Scene);
	}
	
	public static int GetFifoScene( int PlayID )
	{
        return GameFifo.GetScene(PlayID);
	}
	
	public static void UpdateFinger()
	{
		
	}
	
	public static bool IsFingerDown()
	{
		return Input.GetMouseButtonDown(0);
	}
	
	public static bool IsFingerUp()
	{
		return Input.GetMouseButtonUp(0);
	}
	
	public static Vector3 GetFingerPosition()
	{
		return Input.mousePosition;
	}
	
//	void OnApplicationQuit() 
//	{
//		if( g_IsUploadLog )
//		{
//			UploadLog();
//		}
//		//JSK_GameFifo.StopConnect("1P");
//		//JSK_GameFifo.StopConnect("2P");
//		
//		//AchievementManager.SaveAchivement();
//	}
	
    //void UploadLog()
    //{
    //    GameEndTime = DateTime.Now;
		
    //    TimeSpan span = GameEndTime.Subtract ( GameInitTime );
		
    //    int totalTime = (int)span.TotalSeconds;
		
    //    DYGameLog.g_GameLog.ExecTime = totalTime;
		
    //    UnityEngine.Debug.Log("Game Exec time:" + DYGameLog.g_GameLog.ExecTime);
    //    UnityEngine.Debug.Log("Game played time:" + DYGameLog.g_GameLog.GetPlayedTime());
    //    UnityEngine.Debug.Log("Game played count:" + DYGameLog.g_GameLog.GetPlayedCount());
    //    UnityEngine.Debug.Log("Game multi played time:" + DYGameLog.g_GameLog.GetMultiPlayedTime());
    //    UnityEngine.Debug.Log("Game multi played count:" + DYGameLog.g_GameLog.GetMultiPlayedCount());
		
    //    StartCoroutine(DYGameLog.AddLog());
    //}
	
	void OnGUI()
	{
		GUI.depth = 1;
		//float offWidth  = Screen.width / 100.0f;
		//float offHeight = Screen.height / 100.0f;
		
		if( g_IsReleaseDemo )
		{
			float y = Screen.height / 100.0f;
			GUI.DrawTexture(new Rect(Screen.width - y * 21, Screen.height - y * 10, y * 20, y * 10), JSK_GUIProcess.GetPicture("Demo_version"));

			//GUI.DrawTexture( new Rect(offWidth*85, offHeight*92, offWidth*15, offHeight*10),
			//                 JSK_GUIProcess.GetPicture("Demo_version") );
		}
		float offWidth  = Screen.width / 100.0f;
		float offHeight = Screen.height / 100.0f;
		
		GUILayout.BeginHorizontal();
		//GUI.Label(new Rect(Screen.width - offWidth * 5,0,offWidth * 10,offHeight*5),g_Version);
//		if(GUILayout.Button ("Switch keyboard---8")) {
//			g_IsLocalPCKeyBoardTest = !g_IsLocalPCKeyBoardTest;
//		}
		
		GUILayout.EndHorizontal();
//		if( JSK_GameFifo.DebugMode )
//		{
//			GUILayout.Label("Build version:201403010-1");
//
//			GUILayout.Label("stringToencrypt:" + JSK_GameFifo.stringToencrypt);
//			
//			if(GUILayout.Button ("Clear msg")) {
//				JSK_GameFifo.FiFoLogs.Clear();
//			}
//			
//			foreach (string sLog in JSK_GameFifo.FiFoLogs)	
//				GUILayout.Label( sLog );
//		}
		
	}
	public static void ParseNTTParameter(string[] param)
	{
		
//		g_NTTGameMode = 2;
//		JSK_GameProcess.GameMode = 2;
//		JSK_GlobalProcess.g_SelectCharStep = 1;
//		JSK_GlobalProcess.g_SelectMotoStep = 1;
//		
//		return;
/*
		for (int i = 0; i < param.Length; i++)
        {
            string str = param[i];
			
			UnityEngine.Debug.Log("parameter string:" + str);
			
			//if( str == "TreasureHunt" )
			//	JSK_GameProcess.IsDiamondRace = true;
			if( str.IndexOf("2-Player") != -1)
			{
				g_NTTGameMode = 2;
				JSK_GameProcess.GameMode = 2;
				JSK_GlobalProcess.g_SelectCharStep = 1;
				JSK_GlobalProcess.g_SelectMotoStep = 1;
				UnityEngine.Debug.Log("JSK_GlobalProcess.g_SelectCharStep1:" + JSK_GlobalProcess.g_SelectCharStep);
			}
				UnityEngine.Debug.Log("JSK_GlobalProcess.g_SelectCharStep2:" + JSK_GlobalProcess.g_SelectCharStep);
		}
 */ 
	} 
}
