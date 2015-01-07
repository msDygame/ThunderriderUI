using UnityEngine;
using System.Collections;
//define
public enum ENUM_DEFINE_DAILY_BONUS
{
    NONE,
    MAX_DAILY_DAY = 7,//一周七天
    MAX_DAILY_ITEM = 7,//目前有七件物品
    MAX_DAILY_ITEM_AMOUNT,//最大顯示數量9999
    MAX_DAILY_STATUS = 3,//狀態 0:未領取 1:已領取 2:可領取  
    MAX
}
//每日登入獎勵
public class UI_DailyBonusProcess : MonoBehaviour
{
    protected static bool IsActiveMenu = true;
    protected static RandomDisplayDaily[] DailyList = new RandomDisplayDaily[7];//一週七天,每週重置
    protected int iTodayLogin = -1;
    //Daily state
    public Transform menuRootDaily;
    protected GameObject[] MenuDailyItem   = null;//物品,3=動游幣,4=金幣,5=一堆金幣,1=綠防護球,6=黑汽油,7=黃汽油,2=縮小燈
    protected GameObject[] MenuDailyCount1 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount2 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount3 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount4 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount5 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount6 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount7 = null;//數量,0~9999
    protected GameObject[] MenuDailyDay    = null;//天數,1~7
    protected GameObject[] MenuDailyDayRed = null;//天數,1~7
    protected GameObject[] MenuDailyStatus = null;//狀態,0:未領取(紅底框) 1:已領取(藍底框帶綠勾) 2:可領取(藍底框)
    protected GameObject[] MenuDailyStatusR= null;//狀態,0:未領取(紅底框) 1:已領取(藍底框帶綠勾) 2:可領取(藍底框)
    protected GameObject[] MenuDailyCheck  = null;//綠色打勾, 1:已領取
    protected GameObject   MenuPressButton;
    protected GameObject   MenuPressDownButton;
    void Awake()
    {
        JSK_GlobalProcess.InitGlobal();
        //
        //Daily
        MenuDailyItem = new GameObject[7];//一週七天
        MenuDailyItem[0] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[1] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[2] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[3] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[4] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[5] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_item01").gameObject;
        MenuDailyItem[6] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_item01").gameObject;
        MenuDailyCount1 = new GameObject[4];
        MenuDailyCount2 = new GameObject[4];
        MenuDailyCount3 = new GameObject[4];
        MenuDailyCount4 = new GameObject[4];
        MenuDailyCount5 = new GameObject[4];
        MenuDailyCount6 = new GameObject[4];
        MenuDailyCount7 = new GameObject[4];
        MenuDailyCount1[0] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount1[1] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount1[2] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount1[3] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount2[0] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount2[1] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount2[2] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount2[3] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount3[0] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount3[1] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount3[2] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount3[3] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount4[0] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount4[1] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount4[2] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount4[3] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount5[0] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount5[1] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount5[2] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount5[3] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount6[0] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount6[1] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount6[2] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount6[3] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_no3").gameObject;
        MenuDailyCount7[0] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_no0").gameObject;
        MenuDailyCount7[1] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_no1").gameObject;
        MenuDailyCount7[2] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_no2").gameObject;
        MenuDailyCount7[3] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_no3").gameObject;
        MenuDailyDay = new GameObject[7];//一週七天
        MenuDailyDay[0] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[1] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[2] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[3] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[4] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[5] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDay[6] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_week_1g").gameObject;
        MenuDailyDayRed = new GameObject[7];//一週七天(未領取)
        MenuDailyDayRed[0] = menuRootDaily.Find("card_red/card01/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[1] = menuRootDaily.Find("card_red/card02/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[2] = menuRootDaily.Find("card_red/card03/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[3] = menuRootDaily.Find("card_red/card04/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[4] = menuRootDaily.Find("card_red/card05/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[5] = menuRootDaily.Find("card_red/card06/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyDayRed[6] = menuRootDaily.Find("card_red/card07/tr_mk_mu_diary_week_1r").gameObject;
        MenuDailyStatus = new GameObject[7]; ;//一週七天
        MenuDailyStatus[0] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[1] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[2] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[3] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[4] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[5] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatus[6] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_01_7").gameObject;
        MenuDailyStatusR = new GameObject[7]; ;//一週七天(未領取)
        MenuDailyStatusR[0] = menuRootDaily.Find("card_red/card01/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[1] = menuRootDaily.Find("card_red/card02/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[2] = menuRootDaily.Find("card_red/card03/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[3] = menuRootDaily.Find("card_red/card04/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[4] = menuRootDaily.Find("card_red/card05/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[5] = menuRootDaily.Find("card_red/card06/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyStatusR[6] = menuRootDaily.Find("card_red/card07/tr_mk_mu_diary_01_8").gameObject;
        MenuDailyCheck = new GameObject[7]; ;//一週七天
        MenuDailyCheck[0] = menuRootDaily.Find("card_blue/card01/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[1] = menuRootDaily.Find("card_blue/card02/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[2] = menuRootDaily.Find("card_blue/card03/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[3] = menuRootDaily.Find("card_blue/card04/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[4] = menuRootDaily.Find("card_blue/card05/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[5] = menuRootDaily.Find("card_blue/card06/tr_mk_mu_diary_01_44").gameObject;
        MenuDailyCheck[6] = menuRootDaily.Find("card_blue/card07/tr_mk_mu_diary_01_44").gameObject;
        MenuPressButton = menuRootDaily.Find("but_yes/tr_mk_but_a1").gameObject;
        MenuPressDownButton = menuRootDaily.Find("but_yes/tr_mk_but_a2").gameObject;
        for (int i = 0; i < (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY; i++) MenuDailyCheck[i].SetActive(false);
        for (int i = 0; i < (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY; i++) MenuDailyStatusR[i].SetActive(false);
        for (int i = 0; i < (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY; i++) MenuDailyDayRed[i].SetActive(false);
        MenuPressDownButton.SetActive(false);
        //
        IsActiveMenu = true;
        for (int i = 0; i < (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY; i++) DailyList[i] = new RandomDisplayDaily();
    }
	// Use this for initialization
	void Start () 
    {
        //從WebServer讀取每日獎勵清單
        GetDateFromWebServerForDaily();
        //
        //if (IsActiveMenu == false) { Application.LoadLevel("UI_MainMenu"); return; }//WebServer一直都是false?
        //update daily bouns
        for (int i = 0; i < (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY; i++)
        {
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_ICON, DailyList[i].iItemIcon-1, i);
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_COUNT, DailyList[i].iItemCount, i);
            if (DailyList[i].iDailyStatus == 2)
            {
                SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_DAY_RED, DailyList[i].iDailyDay, i);
            }
            else
            {
                SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_DAY, DailyList[i].iDailyDay, i);
            }
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_STATUS, DailyList[i].iDailyStatus, i);
        }
	}	
	// Update is called once per frame
	void Update () 
    {
        checkFifoMenu();
	}
    void OnGUI()
    {

    }
    //壓下 確認鈕,離開此頁面
    void onDailyMove()
    {
        if (iTodayLogin < 0) return;
        MenuDailyCheck[iTodayLogin].SetActive(true);
        MenuPressButton.SetActive(false);
        MenuPressDownButton.SetActive(true);
    }
    //
     void SetStateForDaily(ENUM_UI_DAILY_STATE state, int iIndex, int iOption)
    {
        switch (state)
        {
            case ENUM_UI_DAILY_STATE.NONE: break;
            case ENUM_UI_DAILY_STATE.DAILY_ICON:
                {
                    if ((iIndex > (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_ITEM) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_ITEM_INDEX;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyItem[iOption], iIndex, iSpriteSet);
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_COUNT:
                {
                    if (iIndex < 0 ) break;//out of range                    
                    int iMaxDigit = 4;//位數上限
                    int iLimit = 1;
                    for (int i = 0; i < iMaxDigit; i++) iLimit *= 10;
                    if (iIndex >= iLimit) iIndex = (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_ITEM_AMOUNT;
                    int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE;//行為
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_COUNT_WHITE;//圖庫
                    if      (iOption == 0) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount1, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 1) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount2, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 2) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount3, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 3) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount4, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 4) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount5, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 5) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount6, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    else if (iOption == 6) JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(MenuDailyCount7, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_DAY:
                {
                    if ((iIndex > (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_DAY_INDEX;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyDay[iOption], iIndex, iSpriteSet);                    
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_DAY_RED:
                {
                    if ((iIndex > (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_DAY) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_DAY_INDEX;//圖庫//似乎沒有紅色的一二三四五..
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyDay[iOption], iIndex, iSpriteSet);                    
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_STATUS:
                {
                    if ((iIndex > (int)ENUM_DEFINE_DAILY_BONUS.MAX_DAILY_STATUS) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_STATUS_INDEX;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyStatus[iOption], iIndex, iSpriteSet);
                    if (iIndex == 1) MenuDailyCheck[iOption].SetActive(true);//已領取時打勾
                    break;
                }
            default: break;
        }
    }
    //
    void GetDateFromWebServerForDaily()
    {
        //每日獎勵
        if (JSK_GlobalProcess.g_IsWebServer)
        {
            //
            //class WebServerPorcess:
            //static List<DailyData> DailyList = new List<DailyData>();
            //static bool IsDailyBouns = false;
            //class DailyData:
            //public int ID = 0; //代號
            //public string Icon = "";//ICON
            //public string Description = "";//內容文字
            //public int DayIndex; //日期
            //public int Status = 0; //狀態 0:未領取 1:已領取 2:可領取  
/*
            IsActiveMenu = WebServerProcess.IsDailyBouns;//當天第一次登入時為true,顯示本介面
          //if (IsActiveMenu == false) return;//WebServer一直都是false?
            //get data
            int iCount = 0;
            bool IsErrorHappen = false;
            for (int i = 0; i < WebServerProcess.DailyList.Count ; i++)
            {
                if (i > 7) break; //out array range 一週七天
                DailyList[i].iItemIcon = int.Parse(WebServerProcess.DailyList[i].Icon);
                DailyList[i].iItemCount = int.Parse(WebServerProcess.DailyList[i].Description);
                DailyList[i].iDailyDay = i;
                DailyList[i].iDailyStatus = WebServerProcess.DailyList[i].Status;
                if (DailyList[i].iDailyStatus == 2) iTodayLogin = i;//抓第一個可領取
                iCount++;
                //checking
                if ((DailyList[i].iItemIcon > 7) || (DailyList[i].iItemIcon < 0)) IsErrorHappen = true;//out of range 目前只有七件物品 1~7
                if ((DailyList[i].iItemCount < 0) ) IsErrorHappen = true;//out of range                
                if ((DailyList[i].iDailyStatus >= 3) || (DailyList[i].iItemIcon < 0)) IsErrorHappen = true;//out of range
                if (IsErrorHappen == true)
                {
                    DailyList[iCount].iItemIcon = 0;
                    DailyList[iCount].iItemCount = 0;
                    DailyList[iCount].iDailyStatus = 0;
                }
            }
            //fill
            for (/** / ; iCount < 7 ; iCount++)
            {
                DailyList[iCount].iItemIcon = 0;
                DailyList[iCount].iItemCount = 0;
                DailyList[iCount].iDailyDay = iCount;
                DailyList[iCount].iDailyStatus = 0;
            }
*/
        }
        else
        {         
            int iRandomItem = 0;
            int iRandomCount = 1;
            int iRandomStatus = 0 ;
            bool IsUnique = false ;//可領取 為 當日登入 , 故只會有一個
            bool IsIncoming = false;//從未領取
            int iAmount = 1 ;//1~10
            for (int i = 0 ; i < 7 ; i++)
            {
                iRandomItem = Random.Range(1, 8);//1,2,3,4,5,6,7
                iAmount = Random.Range(1, 11);
                switch (iRandomItem)
                {
                    case 0 : iRandomCount = iAmount * 100 ; break;
                    case 1 : iRandomCount = iAmount * 100 ; break;
                    case 2 : iRandomCount = iAmount * 200 ; break;
                    case 3 : iRandomCount = iAmount ; break;
                    case 4 : iRandomCount = iAmount ; break;
                    case 5 : iRandomCount = iAmount ; break;
                    case 6 : iRandomCount = iAmount ; break;
                    default: break;
                }
                if (IsUnique == false)
                {
                    iRandomStatus = Random.Range(0, 3);//0,1,2
                    if (iRandomStatus == 2) { IsUnique = true; iTodayLogin = i; } //當日登入為可領取
                    if (iRandomStatus == 1) IsIncoming = true;//有領取過
                }
                else
                    iRandomStatus = 0;//未來的都是 未領取
                //
                DailyList[i].iItemIcon = iRandomItem;//動游幣,金幣,一堆金幣,綠防護球,黑汽油,黃汽油,縮小燈
                DailyList[i].iItemCount = iRandomCount;//數量
                DailyList[i].iDailyDay = i;//第X天
                DailyList[i].iDailyStatus = iRandomStatus;//狀態 0:未領取 1:已領取 2:可領取  
            }
            if (IsUnique == false)
            {
                if (IsIncoming == false)                                    
                    iTodayLogin = 0;
                else
                    iTodayLogin = 6;
                DailyList[iTodayLogin].iDailyStatus = 2;
            }
        }      
    }
    //
    void checkFifoMenu()
    {
        string sInputMsg = JSK_GlobalProcess.GetFifoMsg();

        if (sInputMsg.Length > 0) Debug.Log("sInputMsg:" + sInputMsg);

        if (sInputMsg.IndexOf("Left") >= 0) return;
        else if (sInputMsg.IndexOf("Right") >= 0) return;
        else if (sInputMsg.IndexOf("Up") >= 0) return;
        else if (sInputMsg.IndexOf("Down") >= 0) return;
        else if (sInputMsg.IndexOf("Esc") >= 0) return;
        else if (sInputMsg.IndexOf("Confirm") >= 0) Application.LoadLevel("UI_MainMenu");
    }
}
