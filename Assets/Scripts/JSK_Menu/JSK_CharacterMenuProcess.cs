using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSK_CharacterMenuProcess : MonoBehaviour
{
	public  Transform 	uiCamera			= null;	//射线查询相机.
	public  Transform 	menuRoot       		= null; //菜单根节点.
	public  Transform 	menuOtherRoot     	= null; //雜項物件根節點.
	
	private float 		lastMoveTime		= 0;	//上次移动菜单的时间.
	private float 		moveDelayTime		= 0.0f;	//移动菜单的间隔时间,如果不需要延迟,就把这个改成0.
	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 1.1f; //菜单动画的播放速度.
	private float		lastButtonTime		= 0.0f ;//按鈕壓下的間隔時間
	private bool		IsButtonPressDown   = false;//按鈕壓下旗標

	private int 		maxCharacterNum     = 0;  	//最大的人物数量.
	private int 		curCharacterIndex   = 0;  	//当前的人物索引.
	
	private GameObject 	characterNameMenu	= null;	//角色姓名.
	private GameObject 	leftArrowMenu		= null;	//左箭头.
	private GameObject 	rightArrowMenu		= null;	//右箭头.
	private GameObject 	nextButtonMenu		= null;	//确认键.
	private GameObject 	backButtonMenu		= null;	//返回键.
	
	private ArrayList	characterList 		= new ArrayList();
	
	private GameObject 	titleBar1			= null;
	private GameObject 	titleBar2			= null;
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
		
		playMenuAnim(menuRoot, "CharacterMenu", menuAnimSpeed, false, WrapMode.Once);
		
		menuRoot.Find("TypeText").renderer.enabled = false;
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
		leftArrowMenu  = menuOtherRoot.Find("tr_mo_mu_arrow_l01").gameObject;
		rightArrowMenu = menuOtherRoot.Find("tr_mo_mu_arrow_r01").gameObject;
		nextButtonMenu = menuOtherRoot.Find("tr_mo_mu_button_a001").gameObject;
		backButtonMenu = menuOtherRoot.Find("tr_mo_mu_button_x").gameObject;
		if( JSK_GlobalProcess.g_UseNewActor )
		{
//			GameObject.Find("Actor").SetActiveRecursively(false);//20141016remove.
			characterList.Add(GameObject.Find("Male"));
			characterList.Add(GameObject.Find("Female"));
			
			characterNameMenu = menuRoot.Find("NameTextNew").gameObject;
			menuRoot.Find("NameText").renderer.enabled = false;
		}
		else
		{
			GameObject.Find("ActorNew").SetActiveRecursively(false);
			characterList.Add(GameObject.Find("Jim"));
			characterList.Add(GameObject.Find("Aiko"));
			characterList.Add(GameObject.Find("Charles"));
			characterList.Add(GameObject.Find("Giselie"));
			characterList.Add(GameObject.Find("Karl"));
			
			characterNameMenu = menuRoot.Find("NameText").gameObject;
			menuRoot.Find("NameTextNew").renderer.enabled = false;
		}
		
		maxCharacterNum = characterList.Count;
		
		for( int i = 0; i < characterList.Count; i++ )
		{
			GameObject characterObj = characterList[i] as GameObject;
			string animName = "";
			if( JSK_GlobalProcess.g_UseNewActor )
				animName = "idle";
			else
				animName = characterObj.name + "_idle";
			AnimationState anim = characterObj.animation[animName];
			anim.wrapMode = WrapMode.Loop;
			characterObj.animation.Play(animName);
		}
		
		changeCharacter(-1);
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
            	if( hitMenu == nextButtonMenu )		onMenuSelect();
		       	else if( hitMenu == backButtonMenu )onMenuEsc();
            }
			RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2D != null) 
			{
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if( hitMenu == leftArrowMenu )	onCharacterMove(-1);
					else if( hitMenu == rightArrowMenu)	onCharacterMove(1);
				}
			}
	    }
	}

    void checkFifoMenu()
    {
        string sInputMsg = JSK_GlobalProcess.GetFifoMsg();

        if (sInputMsg.Length > 0)
            Debug.Log("sInputMsg:" + sInputMsg);

        if (sInputMsg.IndexOf("Left") >= 0) onCharacterMove(-1);
        else if (sInputMsg.IndexOf("Right") >= 0) onCharacterMove(1);
        else if (sInputMsg.IndexOf("Up") >= 0) onCharacterMove(-1);
        else if (sInputMsg.IndexOf("Down") >= 0) onCharacterMove(1);
        else if (sInputMsg.IndexOf("Esc") >= 0) onMenuEsc();
        else if (sInputMsg.IndexOf("Confirm") >= 0) onMenuSelect();
    }
	
	void notifyMenuState( GameObject hitMenu, JSK_MenuState state, int index )
	{
		hitMenu.GetComponent<JSK_MenuObject>().setMenuState(state, index);
		
		if( hitMenu == nextButtonMenu || hitMenu == backButtonMenu )
		{
			hitMenu.renderer.material.mainTexture = 
				JSK_GUIProcess.GetLanguagePicture(hitMenu.renderer.material.mainTexture.name.Substring(0, hitMenu.renderer.material.mainTexture.name.Length-3));
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
				changeCharacter(0);
		}
	}
	
	void onMenuSelect()
	{
		characterNameMenu.renderer.enabled = false;
		changeCharacter(-1);
		playMenuAnim(menuRoot, "CharacterMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");

        JSK_GlobalProcess.Game1P = curCharacterIndex + 1;//加1处理.
        nextSceneName = "JSK_MotoMenu";
	}
	
	void onMenuEsc()
	{
		characterNameMenu.renderer.enabled = false;
		changeCharacter(-1);
		playMenuAnim(menuRoot, "CharacterMenu", -menuAnimSpeed, false, WrapMode.Once);
		JSK_SoundProcess.PlaySound("MenuSelect");
	
	    nextSceneName = "JSK_MainMenu";
	}
	
	void changeCharacter( int index )
	{
		for( int i = 0; i < characterList.Count; i++ )
		{
			GameObject characterObj = characterList[i] as GameObject;
			
			SkinnedMeshRenderer[] renderList = characterObj.GetComponentsInChildren<SkinnedMeshRenderer>() as SkinnedMeshRenderer[];
			if( i == index )
			{
				foreach( SkinnedMeshRenderer rend in renderList )
					rend.enabled = true;
			}
			else
			{
				foreach( SkinnedMeshRenderer rend in renderList )
					rend.enabled = false;
			}
		}
	}
	
	void onCharacterMove( int val )
	{
		if( Time.time < lastMoveTime )
			return;
	    lastMoveTime = Time.time + moveDelayTime;
	    
	    JSK_SoundProcess.PlaySound("MenuMove");
	    
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;
		
		if( val < 0 )
		{
			curCharacterIndex--;
			if( curCharacterIndex < 0 )
				curCharacterIndex = maxCharacterNum - 1;
		}
		else if( val > 0 )
		{
			curCharacterIndex++;
			if( curCharacterIndex > maxCharacterNum - 1 )
				curCharacterIndex = 0;
		}
		//
		if( val < 0 ) notifyMenuState(leftArrowMenu, JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		if( val > 0 ) notifyMenuState(rightArrowMenu,JSK_MenuState.ENUM_SPRITE_INDEX , 1);
		IsButtonPressDown = true ;
		lastButtonTime = Time.time + 0.10f ;
		//
		notifyMenuState(characterNameMenu, JSK_MenuState.TextureIndex, curCharacterIndex);
	}
	//
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
		changeCharacter(curCharacterIndex);
	}

}
