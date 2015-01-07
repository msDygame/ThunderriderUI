using UnityEngine;
using System.Collections;

//eunm and define Map Index (20140923決議:SenceIndex從1開始)(注:Scene=場景,Sense=感覺,Sence=有人拼錯了..)
public enum JSK_ENUM_SCENE_INDEX : int
{
	SCENE_0_NONE  = 0 ,
	SCENE_1_CHILE = 1 ,//智利;阿茲特克(英文的意思;遊戲中場景名稱)
	SCENE_2_VENICE= 2 ,//威尼斯;水之都
	SCENE_3_CHINA = 3 ,//中國;江南
	SCENE_4_TOKYO = 4 ,//東京;東京地下神殿
	SCENE_5_SIBERIA=5 ,//西伯利亞;極地
	MAX
}
//排行榜 名次 匿稱 累積場次 最快時間/累積積分
public class RandomDisplayRank
{
	public int 		iRanlIndex  = 0 ;//名次
	public string	sPlayerName= "" ;//匿稱
	public int 		iTotalRound = 0 ;//累積場次
	public int 		iFinishTime = 0 ;//最快時間
	public int 		iTotalScore = 0 ;//累積積分
}
public class JSK_ThunderRankMenuProcess : MonoBehaviour 
{
	public  Transform 	uiCamera			= null;	//攝影機.
	public  Transform 	MenuRoot       		= null; //背景根節點選單.
	public  Transform 	RankRoot	   		= null; //爬行榜節點選單.

	//Thunder rank
	private GameObject 	RankFocusBar		= null ;//選擇/閃光棒
	private GameObject 	MapChinaRank  		= null; //江南場景分頁.
	private GameObject 	MapChileRank	   	= null; //智利場景分頁.
	private GameObject 	MapSiberiaRank  	= null; //西伯利亞場景分頁.
	private GameObject 	MapTokyoRank  		= null; //東京場景分頁.
	private GameObject 	MapVeniceRank	  	= null; //威尼斯場景分頁.
	private GameObject 	MapChinaBase  		= null; //江南場景背景.
	private GameObject 	MapChileBase	   	= null; //智利場景背景.
	private GameObject 	MapSiberiaBase  	= null; //西伯利亞場景背景.
	private GameObject 	MapTokyoBase  		= null; //東京場景背景.
	private GameObject 	MapVeniceBase	  	= null; //威尼斯場景背景.
	private GameObject 	backButtonMenu		= null;	//返回鍵.(X)返回

	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 0.9f; //菜单动画的播放速度.

	private int 		maxStageNum     	= 5;  	//最大的关卡数量.
	private int 		curStageIndex   	= 0;  	//当前的关卡索引.
	//排行榜 前12名玩家
	private static RandomDisplayRank[] RankList	= new RandomDisplayRank[12];
	//
	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
		//Init Menu
		RankFocusBar	= RankRoot.Find("ranking_back/tr_mo_mu_ranking_back04").gameObject;
		MapChinaRank	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_a002").gameObject;
		MapChileRank	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_e001").gameObject;
		MapSiberiaRank	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_d001").gameObject;
		MapTokyoRank	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_b002").gameObject;
		MapVeniceRank	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_c001").gameObject;
		MapChinaBase	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_a001").gameObject;
		MapChileBase	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_e002").gameObject;
		MapSiberiaBase	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_d002").gameObject;
		MapTokyoBase	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_b001").gameObject;
		MapVeniceBase	= RankRoot.Find("ranking_map/tr_mo_mu_ranking_c002").gameObject;
		backButtonMenu	= MenuRoot.Find("tr_mo_mu_button_x").gameObject;
		//default
		RankFocusBar.SetActive(false);
		MapChileRank.SetActive(false);
		MapSiberiaRank.SetActive(false);
		MapTokyoRank.SetActive(false);
		MapVeniceRank.SetActive(false);
		//
		for( int i = 0 ; i < 12 ; i++)	RankList[i] = new RandomDisplayRank();
	}
	// Use this for initialization
	void Start () 
	{
		onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_3_CHINA);
	}	
	// Update is called once per frame
	void Update () 
	{
		if( curAnimName != "" )
		{
			updateUIAnim();
			return;
		}
		
		if( nextSceneName != "" )
		{
            if (nextSceneName == "UI_MainMenu")
				JSK_GlobalProcess.ReturnMainMenu();
			else
				JSK_GlobalProcess.ChangeLevel(nextSceneName);
			return;
		}

		if( JSK_GlobalProcess.g_UseTouchMenu )
			checkTouchInput();
		
		checkFifoMenu();
	}
	//
	void OnGUI()
	{	
	
	}
	//
	void checkTouchInput()
	{
		if( !uiCamera )
			return;
		
		JSK_GlobalProcess.UpdateFinger();
		
		if( JSK_GlobalProcess.IsFingerDown() )
		{
			Ray ray = uiCamera.camera.ScreenPointToRay(JSK_GlobalProcess.GetFingerPosition());
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, Mathf.Infinity) )
			{
				GameObject hitMenu = hit.transform.gameObject;
				if( hitMenu == backButtonMenu )
					onMenuEsc();
				else if(hitMenu == MapChileBase)//Base有MeshCollider
					onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_1_CHILE);
				else if(hitMenu == MapVeniceBase)
					onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_2_VENICE);
				else if(hitMenu == MapChinaBase)
					onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_3_CHINA);
				else if(hitMenu == MapTokyoBase)
					onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_4_TOKYO);
				else if(hitMenu == MapSiberiaBase)
					onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_5_SIBERIA);
			}
		}
	}
	//
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
		if     ( sInputMsg.IndexOf("Left") >= 0 )		onMenuMapMove(-1);
		else if( sInputMsg.IndexOf("Right") >= 0 )		onMenuMapMove(1);
		else if( Input.GetKeyDown(KeyCode.X) )			onMenuEsc();
		else if( sInputMsg.IndexOf("Esc") >= 0 )		{ }
		else if( sInputMsg.IndexOf("Confirm") >= 0 )	{ }
	}
	//
	void playMenuAnim( Transform menu, string animName, float speed, bool mid, WrapMode mode )
	{
		if( speed == 0 )
			return;
		AnimationState anim = menu.animation[animName];
		if( anim )
		{
			anim.speed = speed;
			anim.wrapMode = mode;
			if( speed < 0 )
				anim.time = anim.length;
			else
				anim.time = 0;
			float absSpeed = Mathf.Abs(anim.speed);
			curAnimMenu = menu;
			curAnimName = animName;
			curAnimTime = anim.length/absSpeed;
			
			if( mid )
			{
				anim.time = anim.length/2;
				curAnimTime /= 2;
			}
			menu.animation.Play(animName);
		}
		else
		{
			Debug.Log("Can not found anim: " + animName);
		}
	}
	
	void updateUIAnim()
	{
		if( !curAnimMenu.animation.IsPlaying(curAnimName) )
		{
			curAnimName = "";
			curAnimTime = 0;
			curAnimMenu = null;
			JSK_GlobalProcess.ClearFifoMessage();	
		}
	}
	//
	void onMenuSelect()
	{	
	}
	
	void onMenuEsc()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
        int iTargetUIVersion = JSK_GlobalProcess.g_iDemoUIVersion;
        if (iTargetUIVersion == 1)
        {
            Application.LoadLevel("JSK_MainMenu");//Original MainMenu
        }
        else if (iTargetUIVersion == 2)
        {
            Application.LoadLevel("UI_MainMenu");//主選單-Playmake
        }
        else if (iTargetUIVersion == 3)
        {
            Application.LoadLevel("UI_NGUI_Main");//主選單-II
        }
        else if (iTargetUIVersion == 4)
        {
            Application.LoadLevel("UI_NGUI_Main");//主選單-III,nGUI改寫
        }
	}
	//
	void onMenuMapMove( int val )
	{
		JSK_SoundProcess.PlaySound("MenuMove");
		
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;
		
		if( val < 0 )
		{
			curStageIndex--;
			if( curStageIndex < 0 )
				curStageIndex = maxStageNum - 1;
		}
		else if( val > 0 )
		{
			curStageIndex++;
			if( curStageIndex > maxStageNum - 1 )
				curStageIndex = 0;
		}
		//美術給的圖的順序是 江南-東京-水都-極地-阿茲特克
		switch( curStageIndex )
		{		
			case 0: onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_3_CHINA) ;  break ;
			case 1: onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_4_TOKYO);	break ;
			case 2: onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_2_VENICE);	break ;
			case 3: onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_5_SIBERIA);	break ;
			case 4: onMenuMapSelected((int)JSK_ENUM_SCENE_INDEX.SCENE_1_CHILE) ;  break ;
			default:break ;
		}
	}
	//
	void onMenuMapSelected(int iSelectedMap)
	{
		MapChinaRank.SetActive(false);
		MapChileRank.SetActive(false);
		MapSiberiaRank.SetActive(false);
		MapTokyoRank.SetActive(false);
		MapVeniceRank.SetActive(false);
		switch( iSelectedMap )
		{
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_0_NONE: 	break;
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_1_CHILE:	MapChileRank.SetActive(true);	break ;
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_2_VENICE:	MapVeniceRank.SetActive(true);	break ;
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_3_CHINA:	MapChinaRank.SetActive(true);	break ;
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_4_TOKYO:	MapTokyoRank.SetActive(true);	break ;
			case (int)JSK_ENUM_SCENE_INDEX.SCENE_5_SIBERIA:	MapSiberiaRank.SetActive(true);break ;
			default:break ;
		}
		//
		GetWebServerData(iSelectedMap) ;
		//
		SetLoadingMenuState(JSK_ENUM_THUNDER_RANK_STATE.RANKING , iSelectedMap ,0) ;
		//玩家資料
        int iMoney  = 0;
        int iLevel  = 0;
        int iExp    = 0;
        if (JSK_GlobalProcess.g_IsWebServer)
        {
            /*
            iMoney = WebServerProcess.User.Money ;
            iLevel = WebServerProcess.User.UserLevel ;
            iExp   = WebServerProcess.User.Exp ;
             */
        }
        else
        {
            iMoney  = Random.Range(1, 10000);
            iLevel  = Random.Range(1, 100);
            iExp    = (iLevel * 10000) + Random.Range(1, 10000);
        }
        
		string sTarget = "TextUserData/TextLevel" ;
		TextMesh mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
		mTextMesh.text = "Level:" + iLevel ;
		sTarget = "TextUserData/TextExp" ;
		mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
		mTextMesh.text = "Exp:" + iExp ;
		sTarget = "TextUserData/TextMoney" ;
		mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
		mTextMesh.text = "Money:" + iMoney ;
	}
	//
	void SetLoadingMenuState(JSK_ENUM_THUNDER_RANK_STATE state, int iIndex , int iOption)
	{
		switch( state )
		{
			case JSK_ENUM_THUNDER_RANK_STATE.NONE: break;
			case JSK_ENUM_THUNDER_RANK_STATE.RANKING://競速
			{
				//default Map
				if ((iIndex > 5) || (iIndex < 0)) break ;//out of range
				int iLineIndex = 0 ;
				for (int i = 0 ; i < 12 ; i++)
				{
					string sIndex = "" + (iLineIndex+1) ;//從01開始
					string sTemp  = sIndex.PadLeft(2, '0');//兩位數,不足補0
					string sTarget = "" ;					
					//資料錯誤或成積不存在
					if (RankList[i].iRanlIndex == 0) continue ;
					else
					{
						//玩家名
						sTarget = "TextPlayerName/Text" + sTemp ;
						TextMesh mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
						mTextMesh.text = RankList[i].sPlayerName ;
						//名次
						sTarget = "TextRankIndex/Text" + sTemp ;
						mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
						mTextMesh.text = "" + RankList[i].iRanlIndex ;
						//場次
						sTarget = "TextTotalRound/Text" + sTemp ;
						mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
						mTextMesh.text = "" + RankList[i].iTotalRound ;
						//時間
						sTarget = "TextTotalTimer/Text" + sTemp ;
						mTextMesh = RankRoot.FindChild(sTarget).GetComponent<TextMesh>();
						int iSecond = RankList[i].iFinishTime % 60 ;
						int iMinute = (RankList[i].iFinishTime / 60) % 60 ;
						int iHour	= RankList[i].iFinishTime / 3600 ;
						string sTimer = "" + iHour + ":" + iMinute + ":" + iSecond ;
						mTextMesh.text = "" + sTimer ;
						iLineIndex++ ;
					}					
				}
				break ;
			}
			default:break ;
		}
	}
	//
	void GetWebServerData(int iMapIndex)
	{
		if (JSK_GlobalProcess.g_IsWebServer)
		{
/*
			for (int i = 1 ; i <= 10 ; i++)
			{
				RankData data = WebServerProcess.GetTimeRank(iMapIndex, i);
				if (data == null)
				{
					RankList[i].iRanlIndex  = 0 ;
					RankList[i].iTotalRound = 0 ;
					RankList[i].iTotalScore = 0 ;
					RankList[i].iFinishTime = 0 ;
					RankList[i].sPlayerName = "" ;
				}
				else
				{
					//int Rank; //排行
					//int UserID; //玩家代號
					//string Name; //玩家姓名
					//int Score; //分數
					//int PlayCount; //玩的次數
					RankList[i].iRanlIndex  = data.Rank ;
					RankList[i].iTotalRound = data.PlayCount ;
					RankList[i].iTotalScore = data.Score ;
					RankList[i].iFinishTime = (data.Score / 100) ;//單位:秒
					RankList[i].sPlayerName = data.Name ;
				}
			}
 */ 
		}
		else
		{
			int iBaseTimer = 50 ;//default
			int iBaseScore = Random.Range(9000, 10000);
			int iRandomNumber = 0 ;
			string[] sName = new string[5] ;
			int[]    iName = new int[5] ;
			//
			for (int i = 0 ; i < 12 ; i++)
			{
				RankList[i].iRanlIndex = i ;
				RankList[i].iTotalRound = Random.Range(1, 10000);
				RankList[i].iFinishTime = iBaseTimer + Random.Range(10,30);
				RankList[i].iTotalScore = iBaseScore - Random.Range(100 ,300);
				iBaseTimer = RankList[i].iFinishTime ;
				iBaseScore = RankList[i].iTotalScore ;
				//for playerName
				iRandomNumber = UnityEngine.Random.Range(1, 100000);
				iName[4] = iRandomNumber % 10 ;
				iName[3] = (iRandomNumber / 10) % 10 ;
				iName[2] = (iRandomNumber / 100) % 10 ;
				iName[1] = (iRandomNumber / 1000) % 10 ;
				iName[0] = (iRandomNumber / 10000) ;
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
				RankList[i].sPlayerName	= "編號" + sName[0] + sName[1] + sName[2] + sName[3] + sName[4] ;
			}
		}
	}
}

public enum JSK_ENUM_THUNDER_RANK_STATE
{
	NONE,
	RANKING,//競速
	MAX
}
