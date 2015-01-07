using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSK_StageMenuProcess : MonoBehaviour
{
	public  Transform 	uiCamera			= null;	//射线查询相机.
	public  Transform 	menuRoot       		= null; //菜单根节点.
	public  Transform 	menuChinaRoot  		= null; //江南根節點.
	public  Transform 	menuChileRoot    	= null; //智利根節點.
	public  Transform 	menuSiberiaRoot  	= null; //西伯利亞根節點.
	public  Transform 	menuTokyoRoot  		= null; //東京根節點.
	public  Transform 	menuVeniceRoot  	= null; //威尼斯根節點.
	public  Transform 	menuBackground1Root	= null; //雜項根節點.(背景,按鈕,ABX鍵等)
	public  Transform 	menuBackground2Root	= null; //雜項根節點.(地名,圈數,難易度等)
	private float 		lastMoveTime		= 0;	//上次移动菜单的时间.
	private float 		moveDelayTime		= 0.0f;	//移动菜单的间隔时间,如果不需要延迟,就把这个改成0.
	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 0.9f; //菜单动画的播放速度.
	private float		lastButtonTime		= 0.0f ;//按鈕壓下的間隔時間
	private bool		IsButtonPressDown   = false;//按鈕壓下旗標

	private int 		maxStageNum     	= 5;  	//最大的关卡数量.
	private int 		curStageIndex   	= 0;  	//当前的关卡索引.
	
	private GameObject 	leftArrowMenu		= null;	//左箭头.
	private GameObject 	rightArrowMenu		= null;	//右箭头.
	private GameObject 	nextButtonMenu		= null;	//确认键.
	private GameObject 	backButtonMenu		= null;	//返回键.
	
	private GameObject 	curStageMenu		= null;	//当前的场景菜单.
	private GameObject 	leftStageMenu		= null;	//左边的场景菜单.
	private GameObject 	rightStageMenu		= null;	//右边的场景菜单.
	private GameObject 	currentMiniMapMenu	= null;	//當前場景的小地圖.

	private GameObject	map_lap				= null;
	private GameObject	map_lap_num			= null;
	private GameObject	map_lap_Name		= null;
	
	private GameObject	map_lap_difficult			= null;
	private GameObject	map_star_0			= null;
	private GameObject	map_star_1			= null;
	private GameObject	map_star_2			= null;
	private GameObject	map_star_3			= null;
	private GameObject	map_star_4			= null;
	private GameObject	map_star_background = null;
	private Transform	scene_ranking		= null;

	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
		initMenu();
	}
	
	void Start()
	{
		JSK_SoundProcess.PlayMusic("MainMenu");
		JSK_GlobalProcess.SetFifoScene(1,1);
		JSK_GlobalProcess.SetFifoScene(2,1);
		JSK_GlobalProcess.ClearFifoMessage();
		
		playMenuAnim(menuRoot, "StageMenu", menuAnimSpeed, false, WrapMode.Once);	
	}
	
	void Update()
	{
		if( curAnimName != "" )
		{
			updateUIAnim();
			return;
		}
		
		if( nextSceneName != "" )
		{
			if( nextSceneName == "JSK_MainMenu" )
				JSK_GlobalProcess.ReturnMainMenu();
			else
				JSK_GlobalProcess.ChangeLevel(nextSceneName);
			return;
		}
		
		if( JSK_GlobalProcess.g_UseTouchMenu )
			checkTouchInput();

		checkFifoMenu();
		if (IsButtonPressDown) SetRestoreButtonState() ;
	}
	
	void initMenu()
	{
		curStageMenu   = menuChinaRoot.Find("tr_mo_mu_China_a01").gameObject;
		currentMiniMapMenu= menuChinaRoot.Find("littlemap/tr_mk_mu_MimiMap_01").gameObject;
		leftArrowMenu  = menuBackground1Root.Find("tr_mo_mu_arrow_l01").gameObject;
		rightArrowMenu = menuBackground1Root.Find("tr_mo_mu_arrow_r01").gameObject;
		nextButtonMenu = menuBackground1Root.Find("tr_mo_mu_button_a001").gameObject;
		backButtonMenu = menuBackground1Root.Find("tr_mo_mu_button_x").gameObject;
		map_lap	  	   = menuBackground2Root.Find("tr_mo_mu_lap").gameObject;
		map_lap_num	   = menuBackground2Root.Find("tr_mo_mu_lap_number").gameObject;
		map_lap_Name   = menuBackground2Root.Find("tr_mo_mu_Map_Name").gameObject;
		map_lap_difficult= menuBackground2Root.Find("tr_mo_mu_hard").gameObject;
		map_star_0	   = menuBackground2Root.Find("tr_mo_mu_hardstar_a01-1").gameObject;
		map_star_1	   = menuBackground2Root.Find("tr_mo_mu_hardstar_a01-2").gameObject;
		map_star_2	   = menuBackground2Root.Find("tr_mo_mu_hardstar_a01-3").gameObject;
		map_star_3	   = menuBackground2Root.Find("tr_mo_mu_hardstar_a01-4").gameObject;
		map_star_4	   = menuBackground2Root.Find("tr_mo_mu_hardstar_a01-5").gameObject;
		map_star_background= menuBackground2Root.Find("tr_mo_mu_hardstar_a02").gameObject;
		map_lap_num.SetActive(false);
		map_lap_difficult.SetActive(false);
		map_star_background.SetActive(false);
		map_star_0.SetActive(false);
		map_star_1.SetActive(false);
		map_star_2.SetActive(false);
		map_star_3.SetActive(false);
		map_star_4.SetActive(false);
		
		notifyMenuState(curStageMenu, JSK_MenuState.TextureIndex, curStageIndex);
		notifyMenuState(currentMiniMapMenu, JSK_MenuState.ENUM_SPRITE_INDEX, curStageIndex);
    }
	
	int getLeftStageIndex( int index )
	{
		int result = index - 1;
		if( result < 0 )
			result = maxStageNum - 1;
		return result;
	}
	
	int getRightStageIndex( int index )
	{
		int result = index + 1;
		if( result > maxStageNum - 1 )
			result = 0;
		return result;
	}
	
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
	        }
	    }
		else if( JSK_GlobalProcess.IsFingerUp() )
	    {
	    	Ray ray = uiCamera.camera.ScreenPointToRay(JSK_GlobalProcess.GetFingerPosition());
			RaycastHit hit;
        	if( Physics.Raycast(ray, out hit, Mathf.Infinity) )
        	{
        		GameObject hitMenu = hit.transform.gameObject;
	        	if( hitMenu == nextButtonMenu )
		        		onMenuSelect();
		       	else if( hitMenu == backButtonMenu )
		        		onMenuEsc();
            }
			RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2D != null) 
			{
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if( hitMenu == leftArrowMenu )	onStageMove(-1);
					else if( hitMenu == rightArrowMenu)	onStageMove(1);
				}
			}
	    }
	}
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
    	if     ( sInputMsg.IndexOf("Left") >= 0 )		onStageMove(-1);
    	else if( sInputMsg.IndexOf("Right") >= 0 )		onStageMove(1);
		else if( sInputMsg.IndexOf("Up") >= 0 )			onStageMove(-1);
    	else if( sInputMsg.IndexOf("Down") >= 0 )		onStageMove(1);
		else if( sInputMsg.IndexOf("Esc") >= 0 )		onMenuEsc();
		else if( sInputMsg.IndexOf("Confirm") >= 0 )	onMenuSelect();
	}
	
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
	
			map_lap.SetActive(true);
			map_lap_num.SetActive(true);
			
			map_lap_difficult.SetActive(true);
			map_star_background.SetActive(true);
			//
			onStageMove(0) ; 
		}
	}
	
	void onMenuSelect()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
        JSK_GlobalProcess.GamePlace = curStageIndex + 1;//加1处理.
		JSK_GlobalProcess.LoadGameLevel();
	}
	
	void onMenuEsc()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
		nextSceneName = "JSK_MotoMenu";
	}
	
	void notifyMenuState( GameObject hitMenu, JSK_MenuState state, int index )
	{	
		hitMenu.GetComponent<JSK_MenuObject>().setMenuState(state, index);
	}
	
	void onStageMove( int val )
	{
		if( Time.time < lastMoveTime )
			return;
	    lastMoveTime = Time.time + moveDelayTime;
	    
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

		if( val < 0 ) notifyMenuState(leftArrowMenu, JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		if( val > 0 ) notifyMenuState(rightArrowMenu,JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		IsButtonPressDown = true ;
		lastButtonTime = Time.time + 0.1f ;
		//update from webServer
		if (JSK_GlobalProcess.g_IsWebServer)
		{
/*
			int iSceneID = curStageIndex + 1;//加1处理.
			SceneData scene = SceneManager.GetScene(iSceneID);//webServer的
			if (scene == null) return ;
			notifyMenuState(map_lap_num , JSK_MenuState.ENUM_SPRITE_INDEX, scene.RoundCount) ;// 圈數
			notifyMenuState(map_lap_Name, JSK_MenuState.ENUM_SPRITE_INDEX, curStageIndex) ;// 使用的地圖編號的中文名
			setMapDifficultStar(scene.Difficulty * 1.0f) ;// 難度
 */ 
		}
		else
		{
			int iRandom = Random.Range(1, 10);
			int iRandomDefficultStar = Random.Range(0, 6);//0,1,2,3,4,5 star
			notifyMenuState(map_lap_num, JSK_MenuState.ENUM_SPRITE_INDEX,iRandom) ;
			notifyMenuState(map_lap_Name,JSK_MenuState.ENUM_SPRITE_INDEX,curStageIndex) ;
			setMapDifficultStar(iRandomDefficultStar * 1.0f) ;
		}
	}

	void SetRestoreButtonState()
	{
		if( Time.time < lastButtonTime ) return;
		IsButtonPressDown = false ;
		if( leftArrowMenu.GetComponent<JSK_MenuObject>().getMenuState() == JSK_MenuState.ENUM_SPRITE_INDEX )
		{
			notifyMenuState(leftArrowMenu, JSK_MenuState.ENUM_SPRITE_INDEX  , 0 );//index=0 button active		
		}
		if( rightArrowMenu.GetComponent<JSK_MenuObject>().getMenuState() == JSK_MenuState.ENUM_SPRITE_INDEX )
		{
			notifyMenuState(rightArrowMenu, JSK_MenuState.ENUM_SPRITE_INDEX , 0 );//index=0 button active		
		}
		//
		notifyMenuState(curStageMenu, JSK_MenuState.TextureIndex, curStageIndex);
		notifyMenuState(currentMiniMapMenu, JSK_MenuState.ENUM_SPRITE_INDEX, curStageIndex);
	}

	void setMapDifficultStar(float val)
	{
		map_star_0.SetActive(false);
		map_star_1.SetActive(false);
		map_star_2.SetActive(false);
		map_star_3.SetActive(false);
		map_star_4.SetActive(false);

		if( val < 1.0f )
		{
			return ;
		}	
		else if( val < 2.0f )
		{
			map_star_0.SetActive(true);
		}
		else if( val < 3.0f )
		{
			map_star_0.SetActive(true);
			map_star_1.SetActive(true);
		}
		else if( val < 4.0f )
		{
			map_star_0.SetActive(true);
			map_star_1.SetActive(true);
			map_star_2.SetActive(true);
		}
		else if( val < 5.0f )
		{
			map_star_0.SetActive(true);
			map_star_1.SetActive(true);
			map_star_2.SetActive(true);
			map_star_3.SetActive(true);
		}
		else
		{
			map_star_0.SetActive(true);
			map_star_1.SetActive(true);
			map_star_2.SetActive(true);
			map_star_3.SetActive(true);
			map_star_4.SetActive(true);
		}
	}

	private void ClearSceneRanking()
	{
		for (int i = 1; i <= 10; i++)
		{
			SetScenePlayerRankValue(i,"",0,2);
		}
		SetScenePlayerRankValue(0,"",0,3);
	}

	private void ShowSceneRanking(int SceneID)
	{
		ClearSceneRanking();
		//get data from webServer
/*
		RankData data = null;

		for (int i = 1; i <= 10; i++)
		{
			data = WebServerProcess.GetTimeRank(SceneID, i);

			SetScenePlayerRankValue(data.Rank,data.Name,data.Score,0);
		}

		UserScoreData user = WebServerProcess.GetUserScore(SceneID);
		if (user != null) SetScenePlayerRankValue(user.TimeRank,WebServerProcess.NickName,user.BestTime,1);
*/
	}

	private void SetScenePlayerRankValue(int rank,string name,float raceTime,int type)
	{
		TextMesh textMesh = null ;

		if( type == 0 )
		{
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextRanking").GetComponent<TextMesh>();
			textMesh.text = rank.ToString();
			
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextPlayerName").GetComponent<TextMesh>();
			textMesh.text = name;
			
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextPlayTime").GetComponent<TextMesh>();
			textMesh.text = setBestTime((int)raceTime);//.ToString();
		}
		else if( type == 1 )
		{
			textMesh = scene_ranking.transform.Find("Row_Self/TextRanking").GetComponent<TextMesh>();
			textMesh.text = rank.ToString();
			
			textMesh = scene_ranking.transform.Find("Row_Self/TextPlayerName").GetComponent<TextMesh>();
			textMesh.text = name;
			
			textMesh = scene_ranking.transform.Find("Row_Self/TextPlayTime").GetComponent<TextMesh>();
			textMesh.text = setBestTime((int)raceTime);//.ToString();
		}
		else if( type == 2 )
		{
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextRanking").GetComponent<TextMesh>();
			textMesh.text = "";
			
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextPlayerName").GetComponent<TextMesh>();
			textMesh.text = "";
			
			textMesh = scene_ranking.transform.Find("Row_" + rank.ToString() + "/TextPlayTime").GetComponent<TextMesh>();
			textMesh.text = "";
		}
		else if( type == 3 )
		{
			textMesh = scene_ranking.transform.Find("Row_Self/TextRanking").GetComponent<TextMesh>();
			textMesh.text = "";
			
			textMesh = scene_ranking.transform.Find("Row_Self/TextPlayerName").GetComponent<TextMesh>();
			textMesh.text = "";
			
			textMesh = scene_ranking.transform.Find("Row_Self/TextPlayTime").GetComponent<TextMesh>();
			textMesh.text = "";
		}

		//this.transform.Find("UICamera").gameObject;
	}

	private string setBestTime( int bestTime )
	{
		//Debug.Log(bestTime);
		int millisecond = bestTime%100;
		int second = bestTime/100;
		
		int minuteTime = second / 60;
		int sceondTime = second % 60;

		return minuteTime.ToString() + ":" + sceondTime.ToString() + ":" + millisecond.ToString();
	}
}
