using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System ;//for BitConverter
public class JSK_MotoUpgradeMenuProcess : MonoBehaviour
{
	public  Transform 	uiCamera			= null;	//射线查询相机.
	public  Transform 	menuRoot       		= null; //菜单根节点.
	public  Transform 	MotoMenuRoot   		= null; //車選單根跟節.
	public  Transform 	LevelUpMenuRoot		= null; //雜項選單根跟節.
	
	private float 		lastMoveTime		= 0;	//上次移动菜单的时间.
	private float 		moveDelayTime		= 0.0f;	//移动菜单的间隔时间,如果不需要延迟,就把这个改成0.
	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 1.1f; //菜单动画的播放速度.
	private float		lastButtonTime		= 0.0f ;//按鈕壓下的間隔時間
	private bool		IsButtonPressDown   = false;//按鈕壓下旗標

	private int 		maxMotoNum     		= 0;  	//最大的摩托数量.
	private int 		curMotoIndex   		= 0;  	//当前的摩托索引.curMotoIndex = (Row * 3 + Column) ;
	private int 		iCurrentMotoIndexRow= 0 ;	//當前的摩托橫方向索引,換車,001->004->007
	private int 		iCurrentMotoIndexColumn=0;	//當前的摩托直方向索引,換色,001->002->003
	private int			iMaxMotoNumberRow   = 3 ;	//橫方向最大的摩托數量.
	private int			iMaxMotoNumberColumn= 3 ;	//直方向最大的摩托數量.

	private GameObject 	leftArrowMenu		= null;	//左箭头壓下.
	private GameObject 	rightArrowMenu		= null;	//右箭头壓下.
	private GameObject 	upArrowMenu			= null;	//上箭頭壓下.
	private GameObject 	downArrowMenu		= null;	//下箭頭壓下.
	private GameObject 	leftArrowActive		= null;	//左箭頭.
	private GameObject 	rightArrowActive	= null;	//右箭頭.
	private GameObject 	upArrowActive		= null;	//上箭頭.
	private GameObject 	downArrowActive		= null;	//下箭頭.
	private GameObject 	nextButtonMenu		= null;	//确认键.(B)商城
	private GameObject 	backButtonMenu		= null;	//返回键.(X)返回
	private GameObject 	UpgradeButtonMenu	= null;	//購買键.(A)升級
	//moto upgrade state
	private GameObject 	UpgradeStarMenu1	= null ;//升級介面一顆星;顯示快艇目前升級的狀態(0級不亮星星，3級亮3顆星)滿足3星之後才可以解鎖下一級的快艇。(條件1)
	private GameObject 	UpgradeStarMenu2	= null ;//升級介面兩顆星;//同上
	private GameObject 	UpgradeStarMenu3	= null ;//升級介面參顆星;//同上
	private GameObject 	UpgradeBadgeMenu1	= null ;//升級介面徽章金
	private GameObject 	UpgradeBadgeMenu2	= null ;//升級介面徽章銅
	private GameObject 	UpgradeBadgeMenu3	= null ;//升級介面徽章紫
	private GameObject 	UpgradeBuyCar1		= null ;//升級介面已購買左1 (三狀態互斥:可購買/已購買/不能買)(五系列:動力/控制/噴射/資料片/吱廖騙)
	private GameObject 	UpgradeBuyCar2		= null ;//升級介面已購買左2
	private GameObject 	UpgradeBuyCar3		= null ;//升級介面已購買中3
	private GameObject 	UpgradeBuyCar4		= null ;//升級介面已購買右4
	private GameObject 	UpgradeBuyCar5		= null ;//升級介面已購買右5
	private GameObject 	UpgradeCanBuy1		= null ;//升級介面可購買左1
	private GameObject 	UpgradeCanBuy2		= null ;//升級介面可購買左2
	private GameObject 	UpgradeCanBuy3		= null ;//升級介面可購買中3
	private GameObject 	UpgradeCanBuy4		= null ;//升級介面可購買右4
	private GameObject 	UpgradeCanBuy5		= null ;//升級介面可購買右5
	private GameObject 	UpgradeSuspend1		= null ;//升級介面不能買左1
	private GameObject 	UpgradeSuspend2		= null ;//升級介面不能買左2
	private GameObject 	UpgradeSuspend3		= null ;//升級介面不能買中3
	private GameObject 	UpgradeSuspend4		= null ;//升級介面不能買右4
	private GameObject 	UpgradeSuspend5		= null ;//升級介面不能買右5
	private GameObject[] UpgradeBuyCarStar	;		//升級介面五系列三顆星 (五系列:0級不亮星星，3級亮3顆星)
	private GameObject[] UpgradeCarStarBackground  ;//升級介面五系列三顆星的背景(三顆空星)
	private GameObject 	UpgradeSquares1		= null ;//升級介面背景小車左1
	private GameObject 	UpgradeSquares2		= null ;//升級介面背景小車左2
	private GameObject 	UpgradeSquares3		= null ;//升級介面背景小車中3
	private GameObject 	UpgradeSquares4		= null ;//升級介面背景小車右4
	private GameObject 	UpgradeSquares5		= null ;//升級介面背景小車右5
	private GameObject[] UpgradeCash		;		//升級介面消費金額 
	private GameObject[] UpgradeLevel		;		//升級介面Level;顯示目前玩家的等級及經驗值條。玩家等級滿足之後可以解鎖快艇。(條件2)		
	private GameObject[] UpgradeMoney		;		//升級介面Money;玩家購買快艇需要付款
	private GameObject[] UpgradeExp			;		//升級介面Exp  ;顯示目前玩家的等級及經驗值條。
	//
	private ArrayList	motoList 			= new ArrayList();
	
	private float 		scrollSpeed			= 0.3f;
	private float 		curVal        		= 0.0f;
	
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
		
		playMenuAnim(MotoMenuRoot, "CharacterMenu", menuAnimSpeed, false, WrapMode.Once);
		onMotoMove(0) ;	
	}
	
	void Update()
	{
		for( int i = 0; i < motoList.Count; i++ )
		{
			GameObject motoObj = motoList[i] as GameObject;
			motoObj.transform.Rotate(0, Time.deltaTime * 60.0f, 0);
		}
		
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
		
		curVal += Time.deltaTime * scrollSpeed;
		if( curVal > 1.0f )
	    	curVal = 0.0f;
		
		if( JSK_GlobalProcess.g_UseTouchMenu )
			checkTouchInput();

		checkFifoMenu();
		if (IsButtonPressDown) SetRestoreButtonState() ;
	}
	   
	void initMenu()
	{
		nextButtonMenu = LevelUpMenuRoot.Find("shopping/tr_mk_mu_bkground_06_46").gameObject;
		backButtonMenu = menuRoot.Find("tr_mo_mu_button_x").gameObject;
		UpgradeButtonMenu= LevelUpMenuRoot.Find("tr_mo_mu_button_a002").gameObject;
		leftArrowMenu  = LevelUpMenuRoot.Find("arrow_b/tr_mo_mu_carlevel_arrow_b04").gameObject;//表示玩家還可以左右切換快艇的ICON
		rightArrowMenu = LevelUpMenuRoot.Find("arrow_b/tr_mo_mu_carlevel_arrow_b02").gameObject;//表示玩家還可以左右切換快艇的ICON
		upArrowMenu    = LevelUpMenuRoot.Find("arrow_b/tr_mo_mu_carlevel_arrow_b01").gameObject;//提示玩家往上下可以切換系列
		downArrowMenu  = LevelUpMenuRoot.Find("arrow_b/tr_mo_mu_carlevel_arrow_b03").gameObject;//提示玩家往上下可以切換系列
		leftArrowMenu.SetActive(false);
		rightArrowMenu.SetActive(false);
		upArrowMenu.SetActive(false);
		downArrowMenu.SetActive(false);
		//
		leftArrowActive		= LevelUpMenuRoot.Find("arrow_a/tr_mo_mu_carlevel_arrow_a04").gameObject;
		rightArrowActive	= LevelUpMenuRoot.Find("arrow_a/tr_mo_mu_carlevel_arrow_a02").gameObject;
		upArrowActive		= LevelUpMenuRoot.Find("arrow_a/tr_mo_mu_carlevel_arrow_a01").gameObject;
		downArrowActive		= LevelUpMenuRoot.Find("arrow_a/tr_mo_mu_carlevel_arrow_a03").gameObject;
		//moto upgrade state
		UpgradeStarMenu1	= LevelUpMenuRoot.Find("star_b/tr_mo_mu_carlevel_star_b01").gameObject;
		UpgradeStarMenu2	= LevelUpMenuRoot.Find("star_b/tr_mo_mu_carlevel_star_b02").gameObject;
		UpgradeStarMenu3	= LevelUpMenuRoot.Find("star_b/tr_mo_mu_carlevel_star_b03").gameObject;
		UpgradeBadgeMenu1	= LevelUpMenuRoot.Find("badge/tr_mo_mu_carlevel_badge01").gameObject;
		UpgradeBadgeMenu2	= LevelUpMenuRoot.Find("badge/tr_mo_mu_carlevel_badge02").gameObject;
		UpgradeBadgeMenu3	= LevelUpMenuRoot.Find("badge/tr_mo_mu_carlevel_badge03").gameObject;
		UpgradeBuyCar1		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_01").gameObject;
		UpgradeBuyCar2		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_02").gameObject;
		UpgradeBuyCar3		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_03").gameObject;
		UpgradeBuyCar4		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_04").gameObject;
		UpgradeBuyCar5		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_05").gameObject;
		UpgradeCanBuy1		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_a_01").gameObject;
		UpgradeCanBuy2		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_a_02").gameObject;
		UpgradeCanBuy3		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_a_03").gameObject;
		UpgradeCanBuy4		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_a_04").gameObject;
		UpgradeCanBuy5		= LevelUpMenuRoot.Find("buy/tr_mk_mu_buy_a_05").gameObject;
		UpgradeSquares1		= LevelUpMenuRoot.Find("squares_car/tr_mk_mu_car_type_b01").gameObject;
		UpgradeSquares2		= LevelUpMenuRoot.Find("squares_car/tr_mk_mu_car_type_a01").gameObject;
		UpgradeSquares3		= LevelUpMenuRoot.Find("squares_car/tr_mk_mu_car_type_c01").gameObject;
		UpgradeSquares4		= LevelUpMenuRoot.Find("squares_car/tr_mk_mu_car_type_d01").gameObject;
		UpgradeSquares5		= LevelUpMenuRoot.Find("squares_car/tr_mk_mu_car_type_e01").gameObject;
		UpgradeSuspend1		= LevelUpMenuRoot.Find("squares_stop/tr_mk_mu_stop_01").gameObject;
		UpgradeSuspend2		= LevelUpMenuRoot.Find("squares_stop/tr_mk_mu_stop_02").gameObject;
		UpgradeSuspend3		= LevelUpMenuRoot.Find("squares_stop/tr_mk_mu_stop_03").gameObject;
		UpgradeSuspend4		= LevelUpMenuRoot.Find("squares_stop/tr_mk_mu_stop_04").gameObject;
		UpgradeSuspend5		= LevelUpMenuRoot.Find("squares_stop/tr_mk_mu_stop_05").gameObject;
		UpgradeCash = new GameObject[8] ;//8位數
		UpgradeCash[0]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a01").gameObject;
		UpgradeCash[1]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a02").gameObject;
		UpgradeCash[2]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a03").gameObject;
		UpgradeCash[3]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a04").gameObject;
		UpgradeCash[4]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a05").gameObject;
		UpgradeCash[5]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a06").gameObject;
		UpgradeCash[6]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a07").gameObject;
		UpgradeCash[7]		= LevelUpMenuRoot.Find("cash/tr_mo_mu_carlevel_cash_a08").gameObject;
		UpgradeLevel = new GameObject[3] ;//3位數
		UpgradeLevel[0]		= LevelUpMenuRoot.Find("lv/tr_mk_mu_bkground_06_1").gameObject;
		UpgradeLevel[1]		= LevelUpMenuRoot.Find("lv/tr_mk_mu_bkground_06_7").gameObject;
		UpgradeLevel[2]		= LevelUpMenuRoot.Find("lv/tr_mk_mu_bkground_06_10").gameObject;	
		UpgradeMoney = new GameObject[10] ;//10位數
		UpgradeMoney[0]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_23").gameObject;
		UpgradeMoney[1]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_24").gameObject;
		UpgradeMoney[2]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_25").gameObject;
		UpgradeMoney[3]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_26").gameObject;
		UpgradeMoney[4]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_27").gameObject;
		UpgradeMoney[5]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_28").gameObject;
		UpgradeMoney[6]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_29").gameObject;
		UpgradeMoney[7]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_30").gameObject;
		UpgradeMoney[8]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_31").gameObject;
		UpgradeMoney[9]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_32").gameObject;
		UpgradeExp = new GameObject[10] ;//10位數
		UpgradeExp[0]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_12").gameObject;
		UpgradeExp[1]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_13").gameObject;
		UpgradeExp[2]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_14").gameObject;
		UpgradeExp[3]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_15").gameObject;
		UpgradeExp[4]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_16").gameObject;
		UpgradeExp[5]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_17").gameObject;
		UpgradeExp[6]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_18").gameObject;
		UpgradeExp[7]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_19").gameObject;
		UpgradeExp[8]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_20").gameObject;
		UpgradeExp[9]		= LevelUpMenuRoot.Find("Group_$$/tr_mk_mu_bkground_06_21").gameObject;
		UpgradeBuyCarStar = new GameObject[15] ;//5系列3顆星最多15顆
		UpgradeBuyCarStar[0] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a01").gameObject;
		UpgradeBuyCarStar[1] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a02").gameObject;
		UpgradeBuyCarStar[2] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a03").gameObject;
		UpgradeBuyCarStar[3] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a04").gameObject;
		UpgradeBuyCarStar[4] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a05").gameObject;
		UpgradeBuyCarStar[5] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a06").gameObject;
		UpgradeBuyCarStar[6] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a07").gameObject;
		UpgradeBuyCarStar[7] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a08").gameObject;
		UpgradeBuyCarStar[8] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a09").gameObject;
		UpgradeBuyCarStar[9] = LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a10").gameObject;
		UpgradeBuyCarStar[10]= LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a11").gameObject;
		UpgradeBuyCarStar[11]= LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a12").gameObject;
		UpgradeBuyCarStar[12]= LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a13").gameObject;
		UpgradeBuyCarStar[13]= LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a14").gameObject;
		UpgradeBuyCarStar[14]= LevelUpMenuRoot.Find("star_d/tr_mk_mu_star_a15").gameObject;
		UpgradeCarStarBackground = new GameObject[5] ;//5系列星數背景
		UpgradeCarStarBackground[0] = LevelUpMenuRoot.Find("star_c/tr_mk_mu_star_01").gameObject;
		UpgradeCarStarBackground[1] = LevelUpMenuRoot.Find("star_c/tr_mk_mu_star_02").gameObject;
		UpgradeCarStarBackground[2] = LevelUpMenuRoot.Find("star_c/tr_mk_mu_star_03").gameObject;
		UpgradeCarStarBackground[3] = LevelUpMenuRoot.Find("star_c/tr_mk_mu_star_04").gameObject;
		UpgradeCarStarBackground[4] = LevelUpMenuRoot.Find("star_c/tr_mk_mu_star_05").gameObject;

		if( JSK_GlobalProcess.g_UseNewActor )
		{
            //20141020.按這順序 by JSK_ENUM_MOTO_LIST
            motoList.Add(GameObject.Find("tr_moto_007"));//軍武1級
            motoList.Add(GameObject.Find("tr_moto_008"));//軍武2級
            motoList.Add(GameObject.Find("tr_moto_009"));//軍武3級
            motoList.Add(GameObject.Find("tr_moto_004"));//重機1級
            motoList.Add(GameObject.Find("tr_moto_005"));//重機2級
            motoList.Add(GameObject.Find("tr_moto_006"));//重機3級
            motoList.Add(GameObject.Find("tr_moto_003"));//未來1級
            motoList.Add(GameObject.Find("tr_moto_002"));//未來2級
            motoList.Add(GameObject.Find("tr_moto_001"));//未來3級
		}
		else
		{
			GameObject.Find("MotoNew").SetActiveRecursively(false);
			motoList.Add(GameObject.Find("Ac_Moto_01"));
			motoList.Add(GameObject.Find("Ac_Moto_02"));
			motoList.Add(GameObject.Find("Ac_Moto_03"));
			motoList.Add(GameObject.Find("Ac_Moto_04"));
			motoList.Add(GameObject.Find("Ac_Moto_05"));
		}
		
		maxMotoNum = motoList.Count;
		
		changeMoto(-1);
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
				else if( hitMenu == UpgradeButtonMenu )
						onMenuUpgrade() ;
				else if( hitMenu == leftArrowActive )
						onMotoMove((int)ENUM_DIRECTION.LEFT);
				else if( hitMenu == rightArrowActive )
						onMotoMove((int)ENUM_DIRECTION.RIGHT);
				else if( hitMenu == upArrowActive )
						onMotoMove((int)ENUM_DIRECTION.UP);
				else if( hitMenu == downArrowActive )
						onMotoMove((int)ENUM_DIRECTION.DOWN);
	       }
	       RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
		   if (hit2D != null) 
           {
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if( hitMenu == nextButtonMenu )	onMenuSelect();
				}
		   }
	    }
	}
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
		if     ( sInputMsg.IndexOf("Left") >= 0)	onMotoMove((int)ENUM_DIRECTION.LEFT);//onMotoMove(-1);
		else if( sInputMsg.IndexOf("Right") >= 0 )	onMotoMove((int)ENUM_DIRECTION.RIGHT);//onMotoMove(1);
		else if( sInputMsg.IndexOf("Up") >= 0 )		onMotoMove((int)ENUM_DIRECTION.UP);//onMotoMove(-1);
		else if( sInputMsg.IndexOf("Down") >= 0 )	onMotoMove((int)ENUM_DIRECTION.DOWN);//onMotoMove(1);
		else if( sInputMsg.IndexOf("Esc") >= 0)		onMenuEsc();
		else if( sInputMsg.IndexOf("Confirm") >= 0)	onMenuSelect();
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
			if( nextSceneName == "" )
				changeMoto(0);
		}
	}
	
	void onMenuSelect()
	{
		changeMoto(-1);
		playMenuAnim(MotoMenuRoot, "JSK_MainMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");
		nextSceneName = "JSK_MarketMenu";
	}


	
	void onMenuEsc()
	{
		changeMoto(-1);
		playMenuAnim(MotoMenuRoot, "JSK_MainMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");
		
		nextSceneName = "JSK_MainMenu";
	}

	void onMenuUpgrade()
	{
		changeMoto(-1);
		JSK_SoundProcess.PlaySound("MenuSelect");
		
		nextSceneName = "JSK_MotoTalentTreeMenu";
	}  
    //
	void notifyMenuState( GameObject hitMenu, JSK_MenuState state, int index )
	{
		hitMenu.GetComponent<JSK_MenuObject>().setMenuState(state, index);
	}
	
	void changeMoto( int index )
	{
		for( int i = 0; i < motoList.Count; i++ )
		{
			GameObject motoObj = motoList[i] as GameObject;
			
			MeshRenderer[] renderList = motoObj.GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];
			if( i == index )
			{
				foreach( MeshRenderer rend in renderList )
					rend.enabled = true;
			}
			else
			{
				foreach( MeshRenderer rend in renderList )
					rend.enabled = false;
			}
		}
	}
	
	void onMotoMove( int val )
	{
		if( Time.time < lastMoveTime )
			return;
	    lastMoveTime = Time.time + moveDelayTime;
	    
	    JSK_SoundProcess.PlaySound("MenuMove");
	    
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;

		if( val == (int)ENUM_DIRECTION.UP)
		{
			iCurrentMotoIndexColumn-- ;
			if (iCurrentMotoIndexColumn < 0) iCurrentMotoIndexColumn = iMaxMotoNumberColumn -1  ;
		}
		else if( val == (int)ENUM_DIRECTION.DOWN)
		{
			iCurrentMotoIndexColumn++ ;
			if (iCurrentMotoIndexColumn >= iMaxMotoNumberColumn) iCurrentMotoIndexColumn = 0 ;
		}
		else if( val == (int)ENUM_DIRECTION.LEFT)
		{
			iCurrentMotoIndexRow-- ;
			if (iCurrentMotoIndexRow < 0) iCurrentMotoIndexRow = 0 ;//跟據GDD:向左切換1輛快艇。不LOOP。
		}
		else if( val == (int)ENUM_DIRECTION.RIGHT)
		{
			iCurrentMotoIndexRow++ ;
			if (iCurrentMotoIndexRow >= iMaxMotoNumberRow) iCurrentMotoIndexRow = iMaxMotoNumberRow - 1 ;//跟據GDD:向右切換1輛快艇。不LOOP。鎖住的快艇不能選。
		}
		//
		curMotoIndex = iCurrentMotoIndexColumn * iMaxMotoNumberColumn + iCurrentMotoIndexRow ;
		//update from WebServer
		if (JSK_GlobalProcess.g_IsWebServer)
		{
/*
			//按鈕壓下
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BUTTON	,val		,1) ;
			//取得快艇資料
            int iBoatIndex = SubFunctionSelectedMoto(curMotoIndex) ;//1~9
            int iType = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevelX = ((iBoatIndex - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            string sOut = "";
            BoatData data = (BoatData)BoatManager.GetBoat(iType, iLevelX, out sOut);
			if (data == null) return ;
			JSK_MatchedRivalList.Instance().SetMotoUpgradeSelectedBoat(iBoatIndex) ;
			//data.Type = 0;  // 系列
			//data.Lv = 0;    // 等級
			//data.Name = ""; // 道具名稱
			//data.Model = "";//模型名稱
			//data.Icon = ""; //Icon名稱
			//data.Status = 0;// 狀態 0:無法購買 1:已購買 2:可購買 
			//data.Description = "";   // 道具說明
			//data.HP = 0;  // HP
			//data.LevelGRV = 0;  // 重力等級
			//data.LevelPOW = 0;  // 爆發力等級
			//data.LevelDEX = 0;  // 敏捷度等級
			//data.LevelSPD = 0;  // 速度等級
			//data.SellMoney = 0;  // 購買遊戲幣
			//data.SellDYMoney = 0;  // 購買動游幣
			//data.Status = 0; // 狀態 0:無法購買 1:已購買 2:可購買 
			//data.SellNeedLv; // 購買所需玩家等級
			//data.SellNeedID;  // 購買所需快艇
			//data.FoulCost1 = 0;  // 燃料花費(超級汽油)
			//data.FoulCost2 = 0;  // 燃料花費(高級汽油)
			//data.LineSpeedMax = 0; // 最高前進速度
			//data.CollideLoss = 0; // 碰撞速度損失
			//data.TurnSpeedMax = 0; // 最大轉向速度
			//data.TurnSpeedLoss = 0; // 轉向速度損失
			//data.TurnSpeedAcce = 0; // 轉彎加速度
			//data.BackSpeedMax = 0; // 倒車速度
			//data.Gravity = 0;  // 重力
			//data.DriftTurnSpeedMax = 0;//甩尾最大速度
			//data.DriftForce = 0;  // 甩尾力道
			//data.TurboPower = 0;  // 氮氣加速力道
			//data.TurboTime = 0;  // 氮氣加速持續時間
			//data.TurboBoradAcc = 0; // 加速板的加速度
			//data.JumpBoradAcc = 0; // 跳板的加速度
			//data.LowSpeedHelp = 0; // 增強加速限制
			//data.TurboAddTime = 0; // 氮氣累積時間
			//data.TurboCountMax = 0; // 氮氣最大值
			//data.TargetID = "";    //技能裝備快艇ID
			//data.ItemType = 0;  // 道具類別
			//快艇系列ICON	用來表示目前選擇的快艇是哪一系列的 (不是徽章啊...)
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BADGE,	data.Type	,0) ;
			//快艇星數 1~3星ICON	顯示快艇目前升級的狀態(0級不亮星星，3級亮3顆星) 滿足3星之後才可以解鎖下一級的快艇。(條件1)
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.STAR ,	data.Lv, 0) ;
			//快艇資料	用來顯示當前快艇的數值 (顯示內容及方式未定)
			TextMesh mTextMesh = menuRoot.FindChild("TextDescription").GetComponent<TextMesh>();
			mTextMesh.text = data.Name ;
			mTextMesh = menuRoot.FindChild("TextMotoHp_1").GetComponent<TextMesh>();
			mTextMesh.text = "HP:" + data.HP ;
			mTextMesh = menuRoot.FindChild("TextGravity_2").GetComponent<TextMesh>();
			mTextMesh.text = "重力等級:" + data.LevelGRV ;
			mTextMesh = menuRoot.FindChild("TextPower_3").GetComponent<TextMesh>();
			mTextMesh.text = "爆發力等級:" + data.LevelPOW ;
			mTextMesh = menuRoot.FindChild("TextDexterous_4").GetComponent<TextMesh>();
			mTextMesh.text = "敏捷度等級:" + data.LevelDEX ;
			mTextMesh = menuRoot.FindChild("TextSpeed_5").GetComponent<TextMesh>();
			mTextMesh.text = "速度等級:" + data.LevelSPD ;
			mTextMesh = menuRoot.FindChild("TextMotoIcon_7").GetComponent<TextMesh>();
			mTextMesh.text = "Icon:" + data.Icon ;
			mTextMesh = menuRoot.FindChild("TextMotoType_8").GetComponent<TextMesh>();
			mTextMesh.text = "Type:" + data.Type ;
			//消費金融
			int iCash = 0 ;
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.CASH		,iCash	,0) ;
			//玩家資料 WebServerProcess.User
			//user.UserID; //玩家代號
			//user.NickName; //暱稱
			//user.NttID; //裝置認證號
			//user.UserLevel; //等級
			//user.Exp; //經驗值
			//user.NowExp; //目前經驗值
			//user.NextExp; //下一級所需經驗值
			//user.Money; //動游幣
			//user.DYMoney; //動游幣
			//user.FixCount; //修理次數
			//取得玩家資料
			int iMoney = WebServerProcess.User.Money ;
			int iLevel = WebServerProcess.User.UserLevel ;
			int iExp   = WebServerProcess.User.Exp ;
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.LEVEL	,iLevel	,0) ;
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.MONEY	,iMoney ,0) ;
			SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.EXP		,iExp	,0) ;
			mTextMesh = menuRoot.FindChild("TextFixToolCount_6").GetComponent<TextMesh>();
			//mTextMesh.text = "修理工具數:" + WebServerProcess.User.FixCount ;
			//display same type boat
			int iTarget = data.Type ;
			int iCount  = 0 ;
			int[] iSeriesStatus = new int[5] ;//因為畫面上只有五個Icon,最多只取五個
			int[] iSeriesIcon   = new int[5] ;
			int[] iSeriesStar	= new int[5] ;
			for (int i = 0; i < BoatManager.BoatLists.Count; i++)
			{
				if (iCount >= 5) break ;//最多只取五個
				BoatData dataTemp = (BoatData)BoatManager.BoatLists[i];
				if (iTarget != dataTemp.Type) continue ;//同一系列
                iSeriesStatus[iCount] = dataTemp.Status;//狀態
				iSeriesIcon[iCount] = i ;
                iSeriesStar[iCount] = dataTemp.Lv;//星數
				iCount++ ;
			}
			for (int i = 0 ; i < 5  ; i++)
			{
				if (i < iCount)
				{
					//五車狀態: data.Status = 0; // 狀態 0:無法購買 1:已購買 2:可購買 
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.MOTO_BUY	,i		,iSeriesStatus[i]) ;
					//五車(小車)Icon
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.SQUARES	,i		,iSeriesIcon[i]) ;
					//五車星數
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.MINI_CAR_STAR,i	,iSeriesStar[i]) ;
				}
				else
				{
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.MOTO_BUY	,i		,0) ;//無法購買
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.SQUARES	,i		,-1);//
					SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.MINI_CAR_STAR,i	,0) ;//0星
				}
			}
*/      }  
	}
    //
    public int SubFunctionSelectedMoto(int iTarget)
    {
        int iValue = 0;
        switch (iTarget)
        {
            case 0: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_1; break;
            case 1: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_2; break;
            case 2: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_3; break;
            case 3: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_1; break;
            case 4: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_2; break;
            case 5: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_3; break;
            case 6: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_1; break;
            case 7: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_2; break;
            case 8: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_3; break;
            default: break;//what happen?
        }
        return iValue;
    }   
	//
	void SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE state, int iIndex , int iOption)
	{
		switch( state )
		{
			case JSK_ENUM_UPGRADE_STATE.NONE: break;
			case JSK_ENUM_UPGRADE_STATE.STAR:
			{
				//default 0 star
				if ((iIndex > 3) || (iIndex < 0)) iIndex = 0 ;
				if (iIndex == 0) {UpgradeStarMenu1.SetActive(false);	UpgradeStarMenu2.SetActive(false); UpgradeStarMenu3.SetActive(false); }
				if (iIndex == 1) {UpgradeStarMenu1.SetActive(true);		UpgradeStarMenu2.SetActive(false); UpgradeStarMenu3.SetActive(false); }
				if (iIndex == 2) {UpgradeStarMenu1.SetActive(true);		UpgradeStarMenu2.SetActive(true);  UpgradeStarMenu3.SetActive(false); }
				if (iIndex == 3) {UpgradeStarMenu1.SetActive(true);		UpgradeStarMenu2.SetActive(true);  UpgradeStarMenu3.SetActive(true); }
				break;
			}
			case JSK_ENUM_UPGRADE_STATE.BADGE:
			{
				//default
				if ((iIndex > 2) || (iIndex < 0)) break ;//out of range
				if (iIndex == 0) {UpgradeBadgeMenu1.SetActive(true);  UpgradeBadgeMenu2.SetActive(false); UpgradeBadgeMenu3.SetActive(false) ;}
				if (iIndex == 1) {UpgradeBadgeMenu2.SetActive(false); UpgradeBadgeMenu2.SetActive(true);  UpgradeBadgeMenu3.SetActive(false) ;}
				if (iIndex == 2) {UpgradeBadgeMenu3.SetActive(false); UpgradeBadgeMenu2.SetActive(false); UpgradeBadgeMenu3.SetActive(true) ;}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.MOTO_BUY:
			{	
				//default moto
				if ((iIndex > 4) || (iIndex < 0)) break ;//out of range
				//default status
				if ((iOption> 2) || (iOption< 0)) break ;//out of range
				// 狀態 0:無法購買 1:已購買 2:可購買 
				// iOption 0 = you can NOT buy , 1 = already buy , 2 = can buy
				if (iIndex == 0) 
				{	
					if (iOption == 1) {	UpgradeBuyCar1.SetActive(true);  UpgradeCanBuy1.SetActive(false); UpgradeSuspend1.SetActive(false); }//already buy					
					if (iOption == 2) {	UpgradeBuyCar1.SetActive(false); UpgradeCanBuy1.SetActive(true);  UpgradeSuspend1.SetActive(false); }//can buy
					if (iOption == 0) {	UpgradeBuyCar1.SetActive(false); UpgradeCanBuy1.SetActive(false); UpgradeSuspend1.SetActive(true); }//you can NOT buy
				}
				if (iIndex == 1) 
				{
					if (iOption == 1) {	UpgradeBuyCar2.SetActive(true);  UpgradeCanBuy2.SetActive(false); UpgradeSuspend2.SetActive(false);}//already buy					
					if (iOption == 2) {	UpgradeBuyCar2.SetActive(false); UpgradeCanBuy2.SetActive(true);  UpgradeSuspend2.SetActive(false);}//can buy
					if (iOption == 0) {	UpgradeBuyCar2.SetActive(false); UpgradeCanBuy2.SetActive(false); UpgradeSuspend2.SetActive(true); }//you can NOT buy
				}
				if (iIndex == 2) 
				{
					if (iOption == 1) {	UpgradeBuyCar3.SetActive(true);  UpgradeCanBuy3.SetActive(false); UpgradeSuspend3.SetActive(false);}//already buy					
					if (iOption == 2) {	UpgradeBuyCar3.SetActive(false); UpgradeCanBuy3.SetActive(true);  UpgradeSuspend3.SetActive(false);}//can buy
					if (iOption == 0) {	UpgradeBuyCar3.SetActive(false); UpgradeCanBuy3.SetActive(false); UpgradeSuspend3.SetActive(true); }//you can NOT buy
				}
				if (iIndex == 3) 
				{
					if (iOption == 1) {	UpgradeBuyCar4.SetActive(true);  UpgradeCanBuy4.SetActive(false); UpgradeSuspend4.SetActive(false);}//already buy					
					if (iOption == 2) {	UpgradeBuyCar4.SetActive(false); UpgradeCanBuy4.SetActive(true);  UpgradeSuspend4.SetActive(false);}//can buy
					if (iOption == 0) {	UpgradeBuyCar4.SetActive(false); UpgradeCanBuy4.SetActive(false); UpgradeSuspend4.SetActive(true); }//you can NOT buy
				}
				if (iIndex == 4) 
				{
					if (iOption == 1) {	UpgradeBuyCar5.SetActive(true);  UpgradeCanBuy5.SetActive(false); UpgradeSuspend5.SetActive(false);}//already buy					
					if (iOption == 2) {	UpgradeBuyCar5.SetActive(false); UpgradeCanBuy5.SetActive(true);  UpgradeSuspend5.SetActive(false);}//can buy
					if (iOption == 0) {	UpgradeBuyCar5.SetActive(false); UpgradeCanBuy5.SetActive(false); UpgradeSuspend5.SetActive(true); }//you can NOT buy
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.SQUARES:
			{
				//default moto
				if ((iIndex > 4) || (iIndex < 0)) break ;//out of range
				//default icon
				bool IsEnable = true ;
				if ((iOption> 9) || (iOption < 0)) IsEnable = false ;//out of range? 3x3台? 
				if (iIndex == 0) 
				{	
					UpgradeSquares1.SetActive(IsEnable);
					if (IsEnable == false) break ;
					UpgradeSquares1.GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				}
				if (iIndex == 1) 
				{
					UpgradeSquares2.SetActive(IsEnable);
					if (IsEnable == false) break ;
					UpgradeSquares2.GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				}
				if (iIndex == 2) 
				{	
					UpgradeSquares3.SetActive(IsEnable);
					if (IsEnable == false) break ;
					UpgradeSquares3.GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				}
				if (iIndex == 3)
				{	
					UpgradeSquares4.SetActive(IsEnable);
					if (IsEnable == false) break ;
					UpgradeSquares4.GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				}
				if (iIndex == 4)
				{	
					UpgradeSquares5.SetActive(IsEnable);
					if (IsEnable == false) break ;
					UpgradeSquares5.GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, iOption);
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.CASH:
			{
				string sResult = Convert.ToString(iIndex) ;
				int iMaxDigit = 8 ;//消費金額,位數上限為8位數
				for (int i = 0 ; i < iMaxDigit ; i++) UpgradeCash[i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[sResult.Length-1-i] ;//從此字串個位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//UpgradeCash[i]防當
					UpgradeCash[iMaxDigit-1-i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的個位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					UpgradeCash[iMaxDigit-1-iCount].SetActive(false);//把未顯示的位數Hide
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.LEVEL:
			{
				string sResult = Convert.ToString(iIndex) ;
				int iMaxDigit = 3 ;//Level,位數上限為10位數
				for (int i = 0 ; i < iMaxDigit ; i++) UpgradeLevel[i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[i] ;//從此字串最高位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//UpgradeLevel[i]防當
					UpgradeLevel[i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的最高位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					UpgradeLevel[iCount].SetActive(false);//把未顯示的位數Hide
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.MONEY:
			{
				string sResult = Convert.ToString(iIndex) ;
				int iMaxDigit = 10 ;//Level,位數上限為10位數
				for (int i = 0 ; i < iMaxDigit ; i++) UpgradeMoney[i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[i] ;//從此字串最高位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//UpgradeMoney[i]防當
					UpgradeMoney[i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的最高位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					UpgradeMoney[iCount].SetActive(false);//把未顯示的位數Hide
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.EXP:
			{
				string sResult = Convert.ToString(iIndex) ;
				int iMaxDigit = 10 ;//Level,位數上限為10位數
				for (int i = 0 ; i < iMaxDigit ; i++) UpgradeExp[i].SetActive(true);//reset
				int iCount = 0 ;
				for (int i = 0 ; i < sResult.Length ; i++)
				{
					string s = "" + sResult[i] ;//從此字串最高位數開始
					int vOut = int.Parse(s);
					if (i >= iMaxDigit) break ;//UpgradeExp[i]防當
					UpgradeExp[i].GetComponent<JSK_MenuObject>().setMenuState(JSK_MenuState.ENUM_SPRITE_INDEX, vOut);//從畫面上的最高位數開始
					iCount++ ;
				}
				for (/*    */; iCount < iMaxDigit ; iCount++)
				{
					UpgradeExp[iCount].SetActive(false);//把未顯示的位數Hide
				}
				break ;
			}
			case JSK_ENUM_UPGRADE_STATE.BUTTON:
			{
				if (iIndex == (int)ENUM_DIRECTION.LEFT) 
				{	
					if (iOption == 0) { leftArrowMenu.SetActive(false); leftArrowActive.SetActive(true); }//press button active
					if (iOption == 1) { leftArrowMenu.SetActive(true); leftArrowActive.SetActive(false); }//press button down
				}
				if (iIndex == (int)ENUM_DIRECTION.RIGHT) 
				{	
					if (iOption == 0) { rightArrowMenu.SetActive(false); rightArrowActive.SetActive(true); }//press button active
					if (iOption == 1) { rightArrowMenu.SetActive(true); rightArrowActive.SetActive(false); }//press button down
				}
				if (iIndex == (int)ENUM_DIRECTION.UP) 
				{	
					if (iOption == 0) { upArrowMenu.SetActive(false); upArrowActive.SetActive(true); }//press button active
					if (iOption == 1) { upArrowMenu.SetActive(true); upArrowActive.SetActive(false); }//press button down
				}
				if (iIndex == (int)ENUM_DIRECTION.DOWN)
				{	
					if (iOption == 0) { downArrowMenu.SetActive(false); downArrowActive.SetActive(true); }//press button active
					if (iOption == 1) { downArrowMenu.SetActive(true); downArrowActive.SetActive(false); }//press button down
				}
				//
				if (iOption == 1) 
				{
					IsButtonPressDown = true ;
					lastButtonTime = Time.time + 0.1f ;
				}
				break;
			}
			case JSK_ENUM_UPGRADE_STATE.MINI_CAR_STAR:
			{
				//default moto
				if ((iIndex > 4) || (iIndex < 0)) break ;//out of range				
				//default 3 star
				bool IsEnable = true ;
				if ((iOption > 3) || (iOption < 0)) { IsEnable = false ; iOption = 0 ;}
				//
				UpgradeCarStarBackground[iIndex].SetActive(IsEnable);
				int i = iIndex * 3 ;				
				if (iOption == 0) {UpgradeBuyCarStar[i+0].SetActive(false);	UpgradeBuyCarStar[i+1].SetActive(false); UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 1) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(false); UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 2) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(true);  UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 3) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(true);  UpgradeBuyCarStar[i+2].SetActive(true);  }
				break;
			}
			default:break ;
		}
	}
	//
	void SetRestoreButtonState()
	{
		if( Time.time < lastButtonTime ) return;
		IsButtonPressDown = false ;
		if( leftArrowMenu.activeSelf == true)	SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BUTTON	,(int)ENUM_DIRECTION.LEFT,	0) ;
		if( rightArrowMenu.activeSelf == true)	SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BUTTON	,(int)ENUM_DIRECTION.RIGHT,	0) ;
		if( upArrowMenu.activeSelf == true )	SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BUTTON	,(int)ENUM_DIRECTION.UP,	0) ;
		if( downArrowMenu.activeSelf == true)	SetMotoUpgradeState(JSK_ENUM_UPGRADE_STATE.BUTTON	,(int)ENUM_DIRECTION.DOWN,	0) ;
		//
		changeMoto(curMotoIndex);
	}
}
//
public enum JSK_ENUM_UPGRADE_STATE
{
	NONE,
	STAR,
	BADGE,
	MOTO_BUY,
	SQUARES,
	CASH,
	LEVEL,
	MONEY,
	EXP,
	BUTTON,
	MINI_CAR_STAR,
	MAX
}
