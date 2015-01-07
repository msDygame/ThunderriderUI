using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Security.AccessControl;

public class AchievementTable
{
	private static string sGameName = "dyWaveRider";
	
    private static string sAppDataPath = Application.persistentDataPath + "/" + sGameName;
	//private static string sAppDataPath = "D://";//Application.dataPath;
	//遊戲的成就資料結構字串
    public static string TitleString = "";
    public static string TitleVarString = "";
    public static void Init()
    {
		//Debug.Log("sAppDataPath:" + sAppDataPath);
		
		if(!Directory.Exists(sAppDataPath))
		{
			Directory.CreateDirectory(sAppDataPath);
		}
		
		if(!File.Exists(sAppDataPath + "/title_1.txt"))
		{
				
        TitleString = @"//編號	類別	稱號名稱	獲得稱號條件	分數	變量	到達值	已得到
1	0	素人	遊戲次數10次	1	PlayCount	10	0
2	0	初心者	遊戲次數20次	1	PlayCount	20	0
3	0	見習生	遊戲次數50次	1	PlayCount	50	0
4	0	快艇熱血漢	遊戲次數200次	1	PlayCount	200	0
5	0	快艇高手	遊戲次數500次	1	PlayCount	500	0
6	0	快艇勇者	遊戲次數5000次	1	PlayCount	5000	0
7	0	快艇鐵人	遊戲次數10000次	1	PlayCount	10000	0
8	0	半熟者	遊戲勝場次數50次	1	WinCount	50	0
9	0	修行中	遊戲勝場次數100次	1	WinCount	100	0
10	0	快艇能者	遊戲勝場次數1000次	1	WinCount	1000	0
11	0	快艇玄人	遊戲勝場次數2000次	1	WinCount	2000	0
12	0	快艇名人	遊戲勝場次數3000次	1	WinCount	3000	0
13	0	快艇達人	遊戲勝場次數10000次	1	WinCount	10000	0
";
			SaveData(sAppDataPath + "/title_1.txt",TitleString);
		}
		else
			TitleString = LoadData(sAppDataPath + "/title_1.txt");
		
		if(!File.Exists(sAppDataPath + "/title_1Var.txt"))
		{
        TitleVarString = @"//編號	類別	名稱	說明	最大量	累積量
1	0	PlayCount	遊戲次數	10000	0
1	0	WinCount	勝場次數1	10000	0
";
			SaveData(sAppDataPath + "/title_1Var.txt",TitleVarString);
		}
		else
			TitleVarString = LoadData(sAppDataPath + "/title_1Var.txt");
    }
	
	private static void SaveData(string sFilePath,string sFileContent)
	{
		try
    	{	
			//Debug.Log("sFilePath:" + sFilePath);
			//Debug.Log("sFileContent:" + sFileContent);
			
			if(!System.IO.File.Exists(sFilePath))
			{
				//var data = System.IO.File.Create( filePath );
				//Create the file.
	            using (FileStream fs = File.Create(sFilePath))
	            {
	                
					Byte[] info = new UTF8Encoding(true).GetBytes(sFileContent);
	                // Add some information to the file.
	                fs.Write(info, 0, info.Length);
	            }
			}
			else
			{
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(sFilePath))
				{
					file.WriteLine(sFileContent);
				}
			}
			
			 // Remove the access control entry from the file.
//                RemoveFileSecurity(sFilePath, @"DomainName\AccountName",
//                    FileSystemRights.WriteData, AccessControlType.Allow);
		}
		catch (Exception Ex)
    	{
			Debug.Log("sFilePath:" + sFilePath);
			Debug.Log(Ex.ToString());
    	}
	}
	
	public static string LoadData(string sFilePath)
	{
		string sFileContent = "";
		
		try
    	{
			// Open the stream and read it back.
			StreamReader sr = new StreamReader(sFilePath, System.Text.Encoding.UTF8);
            //using (StreamReader sr = File.OpenText(sFilePath))
            using (sr = File.OpenText(sFilePath))
			{
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    //Debug.Log("data:" + s);
					sFileContent = sFileContent + s + "\r\n";
                }				
            }
		}
		catch (Exception Ex)
    	{
        	Debug.Log(Ex.ToString());
    	}
		
		return sFileContent;
	}
	
	public static void Save(string sData,int type)
	{
		if( type == 1)
			SaveData(sAppDataPath + "/title_1.txt",sData);
		else if ( type == 2)
			SaveData(sAppDataPath + "/title_1Var.txt",sData);
	}
	
//	public static void SetAccessRule(string directory)
//    {
//        System.Security.AccessControl.DirectorySecurity sec = System.IO.Directory.GetAccessControl(directory);
//        FileSystemAccessRule accRule = new FileSystemAccessRule(Environment.UserDomainName + "\\" + Environment.UserName, FileSystemRights.FullControl, AccessControlType.Allow);
//        sec.AddAccessRule(accRule);
//    }
	
	
//	// Adds an ACL entry on the specified file for the specified account.
//    public static void AddFileSecurity(string fileName, string account,
//        FileSystemRights rights, AccessControlType controlType)
//    {
//
//
//        // Get a FileSecurity object that represents the
//        // current security settings.
//        FileSecurity fSecurity = File.GetAccessControl(fileName);
//
//        // Add the FileSystemAccessRule to the security settings.
//        fSecurity.AddAccessRule(new FileSystemAccessRule(account,
//            rights, controlType));
//
//        // Set the new access settings.
//        File.SetAccessControl(fileName, fSecurity);
//
//    }
//	
//	// Removes an ACL entry on the specified file for the specified account.
//    public static void RemoveFileSecurity(string fileName, string account,
//        FileSystemRights rights, AccessControlType controlType)
//    {
//
//        // Get a FileSecurity object that represents the
//        // current security settings.
//        FileSecurity fSecurity = File.GetAccessControl(fileName);
//
//        // Remove the FileSystemAccessRule from the security settings.
//        fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
//            rights, controlType));
//
//        // Set the new access settings.
//        File.SetAccessControl(fileName, fSecurity);
//
//    }
}
