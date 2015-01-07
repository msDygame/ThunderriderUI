using UnityEngine;
using System.Collections;
using System ;//for BitConverter

public enum ENUM_SPRITE_RENDERER_BEHAVIOUR : int
{
	FILL_ZERO = 0,			//最高位數為0時補0,						Ex:"0359"
	LEFT_ZERO_DISABLE = 1,	//從最高位數開始,最高位數為0時disable它,	Ex:" 359"
	RIGHT_ZERO_DISABLE = 2,	//從個位數開始,最高位數為0時disable它,		Ex:"359 "
	MAX
}

public enum ENUM_SPRITE_PACKAGE : int
{
	NONE				= 0,
	MONEY_YELLOW		= 1,
	LEVEL_BLUE			= 2,
	EXP_BLUE			= 3,
	STAGE_LAP_WHITE_BLUE= 4,
	MATCH_WHITE			= 5,
	MOTO_UPGRADE_CASH_WHITE = 6,
	MOTO_UPGRADE_CAR_ICON = 7,
	MARKET_ONSALE_WHITE	= 8,
	MARKET_ONCOST_BLUE	= 9,
    DAILY_DAY_INDEX     = 10,
    DAILY_ITEM_INDEX    = 11,
    DAILY_COUNT_WHITE   = 12,
    DAILY_STATUS_INDEX  = 13,
    DAILY_DAY_RED_INDEX = 14,
	MAX
}

public class JSK_SpriteRendererUtility : MonoBehaviour 
{
	public 	Sprite[]	SpriteMoneyYellow = null ;		//index=1
	public 	Sprite[]	SpriteLevelBlue = null ;		//2
	public 	Sprite[]	SpriteExpBlue = null ;			//3
	public 	Sprite[]	SpriteStageLapWhiteBlue = null ;//4
	public 	Sprite[]	SpriteMatchPlayerWhite = null ;	//5
	public 	Sprite[]	SpriteMotoUpgradeCashWhite = null ;//6
	public 	Sprite[]	SpriteMotoUpgradeCarIcon = null ;//7
	public 	Sprite[]	SpriteMarketOnSaleWhite = null ;//8
	public 	Sprite[]	SpriteMarketOnCostBlue = null ;	//9
    public  Sprite[]    SpriteDailyDayIndex = null ;//10
    public  Sprite[]    SpriteDailyItemIndex = null ;//11
    public  Sprite[]    SpriteDailyCountWhite = null ;//12
    public  Sprite[]    SpriteDailyStatisIndex = null;//13
    public  Sprite[]    SpriteDailyDayRedIndex = null;//14
	//self
	public		static 	GameObject	JSK_SpriteRendererObject = null ;//指向自己的prefab
	protected	static 	JSK_SpriteRendererUtility Self;
	protected	Sprite 	SpriteTemp = null ;
	//
	//Init
	void Awake()
	{
		DontDestroyOnLoad(gameObject);//保護自己不被刪除
		gameObject.name = "SpriteRendererUtility(DontDestroy)" ;
	}
	// Use this for initialization
	void Start () 
	{
		Self = this;
	}	
	// Update is called once per frame
	void Update () 
	{
	
	}
	//Instance
	public static JSK_SpriteRendererUtility Instance()
	{
		return Self;
	}
	//Set GameObject 的 SpriteRenderer 給指定 value 的數字
	public void SetGameObjectSprite(GameObject gameObject , int iValue , int iSpritePackageIndex)
	{
		switch( iSpritePackageIndex )
		{
			case (int)ENUM_SPRITE_PACKAGE.NONE : return ;
			case (int)ENUM_SPRITE_PACKAGE.MONEY_YELLOW:SpriteTemp = SpriteMoneyYellow[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE:SpriteTemp = SpriteLevelBlue[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.EXP_BLUE:SpriteTemp = SpriteExpBlue[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.STAGE_LAP_WHITE_BLUE:SpriteTemp = SpriteStageLapWhiteBlue[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.MATCH_WHITE:SpriteTemp = SpriteMatchPlayerWhite[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CASH_WHITE:SpriteTemp = SpriteMotoUpgradeCashWhite[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON:SpriteTemp = SpriteMotoUpgradeCarIcon[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.MARKET_ONSALE_WHITE:SpriteTemp = SpriteMarketOnSaleWhite[iValue]; break ;
			case (int)ENUM_SPRITE_PACKAGE.MARKET_ONCOST_BLUE:SpriteTemp = SpriteMarketOnCostBlue[iValue]; break ;
            case (int)ENUM_SPRITE_PACKAGE.DAILY_DAY_INDEX:SpriteTemp = SpriteDailyDayIndex[iValue]; break ;
            case (int)ENUM_SPRITE_PACKAGE.DAILY_ITEM_INDEX:SpriteTemp = SpriteDailyItemIndex[iValue]; break ;
            case (int)ENUM_SPRITE_PACKAGE.DAILY_COUNT_WHITE:SpriteTemp = SpriteDailyCountWhite[iValue]; break ;
            case (int)ENUM_SPRITE_PACKAGE.DAILY_STATUS_INDEX:SpriteTemp = SpriteDailyStatisIndex[iValue]; break ;
            case (int)ENUM_SPRITE_PACKAGE.DAILY_DAY_RED_INDEX:SpriteTemp = SpriteDailyDayRedIndex[iValue]; break;
			default: return ;
		}
		//set sprite
		gameObject.transform.GetComponent<SpriteRenderer>().sprite = SpriteTemp ;
	}
	//
	public void SetGameObjectNumber(GameObject[] gameObject , int iValue , int iDigit , int iSpritePackageIndex , int iBehaviour)
	{
		int iMaxDigit = iDigit ;//位數上限
		int iLimit = 1 ;
		for (int i = 0 ; i < iMaxDigit ; i++) iLimit *= 10 ;
		if ((iValue >= iLimit) || (iValue < 0)) iValue = 0 ;//out of range
		string sResult = Convert.ToString(iValue) ;
		if (sResult.Length > iMaxDigit)
			return ;//不可能出現?
		else if (sResult.Length == iMaxDigit)
		{
			for (int i = 0 ; i < iMaxDigit ; i++)
			{
				gameObject[i].SetActive(true);//reset enable
				string s = "" + sResult[i] ;//位數與欄位數相符,從那邊開始都可
				int iOut = int.Parse(s);
				Instance().SetGameObjectSprite(gameObject[i] , iOut , iSpritePackageIndex) ;
			}
		}
		else
		{
			for (int i = 0 ; i < iMaxDigit ; i++) gameObject[i].SetActive(true);//reset enable
			//
			int iFillCount = 0 ;
			if (iBehaviour == (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.FILL_ZERO)//最高位數為0時補0
			{
				string sTemp  = sResult.PadLeft(iDigit, '0');//高位數,不足補0
				sResult = sTemp ;
			}
			else if (iBehaviour == (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE)//從最高位數開始,最高位數為0時disable它		
			{
				iFillCount = iMaxDigit - sResult.Length ;
			}
			else if (iBehaviour == (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE)//從個位數開始,最高位數為0時disable它
			{
				iFillCount = 0 ;
			}
			int iCount = 0 ;
			for (int i = 0 ; i < iFillCount ; i++) 
			{
				gameObject[i].SetActive(false);//從最高位數開始,把未顯示的位數Hide
				iCount++ ;
			}
			for (int i = 0 ; i < sResult.Length ; i++)
			{
				string s = "" + sResult[i] ;
				int iOut = int.Parse(s);
				Instance().SetGameObjectSprite(gameObject[iFillCount+i] , iOut , iSpritePackageIndex) ;
				iCount++ ;
			}
			for (/*    */; iCount < iMaxDigit ; iCount++)
			{
				gameObject[iCount].SetActive(false);//把未顯示的位數Hide
			}
		}
	}
}
