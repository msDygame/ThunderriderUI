using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class GameFifo : MonoBehaviour
{
    public static string Version = "1.1.0"; //版號

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
    public static bool IsAndroid = false;
    public static bool DebugMode = true;
#else
    public static bool IsAndroid = true;
    public static bool DebugMode = false;
#endif
#if (UNITY_EDITOR)
    public static bool IsCheckDevice = false; //是否裝置檢查 true false
#else
   public static bool IsCheckDevice = true; //是否裝置檢查 true false
#endif
    public static string LangCode = "MGB"; //ENU MGB
    public static bool IsGetLang = false;

    public static ArrayList FiFoLogs = new ArrayList();
    public static bool IsMenuScene = true; //是否是滑鼠主選單模式
    private static string[] FifoScenes = { "Menu", "Menu", "Race" }; //模式清單０恢復原先１選單２遊戲
    private static int lngFifoHandle = 0;
    public static DeviceData[] Devices = new DeviceData[3] { new DeviceData(), new DeviceData(), new DeviceData() };
    public static UserInfoData UserInfo = new UserInfoData();
    private static float FifoCmdTime = 0;
    //文字輸入框
    public static bool InputTextVisabled = false;
    public static string InputText = "";

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN) //UNITY_ANDROID  UNITY_EDITOR
    [DllImport("FifoInt")]
    private static extern int FifoClnStart([MarshalAs(UnmanagedType.LPWStr)]String name, int ConnectCycle, int ConnNotify, int ReadNotify);

    [DllImport("FifoInt")]
    private static extern int FifoRead(int handle, [MarshalAs(UnmanagedType.LPWStr)]String buf);

    [DllImport("FifoInt")]
    private static extern void FifoStop(int handle);

    [DllImport("FifoInt")]
    private static extern int FifoWrite(int handle, [MarshalAs(UnmanagedType.LPWStr)]String buf);
#else
    [DllImport("dyfifo")]
    private static extern int FifoClnStart([MarshalAs(UnmanagedType.LPStr)]string name, int ConnectCycle, int ConnNotify, int ReadNotify);

    [DllImport("dyfifo")]
    private static extern int FifoRead(int handle, [MarshalAs(UnmanagedType.LPArray)] byte[] buf);

    [DllImport("dyfifo")]
    private static extern void FifoStop(int handle);

    [DllImport("dyfifo")]
    private static extern int FifoWrite(int handle, [MarshalAs(UnmanagedType.LPStr)]string buf);
#endif

    public static void Init()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
#else
        DeviceInput.LaunchGameZone();
#endif
        if (lngFifoHandle == 0) StartConnect();
        if (!IsCheckDevice)
        {
            UserInfo.IsRead = true;
            UserInfo.IsReadTicket = true;
        }
    }

    public static void StartConnect()
    {
        StopConnect();
        lngFifoHandle = FifoClnStart("DyGameWaveRider_II_LV_20", 300, 0, 0);
        AddLog("Fifo Handle=" + lngFifoHandle);
        if (lngFifoHandle == 0)
            Debug.Log("Fifo connect to server failed.");
    }

    public static void StopConnect()
    {
        if (lngFifoHandle > 0)
        {
            Debug.Log("Stop Fifo");
            FifoStop(lngFifoHandle);
            lngFifoHandle = 0;
        }
    }

    public static void SendMsg(String msg)
    {
        Init();
        if (lngFifoHandle > 0)
        {
            //AddLog("Fifo Send >>> " + msg);
            Debug.Log("Fifo Send >>> " + msg);
            FifoWrite(lngFifoHandle, msg);
        }
    }

    public static void SendMsg(int DeviceID, String msg)
    {
        int ID = Devices[DeviceID].ID;
        if (ID < 0) return;
        SendMsg("Dev\t" + ID.ToString() + "\t" + msg);
        AddLog(">> " + "Dev\t" + ID.ToString() + "\t" + msg);
    }

    public static void SendDevMsg(int DeviceID, String msg)
    {
        int ID = Devices[DeviceID].ID;
        if (ID < 0) return;
        SendMsg(msg + "\t" + ID.ToString());
        AddLog(">> " + msg + "\t" + ID.ToString());
    }

    public static String GetMsg()
    {
        //Debug.Log("Read data from FIFO server.......");
        //鍵盤偵測
        //if (IsAndroid)
        //{
        //    string sKey = DeviceInput.DetectKey();
        //    if (sKey != "") AddLog(sKey);
        //    if (DeviceInput.KeyStateString != "") AddLog(DeviceInput.KeyStateString);
        //}
        //鍵盤偵測
        String sData1 = ParseFIFOKey(1);
        String sData2 = ParseFIFOKey(2);
        if (sData1 != "") AddLog(sData1);
        //if (sData1 != "") Debug.LogWarning(  sData1);
        if (sData1 != "" && sData1 == GameFifo.Devices[1].FifoMessage && sData2 == GameFifo.Devices[2].FifoMessage)
        {
            if (FifoCmdTime > 0 && Time.realtimeSinceStartup < FifoCmdTime + 0.1f)
            {
                //Debug.LogWarning("Pass!" + sData1);
                Devices[1].FifoMessage = "";
                Devices[2].FifoMessage = "";
                return "";
            }
        }

        FifoCmdTime = Time.realtimeSinceStartup;
        Devices[1].FifoMessage = sData1;
        Devices[2].FifoMessage = sData2;
        String sData = Devices[1].FifoMessage;
        if (sData != "") AddLog(sData);
        if (sData != "") return sData;


        if (lngFifoHandle > 0)
        {
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
            sData = sData.PadLeft(512);
            int iFifoHaveData = FifoRead(lngFifoHandle, sData);
            if (iFifoHaveData == 0) return "";
#else
            byte[] arrayData = new byte[512];
            int iFifoReadRet = FifoRead(lngFifoHandle, arrayData);
            if (iFifoReadRet == 0) return "";
            sData = System.Text.UTF8Encoding.UTF8.GetString(arrayData);
#endif

        }
        sData = sData.Trim();

        //Debug.Log("Read data from FIFO server:" + sData);
        if (sData.Length > 0) sData = ParseFIFOStatus(sData);
        if (sData != "") AddLog("<< " + sData);
        return sData;
    }

    public static String ParseFIFOKey(int DeviceIndex)
    {
        string sKey = "";
        //if (IsAndroid)
        //{
        //    //DeviceData Device = Devices[DeviceIndex];
        //    DeviceData Device = Devices[0];
        //    if (Device.GetKeyDown(DeviceKey.ENTER)) sKey = "Confirm";
        //    else if (Device.GetKeyDown(DeviceKey.ENTER2)) sKey = "Confirm";
        //    else if (Device.GetKeyDown(DeviceKey.UP)) sKey = "Up";
        //    else if (Device.GetKeyDown(DeviceKey.DOWN)) sKey = "Down";
        //    else if (Device.GetKeyDown(DeviceKey.LEFT)) sKey = "Left";
        //    else if (Device.GetKeyDown(DeviceKey.RIGHT)) sKey = "Right";
        //    else if (Device.GetKeyDown(DeviceKey.BACK)) sKey = "Back";
        //    else if (Device.GetKeyDown(DeviceKey.MENU)) sKey = "Back";
        //    //else if (Input.GetKeyDown(KeyCode.Escape)) sKey = "Menu";
        //    //else if (Input.GetKeyDown(KeyCode.Menu)) sKey = "Menu";
        //    return sKey;
        //}
        if (Input.GetKeyDown(KeyCode.Return)) sKey = "Confirm";
        else if (Input.GetKeyDown(KeyCode.KeypadEnter)) sKey = "Confirm";
        else if (Input.GetKeyDown(KeyCode.Space)) sKey = "Confirm";
        else if (Input.GetKeyDown(KeyCode.Menu)) sKey = "Menu";
        else if (Input.GetKeyDown(KeyCode.Escape)) sKey = "Back";
        else if (Input.GetKeyDown(KeyCode.UpArrow)) sKey = "Up";
        else if (Input.GetKeyDown(KeyCode.DownArrow)) sKey = "Down";
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) sKey = "Left";
        else if (Input.GetKeyDown(KeyCode.RightArrow)) sKey = "Right";
        else if (Input.GetKeyDown(KeyCode.X)) sKey = "X";
        if (sKey != "") return sKey;
        return sKey;
    }

    public static void DebugString(String s)
    {
        // char[] chars = s.ToCharArray();
        string[] Datas = new string[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            Datas[i] = "(" + ((int)s.Substring(i, 1).ToCharArray()[0]).ToString() + ")";
        }
        Debug.Log(string.Join("", Datas));
    }
    public static string ParseFIFOStatus(String s)
    {
        //if (s.IndexOf("ArmAngle") < 0) 
        //Debug.Log(" <<< " + s);
        if (s.IndexOf('\0') > 0) s = s.Substring(0, s.IndexOf('\0'));
        string[] Datas = s.Split("\t"[0]);
        //Debug.Log(" Datas[0]= " + Datas[0]);
        if (Datas[0] == "Dev" || Datas[0] == "DevList")//裝置命令
        {
            if (Datas.Length <= 2) return "";
            int id = -1;
            int.TryParse(Datas[1], out id);
            //Debug.Log(" id= " + id);
            if (id < 0) return "";
            int DeviceIndex = id + 1;
            if (DeviceIndex > Devices.Length) return "";

            if (Datas[2] == "QRString") //檢查裝置對應的QREncodeID代碼是否一置
            {
                Devices[DeviceIndex].QREncodeText = Datas[3];
                return "";
            }
            if (Datas[2] == "Connect" || Datas[0] == "DevList") //裝置連線了,配置到1p或是2p
            {
                Debug.Log(s);
                //DevList	0	Type:Phone	Name:py	OS:Andr	OSVer	Model:HTC Sensation	ClientVer:1.9.11
                //Dev	0	Connect	Type:Phone	Name:py	OS:Andr	OSVer	Model:HTC Sensation	ClientVer:1.9.11
                string DeviceData = Datas[2];
                if (Datas[2] == "Connect") DeviceData = Datas[3];
                Devices[DeviceIndex].ID = id;
                Devices[DeviceIndex].Status = enumDeviceStatus.Connect;
                if (DeviceData == "Type:Phone")
                    Devices[DeviceIndex].Type = enumDeviceType.Phone;
                //2P資訊
                Datas = s.Split("\n\r"[0]);
                if (Datas.Length > 1)
                {
                    Debug.Log("Datas[1]= " + Datas[1]);
                    Datas = Datas[1].Split("\t"[0]);
                    id = -1;
                    int.TryParse(Datas[0], out id);
                    if (id < 0) return "";
                    DeviceIndex = id + 1;
                    DeviceData = Datas[1];
                    Devices[DeviceIndex].ID = id;
                    Devices[DeviceIndex].Status = enumDeviceStatus.Connect;
                    if (DeviceData == "Type:Phone")
                        Devices[DeviceIndex].Type = enumDeviceType.Phone;
                }
                return "";
            }
            //檢查是哪一個裝置的命令
            //Debug.Log(" Length= " + Datas[2].Length+ " Datas[2]= " + Datas[2] + " Length= " + Datas[2].Length);

            if (Datas[2] == "Disconnect")
            {
                Devices[DeviceIndex].Status = enumDeviceStatus.Disconnect;
                Debug.Log("Devices[" + DeviceIndex + "].Status" + Devices[DeviceIndex].Status);
                return "";
            }
            else if (Datas[2] == "Foreground")
            {
                Devices[DeviceIndex].Ground = enumDeviceGround.Foreground;
                Debug.Log("Devices[" + DeviceIndex + "] " + Datas[2]);
                return "";
            }
            else if (Datas[2] == "Background")
            {
                Devices[DeviceIndex].Ground = enumDeviceGround.Background;
                Debug.Log("Devices[" + DeviceIndex + "] " + Datas[2]);
                return "";
            }
            else if (Datas[2] == "Stat")//裝置狀態 0：尚未連結（斷線）1：連線，且為前景（手機）2：連線，但在背景（手機）
            {
                if (Datas[3] == "0")
                    Devices[DeviceIndex].Status = enumDeviceStatus.Disconnect;
                else if (Datas[3] == "1")
                {
                    Devices[DeviceIndex].Status = enumDeviceStatus.Connect;
                    Devices[DeviceIndex].Ground = enumDeviceGround.Foreground;
                }
                else if (Datas[3] == "2")
                {
                    Devices[DeviceIndex].Status = enumDeviceStatus.Connect;
                    Devices[DeviceIndex].Ground = enumDeviceGround.Background;
                }
                return "";
            }
            else if (Datas[2] == "Text")  //關閉文字輸入框
            {
                s = Datas[2];
                if (Datas.Length > 3) InputText = Datas[3];
                GameFifo.AddLog("Text " + InputText);
                return "";
            }
            else if (Datas[2] == "CloseTextInput" || Datas[2] == "CancelTextInput")  //關閉文字輸入框
            {
                InputText = "";
                InputTextVisabled = false;
                AddLog(s);
                return "";
            }
            //其他裝置訊息
            s = Datas[2];
            if (Datas.Length > 3) s = s + ":" + Datas[3];
            if (Datas.Length > 4) s = s + ":" + Datas[4];
            Devices[DeviceIndex].FifoMessage = s;
            //Debug.Log("DeviceIndex = " + DeviceIndex + " Cmd = " + s);
            //s = DeviceIndex + "P_" + s;
            return s;
        }
        else if (Datas[0] == "Lang")//裝置命令 MGB
        {
            IsGetLang = true;
            AddLog(s);
            s = Datas[1].ToUpper();
            if (s == "ALL") return "";
            ////if (s == "ALL") s = "MGB";
            //LangCode = s;
            //Debug.Log("LangCode = " + LangCode);
            //AddLog("LangCode = " + LangCode);
            return "";
        }
        else if (Datas[0] == "UserInfo") //UserInfo [\t] [Name] [\t] [Pass] [\t] [GameID] [\t] [Mac] 
        {
            UserInfo.Name = Datas[1].Trim();
            UserInfo.Pass = Datas[2].Trim();
            UserInfo.GameID = Datas[3].Trim();
            UserInfo.Mac = Datas[4].Trim();
            UserInfo.IsRead = true;
            return "";
        }
        else if (Datas[0] == "UserInfo2") //UserInfo [\t] [Name] [\t] [Pass] [\t] [GameID] [\t] [Mac] 
        {
            //AddLog(s);
            for (int i = 1; i < Datas.Length; i++)
            {
                int dotindex = Datas[i].IndexOf(":");
                if (dotindex <= 0) continue;
                string sKey = Datas[i].Substring(0, dotindex);
                string sValue = Datas[i].Substring(dotindex + 1);
                if (sKey == "UserID") UserInfo.UserID = sValue;//UserID ：user id
                else if (sKey == "Name") UserInfo.Name = sValue; //Name ： 顯示名稱
                else if (sKey == "Pass") UserInfo.Pass = sValue; //Pass ： 密碼
                else if (sKey == "GameID") UserInfo.GameID = sValue;//GameID ： game ID
                else if (sKey == "Mac") UserInfo.Mac = sValue; //Mac ： Mac address
                else if (sKey == "UserType") UserInfo.UserType = sValue; //UserType ： 使用者類型（如自有會員、海信會員）
                else if (sKey == "UserDYID") UserInfo.UserDYID = sValue;//UserID ：user id
                else if (sKey == "Token") UserInfo.Token = sValue; //Token ： Access token
            }
            if (UserInfo.Name == "") UserInfo.Name = "DY" + UserInfo.UserID;
            UserInfo.IsRead = true;
            GameFifo.AddLog("UserID=" + UserInfo.UserID);
            GameFifo.AddLog("Name=" + UserInfo.Name);
            GameFifo.AddLog("Token=" + UserInfo.Token);
            return "";
        }
        else if (Datas[0] == "ReturnTicket") //ReturnTicket \t Status:0 \t UserDYID:xxxxx \t Ticket:xxxxx
        {
            AddLog(s);
            bool IsOK = false;
            for (int i = 1; i < Datas.Length; i++)
            {
                int dotindex = Datas[i].IndexOf(":");
                if (dotindex <= 0) continue;
                string sKey = Datas[i].Substring(0, dotindex);
                string sValue = Datas[i].Substring(dotindex + 1);
                if (sKey == "UserDYID") UserInfo.UserDYID = sValue;
                else if (sKey == "Ticket") UserInfo.Ticket = sValue;
                else if (sKey == "Status" && sValue == "0") IsOK = true;
            }
            UserInfo.IsReadTicket = IsOK;
            GameFifo.AddLog("UserDYID=" + UserInfo.UserDYID);
            GameFifo.AddLog("Ticket=" + UserInfo.Ticket);
            return "";
        }
        return s;
    }

    public static void GetQRString(int DeviceIndex)
    {
        int QREncodeID = DeviceIndex - 1;
        if (Devices[DeviceIndex].QREncodeText == "") SendMsg("GetQRString\t" + QREncodeID);
    }
    //變更搖桿Scene : 0恢復原先 1:Menu  2:Fighting  3:Shake
    public static void SetFifoScene(int DeviceIndex, int Scene)
    {
        //Debug.Log("DeviceIndex:" + DeviceIndex + " SetFifoScene:" + Scene);
        Devices[DeviceIndex].FifoNowScene = Scene;
        if (Scene == 0) //恢復為原先模式
        {
            Scene = Devices[DeviceIndex].FifoScene;
            IsMenuScene = false;
        }
        else if (Scene == 1) //選單模式
        {
            IsMenuScene = true;
        }
        else //不為選單模式則記住目前模式,等到0 才恢復
        {
            IsMenuScene = false;
            Devices[DeviceIndex].FifoScene = Scene;
        }
        //if ( DeviceIndex == 2 ) return;
        //string sTmp = "Scene:" + FifoScenes[Scene];
        if (Devices[DeviceIndex].ID < 0) return;
        //Dev [\t] [裝置id] [\t] Scene [\t] [場景]
        SendMsg("Dev\t" + Devices[DeviceIndex].ID.ToString() + "\tScene\t" + FifoScenes[Scene]);
    }

    public static int GetScene(int DeviceIndex)
    {
        return Devices[DeviceIndex].FifoScene;
    }

    public static void GetFifoDeviceType()
    {
        SendMsg("GetDevList");
    }

    public static void ClearFifoData()
    {//medit fifo buffer size is 20
        string s = "";
        for (int i = 1; i <= 20; i = i + 1)
        {
            s = GetMsg();
            if (s == "") break;
        }
    }
    public static void AddLog(string sMsg)
    {
        Debug.Log(sMsg);
        if (DebugMode)
        {
            if (FiFoLogs.Count >= 10) FiFoLogs.RemoveAt(0);
            FiFoLogs.Add(sMsg);
        }
    }
}

public enum enumDeviceStatus { None, Connect, Disconnect };
public enum enumDeviceType { None, Phone };
public enum enumDeviceGround { None, Background, Foreground };
public class DeviceData
{
    public int ID = -1; //裝置代碼
    public enumDeviceStatus Status = enumDeviceStatus.None; //遊戲裝置狀態
    public enumDeviceType Type = enumDeviceType.None; //遊戲裝置類別
    public enumDeviceGround Ground = enumDeviceGround.None; //遊戲裝置前景後景
    //public int QREncodeID = -1; //QRcode對應的裝置代碼
    public string QREncodeText = "";// "1;3;32500;0000;0;0;;0;1;aiwi-rd;aibelive12345;2;1;1;Connectify-Jason;11111111;2;0;0;;;0;1;10.15.20.57;1;0;10.15.20.11;0;;1;0;;;";
    public int FifoScene = 0;
    public int FifoNowScene = 0;
    public string FifoMessage = "";
    public bool IsSetAsPlayDev = false;
    private Texture2D mQREncodeTexture = null;
    public Texture2D QREncodeTexture
    {
        get
        {
            if (mQREncodeTexture != null) return mQREncodeTexture;
            if (QREncodeText == "") return null;
            mQREncodeTexture = QRcodeManager.QREncode(QREncodeText, 512, 512);
            return mQREncodeTexture;
        }
        set
        {
            mQREncodeTexture = value;
        }
    }

    public bool GetKeyDown(DeviceKey Key)
    {
        //if (ID < 0) return false;
        return DeviceInput.GetKeyDown(ID, Key);
    }

    public bool GetKeyUp(int DeviceID, DeviceKey Key)
    {
        //if (ID < 0) return false;
        return DeviceInput.GetKeyUp(ID, Key);
    }

}

public class UserInfoData
{   //UserInfo [\t] [Name] [\t] [Pass] [\t] [GameID] [\t] [Mac] 
    //傳送UserInfo，包含四個欄位：Name , Pass, GameID, Mac
    public string OEMCode = "dygame";
    public string Name = "test";
    public string Pass = "";
    public string GameID = "";
    public string Mac = "";
    public string UserID = "123";
    public string UserType = "";
    public string Token = "";
    public string Ticket = "koUHHcoFnmKNm49";
    public string UserDYID = "1085367514491317181";
    public bool IsRead = false; //是否已經讀取過   true    false     
    public bool IsReadTicket = false; //是否已經讀取過Ticket     
    public bool IsSendTicketCmd = false; //是否已經送出讀取Ticket指令      
}
