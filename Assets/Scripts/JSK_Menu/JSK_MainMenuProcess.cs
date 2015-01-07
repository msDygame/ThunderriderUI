using UnityEngine;
using System.Collections;
using System.Diagnostics;

public enum JSK_MainMenuState
{
	None,
	InputName,
	Login,
	SettingNickName,
	Normal,
	ShowStageRankingUI,	//20140923.停用
	ShowPersonalDataUI, //20140822.停用
}

public class JSK_MainMenuProcess : MonoBehaviour
{
	private JSK_MainMenuState menuState		= JSK_MainMenuState.None;

	public  Transform 	uiCamera   			= null;	//射线查询相机.
	public  Transform 	menuRoot  			= null;	//菜单根节点.

    private int 		menuIndex  			= 0;	//主菜单索引值.
	private float 		lastMoveTime		= 0.0f;	//上次移动菜单的时间.
	private float		moveDelayTime		= 0.0f;	//移动菜单的间隔时间,如果不需要延迟,就把这个改成0.

	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 0.9f; //菜单动画的播放速度.

	private GameObject[] menuPlayGame		;		//進入遊戲(藍底,綠底,紅底)/(可選,已選,壓下)
	private GameObject[] menuUpgrade		;		//快艇改裝
	private GameObject[] menuRanking		;		//排行榜/寶石模式
	private GameObject[] menuExitGame		;		//離開遊戲
	private int 		iMenuCurrentHilight = 0 ;	//當前高亮的選單
	private GameObject	menuButtons 		;		//menuPlayGame,menuUpgrade,menuRanking,menuExitGame
	//
	private GameObject	camMain				= null;
	private float		startAngleX			= 0.0f;
	private float		startAngleY			= 0.0f;

	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
		initMenu();
		
		//加载场景.
		camMain = GameObject.Find("Scene1/SceneBaseLogic/SceneCamera").gameObject;
		startAngleX = camMain.transform.localRotation.eulerAngles.x;
		startAngleY = camMain.transform.localRotation.eulerAngles.y;

		GameObject gateEffect = GameObject.Find("Scene1/SceneBaseLogic/JSK_GoalDoor_Start/Gate_Effect 1");
		gateEffect.SetActive(false);

	}

	void Start()
	{
		JSK_SoundProcess.PlayMusic("MainMenu");
		JSK_GlobalProcess.SetFifoScene(1,1);
		JSK_GlobalProcess.SetFifoScene(2,1);
		JSK_GlobalProcess.ClearFifoMessage();
		playMenuAnim(menuRoot, "MainMenu4buttonTitle", menuAnimSpeed, false, WrapMode.Once);

		JSK_SoundProcess.PlaySound("WaveRider");
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
	}
	
	void initMenu()
	{
		//進入遊戲
		menuPlayGame = new GameObject[3] ;
		menuPlayGame[0]		= menuRoot.Find("title/tr_mo_title_button_a01").gameObject;
		menuPlayGame[1]		= menuRoot.Find("title/tr_mo_title_button_b01").gameObject;
		menuPlayGame[2]		= menuRoot.Find("title/tr_mo_title_button_c01").gameObject;
		//快艇改裝
		menuUpgrade = new GameObject[3] ;
		menuUpgrade[0]		= menuRoot.Find("title/tr_mo_title_button_a02").gameObject;
		menuUpgrade[1]		= menuRoot.Find("title/tr_mo_title_button_b02").gameObject;
		menuUpgrade[2]		= menuRoot.Find("title/tr_mo_title_button_c02").gameObject;
		//排行榜/寶石模式
		menuRanking = new GameObject[3] ;
		menuRanking[0]		= menuRoot.Find("title/tr_mo_title_button_a03").gameObject;
		menuRanking[1]		= menuRoot.Find("title/tr_mo_title_button_b03").gameObject;
		menuRanking[2]		= menuRoot.Find("title/tr_mo_title_button_c03").gameObject;
		//離開遊戲
		menuExitGame = new GameObject[3] ;
		menuExitGame[0]		= menuRoot.Find("title/tr_mo_title_button_a04").gameObject;
		menuExitGame[1]		= menuRoot.Find("title/tr_mo_title_button_b04").gameObject;
		menuExitGame[2]		= menuRoot.Find("title/tr_mo_title_button_c04").gameObject;
		//
		menuButtons = menuRoot.Find("title").gameObject;
			
		menuIndex = 0;

		//預設是 "進入遊戲"
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN , menuIndex) ;
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE	, 1) ;
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE	, 2) ;
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE	, 3) ;

		if( JSK_GlobalProcess.g_IsMultiPlayer && JSK_GlobalProcess.g_IsWebServer )
		{
/*
//			WebServerProcess.NTTLogin(JSK_GlobalProcess.g_NTTID);//有沒有暱稱都要呼叫

			string LobbyTicket = "1234" ;//大廳Module 的ticket
			WebServerProcess.NTTLogin("TEST" , LobbyTicket ) ;
	//		WebServerProcess.SetNickName(JSK_GlobalProcess.g_PlayerNickName);
			menuState = JSK_MainMenuState.Login;
 */ 
		}
		else
			menuState = JSK_MainMenuState.Normal;
	}
	
	void onMenuMove( int row )
	{
		if( Time.time < lastMoveTime )
			return;
   	 	lastMoveTime = Time.time + moveDelayTime;
		
		JSK_SoundProcess.PlaySound("MenuMove");

        int iMaxCount = 4 ;//四顆按鈕
		int realRow = row % iMaxCount ;
		
		int result = menuIndex + realRow;
		if( result < 0 )		
			menuIndex += (realRow + iMaxCount) ;
		else if( result > iMaxCount - 1 )
			menuIndex += (realRow - iMaxCount) ;
		else
			menuIndex = result;

        if (iMenuCurrentHilight == menuIndex) return ;
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN , menuIndex) ;
		SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE	, iMenuCurrentHilight) ;
		iMenuCurrentHilight = menuIndex ;
	}
	
	void notifyMenuState( GameObject hitMenu, JSK_MenuState state, int index )
	{
		hitMenu.GetComponent<JSK_MenuObject>().setMenuState(state, index);
		
		hitMenu.renderer.material.mainTexture = 
			JSK_GUIProcess.GetLanguagePicture(hitMenu.renderer.material.mainTexture.name.Substring(0, hitMenu.renderer.material.mainTexture.name.Length-3));
	}
	//
	int moveToMenu( JSK_ENUM_MAIN_MENU_BUTTON_STATE state , GameObject hitMenu)
	{
		//default state
		switch( state )
		{
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.NONE: break;
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE:
			{
				if( menuPlayGame[0] == hitMenu)	return 0 ;
				if( menuUpgrade[0] == hitMenu )	return 1 ;
				if( menuRanking[0] == hitMenu ) return 2 ;
				if( menuExitGame[0] == hitMenu) return 3 ;
				break;
			}
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN:
			{
				if( menuPlayGame[1] == hitMenu)	return 0 ;
				if( menuUpgrade[1] == hitMenu )	return 1 ;
				if( menuRanking[1] == hitMenu ) return 2 ;
				if( menuExitGame[1] == hitMenu) return 3 ;
				break;
			}
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN: 
			{
				if( menuPlayGame[2] == hitMenu)	return 0 ;
				if( menuUpgrade[2] == hitMenu )	return 1 ;
				if( menuRanking[2] == hitMenu ) return 2 ;
				if( menuExitGame[2] == hitMenu) return 3 ;
				break;
			}
			default:break ;
		}
		return -1 ;
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
				int i = moveToMenu(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE , hitMenu);
				if (i < 0)
				{
					i = moveToMenu(JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN , hitMenu);
				}
				if (i >= 0)
				{
					SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE	, iMenuCurrentHilight) ;
					menuIndex = i ;
					SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN, menuIndex) ;
					iMenuCurrentHilight = menuIndex ;
				}
        	}
    	}
		else if( JSK_GlobalProcess.IsFingerUp() )
    	{
    		Ray ray = uiCamera.camera.ScreenPointToRay(JSK_GlobalProcess.GetFingerPosition());
			RaycastHit hit;
        	if( Physics.Raycast(ray, out hit, Mathf.Infinity) )
        	{
        		GameObject hitMenu = hit.transform.gameObject;
				int i = moveToMenu(JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN , hitMenu);
				if (i >= 0)
					onMenuSelect();
        	}
			SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN	, iMenuCurrentHilight) ;
    	}
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
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();

		if( menuState == JSK_MainMenuState.Normal)
		{
			if     ( sInputMsg.IndexOf("Left") >= 0 )		onMenuMove(-1);
	    	else if( sInputMsg.IndexOf("Right") >= 0 )		onMenuMove(1);
			else if( sInputMsg.IndexOf("Up") >= 0 )			onMenuMove(-1);
	    	else if( sInputMsg.IndexOf("Down") >= 0 )		onMenuMove(1);
			else if( sInputMsg.IndexOf("Esc") >= 0 )		onMenuEsc();
			else if( sInputMsg.IndexOf("Confirm") >= 0 )             
			{
				SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN, menuIndex) ;
				onMenuSelect();
			}
            else if (sInputMsg.IndexOf("Confirm") >= 0)
            {
                SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN, menuIndex);
                onMenuSelect();
            }
			else if( Input.GetKeyDown(KeyCode.X) )
			{

			}
		}
		else if( menuState == JSK_MainMenuState.InputName)
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{

			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				
			}

			if( sInputMsg.IndexOf("Confirm") >= 0 || Input.GetKeyDown(KeyCode.Return))
			{
/*
				menuState = JSK_MainMenuState.SettingNickName;
//				menuNickname.transform.FindChild("TextNickName").GetComponent<TextMesh>().text = JSK_GlobalProcess.g_PlayerNickName ;
				WebServerProcess.SetNickName(JSK_GlobalProcess.g_PlayerNickName);
 */ 
			}
/*			else if( sInputMsg.IndexOf("Esc") >= 0 || Input.GetKeyDown(KeyCode.Escape))
			{
				menuNickname.transform.FindChild("TextNickName").GetComponent<TextMesh>().text = sInputMsg ;
			}
*/
		}
		else if( menuState == JSK_MainMenuState.Login )
		{
			if( JSK_GlobalProcess.g_IsLogin )
			{
				UnityEngine.Debug.Log("####Already login.");
				menuState = JSK_MainMenuState.Normal;
				return;
			}
/*
			if( WebServerProcess.ServerResult == enumServerResult.Uploading )
			{
				UnityEngine.Debug.Log("Login processing!!!");
			}
			else if( WebServerProcess.ServerResult == enumServerResult.Succeed )
			{
				UnityEngine.Debug.Log("Login success!!!");
				JSK_GlobalProcess.g_IsLogin = true;
				//
				ConfigManager.SaveConfig();
				if( WebServerProcess.ResultStatus == 0 )//已經註冊過
				{
					menuState = JSK_MainMenuState.Normal;
				}
				else if( WebServerProcess.ResultStatus == 1 )//新帳號
				{
					WebServerProcess.SetNickName("1234");
					menuState = JSK_MainMenuState.Normal;
				}
				else if( WebServerProcess.ResultStatus == 99 )//新帳號
				{
					WebServerProcess.SetNickName("1234");
					menuState = JSK_MainMenuState.Normal;					
				}
			}
			else if( WebServerProcess.ServerResult == enumServerResult.Error )
			{
                WebServerProcess.ServerResult = enumServerResult.None;
				UnityEngine.Debug.Log("Login error!!!" + WebServerProcess.ResultStatus );
			}
 		}
		else if( menuState == JSK_MainMenuState.SettingNickName )
		{
			if( WebServerProcess.ServerResult == enumServerResult.Uploading )
			{
				UnityEngine.Debug.Log("Setting nickname processing!!!");
			}
			else if( WebServerProcess.ServerResult == enumServerResult.Succeed )
			{
				UnityEngine.Debug.Log("Setting nickname success!!!");
				menuState = JSK_MainMenuState.Normal;
//				menuNickname.SetActive(false);
				menuButtons.SetActive(true);
			}
			else if( WebServerProcess.ServerResult == enumServerResult.Error )
			{
				UnityEngine.Debug.Log("Setting nickname error!!!" + WebServerProcess.ResultStatus );
			}
*/     }			
	}
	
	void onMenuEsc()
	{
		JSK_GlobalProcess.ExitGame();
	}
	
	void onMenuSelect()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
		{
			if( menuIndex == 0 )
			{
			    playMenuAnim(menuRoot, "MainMenu", -menuAnimSpeed, false, WrapMode.Once);
                nextSceneName = "UI_Motion";
			}
			else if( menuIndex == 1 )
			{
				playMenuAnim(menuRoot, "MainMenu", -menuAnimSpeed, false, WrapMode.Once);
				nextSceneName = "JSK_OptionMenu";
			}
			else if( menuIndex == 2 )
			{
				JSK_GlobalProcess.ExitGame();
			}
		}
		else
		{
			if( menuIndex == 0 )
			{
				playMenuAnim(menuRoot, "MainMenu4buttonTitle", -menuAnimSpeed, false, WrapMode.Once);

                nextSceneName = "JSK_CharacterMenu";
				JSK_GlobalProcess.g_IsMultipleRound = true;
	/*			
				JSK_GameProcess.GameMode = 1;
     */
				JSK_GlobalProcess.g_SelectCharStep = 0;
				JSK_GlobalProcess.g_SelectMotoStep = 0;
			
				UnityEngine.Debug.Log("JSK_GlobalProcess.g_SelectCharStep1:" + JSK_GlobalProcess.g_SelectCharStep);
			}
			else if( menuIndex == 2 )
			{
				if( JSK_GlobalProcess.g_IsMultiPlayer )
				{
					playMenuAnim(menuRoot, "MainMenu4buttonTitle", -menuAnimSpeed, false, WrapMode.Once);					
					nextSceneName = "JSK_ThunderRankMenu";//20140918啟用,新版排行榜(江南,東京地下神殿,水都,極地,阿茲特克)		
				}
				else
				{
					playMenuAnim(menuRoot, "MainMenu4buttonTitle", -menuAnimSpeed, false, WrapMode.Once);					
					nextSceneName = "JSK_ThunderRankMenu";//20140918啟用,新版排行榜(江南,東京地下神殿,水都,極地,阿茲特克)		
				}
			}
			else if( menuIndex == 3 )
			{
			    JSK_GlobalProcess.ExitGame();
			}
			else if( menuIndex == 1 )
			{
				playMenuAnim(menuRoot, "MainMenu4buttonTitle", -menuAnimSpeed, false, WrapMode.Once);

                nextSceneName = "JSK_MarketMenu";
			}
		}
		
	}

	private string setBestTime( int bestTime )
	{
		int millisecond = bestTime%100;
		int second = bestTime/100;
		
		int minuteTime = second / 60;
		int sceondTime = second % 60;
		
		return minuteTime.ToString() + ":" + sceondTime.ToString() + ":" + millisecond.ToString();
	}
	void SetMainMenuButtonState(JSK_ENUM_MAIN_MENU_BUTTON_STATE state, int iIndex)
	{
		//default state
		switch( state )
		{
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.NONE: break;
			case JSK_ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE:
			{
				if (iIndex == 0) { menuPlayGame[0].SetActive(true); menuPlayGame[1].SetActive(false); menuPlayGame[2].SetActive(false); }
				if (iIndex == 1) { menuUpgrade[0].SetActive(true);  menuUpgrade[1].SetActive(false);  menuUpgrade[2].SetActive(false); }
				if (iIndex == 2) { menuRanking[0].SetActive(true);  menuRanking[1].SetActive(false);  menuRanking[2].SetActive(false); }
				if (iIndex == 3) { menuExitGame[0].SetActive(true); menuExitGame[1].SetActive(false); menuExitGame[2].SetActive(false); }
				break;
			}
		case JSK_ENUM_MAIN_MENU_BUTTON_STATE.MOUSE_IN:
			{
				if (iIndex == 0) { menuPlayGame[0].SetActive(false); menuPlayGame[1].SetActive(true); menuPlayGame[2].SetActive(false); }
				if (iIndex == 1) { menuUpgrade[0].SetActive(false);  menuUpgrade[1].SetActive(true);  menuUpgrade[2].SetActive(false); }
				if (iIndex == 2) { menuRanking[0].SetActive(false);  menuRanking[1].SetActive(true);  menuRanking[2].SetActive(false); }
				if (iIndex == 3) { menuExitGame[0].SetActive(false); menuExitGame[1].SetActive(true); menuExitGame[2].SetActive(false); }
				break;
			}
		case JSK_ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN:
			{
				if (iIndex == 0) { menuPlayGame[0].SetActive(false); menuPlayGame[1].SetActive(false); menuPlayGame[2].SetActive(true); }
				if (iIndex == 1) { menuUpgrade[0].SetActive(false);  menuUpgrade[1].SetActive(false);  menuUpgrade[2].SetActive(true); }
				if (iIndex == 2) { menuRanking[0].SetActive(false);  menuRanking[1].SetActive(false);  menuRanking[2].SetActive(true); }
				if (iIndex == 3) { menuExitGame[0].SetActive(false); menuExitGame[1].SetActive(false); menuExitGame[2].SetActive(true); }
				break;
			}
		default:break ;
		}
	}
}
//
public enum JSK_ENUM_MAIN_MENU_BUTTON_STATE
{
	NONE,
	ACTIVE,
	MOUSE_IN,
	PRESS_DOWN
}
