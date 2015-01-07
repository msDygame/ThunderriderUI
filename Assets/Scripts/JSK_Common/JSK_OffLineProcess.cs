using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;

public class JSK_OffLineProcess : MonoBehaviour
{
	//public static int 	g_ConnectState 		= 0;//0连接 -1断线.
	//private static float CheckTime = 0;
	
    //private static string 	exitMsg = "Button_Back to game_L_Marquee";
    //private static System.Timers.Timer aTimer = new System.Timers.Timer();
	
    //public static GameObject g_1P_QRCodeUIObject = null;
    //private static Transform qrCode_1P_UI = null; 
	
	
    //private static bool isShow1PQRCode = false;
    //public static bool b_isDevDisconnect   = false;
    //public static int  devDisconnectTime	 = 0;
	
    //void Awake()
    //{
    //    g_ConnectState = 0;
    //    //CheckTime = Time.realtimeSinceStartup + 2;
    //    LoadPrefab();
		
    //    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
    //    aTimer.Interval = 1000;
    //    aTimer.Enabled = true;
    //}
	
//    private void OnTimedEvent(object source, ElapsedEventArgs e)
//    {
//        //Debug.Log("OnTimedEvent");
	 
//        updateKAATime(0);
//        updateKAATime(1);
		
//        if( b_isDevDisconnect )
//        {
//            devDisconnectTime ++;
//            Debug.Log("devDisconnectTime:" + devDisconnectTime);
//        }
//        //else
//        //	devDisconnectTime = 0;
		 
//    }
//    #if UNITY_STANDALONE_WIN
//    void updateKAATime(int devID)
//    {
//        if( devID == 0)
//        {
//            if( GameFifo.Devices[0].KAAIsSet )
//            {
//                GameFifo.Devices[0].KAAStartTime = GameFifo.Devices[0].KAAStartTime - 1;
//                if( GameFifo.Devices[0].KAAStartTime <= 0 )
//                {
//                    GameFifo.StopConnect( "1P" );
//                    GameFifo.Devices[0].KAAIsSet = false;
//                    //if( Devices[0].Status == enumDeviceStatus.Connect )
//                    //	Devices[0].Status = enumDeviceStatus.Disconnect;
//                    GameFifo.GCAPISendMessage("@TKA#0#T:15$");
//                    GameFifo.FiFoLogs.Add("**Device 0 is KAA timeout.");
					
//                    //if ( !b_isDevDisconnect )
//                    //	b_isDevDisconnect = true;
//                }
//            }
////			else
////			{
////					b_isDevDisconnect = false;
////			}
//        }
//        else if( devID == 1)
//        {
//            if( GameFifo.Devices[1].KAAIsSet )
//            {
//                GameFifo.Devices[1].KAAStartTime = GameFifo.Devices[1].KAAStartTime - 1;
//                if( GameFifo.Devices[1].KAAStartTime <= 0 )
//                {
//                    GameFifo.StopConnect( "2P" );
//                    GameFifo.Devices[1].KAAIsSet = false;
//                    //if( Devices[0].Status == enumDeviceStatus.Connect )
//                    //	Devices[0].Status = enumDeviceStatus.Disconnect;
//                    GameFifo.GCAPISendMessage("@TKA#1#T:15$");
//                    GameFifo.FiFoLogs.Add("**Device 1 is KAA timeout.");
					
////					if ( !b_isDevDisconnect )
////						b_isDevDisconnect = true;
//                }
//            }
////			else
////			{
////				b_isDevDisconnect = false;
////			}
//        }
//    }
//    #endif
//    void Update()
//    {
		
//        if( Application.loadedLevelName == "JSK_LogoMenu" )
//            return;
		
//        if( JSK_GlobalProcess.g_IsLocalPCKeyBoardTest )
//            return;
		
//        if( JSK_GlobalProcess.g_IsQuitGame )	//遊戲正在關閉中
//            return;
		
//        if( b_isDevDisconnect && devDisconnectTime < 15)//遊戲中裝置斷線,要等15秒讓裝置重連,如果沒有就顯示QRCode
//        {
//            if( JSK_GameProcess.GameMode == 1)
//            {
//                if( GameFifo.Devices[0].Status == enumDeviceStatus.Connect)
//                {
//                    b_isDevDisconnect = false;
//                }
//            }
//            else if( JSK_GameProcess.GameMode == 2)
//            {
//                if( GameFifo.Devices[0].Status == enumDeviceStatus.Connect &&   GameFifo.Devices[1].Status == enumDeviceStatus.Connect)
//                {
//                    b_isDevDisconnect = false;
//                }
//            }
			
//            return;
//        }
//        //updateKAATime();
//        //#if UNITY_ANDROID
//        //if( Application.platform == RuntimePlatform.Android )
//        //{
//            //string fifoMsg = GameFifo.GetMsg1P(false) + GameFifo.GetMsg2P(false);
//            string fifoMsg = "";
			
//            if( g_ConnectState == -1)
//            {
//                fifoMsg = GameFifo.GetMsg();
//                if( fifoMsg!="")
//                Debug.Log("offLine fifoMsg:" + fifoMsg);
//            }
			
			
			
//            if( JSK_GameProcess.GameMode == 1)//單打
//            {
//                if( GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect)
//                {
//                    if( Input.GetKeyDown(KeyCode.Escape) ||
//                        Input.GetKeyDown(KeyCode.Return) ||
//                        fifoMsg.IndexOf("Esc")>=0 ||
//                        fifoMsg.IndexOf("Enter")>=0 ||
//                        fifoMsg.IndexOf("Confirm")>=0
//                       )
//                        JSK_GlobalProcess.ExitGame();
//                        g_ConnectState = -1;
//                        exitMsg = "Button_leave_L_Marquee";
//                }
//                else
//                {
//                    g_ConnectState = 0;
//                    b_isDevDisconnect = false;
//                    devDisconnectTime = 0;
//                }
				
				
//            }
//            else if( JSK_GameProcess.GameMode == 2)
//            {
//                if( (GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect) && GameFifo.Devices[1].Status == enumDeviceStatus.Connect ) //如果是1P斷線,剩下2P連線的話偵測到返回鍵離開遊戲
//                {
//                    if( Input.GetKeyDown(KeyCode.Escape) || 
//                        Input.GetKeyDown(KeyCode.Return) ||
//                        fifoMsg.IndexOf("Esc")>=0 || 
//                        fifoMsg.IndexOf("Enter")>=0 ||
//                        fifoMsg.IndexOf("Confirm")>=0
//                       )
//                        JSK_GlobalProcess.ExitGame();
						
//                    g_ConnectState = -1;
					
//                    exitMsg = "Button_leave_L_Marquee";
//                }
//                else if( GameFifo.Devices[0].Status == enumDeviceStatus.Connect && (GameFifo.Devices[1].Status == enumDeviceStatus.None || GameFifo.Devices[1].Status == enumDeviceStatus.Disconnect))//如果是2P斷線,剩下1P連線的話偵測到返回鍵回主選單
//                {
//                    if( Input.GetKeyDown(KeyCode.Escape) || 
//                        Input.GetKeyDown(KeyCode.Return) ||
//                        fifoMsg.IndexOf("Esc")>=0 || 
//                        fifoMsg.IndexOf("Enter")>=0 ||
//                        fifoMsg.IndexOf("Confirm")>=0
//                       )//|| KOB_GameFifo.Devices[1].FifoMessage.IndexOf("Key:Esc")>=0 || KOB_GameFifo.Devices[1].FifoMessage.IndexOf("Key:Enter")>=0 
//                    {
//                        g_ConnectState = 0;
//                        JSK_GameProcess.GameMode = 1;
//                        JSK_GlobalProcess.ReturnMainMenu();
//                    }
//                    else
//                        g_ConnectState = -1;
					
//                    //exitMsg = "Button_Back to game_L_Marquee";
//                    exitMsg = "Button_leave_L_Marquee";
//                }
//                else if( (GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect) && (GameFifo.Devices[1].Status == enumDeviceStatus.None || GameFifo.Devices[1].Status == enumDeviceStatus.Disconnect))//兩支裝置都斷線
//                {
//                    if( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) )
//                            JSK_GlobalProcess.ExitGame();
					
//                    g_ConnectState = -1;
					
//                    exitMsg = "Button_leave_L_Marquee";
//                }
//                else if( GameFifo.Devices[0].Status == enumDeviceStatus.Connect && GameFifo.Devices[1].Status == enumDeviceStatus.Connect ) //1,2P都連線
//                {
//                    g_ConnectState = 0;
//                    b_isDevDisconnect = false;
//                    devDisconnectTime = 0;
//                }
//            }
			
////			if (Time.realtimeSinceStartup > CheckTime)
////	        {
////	            CheckTime = Time.realtimeSinceStartup + 30;
////	            //Debug.Log("IsCheck=" + IsCheck + " GameFifo.Devices[2].Status=" + GameFifo.Devices[2].Status);
////	           	GameFifo.SendMsgNew("GetQRString", 1);
////				GameFifo.SendMsgNew("GetQRString", 2);
////	        }
			
//        //}
//        //#endif
////		g_ConnectState = 0;
////		#if UNITY_ANDROID
////			if( Application.platform == RuntimePlatform.Android )
////			{
////				if( GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect ||
////				    GameFifo.Devices[1].Status == enumDeviceStatus.Disconnect )
////				{
////					if( JSK_GameProcess.GameMode == 1 )
////					{
////						if( GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect )
////						{
////							g_ConnectState = -1;
////						}
////					}
////					else
////					{
////						g_ConnectState = -1;
////					}
////				}
////				
////				if( GameFifo.Devices[1].Status == enumDeviceStatus.None && JSK_GameProcess.GameMode == 2 )
////					g_ConnectState = -1;
////				
////				GameFifo.disconnect = false;
////				if( g_ConnectState == -1 )
////				{
////					GameFifo.disconnect = true;
////					if( Input.GetKeyDown(KeyCode.Escape) || GameFifo.DetectEnter(true) )
////						JSK_GlobalProcess.ExitGame();
////				}
////			}
////		#endif
//        //if( Input.GetKeyDown(KeyCode.O) )
//            //GameFifo.Devices[1].Status = enumDeviceStatus.Disconnect;
		
//        //if( Input.GetKeyDown(KeyCode.P) )
//            //GameFifo.Devices[1].Status = enumDeviceStatus.Connect;
		
//        //GameFifo.Devices[0].QREncodeText = "222222222222222222222222222222222222222222222222222";
//        //GameFifo.Devices[1].QREncodeText = "222222222222222222222222222222222222222222222222222";
//    }
	
//    private string inputMsgString = "";
//    private Vector2 scrollPosition = Vector2.zero;
//    void OnGUI()
//    {
//        if( Application.loadedLevelName == "JSK_LogoMenu" )
//            return;
		
//        if( JSK_GlobalProcess.g_IsLocalPCKeyBoardTest )
//            return;
		
//        if( JSK_GlobalProcess.g_IsQuitGame )	//遊戲正在關閉中
//            return;
		
//        if( b_isDevDisconnect && devDisconnectTime < 15)//遊戲中裝置斷線要等15秒才顯示QRCode
//            return;
		
//        GUI.depth = 0;//越小的在越前面.
		
//        if( GameFifo.DebugMode )
//        {
				
//                GUILayout.BeginVertical();
//                scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (300), GUILayout.Height (150));
//                foreach (string sLog in GameFifo.FiFoLogs)	
//                {	
//                    GUILayout.Label(sLog);
//                }
				
				
//                if (GUILayout.Button ("Clear"))
//                {
//                    GameFifo.FiFoLogs.Clear();
//                    //inputMsgString = "";
//                }
			
//                GUI.EndScrollView ();
//                GUILayout.EndVertical();
			
////				string inputString = "";
////				foreach (string sLog in GameFifo.FiFoLogs)	
////				{	
////					inputString += sLog + "\r\n";
////					//GUILayout.Label( sLog );
////					
////				}
////				inputMsgString = inputString;
////				GUILayout.BeginVertical();
////				scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (300), GUILayout.Height (300));
////				GUILayout.Label(inputMsgString);
////				
////				if (GUILayout.Button ("Clear"))
////				{
////					GameFifo.FiFoLogs.Clear();
////					inputMsgString = "";
////				}
////			
////				GUI.EndScrollView ();
////				GUILayout.EndVertical();
			
//                //GUILayout.Label("Use Fifo :" + GameFifo.EnableFifo);
//                //GUILayout.Label("g_ModuleVerson :" + JSK_GlobalProcess.g_ModuleVerson);
//                //GUILayout.Label("g_ConnectState :" + g_ConnectState);
//                //GUILayout.Label("Fifo status:" + GameFifo.GetFifoStatus());
//                //GUILayout.BeginVertical();
//            #if UNITY_STANDALONE_WIN
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("g_ConnectState:" + g_ConnectState);
			
//            if(GUILayout.Button ("1P Protocol")) 
//            {
//                GameFifo.Clik_MessageReceived("@PV#0#V:1$");
//            }
//            else if(GUILayout.Button ("1P ReplyPhone")) 
//            {
//                GameFifo.Clik_MessageReceived("@RPI#0#I:SL=ACC,MR,GYRO;SR=720x1208;CFV=1.0$");
//            }
//            else if(GUILayout.Button ("1P Load Module R")) 
//            {
//                GameFifo.Clik_MessageReceived("@LMR#0#R:0#MI:8100#V:1309040001#L:5$");
//            }
//            else if(GUILayout.Button ("1P STVR")) 
//            {
//                GameFifo.Clik_MessageReceived("@STVR#0#R:0#MI:8100#SI:0#VI:0$");
//            }
//            else if(GUILayout.Button ("1P Down press")) 
//            {
//                GameFifo.Clik_MessageReceived("@SK#0#MI:8100#SI:0#VI:0#KS:4$");
//            }
//            else if(GUILayout.Button ("1P Down release")) 
//            {
//                GameFifo.Clik_MessageReceived("@SK#0#MI:8100#SI:0#VI:0#KS:0$");
//            }
//            else if(GUILayout.Button ("1P Set Scene Menu")) 
//            {
//                JSK_GlobalProcess.SetFifoScene(1,1);
//            }
//            else if(GUILayout.Button ("1P Set Scene Race")) 
//            {
//                JSK_GlobalProcess.SetFifoScene(1,2);
//            }
			
//            else if(GUILayout.Button ("Set Angle")) 
//            {
//                GameFifo.Clik_MessageReceived("@SS#0#C:1#T:1#-0.00191,-0.02106,-0.94523$");
//                GameFifo.Clik_MessageReceived("@SS#0#C:1#T:1#0.00287,-0.02394,-0.94618$");
//                GameFifo.Clik_MessageReceived("@SS#0#C:1#T:1#0.00861,-0.02202,-0.95576$");
//                //GameFifo.Clik_MessageReceived("@SK#0#MI:8100#SI:1#VI:1#KS:16$");
//                //GameFifo.Clik_MessageReceived("@SK#0#MI:8100#SI:1#VI:1#KS:0$");
//            }
//            else if(GUILayout.Button ("Set KAA"))
//            {
//                GameFifo.Clik_MessageReceived("@KAA#0#I:$");
//            }
//            GUILayout.EndHorizontal();
			
//            GUILayout.BeginHorizontal();
//            if(GUILayout.Button ("2P Protocol")) 
//            {
//                GameFifo.Clik_MessageReceived("@PV#1#V:1$");
//            }
//            else if(GUILayout.Button ("2P ReplyPhone")) 
//            {
//                GameFifo.Clik_MessageReceived("@RPI#1#I:SL=ACC,MR,GYRO;SR=720x1208;CFV=1.0$");
//            }
//            else if(GUILayout.Button ("2P Load Module R")) 
//            {
//                GameFifo.Clik_MessageReceived("@LMR#1#R:0#MI:8100#V:1309040001#L:5$");
//            }
//            else if(GUILayout.Button ("2P STVR")) 
//            {
//                GameFifo.Clik_MessageReceived("@STVR#1#R:0#MI:8100#SI:0#VI:0$");
//            }
//            else if(GUILayout.Button ("2P Down press")) 
//            {
//                GameFifo.Clik_MessageReceived("@SK#1#MI:8100#SI:0#VI:0#KS:4$");
//            }
//            else if(GUILayout.Button ("2P Down release")) 
//            {
//                GameFifo.Clik_MessageReceived("@SK#1#MI:8100#SI:0#VI:0#KS:0$");
//            }
//            else if(GUILayout.Button ("2P Set Scene Menu")) 
//            {
//                JSK_GlobalProcess.SetFifoScene(2,1);
//            }
//            else if(GUILayout.Button ("2P Set Scene Race")) 
//            {
//                JSK_GlobalProcess.SetFifoScene(2,2);
//            }
//            GUILayout.EndHorizontal();
//            #endif
//                GUILayout.Label("Build version:201403027-1");
//                //GUILayout.Label("stringToencrypt:" + GameFifo.stringToencrypt);
			
//                //GUILayout.BeginVertical();
////					if(GUILayout.Button ("Send msg")) {
////						Debug.Log("Send WR hello world");
////						GameFifo.GCAPISendMessage("Send WR hello world");
////					}
////					else if(GUILayout.Button ("Clear msg")) {
////						GameFifo.FiFoLogs.Clear();
////						inputMsgString = "";
////						Debug.Log("inputMsgString:" + inputMsgString);
////					}
//                //GUILayout.EndVertical();
				
				
			
//                GUILayout.Label("Device 1P status:" + GameFifo.Devices[0].Status);
//                GUILayout.Label("Device 1P fifo scene:" + GameFifo.Devices[0].FifoScene);
//                GUILayout.Label("Device 1P fifo QREncodeText:" + GameFifo.Devices[0].QREncodeText);
//                GUILayout.Label("Device 2P status:" + GameFifo.Devices[1].Status);
//                GUILayout.Label("Device 2P fifo scene:" + GameFifo.Devices[1].FifoScene);
//                GUILayout.Label("Device 1P fifo QREncodeText:" + GameFifo.Devices[1].QREncodeText);	
//                //GUILayout.EndVertical();
			
				
//                //GUILayout.TextArea(inputMsgString, GUILayout.Height(50));
//                //inputMsgString = GUILayout.TextField(inputMsgString,300);
//        }
////		float offWidth  = Screen.width / 100.0f;
////		float offHeight = Screen.height / 100.0f;
		
//        if( g_ConnectState == -1 )
//        {
//            if( JSK_GameProcess.GameMode == 1)
//            {
//                Show1PConnectUI();
//            }
//            else if( JSK_GameProcess.GameMode == 2)
//            {
//                Show2PConnectUI();
//            }
//        }
////		else
////		{
////			if( JSK_GameProcess.GameMode == 1)
////				Hide1PConnectUI();
////		}
//    }
	
//    void Show1PConnectUI()
//    {
//        float offWidth  = Screen.width / 100.0f;
//        float offHeight = Screen.height / 100.0f;

//        GUI.DrawTexture(new Rect(offWidth*0, offHeight*0, offWidth*100, offHeight*100),
//                                JSK_GUIProcess.GetLanguagePicture("control device"));
				
//        GUI.DrawTexture(new Rect(offWidth*22, offHeight*18, offWidth*20, offWidth*5),
//                        JSK_GUIProcess.GetPicture("1P_cn"));
		
//        if( GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect )
//        {
//            if( GameFifo.Devices[0].QREncodeTexture != null )
//            {
//                GUI.DrawTexture(new Rect(offWidth*22, offHeight*36, offWidth*15, offWidth*15),
//                                GameFifo.Devices[0].QREncodeTexture);
//            }
			
//            GUI.DrawTexture(new Rect(offWidth*40, offHeight*82, offWidth*20, offWidth*5),
//                                JSK_GUIProcess.GetLanguagePicture(exitMsg));
//        }
//        else
//        {
//            GUI.DrawTexture(new Rect(offWidth*23, offHeight*33, offWidth*17, offWidth*17),
//                            JSK_GUIProcess.GetLanguagePicture("On line"));
//        }
		
//    }
	
//	void Show1PConnectUI()
//	{
////		float offWidth  = Screen.width / 100.0f;
////		float offHeight = Screen.height / 100.0f;
////
////		GUI.DrawTexture(new Rect(offWidth*0, offHeight*0, offWidth*80, offHeight*60),
////				                JSK_GUIProcess.GetLanguagePicture("control device"));
////				
////		GUI.DrawTexture(new Rect(offWidth*22, offHeight*18, offWidth*20, offWidth*5),
////		                JSK_GUIProcess.GetPicture("1P_cn"));
////		
////		if( GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect )
////		{
////			if( GameFifo.Devices[0].QREncodeTexture != null )
////			{
////				GUI.DrawTexture(new Rect(offWidth*25, offHeight*36, offWidth*15, offWidth*15),
////				                GameFifo.Devices[0].QREncodeTexture);
////			}
////			
////			GUI.DrawTexture(new Rect(offWidth*40, offHeight*82, offWidth*20, offWidth*5),
////				                JSK_GUIProcess.GetLanguagePicture(exitMsg));
////		}
////		else
////		{
////			GUI.DrawTexture(new Rect(offWidth*23, offHeight*33, offWidth*17, offWidth*17),
////			                JSK_GUIProcess.GetLanguagePicture("On line"));
////		}
//		if( isShow1PQRCode == false )
//		{
//			isShow1PQRCode = true;
//			
//			if ( GameFifo.Devices[0].QREncodeTexture != null ) 
//				qrCode_1P_UI.renderer.material.mainTexture = GameFifo.Devices[0].QREncodeTexture;
//				
//			g_1P_QRCodeUIObject.SetActiveRecursively(true);
//			
//		}
//		
//		
//		
//	}
//	void Hide1PConnectUI()
//	{
//		if( isShow1PQRCode == true )
//		{	
//			isShow1PQRCode = false;
//			
//			g_1P_QRCodeUIObject.SetActiveRecursively(false);
//			
//		}
//	}
//    void Show2PConnectUI()
//    {
//        float offWidth  = Screen.width / 100.0f;
//        float offHeight = Screen.height / 100.0f;
		
//        GUI.DrawTexture(new Rect(offWidth*0, offHeight*0, offWidth*100, offHeight*100),
//                                JSK_GUIProcess.GetLanguagePicture("control device2"));
				
//        GUI.DrawTexture(new Rect(offWidth*22, offHeight*18, offWidth*20, offWidth*5),
//                        JSK_GUIProcess.GetPicture("1P_cn"));
		
//        GUI.DrawTexture(new Rect(offWidth*58, offHeight*18, offWidth*20, offWidth*5),
//                        JSK_GUIProcess.GetPicture("2P_cn"));
		
//        if( GameFifo.Devices[0].Status == enumDeviceStatus.None || GameFifo.Devices[0].Status == enumDeviceStatus.Disconnect )
//        {
//            //g_ConnectState = -1;
			
//            if( GameFifo.Devices[0].QREncodeTexture != null )
//            {
//                GUI.DrawTexture(new Rect(offWidth*22, offHeight*36, offWidth*15, offWidth*15),
//                                GameFifo.Devices[0].QREncodeTexture);
//            }
//        }
//        else
//        {
//            GUI.DrawTexture(new Rect(offWidth*21, offHeight*33, offWidth*17, offWidth*17),
//                            JSK_GUIProcess.GetLanguagePicture("On line"));
//        }
		
//        if( GameFifo.Devices[1].Status == enumDeviceStatus.Disconnect || GameFifo.Devices[1].Status == enumDeviceStatus.None )
//        {
//            //g_ConnectState = -1;
			
//            if( GameFifo.Devices[1].QREncodeTexture != null )
//            {
//                GUI.DrawTexture(new Rect(offWidth*63, offHeight*36, offWidth*15, offWidth*15),
//                                GameFifo.Devices[1].QREncodeTexture);
//            }
//        }
//        else
//        {
//            GUI.DrawTexture(new Rect(offWidth*62f, offHeight*33, offWidth*17, offWidth*17),
//                            JSK_GUIProcess.GetLanguagePicture("On line"));
//        }
		
//        GUI.DrawTexture(new Rect(offWidth*40, offHeight*82, offWidth*20, offWidth*5),
//                                JSK_GUIProcess.GetLanguagePicture(exitMsg));
		
////		if( GameFifo.Devices[0].Status == enumDeviceStatus.Connect && GameFifo.Devices[1].Status == enumDeviceStatus.Connect )
////		{
////			g_ConnectState = 0;
////		}
//    }
//    void LoadPrefab()
//    {
//        g_1P_QRCodeUIObject = (GameObject)Instantiate(Resources.Load("JSK/Common/1P_QRCode_UI"));
		
//        DontDestroyOnLoad(g_1P_QRCodeUIObject);
		
//        qrCode_1P_UI = g_1P_QRCodeUIObject.transform.Find("QRCode_1P");
			
//        if( qrCode_1P_UI == null )
//            Debug.Log("*************qrCode_1P_UI == null**************");
		
//        g_1P_QRCodeUIObject.SetActiveRecursively(false);
		
//        /*
//        g_2P_QRCodeUIObject = (GameObject)Instantiate(Resources.Load("Archive/Main/2P_QRCode_UI2"));
		
//        DontDestroyOnLoad(g_2P_QRCodeUIObject);
		
//        qrCode_2P_1_UI = g_2P_QRCodeUIObject.transform.Find("Ak_QR_2P_Menu/QRCode_1P");
			
//        if( qrCode_2P_1_UI == null )
//            Debug.Log("*************qrCode_2P_1_UI == null**************");
		
//        qrCode_2P_2_UI = g_2P_QRCodeUIObject.transform.Find("Ak_QR_2P_Menu/QRCode_2P");
			
//        if( qrCode_2P_2_UI == null )
//            Debug.Log("*************qrCode_2P_2_UI == null**************");
		
//        qrCode_2P_UI_Exit = g_2P_QRCodeUIObject.transform.Find("Ak_QR_Menu_ConfirmBack_Simple/Ak_QR_Button_Exit");
		
//        if( qrCode_2P_UI_Exit == null )
//            Debug.Log("*************qrCode_2P_UI_Exit == null**************");
		
//        g_2P_QRCodeUIObject.SetActiveRecursively(false);
//        */
//    }
//    void OnApplicationQuit()
//    {
//        //Debug.Log("offline OnApplicationQuit()");
//        //SendMessage("@GC#0#R:0$");
//        //clikcsharp.Clik.ReportIntentionalExit( "Application exit" );
//        aTimer.Enabled = false;
//    }
	
}
