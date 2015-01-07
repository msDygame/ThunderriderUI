//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//public class DYGameLog
//{
//    //private static string ServerUrl = "http://localhost/dygame/Log/Default.aspx?data=";
//    private static string ServerUrl = "http://10.15.20.76/dygame/Log/Default.aspx?data=";
	
//    public static bool IsReg = false;
//    private static int DecodeKey = 105;
	
//    //大廳傳過來的訊息
//    private static string	LobbyID = "dylobby_id";
//    private static string 	OEMCode = "dygame_oem";
//    private static string 	UserID = "dygame_user";
    
//    private static string 	GameID = "A00000125";
//    private static string	GameVersion = "2.0";		
	
//    static public GameLog g_GameLog = new GameLog();
	
////	private void Awake()
////	{
////		g_GameLog.GameInitTime = DateTime.Now;
////		
////		Debug.Log("g_GameLog.GameInitTime:" + g_GameLog.GameInitTime);
////		
////	}
	
////    void Start()
////    {
////        //StartCoroutine(DYGameLog.AddLog());
////    }

//    public static void SetUserInfo(string sLobbyID, string sUserID, string sOEMCode)
//    {
//        IsReg = true;
//        LobbyID	= sLobbyID;
//        UserID 	= sUserID;
//        OEMCode = sOEMCode;
//    }

//    //取得使用者資料 回傳值 
//    // OK UserMoney UserItems
//    public static IEnumerator AddLog()
//    {
//        if (!IsReg)
//            yield break;

//        Debug.Log("<<ServerProcess.GameLog>>");
//        String 	sData = "Cmd=AddLog\r\nLobbyID=" + LobbyID + "\r\nUserID=" + UserID + "\r\nOEMCode=" + OEMCode + "\r\n";
//                sData += "GameID=" + GameID + "\r\nGameVersion=" + GameVersion + "\r\n";
//                sData += "ExecTime=" + g_GameLog.ExecTime + "\r\nPlayedTime=" + g_GameLog.GetPlayedTime() + "\r\nPlayedCount=" + g_GameLog.GetPlayedCount() + "\r\n";
//                sData += "MultiPlayedTime=" + g_GameLog.GetMultiPlayedTime() + "\r\nMultiPlayedCount=" + g_GameLog.GetMultiPlayedCount();
		
//        Debug.Log("Data : " + sData);
        
//        String sEncrypt = EncryptString(DecodeKey, sData);
//        String sUrl = ServerUrl + sEncrypt;
//        Debug.Log("URL : " + sUrl);

//        String sDecrypt2 = DecryptString(DecodeKey, sEncrypt);
//        Debug.Log("URL DecryptString: : " + sDecrypt2);

//        WWW hs_post = new WWW(sUrl);
//        yield return hs_post;
//        String sRet = hs_post.text.Trim();
//        Debug.Log("URL Return: " + sRet);
//        String sDecrypt = DecryptString(DecodeKey, sRet);
//        Debug.Log("URL Decrypt: " + sDecrypt);
//    }

//    static private String EncryptString(int iKey, String sData)
//    {
//        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
//        byte UserKey = (byte)(iKey % 127);
//        byte RndKey = (byte)(UnityEngine.Random.value * 125);
//        byte[] Bytes = ue.GetBytes(sData);
//        string sEncrypt = "";

//        for (int i = 0; i < Bytes.Length; i++)
//        {
//            Bytes[i] = (byte)(Bytes[i] ^ RndKey ^ UserKey);
//            sEncrypt += System.Convert.ToString(Bytes[i], 16).PadLeft(2, '0');
//        }
//        sEncrypt += System.Convert.ToString(RndKey, 16).PadLeft(2, '0');
//        return sEncrypt;
//    }

//    static private String DecryptString(int iKey, String sData)
//    {
//        try
//        {
//            byte UserKey = (byte)(iKey % 127);
//            byte[] Bytes = new byte[sData.Length / 2 - 1];
//            string s = sData.Substring(sData.Length - 2, 2);
//            //Debug.Log("sData.Length= " + sData.Length + ",s: " + s);

//            byte RndKey = (byte)System.Int32.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
//            for (int i = 0; i < Bytes.Length; i++)
//            {
//                s = sData.Substring(i * 2, 2);
//                Bytes[i] = (byte)System.Int32.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
//                Bytes[i] = (byte)(Bytes[i] ^ RndKey ^ UserKey);
//            }

//            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
//            string sDecrypt = ue.GetString(Bytes);
//            return sDecrypt;
//        }
//        catch
//        {
//        }
//        return "";
//    }
	
////	void OnApplicationQuit()
////	{
////		//Debug.Log("Game exit.");
////		//g_GameLog.GameEndTime = DateTime.Now;
////		
////		//Debug.Log("g_GameLog.GameEndTime:" + g_GameLog.GameEndTime);
////		
////		//Debug.Log("Game Exec time:" + g_GameLog.GetExecTime());
////		Debug.Log("Game played time:" + g_GameLog.GetPlayedTime());
////		Debug.Log("Game played count:" + g_GameLog.GetPlayedCount());
////		Debug.Log("Game multi played time:" + g_GameLog.GetMultiPlayedTime());
////		Debug.Log("Game multi played count:" + g_GameLog.GetMultiPlayedCount());
////	}
//}

//public class GameLog
//{
//    public	string			GameVersion = "2.0";
//    public 	string			UserID = "";
//    public	string			GameID = "";
//    public	string			OEM = "";
//    public	string			LobbyID = "";
//    public	int				ExecTime = 0;
	
//    //public 	DateTime		GameInitTime;			//遊戲啟動時間
//    //public 	DateTime		GameEndTime;			//遊戲結束時間
//    //public	int				ExecTime = 0;			//遊戲總執行時間 = 遊戲結束時間 - 遊戲啟動時間
//    //public	int				PlayedTime = 0;			//遊戲總game play時間 (包含單打、雙打）
//    //public	int				PlayedCount = 0;		//遊戲進入game play的次數 (包含單打、雙打
//    //public	int				MultiPlayedTime = 0;	//多P遊戲總game play時間 （不含單打）
//    //public	int				MultiPlayedCount = 0;	//多P遊戲的game play次數 （不含單打）
	
//    public	List<int>		lstPlayedTime = new List<int>();
//    public	List<int>		lstMultiPlayedTime = new List<int>();
//    /*
//    public int GetExecTime()
//    {
//        TimeSpan span = GameEndTime.Subtract ( GameInitTime );
		
//        int totalTime = (int)span.TotalSeconds;
		
//        return totalTime;
//    }
//    */
//    public	int	GetPlayedTime()
//    {
//        int totalPlayTime = 0;
		
//        for(int i = 0; i < lstPlayedTime.Count ; i++)
//        {
//            totalPlayTime += lstPlayedTime[i];
//        }
		
//        return totalPlayTime;
//    }
	
//    public	int	GetPlayedCount()
//    {
//        return lstPlayedTime.Count;
//    }
	
//    public	int	GetMultiPlayedTime()
//    {
//        int totalPlayTime = 0;
		
//        for(int i = 0; i < lstMultiPlayedTime.Count ; i++)
//        {
//            totalPlayTime += lstMultiPlayedTime[i];
//        }
		
//        return totalPlayTime;
//    }
	
//    public	int	GetMultiPlayedCount()
//    {
//        return lstMultiPlayedTime.Count;
//    }
//}