using UnityEngine;
using System.Collections;
//每日登入獎勵 名次 匿稱 累積場次 最快時間/累積積分
public class RandomDisplayDaily
{   
    public int iItemIcon = 0;//0=動游幣,1=金幣,2=一堆金幣,3=綠防護球,4=黑汽油,5=黃汽油,6=引擎/縮小燈
    public int iItemCount = 0;//數量
    public int iDailyDay = 0;//天數0~6=1~7天
    public int iDailyStatus = 0;//狀態 0:未領取 1:已領取 2:可領取  
}
public class UI_DailyProcess : MonoBehaviour 
{
    protected static bool IsActiveMenu = true ;
    protected static RandomDisplayDaily[] DailyList = new RandomDisplayDaily[7];//一週七天,每週重置
    protected int iTodayLogin = -1;
    //Daily state
    public Transform menuRootDaily;
    protected GameObject[] MenuDailyItem = null;//物品,0=動游幣,1=金幣,2=一堆金幣,3=綠防護球,4=黑汽油,5=黃汽油,6=引擎/縮小燈
    protected GameObject[] MenuDailyCount1 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount2 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount3 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount4 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount5 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount6 = null;//數量,0~9999
    protected GameObject[] MenuDailyCount7 = null;//數量,0~9999
    protected GameObject[] MenuDailyDay = null;//天數,1~7
    protected GameObject[] MenuDailyStatus = null;//狀態,0:未領取 1:已領取 2:可領取
    protected GameObject[] MenuDailyCheck = null;//綠色打勾, 1:已領取
    void Awake()
    {
        JSK_GlobalProcess.InitGlobal();
        //
        //Daily
        MenuDailyItem = new GameObject[7];//一週七天
        MenuDailyItem[0] = menuRootDaily.Find("card01/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[1] = menuRootDaily.Find("card02/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[2] = menuRootDaily.Find("card03/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[3] = menuRootDaily.Find("card04/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[4] = menuRootDaily.Find("card05/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[5] = menuRootDaily.Find("card06/item/tr_mk_daily_item01").gameObject;
        MenuDailyItem[6] = menuRootDaily.Find("card07/item/tr_mk_daily_item01").gameObject;
        MenuDailyCount1 = new GameObject[4];
        MenuDailyCount2 = new GameObject[4];
        MenuDailyCount3 = new GameObject[4];
        MenuDailyCount4 = new GameObject[4];
        MenuDailyCount5 = new GameObject[4];
        MenuDailyCount6 = new GameObject[4];
        MenuDailyCount7 = new GameObject[4];
        MenuDailyCount1[0] = menuRootDaily.Find("card01/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount1[1] = menuRootDaily.Find("card01/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount1[2] = menuRootDaily.Find("card01/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount1[3] = menuRootDaily.Find("card01/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount2[0] = menuRootDaily.Find("card02/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount2[1] = menuRootDaily.Find("card02/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount2[2] = menuRootDaily.Find("card02/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount2[3] = menuRootDaily.Find("card02/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount3[0] = menuRootDaily.Find("card03/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount3[1] = menuRootDaily.Find("card03/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount3[2] = menuRootDaily.Find("card03/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount3[3] = menuRootDaily.Find("card03/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount4[0]= menuRootDaily.Find("card04/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount4[1] = menuRootDaily.Find("card04/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount4[2] = menuRootDaily.Find("card04/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount4[3] = menuRootDaily.Find("card04/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount5[0] = menuRootDaily.Find("card05/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount5[1] = menuRootDaily.Find("card05/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount5[2] = menuRootDaily.Find("card05/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount5[3] = menuRootDaily.Find("card05/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount6[0] = menuRootDaily.Find("card06/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount6[1] = menuRootDaily.Find("card06/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount6[2] = menuRootDaily.Find("card06/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount6[3] = menuRootDaily.Find("card06/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyCount7[0] = menuRootDaily.Find("card07/pic/tr_mk_daily_pic00").gameObject;
        MenuDailyCount7[1] = menuRootDaily.Find("card07/pic/tr_mk_daily_pic01").gameObject;
        MenuDailyCount7[2] = menuRootDaily.Find("card07/pic/tr_mk_daily_pic02").gameObject;
        MenuDailyCount7[3] = menuRootDaily.Find("card07/pic/tr_mk_daily_pic03").gameObject;
        MenuDailyDay = new GameObject[7];//一週七天
        MenuDailyDay[0] = menuRootDaily.Find("card01/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[1] = menuRootDaily.Find("card02/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[2] = menuRootDaily.Find("card03/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[3] = menuRootDaily.Find("card04/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[4] = menuRootDaily.Find("card05/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[5] = menuRootDaily.Find("card06/week/tr_mk_daily_week01").gameObject;
        MenuDailyDay[6] = menuRootDaily.Find("card07/week/tr_mk_daily_week01").gameObject;
        MenuDailyStatus = new GameObject[7]; ;//一週七天
        MenuDailyStatus[0] = menuRootDaily.Find("card01/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[1] = menuRootDaily.Find("card02/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[2] = menuRootDaily.Find("card03/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[3] = menuRootDaily.Find("card04/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[4] = menuRootDaily.Find("card05/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[5] = menuRootDaily.Find("card06/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyStatus[6] = menuRootDaily.Find("card07/bk/tr_mk_daily_01_5").gameObject;
        MenuDailyCheck = new GameObject[7]; ;//一週七天
        MenuDailyCheck[0] = menuRootDaily.Find("tr_mk_mu_gassoline_31").gameObject;
        MenuDailyCheck[1] = menuRootDaily.Find("tr_mk_mu_gassoline_32").gameObject;
        MenuDailyCheck[2] = menuRootDaily.Find("tr_mk_mu_gassoline_33").gameObject;
        MenuDailyCheck[3] = menuRootDaily.Find("tr_mk_mu_gassoline_34").gameObject;
        MenuDailyCheck[4] = menuRootDaily.Find("tr_mk_mu_gassoline_35").gameObject;
        MenuDailyCheck[5] = menuRootDaily.Find("tr_mk_mu_gassoline_36").gameObject;
        MenuDailyCheck[6] = menuRootDaily.Find("tr_mk_mu_gassoline_37").gameObject;
        for (int i = 0; i < 7; i++) MenuDailyCheck[i].SetActive(false);
        for (int i = 0; i < 7; i++) MenuDailyCheck[i].SetActive(false);        
        //
        IsActiveMenu = true;
        for (int i = 0; i < 7; i++) DailyList[i] = new RandomDisplayDaily();
    }
	// Use this for initialization
	void Start () 
    {
	    GetDateFromWebServerForDaily() ;
        //
        //if (IsActiveMenu == false) { Application.LoadLevel("UI_MainMenu"); return; }//WebServer一直都是false?
        //
        for (int i = 0; i < 7; i++)
        {
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_ICON,    DailyList[i].iItemIcon,     i);
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_COUNT,   DailyList[i].iItemCount,    i);
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_DAY,     DailyList[i].iDailyDay,     i);
            SetStateForDaily(ENUM_UI_DAILY_STATE.DAILY_STATUS,  DailyList[i].iDailyStatus,  i);
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
    }
    void SetStateForDaily(ENUM_UI_DAILY_STATE state, int iIndex, int iOption)
    {
        switch (state)
        {
            case ENUM_UI_DAILY_STATE.NONE: break;
            case ENUM_UI_DAILY_STATE.DAILY_ICON:
                {  
                    if ((iIndex > 7) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_ITEM_INDEX;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyItem[iOption], iIndex, iSpriteSet);
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_COUNT:
                {
                    if (iIndex < 0 ) break;//out of range                    
                    int iMaxDigit = 4;//位數上限
                    if (iIndex >= 10000) iIndex = 9999;
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
                    if ((iIndex > 7) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.DAILY_DAY_INDEX;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuDailyDay[iOption], iIndex, iSpriteSet);
                    break;
                }
            case ENUM_UI_DAILY_STATE.DAILY_STATUS:
                {
                    if ((iIndex > 3) || (iIndex < 0)) break;//out of range
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
                if (i > 7) break; //out array range
                DailyList[i].iItemIcon = int.Parse(WebServerProcess.DailyList[i].Icon);
                DailyList[i].iItemCount = int.Parse(WebServerProcess.DailyList[i].Description);
                DailyList[i].iDailyDay = i;
                DailyList[i].iDailyStatus = WebServerProcess.DailyList[i].Status;
                if (DailyList[i].iDailyStatus == 2) iTodayLogin = i;//抓第一個可領取
                iCount++;
                //checking
                if ((DailyList[i].iItemIcon >= 7) || (DailyList[i].iItemIcon < 0)) IsErrorHappen = true;//out of range
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
                iRandomItem = Random.Range(0, 7);//0,1,2,3,4,5,6
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
                DailyList[i].iItemIcon = iRandomItem;//0=動游幣,1=金幣,2=一堆金幣,3=綠防護球,4=黑汽油,5=黃汽油,6=引擎/縮小燈
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
//
public enum ENUM_UI_DAILY_STATE
{
    NONE,
    DAILY_ICON,//0=動游幣,1=金幣,2=一堆金幣,3=綠防護球,4=黑汽油,5=黃汽油,6=引擎/縮小燈
    DAILY_COUNT,//數量,Description
    DAILY_DAY,//圈數0~6=1~7天
    DAILY_STATUS,//狀態 0:未領取 1:已領取 2:可領取
    DAILY_DAY_RED,//圈數0~6=1~7天,未領取
    MAX
}