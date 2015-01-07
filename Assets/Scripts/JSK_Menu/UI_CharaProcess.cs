using UnityEngine;
using System.Collections;

public class UI_CharaProcess : MonoBehaviour
{
    //character motion
    protected int iCurrentMotionIndex = 0;//當前的動作索引,男順時針=1,男逆時針=2,女順時針=3,女逆時針=4 (male_clockwise=1,male_CounterClockwise=2,female_clockwise=3,female_CounterClockwise=4)
    protected int iCurrentCharacterGender = 1;//當前的性別,男=0,女=1 (male=0,Female=1)
    //For animator,因為playmaker沒有animator action選項...
    protected Animator animator;
    public Transform menuRoot;//for find Animator;assign use public gameObject 
    protected GameObject objectAnimator = null;
    protected AnimatorStateInfo currentState;
    protected AnimatorStateInfo previousState;
    //nickname state
    protected GameObject[] MenuNicknameError = null;//"此昵称已存在，" , "昵称包含不雅文字，" , "昵称建立失败，" , "昵称含有特殊字元，" , "昵称过长，"
    protected GameObject MenuNicknameMessage = null;//"請重新輸入"
    protected GameObject MenuNicknameConfirm = null;//"确认是否取此昵称?"
    //set nickname
    protected int iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.NONE ;
    void Awake()
    {
        JSK_GlobalProcess.InitGlobal();
        //
        objectAnimator = menuRoot.Find("character").gameObject;
        //
        MenuNicknameError = new GameObject[5];//錯誤訊息,共五個
        MenuNicknameError[0] = menuRoot.Find("TextErrorMessage1").gameObject;//"此昵称已存在，"
        MenuNicknameError[1] = menuRoot.Find("TextErrorMessage2").gameObject;//"昵称包含不雅文字，"
        MenuNicknameError[2] = menuRoot.Find("TextErrorMessage3").gameObject;//"昵称建立失败，"
        MenuNicknameError[3] = menuRoot.Find("TextErrorMessage4").gameObject;//"昵称含有特殊字元，"
        MenuNicknameError[4] = menuRoot.Find("TextErrorMessage5").gameObject;//"昵称过长，"
        MenuNicknameMessage  = menuRoot.Find("TextErrorRetry").gameObject;//"請重新輸入"
        MenuNicknameConfirm  = menuRoot.Find("TextConfirm").gameObject;//"确认是否取此昵称?"
        for (int i = 0; i < 5; i++) MenuNicknameError[i].SetActive(false);
        MenuNicknameMessage.SetActive(false);
        MenuNicknameConfirm.SetActive(false);
        //
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            //有沒有暱稱都要呼叫
            string LobbyTicket = "1234";//大廳Module 的ticket
            WebServerProcess.NTTLogin("TEST", LobbyTicket);
            iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.LOGIN;//step1
 */ 
        }
        else
            iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.NO_SERVER;
    }
	// Use this for initialization
	void Start () 
    {
        animator = objectAnimator.GetComponent<Animator>();	
	}	
	// Update is called once per frame
    void Update()
    {
        checkFifoMenu();
        //
        if (animator)
        {
            // "IsNext"flag is true , left or right button Pressed (當左鍵或右鍵壓下時)
            if (animator.GetBool("IsNext"))
            {
                // check state if state name is not same => already run next animaiton! reset "IsNext" flag = false
                currentState = animator.GetCurrentAnimatorStateInfo(0);
                if (currentState.IsName("Base Layer.Wait"))
                {
                    /* waiting , nothing happen */
                }
                else
                {
                    //playing animation changed! (當前動作名稱與之前動作名稱不一樣, => 換動作了)
                    if (previousState.nameHash != currentState.nameHash)
                    {
                        //reset flag "IsNext" (清除IsNext,不然動作會撥不停..)
                        animator.SetBool("IsNext", false);
                        previousState = currentState;
                    }
                }
            }
        }
    }
    //
    void OnGUI()
    {
        /*
        if (GUI.Button(new Rect(10, 10, 300, 60), "快速配對按鈕:女,車1,江南,一般油"))
        {
            JSK_GlobalProcess.Game1P = 2;
            JSK_GlobalProcess.GameMoto1P = 1;
            JSK_GlobalProcess.GameOil = 1;
            JSK_GlobalProcess.GamePlace = 2;
            Application.LoadLevel("JSK_MatchMenu");
        }
        if (GUI.Button(new Rect(10, 80, 300, 60), "快速配對按鈕:男,車7,阿茲特克,超級油"))
        {
            JSK_GlobalProcess.Game1P = 1;
            JSK_GlobalProcess.GameMoto1P = 7;
            JSK_GlobalProcess.GameOil = 3;
            JSK_GlobalProcess.GamePlace = 1;
            Application.LoadLevel("JSK_MatchMenu");
        }
        */
    }
    //壓下方向鍵
    void onCharacterMove(int val)
    {     
        if (val == (int)ENUM_DIRECTION.UP)//上
        {
            return;
        }
        else if (val == (int)ENUM_DIRECTION.DOWN)//下
        {
            return;
        }
        else if (val == (int)ENUM_DIRECTION.LEFT)//左
        {
            if      (iCurrentCharacterGender == 0) { iCurrentMotionIndex = 1; iCurrentCharacterGender = 1; }
            else if (iCurrentCharacterGender == 1) { iCurrentMotionIndex = 3; iCurrentCharacterGender = 0 ; }
        }
        else if (val == (int)ENUM_DIRECTION.RIGHT)//右
        {
            if      (iCurrentCharacterGender == 0) { iCurrentMotionIndex = 2; iCurrentCharacterGender = 1; }
            else if (iCurrentCharacterGender == 1) { iCurrentMotionIndex = 4; iCurrentCharacterGender = 0; }
        }
        else
        {
            return;
        }
        //
        if (iCurrentMotionIndex == 0) return;//happen?
        JSK_GlobalProcess.Game1P = iCurrentCharacterGender + 1;//加1處理.
        //MecAnima
        if (animator)
        {
            //設定物件上Animator變數，播放相對應動畫
            animator.SetInteger("iRotate", iCurrentMotionIndex);
            animator.SetBool("IsNext", true);//run the animation
        }
    }
    //
    void onSetNickname()
    {
        GameFifo.SendMsg(1, "PopTextInput"); //傳送給手機 叫出一般文字輸入
    }
    //
    void SetStateForNickname(int iIndex)
    {
        //伺服器端回傳值回傳值0:正常暱稱 1:暱稱變更成功 2:名稱重複 3:不雅文字 4:暱稱過長 5:有特殊符號 6:系統錯誤
        if (iIndex == 0) return;
        else if (iIndex == 1) return ;
        else if (iIndex == 2) MenuNicknameError[0].SetActive(true);//"此昵称已存在，"
        else if (iIndex == 3) MenuNicknameError[1].SetActive(true);//"昵称包含不雅文字，"
        else if (iIndex == 4) MenuNicknameError[4].SetActive(true);//"昵称过长，"
        else if (iIndex == 5) MenuNicknameError[3].SetActive(true);//"昵称含有特殊字元，"
        else if (iIndex == 6) MenuNicknameError[2].SetActive(true);//"昵称建立失败，"
        else return;
        MenuNicknameMessage.SetActive(true);//"請重新輸入"
    }
    //for Keyboard Input
    void checkFifoMenu()
    {
        string sInputMsg = JSK_GlobalProcess.GetFifoMsg();

        if (sInputMsg.Length > 0) Debug.Log("sInputMsg:" + sInputMsg);        
        //
        if (iNicknameState == (int)ENUM_NICKNAME_PIPELINE_STATE.FINISH)
        {
            //Application.LoadLevel("UI_DailyBonus");//20141114 Login後回傳似乎都是已注冊?
            //20141114@暫時接受全按鍵
            if (sInputMsg.IndexOf("Left") >= 0) onCharacterMove((int)ENUM_DIRECTION.LEFT);
            else if (sInputMsg.IndexOf("Right") >= 0) onCharacterMove((int)ENUM_DIRECTION.RIGHT);
            else if (sInputMsg.IndexOf("Up") >= 0) onCharacterMove((int)ENUM_DIRECTION.UP);
            else if (sInputMsg.IndexOf("Down") >= 0) onCharacterMove((int)ENUM_DIRECTION.DOWN);
            else if (sInputMsg.IndexOf("Esc") >= 0) onSetNickname();
            else if (sInputMsg.IndexOf("Back") >= 0) onSetNickname();
            else if (sInputMsg.IndexOf("Confirm") >= 0) Application.LoadLevel("UI_DailyBonus");
        }
        else
        {
            if (sInputMsg.IndexOf("Left") >= 0) onCharacterMove((int)ENUM_DIRECTION.LEFT);
            else if (sInputMsg.IndexOf("Right") >= 0) onCharacterMove((int)ENUM_DIRECTION.RIGHT);
            else if (sInputMsg.IndexOf("Up") >= 0) onCharacterMove((int)ENUM_DIRECTION.UP);
            else if (sInputMsg.IndexOf("Down") >= 0) onCharacterMove((int)ENUM_DIRECTION.DOWN);
            else if (sInputMsg.IndexOf("Esc") >= 0) onSetNickname();
            else if (sInputMsg.IndexOf("Back") >= 0) onSetNickname();
            else if (sInputMsg.IndexOf("Confirm") >= 0) Application.LoadLevel("UI_DailyBonus");
        }
        //
        if (iNicknameState == (int)ENUM_NICKNAME_PIPELINE_STATE.LOGIN)//step1
        {
/*
            if (WebServerProcess.ServerResult == enumServerResult.Uploading)
            {

            }
            else if (WebServerProcess.ServerResult == enumServerResult.Succeed)
            {
                ConfigManager.SaveConfig();
                if (WebServerProcess.ResultStatus == 0)//已經註冊過
                {
*/                  iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.FINISH;
/*                  UnityEngine.Debug.Log("Login success Already");
                }
                else if (WebServerProcess.ResultStatus == 1)//新帳號
                {
                    iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.INPUT_NAME;//step2
                }
                else if (WebServerProcess.ResultStatus == 99)//新帳號
                {
                    iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.INPUT_NAME;//step2
                }
                else
                {
                    iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.FINISH;//不是新帳號,就當作註冊過
                    UnityEngine.Debug.Log("Login success! but result =" + WebServerProcess.ResultStatus);
                }
            }
            else if (WebServerProcess.ServerResult == enumServerResult.Error)
            {
                //halt?
                WebServerProcess.ServerResult = enumServerResult.None;
                UnityEngine.Debug.Log("Login error!!!" + WebServerProcess.ResultStatus);
            }
 */ 
        }
        else if (iNicknameState != (int)ENUM_NICKNAME_PIPELINE_STATE.INPUT_NAME)//step2
        {
            //get nickname form game moudle
            string sPlayerInputNickname = "";

/*            
            WorkStep Step = WorkStep.None ;
            if (Step == WorkStep.InputText)
            {
                GameFifo.SendMsg(1, "PopTextInput"); //傳送給手機 叫出一般文字輸入
                GameFifo.InputText = "";
            }
            else if (Step == WorkStep.Conform)
            {
                GameFifo.SendMsg(1, "CloseTextInput"); //傳送給手機 關閉文字輸入框
            }
            else
            {
                sPlayerInputNickname = "";
                GameFifo.SendMsg(1, "CloseTextInput"); //傳送給手機 關閉文字輸入框
            }
 
            //偵測裝置指令
            if (sInputMsg == "Back" || sInputMsg == "Menu")
            {
                GameFifo.InputTextVisabled = false;
                Hide();
            }
            else if (GameFifo.InputText != "" && sInputMsg == "Confirm")
            {
                NickName = GameFifo.InputText;
                GameFifo.InputText = "";
                //ServerProcess.CheckNickName(NickName);
                WorkResult = TextFilter.CheckSymbol(ref NickName);
            }
            //
            TextMesh mTextMesh = menuRoot.FindChild("TextNickname").GetComponent<TextMesh>();
            mTextMesh.text = GameFifo.InputText;         
            if (string.IsNullOrEmpty(GameFifo.InputText) == true) return;
            //
            MenuNicknameConfirm.SetActive(true);
            iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.SETTING_NICKNAME;//step3
            JSK_GlobalProcess.g_PlayerNickName = string.Copy(GameFifo.InputText);
            //set nickname to webserver
            WebServerProcess.SetNickName(JSK_GlobalProcess.g_PlayerNickName);
*/ 
        }
        else if (iNicknameState != (int)ENUM_NICKNAME_PIPELINE_STATE.SETTING_NICKNAME)//step3
        {
/*
            if (WebServerProcess.ServerResult == enumServerResult.Uploading)
            {
             
            }
            else if (WebServerProcess.ServerResult == enumServerResult.Succeed)
            {
                iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.NORMAL;
            }
            else if (WebServerProcess.ServerResult == enumServerResult.Error)
            {
                UnityEngine.Debug.Log("Setting nickname error!!!" + WebServerProcess.ResultStatus);
                //change state!
                iNicknameState = (int)ENUM_NICKNAME_PIPELINE_STATE.INPUT_NAME;
                //伺服器端回傳值回傳值0:正常暱稱 1:暱稱變更成功 2:名稱重複 3:不雅文字 4:暱稱過長 5:有特殊符號 6:系統錯誤
                SetStateForNickname(WebServerProcess.ResultStatus);
            }
 */ 
        }
        else
        {
            return;
        }
    }
    //called form Playmake
    void onLoadLevel()
    {
        int iTargetUIVersion = JSK_GlobalProcess.g_iDemoUIVersion ;
        if (iTargetUIVersion == 1)
        {
            /* do nothing */
        }
        else if (iTargetUIVersion == 2)
        {
            Application.LoadLevel("UI_Daily");//每日獎勵介面
        }
        else if (iTargetUIVersion == 3)
        {
            Application.LoadLevel("UI_DailyBonus");//每日獎勵介面-II
        }
        else if (iTargetUIVersion == 4)
        {
            Application.LoadLevel("UI_NGUI_Daily");//每日獎勵介面-III,nGUI改寫,drawcall降2.....
        }                
    }
}

public enum ENUM_NICKNAME_PIPELINE_STATE : int
{
    NONE = 0,
    LOGIN = 1,
    INPUT_NAME = 2,
    SETTING_NICKNAME = 3,
    NORMAL = 4,//set nickname succsed  , and wait player press A button to leave
    FINISH = 5,//have nickname already , and auto leave
    NO_SERVER = 6,//no WebServer , for demo
    UI_PROCESS_NICKNAME = 7,
    MAX
}

public enum WorkStep { None, InputText, Conform, Info }