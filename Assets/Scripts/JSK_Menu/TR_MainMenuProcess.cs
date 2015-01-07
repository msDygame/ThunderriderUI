using UnityEngine;
using System.Collections;

public class TR_MainMenuProcess : MonoBehaviour 
{
    //for set state
    public Transform menuRoot;
    //mainmenu state
    protected GameObject[] menuMotion;  	//進入遊戲(藍底,綠底,紅底)/(可選,已選,壓下)
    protected GameObject[] menuMarket;		//進商城
    protected GameObject[] menuRanking;		//排行榜
    protected GameObject[] menuExitGame;    //離開遊戲
    //mainmenu button animation
    protected bool IsPlayingAnimation = false;
    protected string nextSceneName = "";  	//下一個場景的名字.
    void Awake()
    {
        JSK_GlobalProcess.InitGlobal();
        //mainmenu state
        //進入遊戲
        menuMotion = new GameObject[3];
        menuMotion[0] = menuRoot.Find("title/tr_mo_title_button_a01").gameObject;
        menuMotion[1] = menuRoot.Find("title/tr_mo_title_button_b01").gameObject;
        menuMotion[2] = menuRoot.Find("title/tr_mo_title_button_c01").gameObject;
        //進商城
        menuMarket = new GameObject[3];
        menuMarket[0] = menuRoot.Find("title/tr_mo_title_button_a02").gameObject;
        menuMarket[1] = menuRoot.Find("title/tr_mo_title_button_b02").gameObject;
        menuMarket[2] = menuRoot.Find("title/tr_mo_title_button_c02").gameObject;
        //排行榜
        menuRanking = new GameObject[3];
        menuRanking[0] = menuRoot.Find("title/tr_mo_title_button_a03").gameObject;
        menuRanking[1] = menuRoot.Find("title/tr_mo_title_button_b03").gameObject;
        menuRanking[2] = menuRoot.Find("title/tr_mo_title_button_c03").gameObject;
        //離開遊戲
        menuExitGame = new GameObject[3];
        menuExitGame[0] = menuRoot.Find("title/tr_mo_title_button_a04").gameObject;
        menuExitGame[1] = menuRoot.Find("title/tr_mo_title_button_b04").gameObject;
        menuExitGame[2] = menuRoot.Find("title/tr_mo_title_button_c04").gameObject;
    }
	// Use this for initialization
	void Start () 
    {
        JSK_GlobalProcess.InitGlobal();
        JSK_SoundProcess.PlayMusic("MainMenu");
        JSK_GlobalProcess.SetFifoScene(1, 1);
        JSK_GlobalProcess.SetFifoScene(2, 1);
        JSK_GlobalProcess.ClearFifoMessage();
        //MainMenu 4 Button animation
        playMenuAnim(1.0f);
        //
        JSK_SoundProcess.PlaySound("WaveRider");
	}	
	// Update is called once per frame
	void Update () 
    {
        if( IsPlayingAnimation == true )
		{
			updateUIAnim();
			return;
		}
		//
		if( nextSceneName != "" )
		{
			JSK_GlobalProcess.ChangeLevel(nextSceneName);
			return;
		}     
	}
    //
    void OnGUI()
    {
    }    
    //for menu state
    void SetStateForMenu(int iState , int iMenuMenuIndex)
    {
        //reset
        for (int i = 0; i < 3; i++)
        {
            menuMotion[i].SetActive(false);
            menuMarket[i].SetActive(false);
            menuRanking[i].SetActive(false);
            menuExitGame[i].SetActive(false);
        }
        //default:藍底/可選
        menuMotion[0].SetActive(true);
        menuMarket[0].SetActive(true);
        menuRanking[0].SetActive(true);
        menuExitGame[0].SetActive(true);
        //藍底關掉,綠底或紅底(iState)可選
        if (iMenuMenuIndex == (int)ENUM_SELECT_MAINMENU_STATE.SELECTED_MOTION)  { menuMotion[(int)ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE].SetActive(false); menuMotion[iState].SetActive(true); }
        if (iMenuMenuIndex == (int)ENUM_SELECT_MAINMENU_STATE.SELECTED_MARKET)  { menuMarket[(int)ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE].SetActive(false); menuMarket[iState].SetActive(true); }
        if (iMenuMenuIndex == (int)ENUM_SELECT_MAINMENU_STATE.SELECTED_RANKING) { menuRanking[(int)ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE].SetActive(false); menuRanking[iState].SetActive(true); }
        if (iMenuMenuIndex == (int)ENUM_SELECT_MAINMENU_STATE.SELECTED_QUIT)    { menuExitGame[(int)ENUM_MAIN_MENU_BUTTON_STATE.ACTIVE].SetActive(false); menuExitGame[iState].SetActive(true); }
    }
    //for Touch and playmaker only 1 parameter.....
    void SetStateForMenuTouchIn(int iMenuMenuIndex)
    {
        //default:紅底/TouchDown
        SetStateForMenu((int)ENUM_MAIN_MENU_BUTTON_STATE.PRESS_DOWN, iMenuMenuIndex);
    }
  
    //Mainmenu 4 button Animation
    void playMenuAnim(float speed)
    {
        AnimationState anim = menuRoot.animation["MainMenu4buttonTitle"];
        if (anim)
        {
            anim.speed = speed;
            anim.wrapMode = WrapMode.Once;//一次            
            //
            if (speed < 0)
                anim.time = anim.length;//rewind animation
            else
                anim.time = 0;//normal animation
            menuRoot.animation.Play("MainMenu4buttonTitle");
            IsPlayingAnimation = true;
        }
    }
    //checking Mainmenu 4 button Animation is playing
    void updateUIAnim()
    {
        if (!menuRoot.animation.IsPlaying("MainMenu4buttonTitle"))
        {
            IsPlayingAnimation = false;
            JSK_GlobalProcess.ClearFifoMessage();
        }
    }
}
