using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System ;//for BitConverter
//外部调用说明.
//Awake函数里 : JSK_MatchMenuProcess.LoadingStart();
//Start函数里 : StartCoroutine(JSK_MatchMenuProcess.LoadingEnd(2));
//快艇類型 快艇類型數量
public class RandomDisplayBoat
{
	public string	sBoatType = "" ;//快艇類型
	public int 		iBoatNumber = 0;//快艇類型數量
}
//快艇類型 快艇星數 玩家性名 排名
public class RandomDisplayRatio
{
	public int		iShipLevel= 0 ;
	public int		iShipType = 0 ;
	public string	sShipType = "" ;
	public int		iShipStar = 0 ;//0級不亮星星，3級亮3顆星(0=0star,1=1star,2=2star,3=3star,-1=error,4=error.)
	public string 	sPlayerName = "" ;
	public int 		iRationNumber = 0;
}

public class JSK_MatchMenuProcess : MonoBehaviour
{
//	public 	static 	JSK_LoadState	g_LoadState 	= JSK_LoadState.LoadNone;//20140915Fix.
	public  Transform				uiCamera   		= null;	//射线查询相机.
	public  Transform				MenuRoot  		= null;	//菜单根节点.
	private static Transform	    tr_Stage_Ranking_bg = null;//20140822.使用Background
	private static Transform	    tr_Line_wait    = null;	//20140911.使用tr-lineing.prefab,for 配對&AI系統的UI
	private static Transform	    tr_Line_match   = null;	//20140911.使用tr-lineing-2.prefab,for 配對&AI系統的UI
	//tr_Line_wait的
	private GameObject[] WaitPlayerMessage;					//玩家配對等待介面 "配對中",".....","逾時",共7個
	private GameObject[] WaitPlayerTime;					//玩家配對等待介面 等待時間(時,分,秒,共6位數)
	private GameObject[] WaitPlayerAmount;					//玩家配對等待介面 總人數	(7位數)
	private GameObject[] WaitPlayerNumber;					//玩家配對等待介面 總數量	(車量5系列,數量4位數,總共20)
	//tr_Line_match的
	private GameObject[] MatchPlayerStar;					//玩家配對完成介面 三顆星 (12個玩家,0級不亮星星，3級亮3顆星,總共36顆)
	private GameObject[] MatchPlayerRank;					//玩家配對完成介面 總排名 (12個玩家,排名五位數,總共60.....)

	private static 	float			minX			= -3.3f;
	private static 	float			maxX			= 3.3f;
	private	static	float			curLoadTime		= 0.0f;
	private	static	float			curPerTime		= 0.0f;
	public static	bool			isWebFinish		= false;
	//配對佇列中	5種類
	private static	RandomDisplayBoat[] BoatList 	= new RandomDisplayBoat[5];
	//配對完成後 12玩家
	private static	RandomDisplayRatio[] PlayerList	= new RandomDisplayRatio[12];
	//配對等待秒
	private static 	int[]	RandomDisplayWaiting = new int[10] ;
	private static 	int		RandomDisplayWaitingSecond = 0 ;
	private	static	float	showTime		= 0.0f;
	private static 	float	fWaitingTime	= 0.0f ;
	//
	void Awake()
	{
#if _DebugMode_
		if(JSK_GlobalLoadingState.Instance()==null)
			JSK_GlobalProcess.InitGlobal();
#endif
		tr_Stage_Ranking_bg = MenuRoot.Find("tr-Stage background_001");
		tr_Stage_Ranking_bg.gameObject.SetActive(true);
		tr_Line_wait     = MenuRoot.Find("tr-lineing");
		tr_Line_wait.gameObject.SetActive(true);
		tr_Line_match    = MenuRoot.Find("tr-lineing2");
		tr_Line_match.gameObject.SetActive(false);

		string  sTarget = "" ;
		string  sIndex  = "" ; 
		string  sTemp   = "" ; 
		WaitPlayerMessage = new GameObject[7] ;//玩家配對等待介面 "配對中",".....","逾時",共7個
		WaitPlayerMessage[0] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing02").gameObject;//"配對中"
		WaitPlayerMessage[1] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing03").gameObject;//".    "
		WaitPlayerMessage[2] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing04").gameObject;//" .   "
		WaitPlayerMessage[3] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing05").gameObject;//"  .  "
		WaitPlayerMessage[4] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing06").gameObject;//"   . "
		WaitPlayerMessage[5] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing07").gameObject;//"    ."
		WaitPlayerMessage[6] = MenuRoot.Find("tr-lineing/lineing/tr_mo_mu_lineing08").gameObject;//"逾時"
		WaitPlayerMessage[6].SetActive(false) ;
		WaitPlayerTime    = new GameObject[6] ;//玩家配對等待介面 等待時間(時,分,秒,共6位數)
		WaitPlayerTime[0] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b01").gameObject;
		WaitPlayerTime[1] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b02").gameObject;
		WaitPlayerTime[2] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b03").gameObject;
		WaitPlayerTime[3] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b04").gameObject;
		WaitPlayerTime[4] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b05").gameObject;
		WaitPlayerTime[5] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b06").gameObject;
		WaitPlayerAmount  = new GameObject[7] ;//玩家配對等待介面 總人數	(7位數)
		WaitPlayerAmount[0] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b07").gameObject;
		WaitPlayerAmount[1] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b08").gameObject;
		WaitPlayerAmount[2] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b09").gameObject;
		WaitPlayerAmount[3] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b10").gameObject;
		WaitPlayerAmount[4] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b11").gameObject;
		WaitPlayerAmount[5] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b12").gameObject;
		WaitPlayerAmount[6] = MenuRoot.Find("tr-lineing/No_b/tr_mo_mu_lineing_no_b13").gameObject;
		WaitPlayerNumber= new GameObject[20];//玩家配對等待介面 總數量	(車量5系列,數量4位數,總共20)
		for (int i = 0 ; i < 20 ; i++)
		{
			sIndex = "" + i ;
			sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
			sTarget= "tr-lineing/No_c/tr_mo_mu_lineing_no_a" + sTemp ;
			WaitPlayerNumber[i] = MenuRoot.Find(sTarget).gameObject;
		}
		//tr_Line_match的
		MatchPlayerStar = new GameObject[36] ;//玩家配對完成介面 三顆星 (12個玩家,0級不亮星星，3級亮3顆星,總共36顆)
		for (int i = 0 ; i < 36 ; i++)
		{
			sIndex = "" + i ;
			sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
			sTarget= "tr-lineing2/linestar_a/tr_mo_mu_linestar_a" + sTemp ;
			MatchPlayerStar[i] = MenuRoot.Find(sTarget).gameObject;
		}
		MatchPlayerRank = new GameObject[60] ;//玩家配對完成介面 總排名 (12個玩家,排名五位數,總共60)
		for (int i = 0 ; i < 60 ; i++)
		{
			sIndex = "" + i ;
			sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
			sTarget= "tr-lineing2/line_no_01/tr_mo_mu_lineing_no_a" + sTemp ;
			MatchPlayerRank[i] = MenuRoot.Find(sTarget).gameObject;
		}
		//
		for( int i = 0 ; i <  5 ; i++)	BoatList[i]   = new RandomDisplayBoat();
		for( int i = 0 ; i < 12 ; i++)	PlayerList[i] = new RandomDisplayRatio();
	}
	public static IEnumerator LoadingEnd( float second )
	{
		curLoadTime = second;
		curPerTime = 0;
		yield return new WaitForSeconds(second);
		JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.LoadEnd ) ;
	}

	public static IEnumerator LoadingEndMulti( float second )
	{

		while(!isWebFinish)
		{
			Debug.Log("Wait server result...");
			curLoadTime = second;
			curPerTime = 0;

			yield return new WaitForSeconds(second);
		}
		showTime = 0;
		JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.LoadEnd ) ;
	}


	public static void LoadingClear()
	{
		curLoadTime = 0;
		curPerTime = 0;
		JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.LoadNone ) ;
	}
	
	void Start()
	{
		ClearRivalPlayerData() ;//default
		curPerTime = 0 ;//reset
		//set wait state
		JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.WaitMatchResult ) ;
		//background
		tr_Stage_Ranking_bg.gameObject.SetActive(true);
		tr_Line_wait.gameObject.SetActive(true);
		tr_Line_match.gameObject.SetActive(false);
	}

	void Update()
	{
        if (Time.timeScale <= 0.0f) Time.timeScale = 1.0f;//從Pause回來會被設為0.0f
		float delta = Time.deltaTime;
		if( delta > 0.05f )
			delta = 0.05f;

		curPerTime += delta;
		//step1.wait WebServer
		if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.WaitMatchResult)
		{
			//向webServer 提出 getRivalList的需求
/*
			WebServerProcess.GetRivalList(JSK_GameProcess.GamePlace, 12);//SenceID = 1~5
 */ 
			//set waiting time , for ProcessMatchResultB use.
			SetRivalWaitingTime() ;
			//change next state
			JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ProcessMatchResultA ) ;
		}
		//step2.waiting WebServer respond
		else if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.ProcessMatchResultA)
		{
			//Display "waiting"
			SetWaiting() ;
			//check WebServer respond
/*
			if( WebServerProcess.ServerResult == enumServerResult.Uploading)
			{
				//UnityEngine.Debug.Log("Player match processing!!!");
			}
			else if( WebServerProcess.ServerResult == enumServerResult.Succeed)
			{
				//UnityEngine.Debug.Log("Player match  success!!!");
				//change next state . getRivalList的需求Finish! 
	*/			JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ENUM_WAIT_PLAYER_RESULT ) ; 
	/*		}
			else if( WebServerProcess.ServerResult == enumServerResult.Error)
			{
				//UnityEngine.Debug.Log("Player match error!!!" + WebServerProcess.ResultStatus);
				JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ENUM_WAIT_WEB_SERVER_TIMEOUT ) ;//skit it(?
			}
	*/
		}
		//Step3.WebServer Succeed , and get date from webServer. 
		else if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.ENUM_WAIT_PLAYER_RESULT)
		{
			//20140911.使用
			//Display "waiting"
			SetWaiting() ;
			//update by WebServerProcess.GetRivalData(i) ;
			SetRivalPlayerData() ;
			SetRivalPlayerDataToGlobalObject() ;
			//change next stage
			JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ProcessMatchResultB) ;
			//for display matching
			fWaitingTime = curPerTime ;
		}	
		//Display matching
		else if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.ProcessMatchResultB)
		{
			showTime += delta;
			//Display "waiting" and matching Boat number and total Boat number per sec.
			SetWaiting() ;
			if( showTime >= 1 )//每秒更新,假裝每秒都有matching,(事實上WebServerProcess.GetRivalList就全抓了)
			{
				showTime = 0 ;
				//
				int iDuring = (int)(curPerTime - fWaitingTime) ;
				int iCount = 0 ;
				//
				if (iDuring < 10)//check out of range
				{
					for (int i = 0 ; i < iDuring ; i++)
					{
						iCount = RandomDisplayWaiting[i] ;
					}
					for (int i = 0 ; i < iCount ; i++)
					{
						int iBoatIndex = PlayerList[i].iShipType ;
						BoatList[iBoatIndex].iBoatNumber++ ;							
					}
					for (int i = 0 ; i < 5 ; i++)
					{
						SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,i	,BoatList[i].iBoatNumber) ;
					}
				}
				else if (iDuring > (RandomDisplayWaitingSecond+1))
				{
					JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ENUM_MATCH_PLAYER_RESULT ) ;
					showTime = 0 ;
				}
			}
		}	
		//display matched player
		else if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.ENUM_MATCH_PLAYER_RESULT)
		{
			tr_Line_wait.gameObject.SetActive(false);
			tr_Line_match.gameObject.SetActive(true);
			for (int i = 0 ; i < 12 ; i++)
			{
				SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.SHIP_NAME	,i	,PlayerList[i].sShipType) ;
				SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.SHIP_STAR	,i	,PlayerList[i].iShipStar) ;
				SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.PLAYER_NAME	,i	,PlayerList[i].sPlayerName) ;
				SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.PLAYER_NUMBER,i	,PlayerList[i].iRationNumber) ;
			}
			//change next stage
			JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ENUM_READY_GAMEPLAY) ;		
		}
		//ready to game play
		else if(JSK_GlobalLoadingState.Instance().GetLoadState() == JSK_LoadState.ENUM_READY_GAMEPLAY)
		{
			//wait 3 sec , for veiw your Rival
			showTime += delta;
			if( showTime >= 3 )
			{
				//Load Finish and state reset
				JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.LoadEnd ) ;
				//Application.LoadLevel()
				JSK_GlobalProcess.LoadGameLevelEx() ;
			}
		}
	}
	//
	void OnGUI()
	{	

	}
	//reset
	private void ClearRivalPlayerData()
	{
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,0	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,1	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,2	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,3	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,4	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.WAIT_TOTAL	,0	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,5	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,4	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,3	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,2	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,1	,0) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER		,0	,0) ;
	}
	//waiting...表現正在等
	private void SetWaiting()
	{
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,0	,BoatList[0].iBoatNumber) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,1	,BoatList[1].iBoatNumber) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,2	,BoatList[2].iBoatNumber) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,3	,BoatList[3].iBoatNumber) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER	,4	,BoatList[4].iBoatNumber) ;
		int iTotal = BoatList[0].iBoatNumber +  BoatList[1].iBoatNumber + BoatList[2].iBoatNumber + BoatList[3].iBoatNumber + BoatList[4].iBoatNumber ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.WAIT_TOTAL	,iTotal	,0) ;
		//waiting..
		WaitPlayerMessage[1].SetActive(true) ;
		WaitPlayerMessage[2].SetActive(true) ;
		WaitPlayerMessage[3].SetActive(true) ;
		WaitPlayerMessage[4].SetActive(true) ;
		WaitPlayerMessage[5].SetActive(true) ;
		int iIndex = (int)curPerTime ;
		int i = iIndex % 6 ;
		if (i>0) WaitPlayerMessage[i].SetActive(false) ;//壓暗效果,同時忽略WaitPlayerMessage[0] ("佇列中")
		//timer
		int iSecond = iIndex % 60 ;
		int iMinute = iIndex / 60 ;
		int iHour   = (iMinute>0) ? (iMinute / 60) : 0 ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,5	,iSecond%10) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,4	,iSecond/10) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,3	,iMinute%10) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,2	,iMinute/10) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,1	,iHour % 10) ;
		SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE.TIMER	,0	,iHour / 10) ;
		//time out
		if (iIndex > 12/*120*/) WaitPlayerMessage[6].SetActive(true) ;//JSK_GlobalLoadingState.Instance().SetLoadState( JSK_LoadState.ENUM_WAIT_WEB_SERVER_TIMEPUT ) ;
	}
	//Rival = 對手
	private static void SetRivalWaitingTime()
	{
		//設定虛假的等待時間
		int iCount = 0 ;
		int iLimit = 11/*JSK_GameProcess.iMultiPlayer*/ + UnityEngine.Random.Range(1, 12);//目標人數:11個Rival
		int iMaxWaitTime = 10 ;//最多等x秒
		//reset
		RandomDisplayWaitingSecond = 0 ;
		for (int i = 0 ; i < 10 ; i++) RandomDisplayWaiting[i] = 0 ;
		//
		for (int i = 0 ; i < iMaxWaitTime ; i++)
		{
			//每秒假裝有y個玩家加入
			int iHereComeANewChallenger = UnityEngine.Random.Range(0, 4);//每秒0~3個挑戰者
			if ((iCount + iHereComeANewChallenger) > iLimit)
			{
				RandomDisplayWaiting[i] = iLimit - iCount ;
				iCount = RandomDisplayWaiting[i] ;
				RandomDisplayWaitingSecond = i ;
				break ;
			}
			RandomDisplayWaiting[i] = iHereComeANewChallenger ;
			iCount += iHereComeANewChallenger ;
			RandomDisplayWaitingSecond = i ;
		}
		//人數未滿
		if (iCount < iLimit)
		{
			RandomDisplayWaiting[iMaxWaitTime-1] += (iLimit - iCount) ;//不夠的人數,都塞在最後一秒
		}
	}
	private void SetRivalPlayerData()
	{
		if (JSK_GlobalProcess.g_IsWebServer)
		{
/*
			BoatList[0].sBoatType = "A型快艇" ;
			BoatList[1].sBoatType = "B型快艇" ;
			BoatList[2].sBoatType = "C型快艇" ;
			BoatList[3].sBoatType = "D型快艇" ;
			BoatList[4].sBoatType = "E型快艇" ;
			BoatList[0].iBoatNumber = 0 ;
			BoatList[1].iBoatNumber = 0 ;
			BoatList[2].iBoatNumber = 0 ;
			BoatList[3].iBoatNumber = 0 ;
			BoatList[4].iBoatNumber = 0 ;
			//public int Rank; //排行
			//public int UserID; //玩家代號
			//public string Name; //玩家姓名
			//public int Level; //等級
			//public int Ratio; //獲勝比率0-100
			//public string BoatData; //玩家使用的快艇資料
			int iCount = 0 ;
			int iRivalCount = JSK_GameProcess.iMultiPlayer ;//WebServerProcess.RivalList.Count ;
			for (int i = 1 ; i <= iRivalCount ; i++)//default:1~12,RivalList.Count會由WebServer.gamestart決定
			{
				RivalData rival = WebServerProcess.GetRivalData(i);//1~12
				if (rival == null) continue ;
				else
				{
					PlayerList[iCount].iShipLevel= 0 ;
					PlayerList[iCount].iShipType = rival.Boat.Type ;
					PlayerList[iCount].sShipType = rival.Boat.Name ;
					PlayerList[iCount].iShipStar = rival.Level ;
					PlayerList[iCount].sPlayerName = rival.Name ;
					PlayerList[iCount].iRationNumber = rival.Rank ;
					iCount++ ;
				}
			}
			for (int i = iCount ; i < 12; i++)
			{
				PlayerList[i].iShipLevel= 0 ;
				PlayerList[i].iShipType = 0 ;
				PlayerList[i].sShipType = "" ;
				PlayerList[i].iShipStar = 0 ;
				PlayerList[i].sPlayerName = "" ;
				PlayerList[i].iRationNumber = 0 ;
			}
 */ 
		}
		else
		{
			ClearRivalPlayerData() ;
			//random boat
			BoatList[0].sBoatType = "A型快艇" ;
			BoatList[1].sBoatType = "B型快艇" ;
			BoatList[2].sBoatType = "C型快艇" ;
			BoatList[3].sBoatType = "D型快艇" ;
			BoatList[4].sBoatType = "E型快艇" ;
			BoatList[0].iBoatNumber = 0 ;
			BoatList[1].iBoatNumber = 0 ;
			BoatList[2].iBoatNumber = 0 ;
			BoatList[3].iBoatNumber = 0 ;
			BoatList[4].iBoatNumber = 0 ;
			//rabdom player
			string[] sName = new string[5] ;
			int[]    iName = new int[5] ;
			for (int i = 0 ; i < 12 ; i++)
			{
				PlayerList[i].iRationNumber = UnityEngine.Random.Range(1, 100000);
				PlayerList[i].iShipStar		= UnityEngine.Random.Range(0, 5);
				PlayerList[i].iShipLevel	= UnityEngine.Random.Range(1, 4);
				PlayerList[i].iShipType		= UnityEngine.Random.Range(0, 3);//DE未開放
				PlayerList[i].sShipType		= "Lv" + PlayerList[i].iShipLevel + "-" + BoatList[ PlayerList[i].iShipType ].sBoatType ;
				//for playerName
				iName[4] = PlayerList[i].iRationNumber % 10 ;
				iName[3] = (PlayerList[i].iRationNumber / 10) % 10 ;
				iName[2] = (PlayerList[i].iRationNumber / 100) % 10 ;
				iName[1] = (PlayerList[i].iRationNumber / 1000) % 10 ;
				iName[0] = (PlayerList[i].iRationNumber / 10000) ;
				bool IsFirstDigit = true ;
				for (int j = 0 ; j < 5 ;j++)
				{
					sName[j] = "" ;
					switch(iName[j])
					{
						case 0:
						{
							if (IsFirstDigit) break ;
							sName[j] = string.Copy("零") ; break;
						}
						case 1: sName[j] = string.Copy("一") ; IsFirstDigit = false ; break;
						case 2: sName[j] = string.Copy("二") ; IsFirstDigit = false ; break;
						case 3: sName[j] = string.Copy("三") ; IsFirstDigit = false ; break;
						case 4: sName[j] = string.Copy("四") ; IsFirstDigit = false ; break;
						case 5: sName[j] = string.Copy("五") ; IsFirstDigit = false ; break;
						case 6: sName[j] = string.Copy("六") ; IsFirstDigit = false ; break;
						case 7: sName[j] = string.Copy("七") ; IsFirstDigit = false ; break;
						case 8: sName[j] = string.Copy("八") ; IsFirstDigit = false ; break;
						case 9: sName[j] = string.Copy("九") ; IsFirstDigit = false ; break;
						default : break ;
					}
				}
				PlayerList[i].sPlayerName	= "編號" + sName[0] + sName[1] + sName[2] + sName[3] + sName[4] ;
				//for boat counter 
				int iBoatIndex = PlayerList[i].iShipType ;
				BoatList[iBoatIndex].iBoatNumber++ ;
			}
		}			
	}
	//
	private void SetRivalPlayerDataToGlobalObject()
	{	
		JSK_MatchedRivalList.Instance().Reset() ;
		//
		int iRivalCount = 12 ;/*WebServerProcess.RivalList.Count ;*/
		int iCount = 0 ;
		for (int i = 0 ; i < iRivalCount ; i++)
		{
			if (PlayerList[i].iRationNumber == 0) continue ;//不存在
			JSK_MatchedRivalList.Instance().SetRivalList(iCount , PlayerList[i]) ;
			iCount++ ;
		}
		JSK_MatchedRivalList.Instance().SetMaxRivalPlayer(iRivalCount) ;
	}
	//
	void SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE state, int iIndex , int iOption)
	{
		switch( state )
		{
			case JSK_ENUM_LOADING_MENU_STATE.NONE: break;
			case JSK_ENUM_LOADING_MENU_STATE.TIMER:
			{
				if ((iIndex < 0) || (iIndex > 6)) break ;//out of range
				WaitPlayerTime[iIndex].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				break ;
			}
			case JSK_ENUM_LOADING_MENU_STATE.WAIT_TOTAL:
			{
				if ((iIndex < 0) || (iIndex > 9999999)) iIndex = 0 ;//out of range
				//
				string sResult = Convert.ToString(iIndex) ;
				int iMaxDigit = 7 ;//等待總人數,位數上限為7位數
				for (int i = 0 ; i < iMaxDigit ; i++) WaitPlayerAmount[i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[sResult.Length-1-i] ;//從此字串個位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//防當
					WaitPlayerAmount[iMaxDigit-1-i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的個位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					WaitPlayerAmount[iMaxDigit-1-iCount].SetActive(false);//把未顯示的位數Hide
				}
				break ;
			}
			case JSK_ENUM_LOADING_MENU_STATE.BOAT_NAME: break;
			case JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER:
			{
				if ((iIndex < 0)  || (iIndex > 5)) break ;//out of range
				if ((iOption < 0) || (iOption > 9999)) iOption = 0 ;//out of range
				string sResult = Convert.ToString(iOption) ;
				int iMaxDigit = 4 ;//快艇類型數量,位數上限為4位數
				int iOffset = iIndex * iMaxDigit ;//四位數
				for (int i = 0 ; i < iMaxDigit ; i++) WaitPlayerNumber[iOffset+i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[sResult.Length-1-i] ;//從此字串個位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//防當
					WaitPlayerNumber[iOffset+iMaxDigit-1-i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的個位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					WaitPlayerNumber[iOffset+iMaxDigit-1-iCount].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, 0);//從畫面上的個位數開始
				}
				break ;
			}		
			case JSK_ENUM_LOADING_MENU_STATE.SHIP_NAME: break;
			case JSK_ENUM_LOADING_MENU_STATE.SHIP_STAR:
			{
				//default player
				if ((iIndex > 12) || (iIndex < 0)) break ;//out of range
				//default 0 star
				if ((iOption > 3) || (iOption < 0)) iOption = 0 ;
				//
				int iOffset = iIndex * 3 ;//每人3顆星
				if (iOption == 0) {MatchPlayerStar[iOffset+0].SetActive(false);	MatchPlayerStar[iOffset+1].SetActive(false); MatchPlayerStar[iOffset+2].SetActive(false); }
				if (iOption == 1) {MatchPlayerStar[iOffset+0].SetActive(true) ;	MatchPlayerStar[iOffset+1].SetActive(false); MatchPlayerStar[iOffset+2].SetActive(false); }
				if (iOption == 2) {MatchPlayerStar[iOffset+0].SetActive(true) ;	MatchPlayerStar[iOffset+1].SetActive(true) ; MatchPlayerStar[iOffset+2].SetActive(false); }
				if (iOption == 3) {MatchPlayerStar[iOffset+0].SetActive(true) ;	MatchPlayerStar[iOffset+1].SetActive(true) ; MatchPlayerStar[iOffset+2].SetActive(true) ; }
				break ;
			}	
			case JSK_ENUM_LOADING_MENU_STATE.PLAYER_NAME: break;
			case JSK_ENUM_LOADING_MENU_STATE.PLAYER_NUMBER:
			{
				//default player
				if ((iIndex > 12) || (iIndex < 0)) break ;//out of range
				//default value
				if ((iOption < 0) || (iOption > 99999)) iOption = 0 ;//out of range
				// 0 = 抓不到值或錯誤				
				string sResult = Convert.ToString(iOption) ;
				int iMaxDigit = 5 ;//玩家排名,位數上限為5位數				
				int iOffset = iIndex * iMaxDigit ;//5位數
				if (iOption == 0)
				{
					for (int i = 0 ; i < iMaxDigit ; i++) MatchPlayerRank[iOffset+i].SetActive(false);//data error
				}
				else
				{
					for (int i = 0 ; i < iMaxDigit ; i++) MatchPlayerRank[iOffset+i].SetActive(true);//reset
					int iCount = 0 ;
					for (int i = 0 ; i < sResult.Length ; i++)
					{
						string s = "" + sResult[sResult.Length-1-i] ;//從此字串個位數開始
						int vOut = int.Parse(s);
						if (i >= iMaxDigit) break ;//防當
						MatchPlayerRank[iOffset+iMaxDigit-1-i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的個位數開始
						iCount++ ;
					}
					for (/*    */; iCount < iMaxDigit ; iCount++)
					{
						MatchPlayerRank[iOffset+iMaxDigit-1-iCount].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, 0);//從畫面上的個位數開始
					}
				}
				break ;
			}	
			default:break ;
		}
	}
	//overloading for string Parameters
	void SetLoadingMenuState(JSK_ENUM_LOADING_MENU_STATE state, int iIndex , string sOption)
	{
		switch( state )
		{
			case JSK_ENUM_LOADING_MENU_STATE.NONE: break;
			case JSK_ENUM_LOADING_MENU_STATE.TIMER:  break;
			case JSK_ENUM_LOADING_MENU_STATE.WAIT_TOTAL: break;
			case JSK_ENUM_LOADING_MENU_STATE.BOAT_NAME:
			{
				break ;
			}
			case JSK_ENUM_LOADING_MENU_STATE.BOAT_NUMBER: break;
			case JSK_ENUM_LOADING_MENU_STATE.SHIP_NAME:
			{
				//default player
				if ((iIndex > 12) || (iIndex < 0)) break ;//out of range
				//
				string sIndex = "" + (iIndex+1) ;//從01開始
				string sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
				string sTarget = "TextShipName/TextNumber" + sTemp ;
				TextMesh mTextMesh = tr_Line_match.FindChild(sTarget).GetComponent<TextMesh>();
				mTextMesh.text = PlayerList[iIndex].sShipType ;
				break ;
			}	
			case JSK_ENUM_LOADING_MENU_STATE.SHIP_STAR: break;
			case JSK_ENUM_LOADING_MENU_STATE.PLAYER_NAME:
			{
				//default player
				if ((iIndex > 12) || (iIndex < 0)) break ;//out of range
				//
				string sIndex = "" + (iIndex+1) ;//從01開始
				string sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
				string sTarget = "TextPlayerName/Text_" + sTemp ;
				TextMesh mTextMesh = tr_Line_match.FindChild(sTarget).GetComponent<TextMesh>();
				mTextMesh.text = PlayerList[iIndex].sPlayerName ;
				break ;
			}	
			case JSK_ENUM_LOADING_MENU_STATE.PLAYER_NUMBER: break;
			default:break ;
		}
	}
}

//
public enum JSK_ENUM_LOADING_MENU_STATE
{
	NONE,
	TIMER,//Wait頁 等待時間
	WAIT_TOTAL,//Wait頁 等待總人數
	BOAT_NAME,//Wait頁 快艇類型
	BOAT_NUMBER,//Wait頁 快艇類型數量 
	SHIP_NAME,//Match頁 快艇類型 
	SHIP_STAR,//Match頁 快艇星數
	PLAYER_NAME,//Match頁 玩家姓名
	PLAYER_NUMBER,//Match頁 玩家排名
	MAX
}

