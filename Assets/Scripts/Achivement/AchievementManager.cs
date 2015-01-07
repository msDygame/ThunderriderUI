using UnityEngine;
using System.Collections;

public class AchievementManager
{
    public static ArrayList AchievementList = new ArrayList();  //成就資料集
    public static ArrayList AchievementVarList = new ArrayList();      //變量資料集
    //private static Hashtable Achievements = new Hashtable();    //成就資料索引
    private static Hashtable AchievementVars = new Hashtable();        //變量資料索引

    public static void Init()
    {
        if (AchievementList.Count > 0) return;
        //初始化遊戲的成就資料結構字串
        AchievementTable.Init();
        //產生成就資料清單
        AchievementList.Clear();
        string[] cmds = AchievementTable.TitleString.Split("\n"[0]);
		//Debug.Log("AchievementTable.TitleString:" + AchievementTable.TitleString);
        for (int i = 0; i < cmds.Length; i++)
        {	
			string cmd = cmds[i].Trim();
            if (cmd.Length < 10) continue;
            if (cmd.Substring(0, 2) == "//") continue;
            string[] Datas = cmd.Split("\t"[0]);
            if (Datas.Length > 6)
            {
                AchievementData data = new AchievementData();
                int.TryParse(Datas[0], out data.ID);
                int.TryParse(Datas[1], out data.Kind);
                data.Name = Datas[2].Trim();
                data.Description = Datas[3].Trim();
                int.TryParse(Datas[4], out data.Score);
                data.VarName = Datas[5].Trim();
                int.TryParse(Datas[6], out data.MaxCount);
                AchievementList.Add(data);
            }
        }
        //產生變量資料清單
        AchievementVarList.Clear();
        AchievementVars.Clear();
        cmds = AchievementTable.TitleVarString.Split("\n"[0]);
        for (int i = 0; i < cmds.Length; i++)
        {
            string cmd = cmds[i].Trim();
            if (cmd.Length < 10) continue;
            if (cmd.Substring(0, 2) == "//") continue;
            string[] Datas = cmd.Split("\t"[0]);
            if (Datas.Length > 5)
            {
                //編號  類別  名稱    說明    累積量
                AchievementVar data = new AchievementVar();
                int.TryParse(Datas[0], out data.ID);
                int t = 0;
                int.TryParse(Datas[1], out t);
                data.Type = (AchievementVarType)t;
                data.Name = Datas[2].Trim();
                data.Description = Datas[3].Trim();
                int.TryParse(Datas[4], out data.MaxCount);
                int.TryParse(Datas[5], out data.Count);
                AchievementVarList.Add(data);
                AchievementVars.Add(data.Name, data);
            }
        }
    }

    //public static AchievementData GetAchievement(string Key)
    //{
    //    if (Achievements.Contains(Key)) return (AchievementData)Achievements[Key];
    //    return new AchievementData();
    //}

    public static AchievementVar GetAchievementVar(string Key)
    {
        if (AchievementVars.Contains(Key)) return (AchievementVar)AchievementVars[Key];
        return new AchievementVar();
    }

    //檢查是否有達到成就，有則回傳達到的成就清單
    public static ArrayList CheckAchievement()
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < AchievementList.Count; i++)
        {
            AchievementData achive=(AchievementData)AchievementList[i];
            if (achive.IsGet) continue;
            AchievementVar var = GetAchievementVar(achive.VarName);
            if (var.Count<achive.MaxCount)continue;
            achive.IsGet = true;
            list.Add(achive);
        }
        return list;
    }

    //變量值增加
    public static void AddVarCount(string Key,int count)
    {
        AchievementVar var = GetAchievementVar(Key);
        var.Count += count;
        if (var.Count > var.MaxCount) var.Count = var.MaxCount;
    }

    //變量歸零
    public static void SetVarZero(AchievementVarType vtype)
    {
        for (int i = 0; i < AchievementVarList.Count; i++)
        {
            AchievementVar var = (AchievementVar)AchievementVarList[i];
            if (vtype == AchievementVarType.Addition)
                var.Count = 0;
            else if (vtype == AchievementVarType.Addition)
            {
                if (var.Type == AchievementVarType.Application || var.Type == AchievementVarType.Game || var.Type == AchievementVarType.Round) var.Count = 0;
            }
            else if (vtype == AchievementVarType.Game)
            {
                if (var.Type == AchievementVarType.Game || var.Type == AchievementVarType.Round) var.Count = 0;
            }
            else if (vtype == AchievementVarType.Round)
            {
                if (var.Type == AchievementVarType.Round) var.Count = 0;
            }
        }
    }
	
	public static void SaveAchivement()
    {
		Debug.Log("SaveAchivement");
		
		CheckAchievement();
		
		string sAchieveData = "";
		
		for( int i = 0; i < AchievementList.Count ; i++)
		{
			//編號	類別	成就名稱	獲得成就條件	分數	變量	到達值	已得到
			AchievementData data = (AchievementData)AchievementList[i];
			string isGet = "";
			
			if( data.IsGet )
				isGet = "1";
			else
				isGet = "0";
			
			sAchieveData = sAchieveData + data.ID + "\t" + data.Kind + "\t" + data.Name  + "\t" +  data.Description + "\t" + data.Score + "\t" + data.VarName + "\t" + data.MaxCount + "\t" + isGet + "\r\n";
			//Debug.Log("sAchieveData:" + sAchieveData);
		}
		
		AchievementTable.Save(sAchieveData,1);
		
		string sAchieveVarData = "";
		
		for( int i = 0; i < AchievementVarList.Count ; i++)
		{
			 //編號	類別	名稱	說明	最大量	累積量
            AchievementVar data = (AchievementVar)AchievementVarList[i];   
			sAchieveVarData = sAchieveVarData + data.ID + "\t" + data.Type + "\t" + data.Name + "\t" + data.Description + "\t" + data.MaxCount + "\t" + data.Count +"\r\n";
		}
		
		AchievementTable.Save(sAchieveVarData,2);
	}
}

//成就資料結構
public class AchievementData
{
    public int ID=0; //編號
    public int Kind = 0; //類別
    public string Name = ""; //名稱
    public string Description = ""; //說明
    public string VarName = ""; //變量名稱
    public int MaxCount = 0; //到達量
    public int Score = 0; //分數
    public bool IsGet; //是否已得到此成就
}

//變量的使用範圍 Addition：持續累積的 Application：程序啟動時候歸零 Game：每局開始時候歸零 Round：每回合開始歸零
public enum AchievementVarType { Addition, Application, Game, Round };

//變量資料結構
public class AchievementVar
{
    public int ID = 0; //編號
    public AchievementVarType Type = AchievementVarType.Addition; //類別
    public string Name = ""; //變量名稱
    public string Description = ""; //說明
    public int MaxCount = 0; //最大量
    public int Count = 0; //目前累積量
}
