using UnityEngine;
using System.Collections;

//商品類型 (market=商城,item=物品) //20141016:Mall改名為Market,因為small會讓Mall很難找..
// public string ID = "";//商品編號
// public string Name = "";//商品名稱
// public string Icon = "";//商品ICON
// public string Description = "";//商品內容文字
// public int Money; //花費遊戲幣
// public int DYMoney; //花費動游幣
// public int Discount = 0; //活動折扣
// public int Status = 0; //狀態 0:無法購買 1:可購買
public class RandomDisplayMarketItem
{
    public string   sMarketItemID   = "" ;
	public string	sMarketItemName = "" ;//名稱
    public string   sMarketItemIcon = "" ;
    public string   sMarketItemDescription = "";//介紹    
    public int      iMarketItemMoney    = 0;//定價遊戲幣
    public int      iMarketItemDYMoney  = 0;//定價動游幣
    public int      iMarketItemCost     = 0;//售價遊戲幣
    public int      iMarketItemDYCost   = 0;//售價遊戲幣
	public int		iMarketItemOnSale   = 100 ;//折扣(%). 折扣後的.
    public int      iMarketItemStatus   = 0;//狀態 0:無法購買 1:可購買
}

public class JSK_MarketMenuProcess : MonoBehaviour
{
	public  Transform 	uiCamera			= null;	//射線查詢相機.
	public  Transform 	menuRoot       		= null; //選單根節點.
	//
	private float 		lastMoveTime		= 0;	//上次移动菜单的时间.
	private float 		moveDelayTime		= 0.0f;	//移动菜单的间隔时间,如果不需要延迟,就把这个改成0.
	private string 		nextSceneName   	= "";  	//下一个场景的名字.
	private Transform 	curAnimMenu 		= null;	//当前正在播放UI动画的物体.
	private string 		curAnimName 		= "";	//当前播放的UI动画名称.
	private float 		curAnimTime 		= 0;	//当前是否正在播放的UI动画的时间.
	private float		menuAnimSpeed   	= 1.1f; //菜单动画的播放速度.
	private float		lastButtonTime		= 0.0f ;//按鈕壓下的間隔時間
	private bool		IsButtonPressDown   = false;//按鈕壓下旗標
    private bool        IsMarketItemBuy     = false;//購買按鈕壓下(買了可能不成功?)
	//
	private int 		iMaxMarketItemNumber= 0;  	//最大的特價商品數量.
	private int 		iCurrentMarketItemIndex= 0; //當前的特價商品索引.
    //temp
    private int         iLevel              = 0;
    private int         iMoney              = 0;
    private int         iExp                = 0;
    private int         iDYGameMoney        = 0;//玩家的動游幣
	//gameObject
	private GameObject 	leftArrowMenu		= null;	//左箭頭壓下.
	private GameObject 	rightArrowMenu		= null;	//右箭頭壓下.
	private GameObject 	leftArrowActive		= null;	//左箭頭.
	private GameObject 	rightArrowActive	= null;	//右箭頭.
	//
	private GameObject 	nextButtonMenu		= null;	//確認鍵.(A)購買
	private GameObject 	backButtonMenu		= null;	//返回鍵.(X)返回
	private GameObject 	onSaleLeftMenu		= null;	//左特價商品
	private GameObject 	onSaleMiddleMenu	= null;	//中特價商品
	private GameObject 	onSaleRightMenu		= null;	//右特價商品
	private GameObject[] onSalePercent		;		//商城介面消費折扣(%)
	private GameObject[] onSaleCash			;		//商城介面消費金額(折扣後)
	private GameObject[] onSaleLevel		;		//商城介面Level;顯示目前玩家的等級及經驗值條。
	private GameObject[] onSaleMoney		;		//商城介面Money;
	private GameObject[] onSaleExp			;		//商城介面Exp  ;顯示目前玩家的等級及經驗值條。
	//販售中商品
	private static	RandomDisplayMarketItem[] MarketItemList = null ;
	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
		//20141001NewUI Add this.
		nextButtonMenu = menuRoot.Find("button/tr_mk_market_button_a").gameObject;
		backButtonMenu = menuRoot.Find("button/tr_mk_market_button_x").gameObject;
	
		leftArrowMenu  = menuRoot.Find("arrow_l/tr_mo_mu_carlevel_arrow_b04").gameObject;//表示玩家還可以左右切換快艇的ICON
		rightArrowMenu = menuRoot.Find("arrow_r/tr_mo_mu_carlevel_arrow_b02").gameObject;//表示玩家還可以左右切換快艇的ICON
		leftArrowMenu.SetActive(false);
		rightArrowMenu.SetActive(false);
		leftArrowActive	= menuRoot.Find("arrow_l/tr_mo_mu_carlevel_arrow_a04").gameObject;
		rightArrowActive= menuRoot.Find("arrow_r/tr_mo_mu_carlevel_arrow_a02").gameObject;
		onSaleCash = new GameObject[10] ;//8位數
		onSaleCash[0]		= menuRoot.Find("card/tr_mk_market_coin_00").gameObject;
		onSaleCash[1]		= menuRoot.Find("card/tr_mk_market_coin_01").gameObject;
		onSaleCash[2]		= menuRoot.Find("card/tr_mk_market_coin_02").gameObject;
		onSaleCash[3]		= menuRoot.Find("card/tr_mk_market_coin_03").gameObject;
		onSaleCash[4]		= menuRoot.Find("card/tr_mk_market_coin_04").gameObject;
		onSaleCash[5]		= menuRoot.Find("card/tr_mk_market_coin_05").gameObject;
		onSaleCash[6]		= menuRoot.Find("card/tr_mk_market_coin_06").gameObject;
		onSaleCash[7]		= menuRoot.Find("card/tr_mk_market_coin_07").gameObject;
		onSaleCash[8]		= menuRoot.Find("card/tr_mk_market_coin_08").gameObject;
		onSaleCash[9]		= menuRoot.Find("card/tr_mk_market_coin_09").gameObject;
		onSalePercent = new GameObject[3] ;//3位數
		onSalePercent[0]	= menuRoot.Find("on sale/tr_mk_market_sale_00").gameObject;
		onSalePercent[1]	= menuRoot.Find("on sale/tr_mk_market_sale_09").gameObject;
		onSalePercent[2]	= menuRoot.Find("on sale/tr_mk_market_sale_08").gameObject;
		onSalePercent[0].SetActive(false);
		onSalePercent[1].SetActive(false);
		onSalePercent[2].SetActive(false);
		onSaleMoney = new GameObject[10] ;//10位數
		onSaleMoney[0]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_23").gameObject;
		onSaleMoney[1]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_24").gameObject;
		onSaleMoney[2]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_25").gameObject;
		onSaleMoney[3]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_26").gameObject;
		onSaleMoney[4]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_27").gameObject;
		onSaleMoney[5]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_28").gameObject;
		onSaleMoney[6]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_29").gameObject;
		onSaleMoney[7]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_30").gameObject;
		onSaleMoney[8]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_31").gameObject;
		onSaleMoney[9]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_32").gameObject;
		onSaleExp = new GameObject[10] ;//10位數
		onSaleExp[0]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_12").gameObject;
		onSaleExp[1]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_13").gameObject;
		onSaleExp[2]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_14").gameObject;
		onSaleExp[3]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_15").gameObject;
		onSaleExp[4]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_16").gameObject;
		onSaleExp[5]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_17").gameObject;
		onSaleExp[6]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_18").gameObject;
		onSaleExp[7]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_19").gameObject;
		onSaleExp[8]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_20").gameObject;
		onSaleExp[9]		= menuRoot.Find("lv_and_exp/Group_$$/tr_mk_mu_bkground_06_21").gameObject;
		onSaleLevel = new GameObject[3] ;//3位數
		onSaleLevel[0]		= menuRoot.Find("lv_and_exp/lv/tr_mk_mu_bkground_06_1").gameObject;
		onSaleLevel[1]		= menuRoot.Find("lv_and_exp/lv/tr_mk_mu_bkground_06_7").gameObject;
		onSaleLevel[2]		= menuRoot.Find("lv_and_exp/lv/tr_mk_mu_bkground_06_10").gameObject;	
		//
		onSaleLeftMenu		= menuRoot.Find("card/tr_mk_market_picture_01").gameObject;	//左特價商品
		onSaleMiddleMenu	= menuRoot.Find("card/tr_mk_market_picture_02").gameObject;	//中特價商品
		onSaleRightMenu		= menuRoot.Find("card/tr_mk_market_picture_03").gameObject;	//右特價商品       
	}
	// Use this for initialization
	void Start () 
	{
        GetMarketDataFromWebServer();
        //		
		onMarketItemMove(0) ;
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
		if (IsButtonPressDown) SetRestoreButtonState() ;
        //
        if (IsMarketItemBuy) WaitMarketDataResultFromWebServer();
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
			}
		}
		else if( JSK_GlobalProcess.IsFingerUp() )
		{
			Ray ray = uiCamera.camera.ScreenPointToRay(JSK_GlobalProcess.GetFingerPosition());
			RaycastHit hit;
			if( Physics.Raycast(ray, out hit, Mathf.Infinity) )
			{
				GameObject hitMenu = hit.transform.gameObject;
				if( hitMenu == leftArrowActive )  onMarketItemMove(-1);
				else if( hitMenu == rightArrowActive ) onMarketItemMove(1);
			}
			RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2D != null) 
			{
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if( hitMenu == nextButtonMenu )		onMenuSelect();
					else if( hitMenu == backButtonMenu )onMenuEsc();
				}
			}
		}
	}
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
		if     ( sInputMsg.IndexOf("Left") >= 0 )		onMarketItemMove(-1);
		else if( sInputMsg.IndexOf("Right") >= 0 )		onMarketItemMove(1) ;
		else if( sInputMsg.IndexOf("Up") >= 0 )			onMarketItemMove(-1);
		else if( sInputMsg.IndexOf("Down") >= 0 )		onMarketItemMove(1) ;
		else if( sInputMsg.IndexOf("Esc") >= 0 )		onMenuEsc();
        else if (sInputMsg.IndexOf("Back") >= 0)        onMenuEsc();
		else if( sInputMsg.IndexOf("Confirm") >= 0 )	onMenuSelect();
        else if (Input.GetKeyDown(KeyCode.X))           onMenuEsc();
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
				onMarketItemMove(0);
		}
	}
	
	void onMenuSelect()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
         //get date from webServer
        if (JSK_GlobalProcess.g_IsWebServer)
        {
            //購買商城物品ShopItemData：傳入商品編號ItemID,數量ItemCount，傳回最新玩家資料與交易結果 (商品表請參照ShopItemData內的物品清單)
            //public static void BuyShopItem(string ItemID)
            //購買特殊物品ItemData (汽油,加速)：傳入物品編號ItemID，傳回最新玩家資料與交易結果 (物品表請參照ItemManager內的物品清單)
            //public static void BuySpecialItem(string ItemID)            
            if (iMaxMarketItemNumber <= 0) return;//沒商品
            string sItemID = MarketItemList[iCurrentMarketItemIndex].sMarketItemID;
            if (true == string.IsNullOrEmpty(sItemID)) return;//防當
            //check money enough
            int iTargetMoney = MarketItemList[iCurrentMarketItemIndex].iMarketItemDYMoney;
            if (iTargetMoney > 0)
            {
                if (iDYGameMoney < iTargetMoney) return;//沒錢
            }
            else 
            {
                iTargetMoney = MarketItemList[iCurrentMarketItemIndex].iMarketItemMoney;
                if (iMoney < iTargetMoney) return;//沒錢
            }
/*
            WebServerProcess.BuyShopItem(sItemID);//買動游幣,修理工具 
 */ 
            IsMarketItemBuy = true;
          //WebServerProcess.BuySpecialItem(sItemID);//買汽油
        }
        else
        {
            return;
        }
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
    void GetMarketDataFromWebServer()
    {
        //get date from webServer
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            iMaxMarketItemNumber = WebServerProcess.ShopItemList.Count;
            if (iMaxMarketItemNumber <= 0) return;
            //
            MarketItemList = new RandomDisplayMarketItem[iMaxMarketItemNumber];
            for (int i = 0; i < iMaxMarketItemNumber; i++)
            {
                ShopItemData item = (ShopItemData)WebServerProcess.ShopItemList[i];
                //
                MarketItemList[i] = new RandomDisplayMarketItem();
                MarketItemList[i].sMarketItemID     = string.Copy(item.ID) ;
	            MarketItemList[i].sMarketItemName   = string.Copy(item.Name) ;//名稱
                MarketItemList[i].sMarketItemIcon   = string.Copy(item.Icon) ;
                MarketItemList[i].sMarketItemDescription = string.Copy(item.Description);//介紹                
                MarketItemList[i].iMarketItemMoney  = item.Money;//定價
                //MarketItemList[i].iMarketItemDYMoney= item.DYMoney;//定價動游幣
                MarketItemList[i].iMarketItemOnSale = 100 - item.Discount;//折扣(%). 折扣後的.
                MarketItemList[i].iMarketItemCost   = (MarketItemList[i].iMarketItemMoney * MarketItemList[i].iMarketItemOnSale) / 100;//售價
                MarketItemList[i].iMarketItemDYCost = (MarketItemList[i].iMarketItemDYMoney * MarketItemList[i].iMarketItemOnSale) / 100;//售價
                MarketItemList[i].iMarketItemStatus = item.Status;//狀態 0:無法購買 1:可購買
            }
            //玩家資料 WebServerProcess.User
            //user.UserID; //玩家代號
            //user.UserLevel; //等級
            //user.Exp; //經驗值
            //user.NowExp; //目前經驗值
            //user.NextExp; //下一級所需經驗值
            //user.Money; //動游幣
            //user.DYMoney; //動游幣
            //取得玩家資料
            iMoney      = WebServerProcess.User.Money;
            iLevel      = WebServerProcess.User.UserLevel;
            iExp        = WebServerProcess.User.Exp;
            iDYGameMoney= WebServerProcess.User.DYMoney;
 */ 
        }
        else
        {           
            //隨機產生9件商品
            iMaxMarketItemNumber = 9;
            MarketItemList = new RandomDisplayMarketItem[9];
            for (int i = 0; i < 9; i++)
            {
                MarketItemList[i] = new RandomDisplayMarketItem();
                MarketItemList[i].iMarketItemOnSale = UnityEngine.Random.Range(50, 100);
                int iTemp = 1;
                int iCashDigit = UnityEngine.Random.Range(4, 10);
                for (int j = 0; j < iCashDigit; j++) iTemp *= 10;
                MarketItemList[i].iMarketItemCost = (iTemp * MarketItemList[i].iMarketItemOnSale) / 100;
            }
            MarketItemList[0].sMarketItemName = "一般汽油";
            MarketItemList[0].sMarketItemDescription = "一般的汽油.";
            MarketItemList[1].sMarketItemName = "高級汽油";
            MarketItemList[1].sMarketItemDescription = "高級的汽油.";
            MarketItemList[2].sMarketItemName = "超級汽油";
            MarketItemList[2].sMarketItemDescription = "超級的汽油.";
            MarketItemList[3].sMarketItemName = "特級汽水";
            MarketItemList[3].sMarketItemDescription = "特級的汽水.";
            MarketItemList[4].sMarketItemName = "修車工具";
            MarketItemList[4].sMarketItemDescription = "一般的修車工具.";
            MarketItemList[5].sMarketItemName = "高級修車工具";
            MarketItemList[5].sMarketItemDescription = "高級的修車工具.";
            MarketItemList[6].sMarketItemName = "超級修車工具";
            MarketItemList[6].sMarketItemDescription = "超級的修車工具.";
            MarketItemList[7].sMarketItemName = "特級修車工具";
            MarketItemList[7].sMarketItemDescription = "扳手.";
            MarketItemList[8].sMarketItemName = "高級快艇";
            MarketItemList[8].sMarketItemDescription = "高級的快艇模型.";
            iLevel  = UnityEngine.Random.Range(1, 1000);
            iMoney  = UnityEngine.Random.Range(1, 1000000000);
            iExp    = UnityEngine.Random.Range(1, 1000000000);
            iDYGameMoney = 0;
        }
    }
    //買了之後等結果,有/成功 就更新
    void WaitMarketDataResultFromWebServer()
    {
         //get date from webServer
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            if (WebServerProcess.User.IsUpdate)
            {
                WebServerProcess.User.IsUpdate = false;
            }
            else
            {
                if (IsMarketItemBuy)
                {
                    if (WebServerProcess.ServerResult == enumServerResult.Uploading)
                    {
                        UnityEngine.Debug.Log("Player match processing!!!");
                    }
                    else if (WebServerProcess.ServerResult == enumServerResult.Succeed)
                    {
                        //UnityEngine.Debug.Log("Player match  success!!!");
                        IsMarketItemBuy = false;
                        iMoney = WebServerProcess.User.Money;
                        iLevel = WebServerProcess.User.UserLevel;
                        iExp = WebServerProcess.User.Exp;
                        iDYGameMoney = WebServerProcess.User.DYMoney;
                        SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.LEVEL, iLevel, 0);
                        SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.MONEY, iMoney, 0);
                        SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.EXP, iExp, 0);
                    }
                    else if (WebServerProcess.ServerResult == enumServerResult.Error)
                    {
                        //UnityEngine.Debug.Log("Player match error!!!" + WebServerProcess.ResultStatus);                     
                    }
                }
           }
 */ 
        }
        else
        {
            IsMarketItemBuy = false;    
        }
    }
    //
    void onMarketItemMove( int val )
	{
		if( Time.time < lastMoveTime )
			return;
		lastMoveTime = Time.time + moveDelayTime;
		
		JSK_SoundProcess.PlaySound("MenuMove");
		
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;

		int iButton = 0 ;
		if( val < 0 )
		{
            iCurrentMarketItemIndex--;
            if (iCurrentMarketItemIndex < 0) iCurrentMarketItemIndex = iMaxMarketItemNumber - 1;
			iButton = (int)ENUM_DIRECTION.LEFT ;
		}
		else if( val > 0)
		{
            iCurrentMarketItemIndex++;
            if (iCurrentMarketItemIndex >= iMaxMarketItemNumber) iCurrentMarketItemIndex = 0;
			iButton = (int)ENUM_DIRECTION.RIGHT ;
		}
		//
        int iCost = MarketItemList[iCurrentMarketItemIndex].iMarketItemDYMoney;
        int iOnSale = MarketItemList[iCurrentMarketItemIndex].iMarketItemOnSale;    	
		//按鈕壓下
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.BUTTON,	iButton	,1) ;
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.ONSALE,	iOnSale	,0) ;
        SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.ITEM,     iCurrentMarketItemIndex, 0);
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.CASH,		iCost	,0) ;
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.LEVEL,	iLevel	,0) ;
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.MONEY,	iMoney	,0) ;
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.EXP,		iExp	,0) ;
        string sItemCost = "";
        if (iCost > 0)
        {
            sItemCost = "動游幣 " + MarketItemList[iCurrentMarketItemIndex].iMarketItemDYCost; ;
        }
        else
        {
            sItemCost = "遊戲幣 " + MarketItemList[iCurrentMarketItemIndex].iMarketItemCost;
        }
        SetMallMarketItemContent(MarketItemList[iCurrentMarketItemIndex].sMarketItemName, MarketItemList[iCurrentMarketItemIndex].sMarketItemDescription, sItemCost);
	}
	//
	void SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE state, int iIndex , int iOption)
	{
		switch( state )
		{
		case JSK_ENUM_MALL_MARKET_STATE.NONE: break;
		case JSK_ENUM_MALL_MARKET_STATE.ONSALE:
		{
			int iMaxDigit	= 3 ;//位數上限
			int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.FILL_ZERO ;//行為
			int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.MARKET_ONSALE_WHITE ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(onSalePercent , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
			break;
		}	
		case JSK_ENUM_MALL_MARKET_STATE.ITEM:
		{
			//default item
			if ((iIndex > 9) || (iIndex < 0)) break ;//out of range
			int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(onSaleMiddleMenu , iIndex , iSpriteSet) ;
            //左右鄰兵的物品Icon
			int iLeft = iIndex - 1 ;
			int iRight= iIndex + 1 ;
            if (iLeft < 0) iLeft = iMaxMarketItemNumber - 1;
            if (iRight >= iMaxMarketItemNumber) iRight = 0;
			JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(onSaleLeftMenu	, iLeft , iSpriteSet) ;
			JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(onSaleRightMenu, iRight, iSpriteSet) ;
			break ;
		}
		case JSK_ENUM_MALL_MARKET_STATE.CASH:
		{
			int iMaxDigit	= 10 ;//位數上限
			int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE ;//行為
			int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.MARKET_ONCOST_BLUE ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(onSaleCash , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
			break ;
		}
		case JSK_ENUM_MALL_MARKET_STATE.LEVEL:
		{
			int iMaxDigit	= 3 ;//位數上限
			int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
			int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(onSaleLevel , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
			break ;
		}
		case JSK_ENUM_MALL_MARKET_STATE.MONEY:
		{
			int iMaxDigit	= 10 ;//位數上限
			int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
			int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.MONEY_YELLOW ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(onSaleMoney , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
			break ;
		}
		case JSK_ENUM_MALL_MARKET_STATE.EXP:
		{
			int iMaxDigit	= 10 ;//位數上限
			int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
			int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.EXP_BLUE ;//圖庫
			JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(onSaleExp , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
			break ;
		}
		case JSK_ENUM_MALL_MARKET_STATE.BUTTON:
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
			//
			if (iOption == 1) 
			{
				IsButtonPressDown = true ;
				lastButtonTime = Time.time + 0.1f ;
			}
			break;
		}		
		default:break ;
		}
	}
	//
    void SetMallMarketItemContent(string sItemName, string sDescription, string sItemCost)
	{
		TextMesh mTextMesh = menuRoot.FindChild("TextItemName").GetComponent<TextMesh>();
		mTextMesh.text = sItemName ;
		//
		mTextMesh = menuRoot.FindChild("TextItemDescription").GetComponent<TextMesh>();
		mTextMesh.text = sDescription ;
        //
        mTextMesh = menuRoot.FindChild("TextItemCost").GetComponent<TextMesh>();
        mTextMesh.text = sItemCost;
	}
	//
	void SetRestoreButtonState()
	{
		if( Time.time < lastButtonTime ) return;
		IsButtonPressDown = false ;
		if( leftArrowMenu.activeSelf == true)	SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.BUTTON	,(int)ENUM_DIRECTION.LEFT,	0) ;
		if( rightArrowMenu.activeSelf == true)	SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.BUTTON	,(int)ENUM_DIRECTION.RIGHT,	0) ;
		//
		SetMarketMenuState(JSK_ENUM_MALL_MARKET_STATE.ITEM,	iCurrentMarketItemIndex	,0);
	}
}
//
public enum JSK_ENUM_MALL_MARKET_STATE
{
	NONE,
	ITEM,
	ONSALE,
	CASH,
	LEVEL,
	MONEY,
	EXP,
	BUTTON,
	MAX
}