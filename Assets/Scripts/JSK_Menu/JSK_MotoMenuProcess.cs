using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ENUM_DIRECTION : int
{ 
	UP = 1 ,
	DOWN = 2 ,
	LEFT = 3 ,
	RIGHT = 4 
}
public class JSK_MotoMenuProcess : MonoBehaviour
{
	public  Transform 	uiCamera			= null;	//射线查询相机.
	public  Transform 	menuRoot       		= null; //菜单根节点.
	public  Transform 	MotoMenuRoot   		= null; //車選單根節點.
	public  Transform 	BoatMenuRoot		= null; //雜項選單根節點.
	
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

	private GameObject 	leftArrowMenu		= null;	//左箭头.
	private GameObject 	rightArrowMenu		= null;	//右箭头.
	private GameObject 	nextButtonMenu		= null;	//确认键.
	private GameObject 	backButtonMenu		= null;	//返回键.
	
	private ArrayList	motoList 			= new ArrayList();
	private GameObject	map_star_0			= null;
	private GameObject	map_star_1			= null;
	private GameObject	map_star_2			= null;
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
		nextButtonMenu = menuRoot.Find("tr_mo_mu_button_a001").gameObject;
		backButtonMenu = menuRoot.Find("tr_mo_mu_button_x").gameObject;
		leftArrowMenu  = menuRoot.Find("tr_mo_mu_arrow_l01").gameObject;
		rightArrowMenu = menuRoot.Find("tr_mo_mu_arrow_r01").gameObject;
		map_star_0	   = BoatMenuRoot.Find("star/tr_mo_mu_hardstar_a01").gameObject;
		map_star_1	   = BoatMenuRoot.Find("star/tr_mo_mu_hardstar_a02").gameObject;
		map_star_2	   = BoatMenuRoot.Find("star/tr_mo_mu_hardstar_a03").gameObject;
		map_star_0.SetActive(false);
		map_star_1.SetActive(false);
		map_star_2.SetActive(false);
		if( JSK_GlobalProcess.g_UseNewActor )
		{
			//20140827.Add This.車全換了.11台換9台.3種類3顏色
            //20141020.按這順序 by JSK_ENUM_MOTO_LIST
			motoList.Add(GameObject.Find("tr_moto_007"));
			motoList.Add(GameObject.Find("tr_moto_008"));
			motoList.Add(GameObject.Find("tr_moto_009"));
			motoList.Add(GameObject.Find("tr_moto_004"));
			motoList.Add(GameObject.Find("tr_moto_005"));
			motoList.Add(GameObject.Find("tr_moto_006"));
			motoList.Add(GameObject.Find("tr_moto_003"));
			motoList.Add(GameObject.Find("tr_moto_002"));
			motoList.Add(GameObject.Find("tr_moto_001"));
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
		//		notifyMenuState(hitMenu, JSK_MenuState.Down, 0);
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
					if( hitMenu == leftArrowMenu )	onMotoMove(-1);
					else if( hitMenu == rightArrowMenu)	onMotoMove(1);
				}
			}
	    }
	}
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
        if (sInputMsg.IndexOf("Left") >= 0) onMotoMove(-1);
        else if (sInputMsg.IndexOf("Right") >= 0) onMotoMove(1);
        else if (sInputMsg.IndexOf("Up") >= 0) onMotoMove(-1);
        else if (sInputMsg.IndexOf("Down") >= 0) onMotoMove(1);
        else if (sInputMsg.IndexOf("Esc") >= 0) onMenuEsc();
        else if (sInputMsg.IndexOf("Confirm") >= 0) onMenuSelect();
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
		playMenuAnim(MotoMenuRoot, "CharacterMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");

        JSK_GlobalProcess.GameMoto1P = SubFunctionSelectedMoto(curMotoIndex);
		nextSceneName = "JSK_StageMenu";
	}
	
	void onMenuEsc()
	{
		changeMoto(-1);
		playMenuAnim(MotoMenuRoot, "CharacterMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");
		
	    nextSceneName = "JSK_CharacterMenu";
	}
	
	void notifyMenuState( GameObject hitMenu, JSK_MenuState state, int index )
	{
		hitMenu.GetComponent<JSK_MenuObject>().setMenuState(state, index);
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

		if( val < 0 )
		{
			curMotoIndex--;
			if( curMotoIndex < 0 )
				curMotoIndex = maxMotoNum - 1;
		}
		else if( val > 0 )
		{
			curMotoIndex++;
			if( curMotoIndex > maxMotoNum - 1 )
				curMotoIndex = 0;
		}
		//
		if( val < 0 ) notifyMenuState(leftArrowMenu, JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		if( val > 0 ) notifyMenuState(rightArrowMenu,JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		IsButtonPressDown = true ;
		lastButtonTime = Time.time + 0.1f ;
		//update from WebServer
		if (JSK_GlobalProcess.g_IsWebServer)
		{		
/*
            int iBoatIndex = SubFunctionSelectedMoto(curMotoIndex);//1~9
            int iType = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevel = ((iBoatIndex - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            string sOut = "";
            BoatData data = (BoatData)BoatManager.GetBoat(iType, iLevel, out sOut);
            if (data == null) return;
			//data.Type = 0;  // 系列
			//data.Lv = 0;    // 等級
			//data.Name = ""; // 道具名稱
			//data.Model = "";//模型名稱
			//data.Icon = ""; //Icon名稱
			//data.Status = 0;// 狀態 0:無法購買 1:已購買 2:可購買 
			//data.Description = "";   // 道具說明
			//星數
			map_star_0.SetActive(false);
			map_star_1.SetActive(false);
			map_star_2.SetActive(false);
			if (data.Lv == 0)
			{
			}
			else if (data.Lv == 1)
			{
				map_star_0.SetActive(true);
			}
			else if (data.Lv == 2)
			{
				map_star_0.SetActive(true);
				map_star_1.SetActive(true);
			}
			else if (data.Lv == 3)
			{
				map_star_0.SetActive(true);
				map_star_1.SetActive(true);
				map_star_2.SetActive(true);
			}
			//快艇資料	用來顯示當前快艇的數值 (顯示內容及方式未定)
			TextMesh mTextMesh = BoatMenuRoot.FindChild("TextDescription").GetComponent<TextMesh>();
			mTextMesh.text = data.Name ;
			//目前的快艇(號)／全部的快艇(數)	用來顯示玩家目前有多少可以使用的快艇(分母) /	用來顯示玩家目前選用第幾輛快艇(分子) .
			mTextMesh = BoatMenuRoot.FindChild("TextCurrentMotoIndex").GetComponent<TextMesh>();
			mTextMesh.text = "" + (curMotoIndex+1) + " / " + maxMotoNum ;//當前/最大的摩托数量.
*/      }  
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
		changeMoto(curMotoIndex);
	}
}
