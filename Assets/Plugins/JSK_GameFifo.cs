//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Runtime.InteropServices;
//using System.IO;
//using System.Text;
//using System;

//public enum enumDeviceStatus { None, Connect, Disconnect };
//public enum enumDeviceType { None, Phone };
//public enum enumDeviceGround { None, Background, Foreground };

//public class DeviceData
//{
//    public enumDeviceStatus Status = enumDeviceStatus.None;
//    public enumDeviceType Type = enumDeviceType.None;
//    public enumDeviceGround Ground = enumDeviceGround.None;
//    public string QREncodeText = "";
//    public int FifoScene = 0;
//    private Texture2D mQREncodeTexture = null;
//    public Texture2D QREncodeTexture
//    {
//        get
//        {
//            if (mQREncodeTexture != null)
//                return mQREncodeTexture;
//            if (QREncodeText == "")
//                return null;
//            mQREncodeTexture = QRcodeManager.QREncode(QREncodeText, 512, 512);
//            return mQREncodeTexture;
//        }
//        set
//        {
//            mQREncodeTexture = value;
//        }
//    }
//    public string DeviceInfo = "@RPI#0#I:SR=320x480;CFV=1.0;SL=ACC,GYRO,MR$";
//    public bool KAAIsSet = false;
//    public float KAACount = 0;//計算keep alive
//    public float KAAStartTime = 0;
//}

//public class JSK_GameFifo : MonoBehaviour
//{
//#if UNITY_ANDROID
//    [DllImport("dyfifo")]
//    private static extern int FifoClnStart([MarshalAs(UnmanagedType.LPStr)]string name, int ConnectCycle, int ConnNotify, int ReadNotify);

//    [DllImport("dyfifo")]
//    private static extern int FifoWrite(int handle, [MarshalAs(UnmanagedType.LPStr)]string buf);

//    [DllImport("dyfifo")]
//    private static extern int FifoRead(int handle, [MarshalAs(UnmanagedType.LPArray)] byte[] buf);

//    [DllImport("dyfifo")]
//    private static extern void FifoStop(int handle);

//#else
////	[DllImport ("FifoInt")]
////	private static extern int FifoClnStart([MarshalAs(UnmanagedType.LPWStr)]string name, int ConnectCycle, int ConnNotify, int ReadNotify); 
////	
////	[DllImport ("FifoInt")]
////	private static extern int FifoWrite(int handle, [MarshalAs(UnmanagedType.LPWStr)]string buf);	
////	   
////	[DllImport ("FifoInt")]
////	private static extern int FifoRead(int handle,[MarshalAs(UnmanagedType.LPWStr)]string buf);
////
////	[DllImport ("FifoInt")]
////	private static extern void FifoStop(int handle);
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern int Init([MarshalAs(UnmanagedType.LPArray)] byte[] modulePath, int gmInt, int devMsgReader); 
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern int NttAttachDevice(int devIndex, [MarshalAs(UnmanagedType.LPArray)] byte[] phoneInfo); 
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern void NttDetachDevice(int devIndex);
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern void NttInputDevMsg([MarshalAs(UnmanagedType.LPArray)] byte[] msg);
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern void NttInputGameMsg([MarshalAs(UnmanagedType.LPArray)] byte[] msg);
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern void NttReadMsg([MarshalAs(UnmanagedType.LPArray)] byte[] buf);
	
//    [DllImport ("DyJetSki20_NTT")]
//    private static extern void Uninit();
	
//#endif

//    //private static string [] deviceInfo = {"@RPI#0#I:SR=320x480;CFV=1.0;SL=ACC,GYRO,MR$",""};

//    static private int lngFifoHandle1P = 0;
//    static private int lngFifoHandle2P = 0;

//    static private string[] FifoScenes = { "Menu", "Menu", "Race" };

//    static private string lastFifoKey = "";
//    static private float lastCheckTime = 0;
//    static private float checkDelayTime = 0.2f;

//#if UNITY_ANDROID
//    //private static int      newEnterCount   = 0;
//    //private static int      oldEnterCount   = 0;

//    private static float CheckTime = 0;
//    public static AndroidJavaObject m_pluginObject;
//    //    private static AndroidJavaClass     	cls_UnityPlayer;
//    //    private static AndroidJavaObject    	obj_Activity;
//    //    private static AndroidJavaClass     	cls_CheckEnterActivity;
//#endif

//    static private bool g_LenovoVerson = true;
//    static private bool EnableFifo = true;

//    public static DeviceData[] Devices = new DeviceData[2] { new DeviceData(), new DeviceData() };
//    public static bool disconnect = false;

//    private static string gameID = "a00000125";
//    private static int signalBadTimeout = 3;
//    private static int autoRecoveryTimeout = 15;
//    //private static int deviceIndex = 0;
//    private static string encryptContent = "";
//    public static string stringToencrypt = "";

//    private static string w_clientpname = "";
//    private static string w_ip = "";
//    private static string w_token = "";
//    private static string w_ports = "";
//    private static string w_rts = "";
//    private static int w_port;
//    private static int w_ret;

//    public static bool DebugMode = false;

//    //	public static int InitGameFifoNTT()
//    //	{
//    //		return Init("./",0,0);
//    //	}
//    //	
//    //	public static int StartConnectNTT(int devIndex)
//    //	{
//    //		return NttAttachDevice(devIndex,deviceInfo[devIndex]);
//    //	}
//    //	
//    //	public static void StopConnectNTT(int devIndex)
//    //	{
//    //		NttDetachDevice(devIndex);
//    //	}
//    public static ArrayList FiFoLogs = new ArrayList();
//    public static int m_MsgCount = 30;

//    private static bool bStartSkipmsg = false;
//    private static void AddFifoLog(string msg)
//    {
//        if (FiFoLogs.Count >= m_MsgCount) FiFoLogs.RemoveAt(0);

//        FiFoLogs.Add("<< " + msg);

//    }

//#if UNITY_STANDALONE_WIN
	
//    public static void Clik_MessageReceived(string _message)
//    {
//        //Debug.Log("msg receive~~");
		
		
//        if ( _message.IndexOf( "@KAA#" ) < 0 && _message.IndexOf("@SS#") < 0)
//            AddFifoLog("<< " + _message);

		
		
//        //if( bStartSkipmsg )
//        //	return;
//        //TODO: handle application message
//        if(_message!="")
//        {
//            //確認手機與Game使用的協定號碼
//            // > 0 的數字
//            //工程版的協定編號為 1
//            if( _message.IndexOf("@") < 0 )
//                return ;
			
//            if(_message == "@PV#0#V:1$")
//            {
//                GCAPISendMessage("@PV#0#V:1$");
//            }
//            else if(_message == "@PV#1#V:1$")
//            {
//                GCAPISendMessage("@PV#1#V:1$");
//            }
//            else if(_message == "@DD#0#R:0$")//1P 手機 通知 Game 已經斷線
//            {
//                StopConnect("1P");
//            }
//            else if(_message == "@DD#1#R:0$")//2P 手機 通知 Game 已經斷線
//            {
//                StopConnect("2P");
//            }
//            else if(_message.IndexOf("@RPI#0#") >= 0 )//1P手機回應資訊
//            {
//                Devices[0].DeviceInfo = _message;
				
//                GCAPISendMessage("@RACI#0#I:Moto Xoom;32G$");//遊戲大廳回應手機資訊
				
//                StartConnect("1P");
//                if( !Devices[0].KAAIsSet )
//                    Devices[0].KAAIsSet = true;
//                //Devices[0].KAACount = signalBadTimeout;
//                Devices[0].KAAStartTime = 1 + signalBadTimeout;//加3秒然後倒數
//                //Devices[0].KAAStartTime = Time.realtimeSinceStartup + signalBadTimeout;//加3秒然後倒數
				
//            }
//            else if(_message.IndexOf("@RPI#1#") >= 0 )//2P手機回應資訊
//            {
//                Devices[1].DeviceInfo = _message;
//                GCAPISendMessage("@RACI#1#I:Moto Xoom;32G$");//遊戲大廳回應手機資訊
				
//                StartConnect("2P");
//                if( !Devices[1].KAAIsSet )
//                    Devices[1].KAAIsSet = true;
//                //Devices[0].KAACount = signalBadTimeout;
//                Devices[1].KAAStartTime = 1 + signalBadTimeout;//加3秒然後倒數
//            }
//            //else if( _message.IndexOf("@LM#0#") > 0 )
//            //{
//            //	NttInputDevMsg("@LMR#0#R:0#MI:8100#V:0#L:5$");
//            //}
//            else if (_message.IndexOf("@KAA#0")>=0)
//            {
//                //Debug.Log("WRKAA");
//                //GCAPISendMessage("WRKAA");//遊戲大廳回應手機資訊
//                if( !Devices[0].KAAIsSet )
//                    Devices[0].KAAIsSet = true;
//                //Devices[0].KAACount = signalBadTimeout;
//                Devices[0].KAAStartTime = 1 + signalBadTimeout;//加3秒然後倒數
//                //Devices[0].KAAStartTime = Time.realtimeSinceStartup + signalBadTimeout;//加3秒然後倒數
//            }
//            else if (_message.IndexOf("@KAA#1")>=0)
//            {
//                if( !Devices[1].KAAIsSet )
//                    Devices[1].KAAIsSet = true;
//                Devices[1].KAAStartTime = 1 + signalBadTimeout;//加3秒然後倒數
//            }
//            else//其他手機端送來的訊息
//            {
				
//                //Debug.Log("Msg fom phone:" + _message);
//                byte[] arrayData = new byte[512];
//                arrayData = System.Text.Encoding.Default.GetBytes ( _message );
//                NttInputDevMsg(arrayData);
				
//            }
//            //m_msgRecv = _message;
//            //Debug.Log("msg receive~~");
//        }
//    }
	
//    public static void GCAPISendMessage(string msg)
//    {
//        //Debug.Log(">> GCAPISendMessage:" + msg);
//        AddFifoLog(">> " + msg);
		
//        clikcsharp.Clik.SendApplicationMessage(msg);
//    }
//#endif

//    static public void InitGameFifo()
//    {
//#if UNITY_ANDROID
//        if (Application.platform != RuntimePlatform.Android)
//            EnableFifo = false;

//        lngFifoHandle1P = 0;
//        lngFifoHandle2P = 0;

//        lastFifoKey = "";
//        lastCheckTime = 0;
//        Init("1P");
//        Init("2P");


//#elif UNITY_STANDALONE_WIN
//        lngFifoHandle1P	= 0;
//        lngFifoHandle2P = 0;
	
//        lastFifoKey    = "";
//        lastCheckTime  = 0;
		
//        byte[] arrayData = new byte[512];
//        arrayData = System.Text.Encoding.Default.GetBytes ( "./" );
		
//        Init(arrayData,0,0);
//        AddFifoLog(">> Init");
		
//        w_clientpname = "";
//        w_ret = clikcsharp.Clik.GetInputConnectionDetails("", w_clientpname, out w_ip, out w_token, out w_port );
//        w_rts = w_ret.ToString("d");
//        w_ports = w_port.ToString ("d");
		
//        stringToencrypt = gameID + ";" + w_ip + ";" + w_ports + ";" + w_token + ";" + signalBadTimeout.ToString() + ";" + autoRecoveryTimeout.ToString() + ";0";// + deviceIndex.ToString();
//        Devices[0].QREncodeText =  "https://play.google.com/store/apps/details?id=com.dygame.gcclient?NTT:@;" + encodeQRCode(stringToencrypt);
		
//        stringToencrypt = gameID + ";" + w_ip + ";" + w_ports + ";" + w_token + ";" + signalBadTimeout.ToString() + ";" + autoRecoveryTimeout.ToString() + ";1";// + deviceIndex.ToString();
//        Devices[1].QREncodeText =  "https://play.google.com/store/apps/details?id=com.dygame.gcclient?NTT:@;" + encodeQRCode(stringToencrypt);
		
//        clikcsharp.Clik.MessageReceived += Clik_MessageReceived;
//#endif




//    }

//    static public void Init(string sPlayer)
//    {
//        AddFifoLog("*** Init()");
//#if UNITY_ANDROID
//        if (sPlayer == "1P" && lngFifoHandle1P == 0)
//            StartConnect("1P");
//#elif UNITY_STANDALONE_WIN
//            if( sPlayer == "1P" && lngFifoHandle1P == 0 )
//                StartConnect("1P");
//            else if( sPlayer == "2P" && lngFifoHandle2P == 0 )
//                StartConnect("2P");
//#endif

//    }

//    static public void StartConnect(string sPlayer)
//    {
//        if (!EnableFifo)
//            return;

//#if UNITY_ANDROID
//        if (!JSK_PlatformManager.g_LaunchAiwi)
//            return;

//        Debug.Log("Start Fifo Client: " + sPlayer);
//        StopConnect(sPlayer);
//        if (sPlayer == "1P")
//        {
//            if (g_LenovoVerson)
//                lngFifoHandle1P = FifoClnStart("DyGameWaveRider_LV_20", 300, 0, 0);
//            else
//                lngFifoHandle1P = FifoClnStart("Aiwi_JetSki_P1", 300, 0, 0);

//            Debug.Log("lngFifoHandle1P: " + lngFifoHandle1P);
//            if (lngFifoHandle1P == 0)
//                Debug.Log(sPlayer + " connect to server failed.");
//        }
//        else if (sPlayer == "2P")
//        {
//            lngFifoHandle2P = FifoClnStart("Aiwi_JetSki_P2", 300, 0, 0);
//            Debug.Log("lngFifoHandle2P: " + lngFifoHandle2P);
//            if (lngFifoHandle2P == 0)
//                Debug.Log(sPlayer + " connect to server failed.");
//        }
//#elif UNITY_STANDALONE_WIN
//        AddFifoLog("*** StartConnect");
//        //Debug.Log("StartConnect--Start Fifo Client: " + sPlayer);
//        StopConnect(sPlayer);
//        if( sPlayer == "1P" )
//        {
//            if( g_LenovoVerson )
//            {
//                byte[] arrayData = new byte[512];
//                arrayData = System.Text.Encoding.Default.GetBytes ( Devices[0].DeviceInfo );
				
//                int ret = NttAttachDevice(0,arrayData);
//                if( ret == 0)
//                {
//                    lngFifoHandle1P = 101;
//                    Devices[0].Status = enumDeviceStatus.Connect;
//                    SendMsgNew("SetAsPlayDev", 1);
//                }
					
//                //lngFifoHandle1P = FifoClnStart("DyGameWaveRider_LV_20", 300, 0, 0);
//                //lngFifoHandle1P = FifoClnStart("DyGameJetSki_LV_20", 300, 0, 0);
//            }
//            //else
//            //	lngFifoHandle1P = FifoClnStart("Aiwi_JetSki_P1", 300, 0, 0);
			
//            //Debug.Log("lngFifoHandle1P: " + lngFifoHandle1P);
//            //if( lngFifoHandle1P == 0 )
//            //	Debug.Log(sPlayer + " connect to server failed.");
//        }
//        else if( sPlayer == "2P" )
//        {
//            //lngFifoHandle2P = FifoClnStart("Aiwi_JetSki_P2", 300, 0, 0);
//            byte[] arrayData = new byte[512];
//            arrayData = System.Text.Encoding.Default.GetBytes ( Devices[1].DeviceInfo );
			
//            int ret = NttAttachDevice(1,arrayData);
//            if( ret == 0)
//            {
//                lngFifoHandle2P = 102;
//                SendMsgNew("SetAsPlayDev", 2);
//                Devices[1].Status = enumDeviceStatus.Connect;
//            }
			
//            //Debug.Log("lngFifoHandle2P: " + lngFifoHandle2P);
//            //if( lngFifoHandle2P == 0 )
//            //	Debug.Log(sPlayer + " connect to server failed.");
//        }
		
//#endif

//    }

//    static public void StopConnect(string sPlayer)
//    {
//        if (!EnableFifo)
//            return;

//#if UNITY_ANDROID
//        if (!JSK_PlatformManager.g_LaunchAiwi)
//            return;
//        if (sPlayer == "1P")
//        {
//            if (lngFifoHandle1P > 0)
//            {
//                Debug.Log("Stop Fifo: " + sPlayer);
//                FifoStop(lngFifoHandle1P);
//                lngFifoHandle1P = 0;
//            }
//        }
//        else if (sPlayer == "2P")
//        {
//            if (lngFifoHandle2P > 0)
//            {
//                Debug.Log("Stop Fifo: " + sPlayer);
//                FifoStop(lngFifoHandle2P);
//                lngFifoHandle2P = 0;
//            }
//        }

//#elif UNITY_STANDALONE_WIN
//        AddFifoLog("*** StopConnect");
//        if( sPlayer == "1P" )
//        {
//            if( lngFifoHandle1P > 0 )
//            {
//                //Debug.Log("Stop Fifo: " + sPlayer);
				
//                //FifoStop(lngFifoHandle1P);
//                NttDetachDevice(0);
//                lngFifoHandle1P = 0;
//                //if( Devices[0].Status == enumDeviceStatus.Connect )
//                    Devices[0].Status = enumDeviceStatus.Disconnect;
//            }
//        }
//        else if( sPlayer == "2P" )
//        {
//            if( lngFifoHandle2P > 0 )
//            {
//                //Debug.Log("Stop Fifo: " + sPlayer);
//                //FifoStop(lngFifoHandle2P);
//                NttDetachDevice(1);
//                lngFifoHandle2P = 0;
//                Devices[1].Status = enumDeviceStatus.Disconnect;
//            }
//        }
		
//#endif

//    }

//    public static void SendMsgNew(string msg, int deviceID)
//    {
//        SendMsg(msg + "\t" + (deviceID - 1).ToString(), "1P");
//    }

//    static public void SendMsg(string msg, string sPlayer)
//    {
//        if (!EnableFifo)
//            return;

//#if UNITY_ANDROID
//        if (!JSK_PlatformManager.g_LaunchAiwi)
//            return;
//        Init(sPlayer);
//        if (sPlayer == "1P")
//        {
//            if (lngFifoHandle1P > 0)
//            {
//                FifoWrite(lngFifoHandle1P, msg);
//                Debug.Log(sPlayer + " SendMsg To Fifo server : " + msg);
//            }
//            else
//                Debug.Log(sPlayer + " Fifo server may not be connected.");
//        }
//        else if (sPlayer == "2P")
//        {
//            if (lngFifoHandle2P > 0)
//            {
//                FifoWrite(lngFifoHandle2P, msg);
//                Debug.Log(sPlayer + " SendMsg To Fifo server : " + msg);
//            }
//            else
//                Debug.Log(sPlayer + " Fifo server may not be connected.");
//        }
//#elif UNITY_STANDALONE_WIN
//            AddFifoLog("@@ send msg:" + msg +" player:" + sPlayer);
		
//        //Init(sPlayer);
//        if( sPlayer == "1P" )
//        {
//            if( lngFifoHandle1P > 0 )
//            {
//                //FifoWrite(lngFifoHandle1P, msg);
				
//                //if( msg.IndexOf("@") > 0 )//如果訊息是 @ 開頭，請轉送給手機。
//                //{
//                //	NttInputDevMsg(msg);
//                //}
//                //else
//                byte[] arrayData = new byte[512];
//                arrayData = System.Text.Encoding.Default.GetBytes ( msg );
//                NttInputGameMsg(arrayData);
				
//                //Debug.Log(sPlayer + " SendMsg To Fifo server : " + msg);
//            }
//            //else
//            //	Debug.Log(sPlayer + " Fifo server may not be connected.");
//        }
//        else if( sPlayer == "2P" )
//        {
//            if( lngFifoHandle2P > 0 )
//            {
//                //FifoWrite(lngFifoHandle2P,msg);
//                //if( msg.IndexOf("@") > 0 )//如果訊息是 @ 開頭，請轉送給手機。
//                //{
//                //	NttInputDevMsg(msg);
//                //}
//                //else
//                byte[] arrayData = new byte[512];
//                arrayData = System.Text.Encoding.Default.GetBytes ( msg );
//                NttInputGameMsg(arrayData);
				
//                //Debug.Log(sPlayer + " SendMsg To Fifo server : " + msg);
//            }
//            //else
//            //	Debug.Log(sPlayer + " Fifo server may not be connected.");
//        }
//#endif


//    }

//    static public string GetMsg(bool useKeyBoard)
//    {
//        //return GetMsg1P(useKeyBoard) + GetMsg2P(useKeyBoard);
//        return GetMsg1P(useKeyBoard);
//    }

//    static public string GetMsg1P(bool useKeyBoard)
//    {
//        if (useKeyBoard)
//        {
//            string keyData = ParseFIFOKeyBoard(1);
//            if (keyData != "")
//                return "1P_" + keyData;
//        }

//        string sData = "";
//        if (EnableFifo && lngFifoHandle1P > 0)
//        {
//#if UNITY_ANDROID
//            byte[] arrayData = new byte[512];
//            int iFifoReadRet = FifoRead(lngFifoHandle1P, arrayData);
//            if (iFifoReadRet == 1)
//            {
//                sData = "1P_" + System.Text.ASCIIEncoding.ASCII.GetString(arrayData);
//            }
//            else
//            {
//                sData = "";
//                //Debug.Log("err : " + iFifoHaveData);
//            }
//#else
//            sData = sData.PadLeft(512);
			
//            //int iFifoHaveData = FifoRead(lngFifoHandle1P,sData);
//            //NttReadMsg(sData);
//            byte[] arrayData = new byte[512];
//            NttReadMsg(arrayData);
//            sData = System.Text.ASCIIEncoding.ASCII.GetString(arrayData).Trim();
			
//            if( sData.IndexOf('\0') >= 0 )
//            {
//                //Debug.Log("sData.IndexOf > 0:" + sData);
//                sData = sData.Substring(0, sData.IndexOf('\0'));
//            }
			
//            if( sData.Length > 0 )
//            {
			
//                if( sData.IndexOf("@") >= 0 )//如果訊息是 @ 開頭，請轉送給手機。
//                {
//                    //Debug.Log("Redirect to phone.");
//                    GCAPISendMessage(sData);
//                    sData = "";
//                }
//                else
//                    sData = "1P_" + sData;
//                if(sData.IndexOf("Angle:")<0)
//                    AddFifoLog("**Fifo1P message:" + sData);
//            }
//            else
//                sData = "";
			
////			if( iFifoHaveData != 0 )
////			{
////				sData = "1P_" + sData;
////				//Debug.Log("Read data from FIFO server : " + sData);
////			}
////			else
////			{
////				sData = ""; 
////				//Debug.Log("err : " + iFifoHaveData);
////			}
//#endif
//        }
//        CheckOffLine(sData.Trim());
//        if (g_LenovoVerson && useKeyBoard && sData.Length > 0)
//            sData = ParseFIFOStatus(sData);
//        return sData.Trim();
//    }

//    public static string ParseFIFOStatus(string s)
//    {
//        if (s.IndexOf('\0') > 0)
//            s = s.Substring(0, s.IndexOf('\0'));
//        s = s.Replace("\t", ":");

//        //Debug.Log("Fifo sData" + s);
//        return s;
//    }

//    static public string GetMsg2P(bool useKeyBoard)
//    {
//        if (useKeyBoard)
//        {
//            string keyData = ParseFIFOKeyBoard(1);
//            if (keyData != "")
//                return "2P_" + keyData;
//        }

//        //string sData_2P = "                                                                                                                                                                                                                   ";
//        string sData_2P = "";

//        if (EnableFifo && lngFifoHandle2P > 0)
//        {
//#if UNITY_ANDROID
//            byte[] arrayData = new byte[512];
//            int iFifoHaveData_2P = FifoRead(lngFifoHandle2P, arrayData);
//            if (iFifoHaveData_2P == 1)
//            {
//                sData_2P = "2P_" + System.Text.ASCIIEncoding.ASCII.GetString(arrayData);
//            }
//            else
//            {
//                sData_2P = "";
//                //Debug.Log("err : " + iFifoHaveData);
//            }
//#else
//            sData_2P = sData_2P.PadLeft(512);
			
//            //int iFifoHaveData = FifoRead(lngFifoHandle1P,sData);
//            //NttReadMsg(sData_2P);
//            byte[] arrayData = new byte[512];
//            NttReadMsg(arrayData);
//            sData_2P = System.Text.ASCIIEncoding.ASCII.GetString(arrayData).Trim();
			
//            if( sData_2P.IndexOf('\0') >= 0 )
//            {
//                //Debug.Log("sData.IndexOf > 0:" + sData_2P);
//                sData_2P = sData_2P.Substring(0, sData_2P.IndexOf('\0'));
//            }
			
//            if( sData_2P.Length > 0 )
//            {
				
//                if( sData_2P.IndexOf("@") >= 0 )//如果訊息是 @ 開頭，請轉送給手機。
//                {
//                    //Debug.Log("Redirect to phone.");
//                    GCAPISendMessage(sData_2P);
//                    sData_2P = "";
//                }
//                else
//                    sData_2P = "2P_" + sData_2P;
				
//                if(sData_2P.IndexOf("Angle:")<0)
//                    AddFifoLog("**Fifo2P message:" + sData_2P);
//            }
//            else
//                sData_2P = "";
			
////			int iFifoHaveData_2P = FifoRead(lngFifoHandle2P,sData_2P);
////			if( iFifoHaveData_2P != 0 )
////			{
////				sData_2P = "2P_" + sData_2P;
////				//Debug.Log("Read data from FIFO server : " + sData_2P);
////			}
////			else
////			{
////				sData_2P = ""; 
////				//Debug.Log("err : " + iFifoHaveData);
////			}
//#endif
//        }
//        CheckOffLine(sData_2P.Trim());
//        if (g_LenovoVerson && useKeyBoard && sData_2P.Length > 0)
//            sData_2P = ParseFIFOStatus(sData_2P);
//        return sData_2P.Trim();
//    }

//    static public string GetFifoMsg()
//    {
//        string sData = "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       ";
//        if (EnableFifo && lngFifoHandle1P > 0)
//        {
//#if UNITY_ANDROID
//            byte[] arrayData = new byte[512];
//            int iFifoReadRet = FifoRead(lngFifoHandle1P, arrayData);
//            if (iFifoReadRet == 1)
//            {
//                sData = "1P_" + System.Text.ASCIIEncoding.ASCII.GetString(arrayData);
//            }
//            else
//            {
//                sData = "";
//                //Debug.Log("err : " + iFifoHaveData);
//            }
//#else
//            sData = sData.PadLeft(512);
			
//            //int iFifoHaveData = FifoRead(lngFifoHandle1P,sData);
//            //NttReadMsg(sData);
//            byte[] arrayData = new byte[512];
//            NttReadMsg(arrayData);
//            sData = System.Text.ASCIIEncoding.ASCII.GetString(arrayData).Trim();
			
//            if( sData.IndexOf('\0') >= 0 )
//            {
//                //Debug.Log("sData.IndexOf > 0:" + sData);
//                sData = sData.Substring(0, sData.IndexOf('\0'));
//            }
			
//            if( sData.Length > 0 )
//            {
				
//                if( sData.IndexOf("@") >= 0 )//如果訊息是 @ 開頭，請轉送給手機。
//                {
//                    //Debug.Log("Redirect to phone.");
//                    GCAPISendMessage(sData);
//                    sData = "";
//                }
//                else
//                    sData = "1P_" + sData;
				
//                if(sData.IndexOf("Angle:")<0)
//                    AddFifoLog("**Fifo message:" + sData);
//            }
//            else
//                sData = "";
			
			
////			int iFifoHaveData = FifoRead(lngFifoHandle1P,sData);
////			if( iFifoHaveData != 0 )
////			{
////				sData = "1P_" + sData;
////				//Debug.Log("Read data from FIFO server : " + sData);
////			}
////			else
////			{
////				sData = ""; 
////				//Debug.Log("err : " + iFifoHaveData);
////			}
//#endif
//        }

//        //FiFoLogs.Add("Fifo message:" + sData);
//        CheckOffLine(sData.Trim());
//        if (g_LenovoVerson && sData.Length > 0)
//            sData = ParseFIFOStatus(sData);

//        return sData.Trim();
//    }

//    static public string ParseFIFOKeyBoard(int DeviceIndex)
//    {
//        int sceneType = Devices[DeviceIndex - 1].FifoScene;
//        //enumDeviceType deviceIndex = Devices[DeviceIndex-1].Type;

//        string sKey = "";

//        if (DetectEnter(false)) sKey = "Confirm";
//        else if (Input.GetKeyDown(KeyCode.Return)) sKey = "Confirm";
//        else if (Input.GetKeyDown(KeyCode.KeypadEnter)) sKey = "Confirm";
//        else if (Input.GetKeyDown(KeyCode.Menu)) sKey = "Esc";
//        else if (Input.GetKeyDown(KeyCode.Home)) sKey = "Esc";
//        else if (Input.GetKeyDown(KeyCode.Escape)) sKey = "Esc";
//        else if (Input.GetKeyDown(KeyCode.UpArrow)) sKey = "Up";
//        else if (Input.GetKeyDown(KeyCode.DownArrow)) sKey = "Down";
//        else if (Input.GetKeyDown(KeyCode.LeftArrow)) sKey = "Left";
//        else if (Input.GetKeyDown(KeyCode.RightArrow)) sKey = "Right";

//        if (sceneType != 2)//在菜单场景中,限制速度.
//        {
//            if (sKey != "")
//            {
//                if (sKey == lastFifoKey && Time.realtimeSinceStartup < (lastCheckTime + checkDelayTime))
//                    sKey = "";
//                else
//                    lastCheckTime = Time.realtimeSinceStartup;
//                lastFifoKey = sKey;
//            }
//        }
//        return sKey;
//    }

//    //<PlayID> <Scene 0:恢复原来  1:Menu  2:Fighting>
//    static public void SetScene(int PlayID, int Scene)//这个函数的0(恢复原来),只在暂停时使用,其他地方慎用.
//    {
//        if (Scene == 0)
//        {
//            Scene = Devices[PlayID - 1].FifoScene;
//        }
//        else if (Scene != 1)
//        {
//            Devices[PlayID - 1].FifoScene = Scene;
//        }

//        string sTmp = "";

//        if (g_LenovoVerson)
//        {
//            sTmp = "Dev\t" + (PlayID - 1).ToString() + "\tScene\t" + FifoScenes[Scene];
//            SendMsg(sTmp, "1P");
//        }
//        else
//        {
//            sTmp = "Scene:" + FifoScenes[Scene];
//            SendMsg(sTmp, PlayID.ToString() + "P");
//        }
//    }

//    static public int GetScene(int PlayID)
//    {
//        return Devices[PlayID - 1].FifoScene;
//    }

//    static public void GetDeviceType()
//    {
//        SendMsg("DeviceType", "1P");
//        SendMsg("DeviceType", "2P");
//    }

//    static public void GetDeviceState()
//    {
//        if (g_LenovoVerson)
//        {
//            SendMsgNew("GetDevStat", 1);
//            SendMsgNew("GetDevStat", 2);
//        }
//        else
//        {
//            SendMsg("DeviceStat", "1P");
//            SendMsg("DeviceStat", "2P");
//        }
//    }

//    static public void ClearMsg()
//    {
//        string sInputMsg = GetMsg(false);
//        while (sInputMsg.Length > 0)
//            sInputMsg = GetMsg(false);
//        DetectEnter(true);
//    }

//    static public bool DetectEnter(bool force)
//    {
//        if (Application.platform != RuntimePlatform.Android || !JSK_PlatformManager.g_LaunchAiwi)
//            return false;

//#if UNITY_ANDROID

//        bool IsEnter = false;
//        //			string KeyStateString = "";
//        //			
//        //			if(EnableFifo)
//        //			{
//        //				if (CheckTime == 0)
//        //		        {
//        //		            using( var pluginClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
//        //					{
//        //						m_pluginObject = pluginClass.GetStatic<AndroidJavaObject>("currentActivity");
//        //					}
//        //					
//        //		        }
//        //		        if (Time.time < CheckTime + 1)
//        //		        {
//        //					KeyStateString = m_pluginObject.CallStatic<string>("getKeyState");
//        //		        }
//        //		        CheckTime = Time.time;
//        //		       	
//        //				if( KeyStateString.IndexOf(":66:1,")>=0 || KeyStateString.IndexOf(":23:1,")>=0)	//百是通的ok鍵是23
//        //					IsEnter = true;
//        //			}

//        return IsEnter;
//#else
//            return false;
//#endif
//    }

//    //    static public bool DetectEnter( bool force )
//    //    {
//    //        if( Application.platform != RuntimePlatform.Android || !JSK_PlatformManager.g_LaunchAiwi )
//    //            return false;
//    //		
//    //		if( disconnect )
//    //		{
//    //			if( !force )
//    //				return false;
//    //		}
//    //		
//    //		#if UNITY_ANDROID
//    //        if( checkTime == 0 )
//    //        {
//    //            AndroidJNI.AttachCurrentThread();
//    //            cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//    //            cls_CheckEnterActivity = new AndroidJavaClass("com.aibelive.CheckEnter.CheckEnterActivity");
//    //			obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//    //            cls_CheckEnterActivity.CallStatic("Init", obj_Activity);
//    //        }
//    //		
//    //        bool IsEnter = false;
//    //        if( Time.realtimeSinceStartup > checkTime )
//    //        {
//    //			checkTime = Time.realtimeSinceStartup + 0.5f;
//    //            newEnterCount = cls_CheckEnterActivity.CallStatic<int>("getEnterCount");
//    //            if( newEnterCount != oldEnterCount )
//    //            {
//    //                oldEnterCount = newEnterCount;
//    //                IsEnter = true;
//    //            }
//    //        }
//    //        
//    //        return IsEnter;
//    //		#else
//    //		return false;
//    //		#endif
//    //    }

//    static public void CheckOffLine(string str)
//    {
//        if (g_LenovoVerson)
//        {
//            if (str.IndexOf('\0') > 0)
//                str = str.Substring(0, str.IndexOf('\0'));
//            string[] Datas = str.Split("\t"[0]);
//            if (Datas.Length > 0)
//            {
//                if (Datas[0].IndexOf("Dev") >= 0)
//                {
//                    if (Datas.Length <= 2)
//                        return;

//                    int id = -1;
//                    int.TryParse(Datas[1], out id);
//                    if (id < 0)
//                        return;

//                    if (Datas[2] == "Disconnect")
//                    {
//                        Devices[id].Status = enumDeviceStatus.Disconnect;
//                        //Debug.Log(str);
//                    }
//                    else if (Datas[2] == "Connect")
//                    {
//                        Devices[id].Status = enumDeviceStatus.Connect;
//                        //Debug.Log(str);
//                    }
//                    else if (Datas[2] == "Foreground")
//                    {
//                        Devices[id].Ground = enumDeviceGround.Foreground;
//                        //Debug.Log(str);
//                    }
//                    else if (Datas[2] == "Background")
//                    {
//                        Devices[id].Ground = enumDeviceGround.Background;
//                        //Debug.Log(str);
//                    }
//                    else if (Datas[2] == "Stat")
//                    {
//                        if (Datas[3] == "0")
//                            Devices[id].Status = enumDeviceStatus.Disconnect;
//                        else if (Datas[3] == "1")
//                        {
//                            Devices[id].Status = enumDeviceStatus.Connect;
//                            Devices[id].Ground = enumDeviceGround.Foreground;
//                        }
//                        else if (Datas[3] == "2")
//                        {
//                            Devices[id].Status = enumDeviceStatus.Connect;
//                            Devices[id].Ground = enumDeviceGround.Background;
//                        }
//                        //Debug.Log(str);
//                    }
//                }
//            }
//        }
//        else
//        {
//            if (str.IndexOf("1P_DevStat:0") >= 0)
//                Devices[0].Status = enumDeviceStatus.Disconnect;
//            if (str.IndexOf("1P_DevStat:1") >= 0)
//                Devices[0].Status = enumDeviceStatus.Connect;

//            if (str.IndexOf("2P_DevStat:0") >= 0)
//                Devices[1].Status = enumDeviceStatus.Disconnect;
//            if (str.IndexOf("2P_DevStat:1") >= 0)
//                Devices[1].Status = enumDeviceStatus.Connect;
//        }
//    }

//    //加密的 function 如下
//    private static string encodeQRCode(string sQRCode)
//    {
//        if (sQRCode == null)
//            return "";

//        if (sQRCode.Length == 0)
//            return "";

//        char[] encodeMap = 
//        {
//            Convert.ToChar(0), Convert.ToChar(1), Convert.ToChar(2), Convert.ToChar(3), Convert.ToChar(4), Convert.ToChar(5), Convert.ToChar(6), Convert.ToChar(7), Convert.ToChar(8), Convert.ToChar(9), Convert.ToChar(10),
//            Convert.ToChar(11), Convert.ToChar(12), Convert.ToChar(13), Convert.ToChar(14), Convert.ToChar(15), Convert.ToChar(16), Convert.ToChar(17), Convert.ToChar(18), Convert.ToChar(19), Convert.ToChar(20),
//            Convert.ToChar(21), Convert.ToChar(22), Convert.ToChar(23), Convert.ToChar(24), Convert.ToChar(25), Convert.ToChar(26), Convert.ToChar(27), Convert.ToChar(28), Convert.ToChar(29), Convert.ToChar(30),
//            Convert.ToChar(31), Convert.ToChar(32), Convert.ToChar(33), Convert.ToChar(34), Convert.ToChar(35), Convert.ToChar(36), Convert.ToChar(37), Convert.ToChar(38), Convert.ToChar(39), Convert.ToChar(40),
//            Convert.ToChar(41), Convert.ToChar(42), Convert.ToChar(43), Convert.ToChar(44), Convert.ToChar(45), Convert.ToChar(46), Convert.ToChar(47), 'w', 'h', 'Y',
//            'J', 'b', '1', 'R', 'M', 'u', 'Z', ':', 'U', '<',
//            '=', '>', '?', '@', 'l', 'O', '6', 'z', 'H', '7',
//            'I', 'e', '2', 's', 'V', 'A', 'n', 'K', 'x', 'd',
//            '0', 'N', 'q', 'E', 'i', 'W', 'y', '9', 'T', 'k',
//            '[', '\\', ']', '^', '_', '`', 'p', 'f', 'X', 'D',
//            'm', 't', '3', 'j', 'S', '8', 'F', ';', 'v', 'o',
//            'Q', 'a', 'C', 'L', '5', 'g', 'r', 'P', 'B', 'c',
//            'G', '4', '{', '|', '}', '~', Convert.ToChar(127),
//        };

//        char[] aryQRCode = sQRCode.ToCharArray();

//        if (aryQRCode == null)
//            return "";

//        for (int i = 0; i < aryQRCode.Length; i++)
//        {
//            if (aryQRCode[i] < 0 || aryQRCode[i] > 127)
//                continue;

//            aryQRCode[i] = encodeMap[aryQRCode[i]];
//        }

//        string sRtn = new string(aryQRCode);

//        if (sRtn == null)
//            sRtn = "";

//        return sRtn;
//    }
//    //	void OnGUI()
//    //	{
//    //		GUILayout.Label("Build version:201403010-1");
//    //		//GUILayout.Label("Server info:" + w_msg);
//    //		GUILayout.Label("stringToencrypt:" + stringToencrypt);
//    //		
//    //		if(GUILayout.Button ("Clear msg")) {
//    //			FiFoLogs.Clear();
//    //		}
//    //		
//    //		foreach (string sLog in FiFoLogs)	
//    //			GUILayout.Label( sLog );
//    //	}

//    public static int exitType = 0;
//    void OnApplicationQuit()
//    {
//        //Debug.Log("GameFifo OnApplicationQuit()");
//        //GCAPISendMessage("@GC#0#R:0$");
//        //SendMessage("@GC#1#R:0$");
//        //		GCAPISendMessage("@GC#0#R:0$");
//        //		GCAPISendMessage("@GC#1#R:0$");
//        //		
//        //		StopConnect("1P");
//        //		StopConnect("2P");

//#if UNITY_STANDALONE_WIN
//        Uninit();
//#endif
//        //clikcsharp.Clik.ReportIntentionalExit( "Application exit" );

//        //Debug.Log("exitType:" + exitType);
//        //if( exitType == 0)

//        //Process.GetCurrentProcess().Kill();
//        //clikcsharp.Clik.ReportIntentionalExit( "Application exit" );
//    }
//}
