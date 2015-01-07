using UnityEngine;
using System.Collections;
using System.ComponentModel;//for enum Description
using System.Reflection;//for enum FieldInfo , DescriptionAttribute
//3狀態(已購買,可購買,不能買)
public enum ENUM_UPGRADE_STATE : int
{ 
	UPGRADED = 0,//已購買
	ENABLE   = 1,//可購買
	DISABLE  = 2,//不能買
	SELECTED = 3,//附加狀態:選框圈選中,可與3狀態疊加
	MAX, 
}
//現有9輛車,分三系列(Type):未來,重機,魚型,每系列三等級(Level)
public enum JSK_ENUM_MOTO_LIST : int
{
    [Description("")]
    NONE = 0,
    //enum,       iIndex,sIndex for webServer,ArtModelName,ItemName,ItemDescripte   GamePlayIndex
    [Description("0301")]
    TYPE_1_LEVEL_1 = 7,//B0301	tr_moto_007	軍武系的鯊魚機     鯊魚艦                   7  
    [Description("0302")]
    TYPE_1_LEVEL_2 = 8,//B0302	tr_moto_008	軍武系的旗魚機     旗魚艦                   8
    [Description("0303")]
    TYPE_1_LEVEL_3 = 9,//B0303	tr_moto_009	軍武系的鎚頭鯊機   鎚頭鯊艦                 9
    [Description("0201")]
    TYPE_2_LEVEL_1 = 4,//B0201	tr_moto_004	重機-紅            紅色新星                 4
    [Description("0202")]
    TYPE_2_LEVEL_2 = 5,//B0202	tr_moto_005	重機-白            白色流星                 5
    [Description("0203")]
    TYPE_2_LEVEL_3 = 6,//B0203	tr_moto_006	重機-火焰          火焰慧星                 6
    [Description("0101")]
    TYPE_3_LEVEL_1 = 3,//B0101	tr_moto_003	未來系飛船         --                       3
    [Description("0102")]
    TYPE_3_LEVEL_2 = 2,//B0102	tr_moto_002	未來系火焰         --                       2
    [Description("0103")]
    TYPE_3_LEVEL_3 = 1,//B0103	tr_moto_001	未來系冰河		   --                       1
    MAX = 10,//TYPE * LEVEL = 9
    MAX_LEVEL = 3,
    MAX_TYPE = 3,
}

//可升級的屬性分為六類
public enum ENUM_UPGRADE_ATTRIBUTE : int
{ 
	STAR = 0,//星(1~3)
	GRAVITY = 1,//重力(1~3)
	POWER = 2,//爆發(1~3)
	DEXTERITY = 3,//敏捷(1~3)
	SPEED = 4,//速度(1~3)
	NEXT_MOTO = 5,//同系列下一款新車(1)
	MAX, 
}

public class JSK_MotoTalentTreeMenuProcess : MonoBehaviour
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
	//縱:重力,爆發,敏捷,速度,買車 ;
	//橫:一星,二星,三星,一級,二級,三級
	private JSK_ENUM_TALENT_TREE_STATE currentSelected = JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL ;//當前選框
	private JSK_ENUM_TALENT_TREE_STATE currentButton   = JSK_ENUM_TALENT_TREE_STATE.STAR_1_BUTTON;//當前箭頭按鈕
    private int         iCurrentSelectedMoto = 0; //由選車升級頁面(MotoUpgrade)所選擇的車的索引(JSK_ENUM_MOTO_LIST)
	//
	private GameObject 	nextButtonMenu		= null;	//确认键.(A)升級
	private GameObject 	backButtonMenu		= null;	//返回键.(X)返回
	private GameObject 	nextButtonActive	= null;	//确认键.
	private GameObject 	backButtonActive	= null;	//返回键.
	//moto talent tree state
	private GameObject[] PlayerStar1		;//一星
	private GameObject[] PlayerStar2		;//二星
	private GameObject[] PlayerStar3		;//三星
	private GameObject[] PlayerGravity1		;//重力一級
	private GameObject[] PlayerGravity2		;//重力二級
	private GameObject[] PlayerGravity3		;//重力三級
	private GameObject[] PlayerPower1		;//爆發一級
	private GameObject[] PlayerPower2		;//爆發二級
	private GameObject[] PlayerPower3		;//爆發三級
	private GameObject[] PlayerDexterity1	;//敏捷一級
	private GameObject[] PlayerDexterity2	;//敏捷二級
	private GameObject[] PlayerDexterity3	;//敏捷三級
	private GameObject[] PlayerSpeed1		;//速度一級
	private GameObject[] PlayerSpeed2		;//速度二級
	private GameObject[] PlayerSpeed3		;//速度三級
	private GameObject[] PlayerMotoSelected	;//車(旋轉的)
	private GameObject[] PlayerNextMotoBuy	;//買下一款車的背景
	private GameObject[] PlayerNextMotoIcon	;//買下一款車模型
	private GameObject[] TalentCash			;//科技樹介面消費金額 
	private GameObject[] TalentLevel		;//科技樹介面Level;
	private GameObject[] TalentMoney		;//科技樹介面Money;
	private GameObject[] TalentExp			;//科技樹介面Exp  ;
	private GameObject[] TalentDYCoin		;//科技樹介面動游幣;
	private GameObject[] ButtonStar1Menu	;//箭頭壓下.
	private GameObject[] ButtonStar2Menu	;//箭頭壓下.
	private GameObject[] ButtonStar3Menu	;//箭頭壓下.
	private GameObject[] ButtonGravity1Menu	;//箭頭壓下.
	private GameObject[] ButtonGravity2Menu	;//箭頭壓下.
	private GameObject[] ButtonGravity3Menu	;//箭頭壓下.
	private GameObject[] ButtonDexterity1Menu	;//箭頭壓下.
	private GameObject[] ButtonDexterity2Menu	;//箭頭壓下.
	private GameObject[] ButtonDexterity3Menu	;//箭頭壓下.
	private GameObject[] ButtonPower1Menu	;//箭頭壓下.
	private GameObject[] ButtonPower2Menu	;//箭頭壓下.
	private GameObject[] ButtonPower3Menu	;//箭頭壓下.
	private GameObject[] ButtonSpeed1Menu	;//箭頭壓下.
	private GameObject[] ButtonSpeed2Menu	;//箭頭壓下.
	private GameObject[] ButtonSpeed3Menu	;//箭頭壓下.
	private GameObject[] ButtonNextMotoBuy	;//箭頭壓下.
	//
    int iPlayerGravity = 0;// 重力等級
    int iPlayerSpeed = 0;// 速度等級
    int iPlayerPower = 0;// 爆發力等級
    int iPlayerDexterity = 0; // 敏捷度等級
	private int[]		iStarLevel = new int[3] ;
	private int[]		iGravityLevel = new int[3] ;
	private int[]		iPowerLevel = new int[3] ;
	private int[]		iDexterityLevel = new int[3] ;
	private int[]		iSpeedLevel = new int[3] ;
	private int			iNextMotoBuy = (int)ENUM_UPGRADE_STATE.DISABLE ;
	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
		//
		nextButtonMenu   = menuRoot.Find("new_tr-market_bt/tr_mk_market_bt_a01").gameObject;
		backButtonMenu   = menuRoot.Find("new_tr-market_bt/tr_mk_market_bt_x01").gameObject;
		nextButtonActive = menuRoot.Find("new_tr-market_bt/tr_mk_market_bt_a02").gameObject;
		backButtonActive = menuRoot.Find("new_tr-market_bt/tr_mk_market_bt_x02").gameObject;
		nextButtonMenu.SetActive(false);
		backButtonMenu.SetActive(false);
		//4狀態(已購買,可購買,不能買,圈選中)
		PlayerStar1 = new GameObject[4] ;//一星
		PlayerStar1[0]		= menuRoot.Find("new_tr-market_1star/tr_mk_market_1star_02").gameObject;
		PlayerStar1[1]		= menuRoot.Find("new_tr-market_1star/tr_mk_market_1star_01").gameObject;
		PlayerStar1[2]		= menuRoot.Find("new_tr-market_1star/tr_mk_market_1star_03").gameObject;
		PlayerStar1[3]		= menuRoot.Find("new_tr-market_1star/tr_mk_market_light_01").gameObject;
		PlayerStar2 = new GameObject[4] ;//二星
		PlayerStar2[0]		= menuRoot.Find("new_tr-market_2star/tr_mk_market_2star_02").gameObject;
		PlayerStar2[1]		= menuRoot.Find("new_tr-market_2star/tr_mk_market_2star_01").gameObject;
		PlayerStar2[2]		= menuRoot.Find("new_tr-market_2star/tr_mk_market_2star_03").gameObject;
		PlayerStar2[3]		= menuRoot.Find("new_tr-market_2star/tr_mk_market_light_01").gameObject;
		PlayerStar3 = new GameObject[4] ;//三星
		PlayerStar3[0]		= menuRoot.Find("new_tr-market_3star/tr_mk_market_3star_02").gameObject;
		PlayerStar3[1]		= menuRoot.Find("new_tr-market_3star/tr_mk_market_3star_01").gameObject;
		PlayerStar3[2]		= menuRoot.Find("new_tr-market_3star/tr_mk_market_3star_03").gameObject;
		PlayerStar3[3]		= menuRoot.Find("new_tr-market_3star/tr_mk_market_light_01").gameObject;
		PlayerGravity1 = new GameObject[4] ;//重力一級
		PlayerGravity1[0]	= menuRoot.Find("tr_mk_newmarket_gravity_a/tr_mk_newmarket_gravity_a01").gameObject;
		PlayerGravity1[1]	= menuRoot.Find("tr_mk_newmarket_gravity_a/tr_mk_newmarket_gravity_a02").gameObject;
		PlayerGravity1[2]	= menuRoot.Find("tr_mk_newmarket_gravity_a/tr_mk_newmarket_gravity_a03").gameObject;
		PlayerGravity1[3]	= menuRoot.Find("tr_mk_newmarket_gravity_a/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerGravity2 = new GameObject[4] ;//重力二級
		PlayerGravity2[0]	= menuRoot.Find("tr_mk_newmarket_gravity_b/tr_mk_newmarket_gravity_b01").gameObject;
		PlayerGravity2[1]	= menuRoot.Find("tr_mk_newmarket_gravity_b/tr_mk_newmarket_gravity_b02").gameObject;
		PlayerGravity2[2]	= menuRoot.Find("tr_mk_newmarket_gravity_b/tr_mk_newmarket_gravity_b03").gameObject;
		PlayerGravity2[3]	= menuRoot.Find("tr_mk_newmarket_gravity_b/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerGravity3 = new GameObject[4] ;//重力三級
		PlayerGravity3[0]	= menuRoot.Find("tr_mk_newmarket_gravity_c/tr_mk_newmarket_gravity_c01").gameObject;
		PlayerGravity3[1]	= menuRoot.Find("tr_mk_newmarket_gravity_c/tr_mk_newmarket_gravity_c02").gameObject;
		PlayerGravity3[2]	= menuRoot.Find("tr_mk_newmarket_gravity_c/tr_mk_newmarket_gravity_c03").gameObject;
		PlayerGravity3[3]	= menuRoot.Find("tr_mk_newmarket_gravity_c/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerPower1 = new GameObject[4] ;//爆發一級
		PlayerPower1[0]	= menuRoot.Find("tr_mk_newmarket_power_a/tr_mk_newmarket_gravity_a01").gameObject;
		PlayerPower1[1]	= menuRoot.Find("tr_mk_newmarket_power_a/tr_mk_newmarket_gravity_a02").gameObject;
		PlayerPower1[2]	= menuRoot.Find("tr_mk_newmarket_power_a/tr_mk_newmarket_gravity_a03").gameObject;
		PlayerPower1[3]	= menuRoot.Find("tr_mk_newmarket_power_a/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerPower2 = new GameObject[4] ;//爆發二級
		PlayerPower2[0]	= menuRoot.Find("tr_mk_newmarket_power_b/tr_mk_newmarket_gravity_b01").gameObject;
		PlayerPower2[1]	= menuRoot.Find("tr_mk_newmarket_power_b/tr_mk_newmarket_gravity_b02").gameObject;
		PlayerPower2[2]	= menuRoot.Find("tr_mk_newmarket_power_b/tr_mk_newmarket_gravity_b03").gameObject;
		PlayerPower2[3]	= menuRoot.Find("tr_mk_newmarket_power_b/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerPower3 = new GameObject[4] ;//爆發三級
		PlayerPower3[0]	= menuRoot.Find("tr_mk_newmarket_power_c/tr_mk_newmarket_gravity_c01").gameObject;
		PlayerPower3[1]	= menuRoot.Find("tr_mk_newmarket_power_c/tr_mk_newmarket_gravity_c02").gameObject;
		PlayerPower3[2]	= menuRoot.Find("tr_mk_newmarket_power_c/tr_mk_newmarket_gravity_c03").gameObject;
		PlayerPower3[3]	= menuRoot.Find("tr_mk_newmarket_power_c/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerDexterity1 = new GameObject[4] ;//敏捷一級
		PlayerDexterity1[0]	= menuRoot.Find("tr_mk_newmarket_dexterity_a/tr_mk_newmarket_gravity_a01").gameObject;
		PlayerDexterity1[1]	= menuRoot.Find("tr_mk_newmarket_dexterity_a/tr_mk_newmarket_gravity_a02").gameObject;
		PlayerDexterity1[2]	= menuRoot.Find("tr_mk_newmarket_dexterity_a/tr_mk_newmarket_gravity_a03").gameObject;
		PlayerDexterity1[3]	= menuRoot.Find("tr_mk_newmarket_dexterity_a/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerDexterity2 = new GameObject[4] ;//敏捷二級
		PlayerDexterity2[0]	= menuRoot.Find("tr_mk_newmarket_dexterity_b/tr_mk_newmarket_gravity_b01").gameObject;
		PlayerDexterity2[1]	= menuRoot.Find("tr_mk_newmarket_dexterity_b/tr_mk_newmarket_gravity_b02").gameObject;
		PlayerDexterity2[2]	= menuRoot.Find("tr_mk_newmarket_dexterity_b/tr_mk_newmarket_gravity_b03").gameObject;
		PlayerDexterity2[3]	= menuRoot.Find("tr_mk_newmarket_dexterity_b/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerDexterity3 = new GameObject[4] ;//敏捷三級
		PlayerDexterity3[0]	= menuRoot.Find("tr_mk_newmarket_dexterity_c/tr_mk_newmarket_gravity_c01").gameObject;
		PlayerDexterity3[1]	= menuRoot.Find("tr_mk_newmarket_dexterity_c/tr_mk_newmarket_gravity_c02").gameObject;
		PlayerDexterity3[2]	= menuRoot.Find("tr_mk_newmarket_dexterity_c/tr_mk_newmarket_gravity_c03").gameObject;
		PlayerDexterity3[3]	= menuRoot.Find("tr_mk_newmarket_dexterity_c/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerSpeed1 = new GameObject[4] ;//速度一級
		PlayerSpeed1[0]	= menuRoot.Find("tr_mk_newmarket_speed_a/tr_mk_newmarket_gravity_a01").gameObject;
		PlayerSpeed1[1]	= menuRoot.Find("tr_mk_newmarket_speed_a/tr_mk_newmarket_gravity_a02").gameObject;
		PlayerSpeed1[2]	= menuRoot.Find("tr_mk_newmarket_speed_a/tr_mk_newmarket_gravity_a03").gameObject;
		PlayerSpeed1[3]	= menuRoot.Find("tr_mk_newmarket_speed_a/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerSpeed2 = new GameObject[4] ;//速度二級
		PlayerSpeed2[0]	= menuRoot.Find("tr_mk_newmarket_speed_b/tr_mk_newmarket_gravity_b01").gameObject;
		PlayerSpeed2[1]	= menuRoot.Find("tr_mk_newmarket_speed_b/tr_mk_newmarket_gravity_b02").gameObject;
		PlayerSpeed2[2]	= menuRoot.Find("tr_mk_newmarket_speed_b/tr_mk_newmarket_gravity_b03").gameObject;
		PlayerSpeed2[3]	= menuRoot.Find("tr_mk_newmarket_speed_b/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerSpeed3 = new GameObject[4] ;//速度三級
		PlayerSpeed3[0]	= menuRoot.Find("tr_mk_newmarket_speed_c/tr_mk_newmarket_gravity_c01").gameObject;
		PlayerSpeed3[1]	= menuRoot.Find("tr_mk_newmarket_speed_c/tr_mk_newmarket_gravity_c02").gameObject;
		PlayerSpeed3[2]	= menuRoot.Find("tr_mk_newmarket_speed_c/tr_mk_newmarket_gravity_c03").gameObject;
		PlayerSpeed3[3]	= menuRoot.Find("tr_mk_newmarket_speed_c/tr_mk_newmarket_gravity_a04").gameObject;
		PlayerNextMotoBuy = new GameObject[4] ;//買
		PlayerNextMotoBuy[0]= menuRoot.Find("new_tr-market_nextcar/new_tr_mk_market_car02").gameObject;
		PlayerNextMotoBuy[1]= menuRoot.Find("new_tr-market_nextcar/new_tr_mk_market_car01").gameObject;
		PlayerNextMotoBuy[2]= menuRoot.Find("new_tr-market_nextcar/new_tr_mk_market_car03").gameObject;
		PlayerNextMotoBuy[3]= menuRoot.Find("new_tr-market_nextcar/new_tr_mk_market_car04").gameObject;
		PlayerNextMotoIcon = new GameObject[9] ;//九輛車
		PlayerNextMotoIcon[0]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_007").gameObject;
		PlayerNextMotoIcon[1]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_008").gameObject;
		PlayerNextMotoIcon[2]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_009").gameObject;
		PlayerNextMotoIcon[3]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_004").gameObject;
		PlayerNextMotoIcon[4]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_005").gameObject;
		PlayerNextMotoIcon[5]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_006").gameObject;
		PlayerNextMotoIcon[6]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_003").gameObject;
		PlayerNextMotoIcon[7]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_002").gameObject;
		PlayerNextMotoIcon[8]= menuRoot.Find("new_tr_moto_01/tr_mk_mu_moto_001").gameObject;
		PlayerMotoSelected = new GameObject[9] ;//九輛車
		PlayerMotoSelected[0]= menuRoot.Find("new_tr_moto/tr_moto_007").gameObject;
		PlayerMotoSelected[1]= menuRoot.Find("new_tr_moto/tr_moto_008").gameObject;
		PlayerMotoSelected[2]= menuRoot.Find("new_tr_moto/tr_moto_009").gameObject;
		PlayerMotoSelected[3]= menuRoot.Find("new_tr_moto/tr_moto_004").gameObject;
		PlayerMotoSelected[4]= menuRoot.Find("new_tr_moto/tr_moto_005").gameObject;
		PlayerMotoSelected[5]= menuRoot.Find("new_tr_moto/tr_moto_006").gameObject;
		PlayerMotoSelected[6]= menuRoot.Find("new_tr_moto/tr_moto_003").gameObject;
		PlayerMotoSelected[7]= menuRoot.Find("new_tr_moto/tr_moto_002").gameObject;
		PlayerMotoSelected[8]= menuRoot.Find("new_tr_moto/tr_moto_001").gameObject;
		TalentCash = new GameObject[10] ;//10位數
		TalentCash[0]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_0").gameObject;
		TalentCash[1]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_1").gameObject;
		TalentCash[2]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_2").gameObject;
		TalentCash[3]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_3").gameObject;
		TalentCash[4]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_4").gameObject;
		TalentCash[5]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_5").gameObject;
		TalentCash[6]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_6").gameObject;
		TalentCash[7]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_7").gameObject;
		TalentCash[8]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_8").gameObject;
		TalentCash[9]		= menuRoot.Find("new_tr-market_money/tr_mk_market_money_9").gameObject;
		TalentLevel = new GameObject[3] ;//3位數
		TalentLevel[0]		= menuRoot.Find("new_tr_mk_market_lv/tr_mk_market_lv_0").gameObject;
		TalentLevel[1]		= menuRoot.Find("new_tr_mk_market_lv/tr_mk_market_lv_1").gameObject;
		TalentLevel[2]		= menuRoot.Find("new_tr_mk_market_lv/tr_mk_market_lv_2").gameObject;	
		TalentMoney = new GameObject[10] ;//10位數
		TalentMoney[0]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$0").gameObject;
		TalentMoney[1]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$1").gameObject;
		TalentMoney[2]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$2").gameObject;
		TalentMoney[3]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$3").gameObject;
		TalentMoney[4]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$4").gameObject;
		TalentMoney[5]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$5").gameObject;
		TalentMoney[6]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$6").gameObject;
		TalentMoney[7]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$7").gameObject;
		TalentMoney[8]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$8").gameObject;
		TalentMoney[9]		= menuRoot.Find("new_tr_mk_market_$/tr_mk_market_$9").gameObject;
		TalentExp = new GameObject[10] ;//10位數
		TalentExp[0]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_0").gameObject;
		TalentExp[1]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_1").gameObject;
		TalentExp[2]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_2").gameObject;
		TalentExp[3]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_3").gameObject;
		TalentExp[4]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_4").gameObject;
		TalentExp[5]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_5").gameObject;
		TalentExp[6]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_6").gameObject;
		TalentExp[7]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_7").gameObject;
		TalentExp[8]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_8").gameObject;
		TalentExp[9]		= menuRoot.Find("new_tr_mk_market_exp/tr_mk_market_exp_9").gameObject;
		TalentDYCoin = new GameObject[10] ;//10位數
		TalentDYCoin[0]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_0").gameObject;
		TalentDYCoin[1]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_1").gameObject;
		TalentDYCoin[2]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_2").gameObject;
		TalentDYCoin[3]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_3").gameObject;
		TalentDYCoin[4]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_4").gameObject;
		TalentDYCoin[5]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_5").gameObject;
		TalentDYCoin[6]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_6").gameObject;
		TalentDYCoin[7]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_7").gameObject;
		TalentDYCoin[8]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_8").gameObject;
		TalentDYCoin[9]		= menuRoot.Find("new_tr-market_dycash/tr_mk_market_dycash_9").gameObject;
		//default all disable
		for (int i = 0 ; i < 4 ; i++) PlayerStar1[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerStar2[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerStar3[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerGravity1[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerGravity2[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerGravity3[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerPower1[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerPower2[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerPower3[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerDexterity1[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerDexterity2[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerDexterity3[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerSpeed1[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerSpeed2[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerSpeed3[i].SetActive(false) ;
		for (int i = 0 ; i < 4 ; i++) PlayerNextMotoBuy[i].SetActive(false) ;
		for (int i = 1 ; i < 9 ; i++) PlayerNextMotoIcon[i].SetActive(false) ;
		for (int i = 1 ; i < 9 ; i++) PlayerMotoSelected[i].SetActive(false) ;
		//default star
		PlayerStar1[2].SetActive(true) ;
		PlayerStar2[2].SetActive(true) ;
		PlayerStar3[2].SetActive(true) ;
		//
		//4方向
		//enum ENUM_DIRECTION : UP = 1 , DOWN = 2 ,	LEFT = 3 , RIGHT = 4
		ButtonStar1Menu	= new GameObject[4] ;
		ButtonStar1Menu[0]		= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonStar1Menu[1]		= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonStar1Menu[2]		= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;	
		ButtonStar1Menu[3]		= menuRoot.Find("tr_mk_market_aar001/tr_mk_market_aar_04").gameObject;	
		ButtonStar2Menu	= new GameObject[4] ;
		ButtonStar2Menu[0]		= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonStar2Menu[1]		= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonStar2Menu[2]		= menuRoot.Find("tr_mk_market_aar002/tr_mk_market_aar_03").gameObject;	
		ButtonStar2Menu[3]		= menuRoot.Find("tr_mk_market_aar002/tr_mk_market_aar_04").gameObject;	
		ButtonStar3Menu = new GameObject[4] ;
		ButtonStar3Menu[0]		= menuRoot.Find("tr_mk_market_aar003/tr_mk_market_aar_01").gameObject;
		ButtonStar3Menu[1]		= menuRoot.Find("tr_mk_market_aar003/tr_mk_market_aar_02").gameObject;
		ButtonStar3Menu[2]		= menuRoot.Find("tr_mk_market_aar003/tr_mk_market_aar_03").gameObject;	
		ButtonStar3Menu[3]		= menuRoot.Find("tr_mk_market_aar003/tr_mk_market_aar_04").gameObject;	
		ButtonGravity1Menu = new GameObject[4] ;
		ButtonGravity1Menu[0]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonGravity1Menu[1]	= menuRoot.Find("tr_mk_market_aar004/tr_mk_market_aar_02").gameObject;
		ButtonGravity1Menu[2]	= menuRoot.Find("tr_mk_market_aar004/tr_mk_market_aar_03").gameObject;	
		ButtonGravity1Menu[3]	= menuRoot.Find("tr_mk_market_aar004/tr_mk_market_aar_04").gameObject;	
		ButtonGravity2Menu = new GameObject[4] ;
		ButtonGravity2Menu[0]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonGravity2Menu[1]	= menuRoot.Find("tr_mk_market_aar005/tr_mk_market_aar_02").gameObject;
		ButtonGravity2Menu[2]	= menuRoot.Find("tr_mk_market_aar005/tr_mk_market_aar_03").gameObject;	
		ButtonGravity2Menu[3]	= menuRoot.Find("tr_mk_market_aar005/tr_mk_market_aar_04").gameObject;	
		ButtonGravity3Menu = new GameObject[4] ;
		ButtonGravity3Menu[0]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonGravity3Menu[1]	= menuRoot.Find("tr_mk_market_aar006/tr_mk_market_aar_02").gameObject;
		ButtonGravity3Menu[2]	= menuRoot.Find("tr_mk_market_aar006/tr_mk_market_aar_03").gameObject;	
		ButtonGravity3Menu[3]	= menuRoot.Find("tr_mk_market_aar006/tr_mk_market_aar_04").gameObject;	
		ButtonPower1Menu = new GameObject[4] ;
		ButtonPower1Menu[0]	= menuRoot.Find("tr_mk_market_aar007/tr_mk_market_aar_01").gameObject;
		ButtonPower1Menu[1]	= menuRoot.Find("tr_mk_market_aar007/tr_mk_market_aar_02").gameObject;
		ButtonPower1Menu[2]	= menuRoot.Find("tr_mk_market_aar007/tr_mk_market_aar_03").gameObject;	
		ButtonPower1Menu[3]	= menuRoot.Find("tr_mk_market_aar007/tr_mk_market_aar_04").gameObject;	
		ButtonPower2Menu = new GameObject[4] ;
		ButtonPower2Menu[0]	= menuRoot.Find("tr_mk_market_aar008/tr_mk_market_aar_01").gameObject;
		ButtonPower2Menu[1]	= menuRoot.Find("tr_mk_market_aar008/tr_mk_market_aar_02").gameObject;
		ButtonPower2Menu[2]	= menuRoot.Find("tr_mk_market_aar008/tr_mk_market_aar_03").gameObject;	
		ButtonPower2Menu[3]	= menuRoot.Find("tr_mk_market_aar008/tr_mk_market_aar_04").gameObject;	
		ButtonPower3Menu = new GameObject[4] ;
		ButtonPower3Menu[0]	= menuRoot.Find("tr_mk_market_aar009/tr_mk_market_aar_01").gameObject;
		ButtonPower3Menu[1]	= menuRoot.Find("tr_mk_market_aar009/tr_mk_market_aar_02").gameObject;
		ButtonPower3Menu[2]	= menuRoot.Find("tr_mk_market_aar009/tr_mk_market_aar_03").gameObject;	
		ButtonPower3Menu[3]	= menuRoot.Find("tr_mk_market_aar009/tr_mk_market_aar_04").gameObject;	
		ButtonDexterity1Menu = new GameObject[4] ;
		ButtonDexterity1Menu[0]	= menuRoot.Find("tr_mk_market_aar010/tr_mk_market_aar_01").gameObject;
		ButtonDexterity1Menu[1]	= menuRoot.Find("tr_mk_market_aar010/tr_mk_market_aar_02").gameObject;
		ButtonDexterity1Menu[2]	= menuRoot.Find("tr_mk_market_aar010/tr_mk_market_aar_03").gameObject;	
		ButtonDexterity1Menu[3]	= menuRoot.Find("tr_mk_market_aar010/tr_mk_market_aar_04").gameObject;	
		ButtonDexterity2Menu = new GameObject[4] ;
		ButtonDexterity2Menu[0]	= menuRoot.Find("tr_mk_market_aar011/tr_mk_market_aar_01").gameObject;
		ButtonDexterity2Menu[1]	= menuRoot.Find("tr_mk_market_aar011/tr_mk_market_aar_02").gameObject;
		ButtonDexterity2Menu[2]	= menuRoot.Find("tr_mk_market_aar011/tr_mk_market_aar_03").gameObject;	
		ButtonDexterity2Menu[3]	= menuRoot.Find("tr_mk_market_aar011/tr_mk_market_aar_04").gameObject;	
		ButtonDexterity3Menu = new GameObject[4] ;
		ButtonDexterity3Menu[0]	= menuRoot.Find("tr_mk_market_aar012/tr_mk_market_aar_01").gameObject;
		ButtonDexterity3Menu[1]	= menuRoot.Find("tr_mk_market_aar012/tr_mk_market_aar_02").gameObject;
		ButtonDexterity3Menu[2]	= menuRoot.Find("tr_mk_market_aar012/tr_mk_market_aar_03").gameObject;	
		ButtonDexterity3Menu[3]	= menuRoot.Find("tr_mk_market_aar012/tr_mk_market_aar_04").gameObject;	
		ButtonSpeed1Menu = new GameObject[4] ;
		ButtonSpeed1Menu[0]	= menuRoot.Find("tr_mk_market_aar013/tr_mk_market_aar_01").gameObject;
		ButtonSpeed1Menu[1]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonSpeed1Menu[2]	= menuRoot.Find("tr_mk_market_aar013/tr_mk_market_aar_03").gameObject;	
		ButtonSpeed1Menu[3]	= menuRoot.Find("tr_mk_market_aar013/tr_mk_market_aar_04").gameObject;	
		ButtonSpeed2Menu = new GameObject[4] ;
		ButtonSpeed2Menu[0]	= menuRoot.Find("tr_mk_market_aar014/tr_mk_market_aar_01").gameObject;
		ButtonSpeed2Menu[1]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonSpeed2Menu[2]	= menuRoot.Find("tr_mk_market_aar014/tr_mk_market_aar_03").gameObject;	
		ButtonSpeed2Menu[3]	= menuRoot.Find("tr_mk_market_aar014/tr_mk_market_aar_04").gameObject;	
		ButtonSpeed3Menu = new GameObject[4] ;
		ButtonSpeed3Menu[0]	= menuRoot.Find("tr_mk_market_aar015/tr_mk_market_aar_01").gameObject;
		ButtonSpeed3Menu[1]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonSpeed3Menu[2]	= menuRoot.Find("tr_mk_market_aar015/tr_mk_market_aar_03").gameObject;	
		ButtonSpeed3Menu[3]	= menuRoot.Find("tr_mk_market_aar015/tr_mk_market_aar_04").gameObject;
		ButtonNextMotoBuy = new GameObject[4] ;
		ButtonNextMotoBuy[0]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonNextMotoBuy[1]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
		ButtonNextMotoBuy[2]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;	
		ButtonNextMotoBuy[3]	= menuRoot.Find("tr_mk_market_aar001/YOU_CAN_NOT_PASS").gameObject;
	}
	// Use this for initialization
	void Start () 
	{
		onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL) ;//default selected.
		GetBoatDataFromWebServer(0) ;
	}	
	// Update is called once per frame
	void Update () 
	{
/*
        if (WebServerProcess.User.IsUpdate)
        {
            WebServerProcess.User.IsUpdate = false;
        }
 */ 
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
		if (IsButtonPressDown) SetRestoreButtonState() ;
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
			RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2D != null) 
			{
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if( hitMenu == nextButtonActive )	  { nextButtonActive.SetActive(false) ; nextButtonMenu.SetActive(true); }
					else if( hitMenu == backButtonActive ){ backButtonActive.SetActive(false) ; backButtonMenu.SetActive(true); }
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
			}
			RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit2D != null) 
			{
				if (hit2D.collider !=null)
				{
					GameObject hitMenu = hit2D.transform.gameObject;
					if (    (hitMenu == nextButtonMenu) || (hitMenu == nextButtonActive)) { nextButtonActive.SetActive(true) ; nextButtonMenu.SetActive(false); onMenuSelect(); }
					else if((hitMenu == backButtonMenu) || (hitMenu == backButtonActive)) { backButtonActive.SetActive(true) ; backButtonMenu.SetActive(false); onMenuEsc(); }
					else if((hitMenu == PlayerStar1[0]) || (hitMenu == PlayerStar1[1]) || (hitMenu == PlayerStar1[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL) ;
					else if((hitMenu == PlayerStar2[0]) || (hitMenu == PlayerStar2[1]) || (hitMenu == PlayerStar2[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL) ;
					else if((hitMenu == PlayerStar3[0]) || (hitMenu == PlayerStar3[1]) || (hitMenu == PlayerStar3[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL) ;
					else if((hitMenu == PlayerGravity1[0]) || (hitMenu == PlayerGravity1[1]) || (hitMenu == PlayerGravity1[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL) ;
					else if((hitMenu == PlayerGravity2[0]) || (hitMenu == PlayerGravity2[1]) || (hitMenu == PlayerGravity2[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL) ;
					else if((hitMenu == PlayerGravity3[0]) || (hitMenu == PlayerGravity3[1]) || (hitMenu == PlayerGravity3[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL) ;
					else if((hitMenu == PlayerPower1[0]) || (hitMenu == PlayerPower1[1]) || (hitMenu == PlayerPower1[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL) ;
					else if((hitMenu == PlayerPower2[0]) || (hitMenu == PlayerPower2[1]) || (hitMenu == PlayerPower2[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL) ;
					else if((hitMenu == PlayerPower3[0]) || (hitMenu == PlayerPower3[1]) || (hitMenu == PlayerPower3[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL) ;
					else if((hitMenu == PlayerDexterity1[0]) || (hitMenu == PlayerDexterity1[1]) || (hitMenu == PlayerDexterity1[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL) ;
					else if((hitMenu == PlayerDexterity2[0]) || (hitMenu == PlayerDexterity2[1]) || (hitMenu == PlayerDexterity2[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL) ;
					else if((hitMenu == PlayerDexterity3[0]) || (hitMenu == PlayerDexterity3[1]) || (hitMenu == PlayerDexterity3[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL) ;
					else if((hitMenu == PlayerSpeed1[0]) || (hitMenu == PlayerSpeed1[1]) || (hitMenu == PlayerSpeed1[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL) ;
					else if((hitMenu == PlayerSpeed2[0]) || (hitMenu == PlayerSpeed2[1]) || (hitMenu == PlayerSpeed2[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL) ;
					else if((hitMenu == PlayerSpeed3[0]) || (hitMenu == PlayerSpeed3[1]) || (hitMenu == PlayerSpeed3[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL) ;
					else if((hitMenu == PlayerNextMotoBuy[0]) || (hitMenu == PlayerNextMotoBuy[1]) || (hitMenu == PlayerNextMotoBuy[2]) ) onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY) ;
				}
			}
		}
	}
	
	void checkFifoMenu()
	{
		string sInputMsg = JSK_GlobalProcess.GetFifoMsg();
		if     ( sInputMsg.IndexOf("Left") >= 0)		onSelectedMove((int)ENUM_DIRECTION.LEFT);//onMotoMove(-1);
		else if( sInputMsg.IndexOf("Right") >= 0)		onSelectedMove((int)ENUM_DIRECTION.RIGHT);//onMotoMove(1);
		else if( sInputMsg.IndexOf("Up") >= 0 )			onSelectedMove((int)ENUM_DIRECTION.UP);//onMotoMove(-1);
		else if( sInputMsg.IndexOf("Down") >= 0)		onSelectedMove((int)ENUM_DIRECTION.DOWN);//onMotoMove(1);
		else if( sInputMsg.IndexOf("Esc") >= 0 )		onMenuEsc();
        else if (sInputMsg.IndexOf("Back") >= 0)        onMenuEsc();
		else if( sInputMsg.IndexOf("Confirm") >= 0)		onMenuSelect();
		else if( Input.GetKeyDown(KeyCode.X) )			onMenuEsc();
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
				onSelectedMove(0);
		}
	}
	
	void onMenuSelect()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");
		//升級
		int iTargetGroup = 0 ;
		int iTargetLevel = 0 ;
		//找出 被框選的
		SubFunctionUpgradeSelected(ref iTargetGroup , ref iTargetLevel) ;
        //get date from webServer
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            //購買購買快艇物品BoatData (快艇,技能)：傳入快艇編號ItemID，傳回最新玩家資料與交易結果 (快艇表請參照BoatManager內的快艇清單)
            //public static void BuyBoatItem(string ItemID)

            //取得 車或科技樹的ItemID
            string sBoatItemID = "";
            //取得車/技的WebServerItemName
            SubFunctionGetWebServerItemName(iTargetGroup, iTargetLevel, ref sBoatItemID);            
            //買車/買技
            WebServerProcess.BuyBoatItem(sBoatItemID);            
            //Debug
            TextMesh mTextMesh = menuRoot.FindChild("TextDebug").GetComponent<TextMesh>();
            mTextMesh.text = sBoatItemID;

            //購買 被框選的
            SubFunctionUpgradeAndUnlock(iTargetGroup, iTargetLevel);
            //更新 全部科技樹
            SubFunctionUpdateLevel();
 */
        }
        else
        {
            //購買 被框選的
            SubFunctionUpgradeAndUnlock(iTargetGroup, iTargetLevel);
            //更新 全部科技樹
            SubFunctionUpdateLevel();
        }
	}	
	
	void onMenuEsc()
	{
		JSK_SoundProcess.PlaySound("MenuSelect");		
	    nextSceneName = "UI_Motion";		
	}
	//
	void GetBoatDataFromWebServer(int iSelectedBoat)
	{
		if (JSK_GlobalProcess.g_IsWebServer)
		{
/*
			//取得快艇資料
			int iBoatIndex = JSK_MatchedRivalList.Instance().GetMotoUpgradeSelectedBoat();//1~9
            int iType = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevelX = ((iBoatIndex - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            string sOut = "";
            BoatData data = (BoatData)BoatManager.GetBoat(iType, iLevelX , out sOut);
            if (data == null) return;
            iCurrentSelectedMoto = iBoatIndex;
            int iIndex = iBoatIndex - 1;
            if (iIndex < 0) return;
			PlayerMotoSelected[0].SetActive(false) ;
            PlayerMotoSelected[iIndex].SetActive(true);
			PlayerNextMotoIcon[0].SetActive(false) ;
            PlayerNextMotoIcon[iIndex].SetActive(true);
            //data.ID = "";    // 編號
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
			int iPlayerStar = data.Lv ;//快艇星數
			iPlayerGravity = data.LevelGRV ;
			iPlayerSpeed = data.LevelSPD ;
			iPlayerPower = data.LevelPOW ;
			iPlayerDexterity = data.LevelDEX ;
			iNextMotoBuy = (int)ENUM_UPGRADE_STATE.DISABLE ;
			//玩家資料 WebServerProcess.User
			//user.UserID; //玩家代號
			//user.UserLevel; //等級
			//user.Exp; //經驗值
			//user.NowExp; //目前經驗值
			//user.NextExp; //下一級所需經驗值
			//user.Money; //動游幣
			//user.DYMoney; //動游幣
			//取得玩家資料
			int iMoney = WebServerProcess.User.Money ;
			int iLevel = WebServerProcess.User.UserLevel ;
			int iExp   = WebServerProcess.User.Exp ;
			int iDYCoin= WebServerProcess.User.DYMoney ;
            //Debug
            int iResult= WebServerProcess.User.Boats.GetStatus("S0302A2");
			//消費金額
            int iCash  = data.SellMoney ;
			//
			SubFunctionAnalysisLevel(iPlayerStar , ref iStarLevel) ;
			SubFunctionAnalysisLevel(iPlayerGravity , ref iGravityLevel) ;
			SubFunctionAnalysisLevel(iPlayerSpeed , ref iSpeedLevel) ;
			SubFunctionAnalysisLevel(iPlayerPower , ref iPowerLevel) ;
			SubFunctionAnalysisLevel(iPlayerDexterity , ref iDexterityLevel) ;
			//三星解鎖下一系列
			if (iPlayerStar == 3) 
			{	
				iGravityLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
				iPowerLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
				iDexterityLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
				iSpeedLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
				iNextMotoBuy = (int)ENUM_UPGRADE_STATE.ENABLE ;
			}
			SubFunctionUpdateLevel() ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.CASH,		iCash	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.LEVEL,	iLevel	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.MONEY,	iMoney	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.EXP,		iExp	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.DY_COIN,	iDYCoin	,0) ;
            //
            TextMesh mTextMesh = menuRoot.FindChild("TextDescription").GetComponent<TextMesh>();
            mTextMesh.text = data.Description;
            mTextMesh = menuRoot.FindChild("TextData").GetComponent<TextMesh>();
            mTextMesh.text = data.Name;
			//目前最多就九輛,同系列,滿足三星之後才可以解鎖。            
            int iNextBoat = iLevelX + 1;
            if (iNextBoat > (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) 
			{
				//同系列沒下一輛了,選自己,改 已購買
				iNextMotoBuy = (int)ENUM_UPGRADE_STATE.UPGRADED ;
				SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY,	iNextMotoBuy,	0) ;
				return ;
			}
            BoatData NextData = (BoatData)BoatManager.GetBoat(iType, iNextBoat, out sOut);
			//同系列下一輛
            int iNextIcon = iNextBoat - 1;
			PlayerNextMotoIcon[iIndex].SetActive(false) ;
            PlayerNextMotoIcon[iNextIcon].SetActive(true);        
 */ 
		}
		else
		{
			//
			int iCash	= UnityEngine.Random.Range(1, 1000000000);
			int iLevel	= UnityEngine.Random.Range(1, 1000);
			int iMoney	= UnityEngine.Random.Range(1, 1000000000);
			int iExp	= UnityEngine.Random.Range(1, 1000000000);
			int iDYCoin = UnityEngine.Random.Range(1, 1000000000);
			int iPlayerStar = UnityEngine.Random.Range(1, 4);//1~3星
			iPlayerGravity = 0 ;
		    iPlayerSpeed = 0 ;
			iPlayerPower = 0 ;
			iPlayerDexterity = 0 ;
			iNextMotoBuy = (int)ENUM_UPGRADE_STATE.DISABLE ; 
			//
			if (iPlayerStar == 3) 
			{
				iPlayerGravity = UnityEngine.Random.Range(0, 4);//0~3級
				iPlayerSpeed = UnityEngine.Random.Range(0, 4);//0~3級
				iPlayerPower = UnityEngine.Random.Range(0, 4);//0~3級
				iPlayerDexterity = UnityEngine.Random.Range(0, 4);//0~3級
				iNextMotoBuy = (int)ENUM_UPGRADE_STATE.ENABLE ;
			}
			//
			SubFunctionAnalysisLevel(iPlayerStar , ref iStarLevel) ;
			SubFunctionAnalysisLevel(iPlayerGravity , ref iGravityLevel) ;
			SubFunctionAnalysisLevel(iPlayerSpeed , ref iSpeedLevel) ;
			SubFunctionAnalysisLevel(iPlayerPower , ref iPowerLevel) ;
			SubFunctionAnalysisLevel(iPlayerDexterity , ref iDexterityLevel) ;
			SubFunctionUpdateLevel() ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.CASH,		iCash	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.LEVEL,	iLevel	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.MONEY,	iMoney	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.EXP,		iExp	,0) ;
			SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.DY_COIN,	iDYCoin	,0) ;            
		}
	}
	//
	void SubFunctionAnalysisLevel(int iTarget , ref int[] iAnswer)
	{
		if (iTarget <= 0)//out of range
		{
			for (int i = 0 ; i < 3 ; i++) iAnswer[i] = (int)ENUM_UPGRADE_STATE.DISABLE ;//不能
			return ;
		}
		//
		int iCount = 0 ;
		bool IsEnable = false ;//only one Enable
		for (int i = 0 ; i < 3 ; i++)//3狀態(已購買,可購買,不能買)
		{
			if (iCount < iTarget)
			{
				iAnswer[i] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//已購
				iCount++ ;
			}
			else
			{
				if (IsEnable == false)
				{
					iAnswer[i] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購
					iCount++ ;
					IsEnable = true ;//只有一個可購
				}
				else
				{
					iAnswer[i] = (int)ENUM_UPGRADE_STATE.DISABLE ;//不能
					iCount++ ;
				}
			}
		}
	}
	//
	void SubFunctionUpdateLevel()
	{
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL,	iStarLevel[0],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL,	iStarLevel[1],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL,	iStarLevel[2],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL,	iGravityLevel[0],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL,	iGravityLevel[1],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL,	iGravityLevel[2],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL,	iPowerLevel[0],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL,	iPowerLevel[1],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL,	iPowerLevel[2],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL,	iDexterityLevel[0],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL,	iDexterityLevel[1],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL,	iDexterityLevel[2],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL,	iSpeedLevel[0],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL,	iSpeedLevel[1],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL,	iSpeedLevel[2],	0) ;
		SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY,	iNextMotoBuy,	0) ; 
        //
        DrawPolygon RadarChartScript = menuRoot.FindChild("new_tr-market_message_01/tr_mk_market_radar/RadarChart").GetComponent<DrawPolygon>();
        RadarChartScript.value[0] = iPlayerGravity;//上:重力等級
        RadarChartScript.value[1] = iPlayerPower;//右:爆發力等級
        RadarChartScript.value[2] = iPlayerDexterity;//下:敏捷度等級
        RadarChartScript.value[3] = iPlayerSpeed;//左:速度等級
	}
    //      
    void SubFunctionGetWebServerItemName(int iTargetGroup, int iTargetLevel , ref string sItemName)
    {
        //是買車或買技
        bool IsBoatOrTalent = false;//Boat == false ; Talent == true
        switch (iTargetGroup)
		{
            case (int)ENUM_UPGRADE_ATTRIBUTE.STAR: IsBoatOrTalent = true; break;
            case (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY: IsBoatOrTalent = true; break;
            case (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY: IsBoatOrTalent = true; break;
            case (int)ENUM_UPGRADE_ATTRIBUTE.POWER: IsBoatOrTalent = true; break;
            case (int)ENUM_UPGRADE_ATTRIBUTE.SPEED: IsBoatOrTalent = true; break;
            case (int)ENUM_UPGRADE_ATTRIBUTE.NEXT_MOTO: IsBoatOrTalent = false; break;
            default: return;//what happen?
        }
        //step1
        //Ex:Boat = "B0101" , Talent = "S0101A1" ;
        string sItemType = (IsBoatOrTalent) ? "S" : "B";
        //step2        
        int iType = ((iCurrentSelectedMoto - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
        int iLevel = ((iCurrentSelectedMoto - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
        int iNextBoat = iLevel + 1 ;
        if (iNextBoat > (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) //同系列沒下一輛了,選自己
        {
            iNextBoat = iLevel;
        }
        int iNextMoto = iType * (int)JSK_ENUM_MOTO_LIST.MAX_TYPE + iNextBoat;
        int iTemp = (IsBoatOrTalent) ? iCurrentSelectedMoto : iNextMoto;//買技能是買本車的技能;買車是買本系列的下一輛車,故本車+1        
        string sBoatName = "0101";
        JSK_ENUM_MOTO_LIST temp = JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_1;
        switch (iTemp)
        {
            case 0: temp = JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_1 ; break;
            case 1: temp = JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_2 ; break;
            case 2: temp = JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_3 ; break;
            case 3: temp = JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_1 ; break;
            case 4: temp = JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_2 ; break;
            case 5: temp = JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_3 ; break;
            case 6: temp = JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_1 ; break;
            case 7: temp = JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_2 ; break;
            case 8: temp = JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_3 ; break;
            default: return;//what happen?
        }
        FieldInfo EnumInfoBoat = temp.GetType().GetField(temp.ToString());
        DescriptionAttribute[] EnumAttributesBoat = (DescriptionAttribute[])EnumInfoBoat.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (EnumAttributesBoat.Length > 0)
        {
            sBoatName = string.Copy(EnumAttributesBoat[0].Description);
        }
        //step3
        string sLevelName = "";
        FieldInfo EnumInfo = currentSelected.GetType().GetField(currentSelected.ToString());
        DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (EnumAttributes.Length > 0)
        {
            sLevelName = string.Copy(EnumAttributes[0].Description);
        }
        //step4 
        //取得車/技的WebServerItemName 
        sItemName = sItemType + sBoatName + sLevelName;
    }
	//
	void onSelectedTouch(JSK_ENUM_TALENT_TREE_STATE target)
	{
		if( Time.time < lastMoveTime )
			return;
		lastMoveTime = Time.time + moveDelayTime;
		
		JSK_SoundProcess.PlaySound("MenuMove");
		
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;
		//必定可選,清除上一個選框
		SetTalentTreeState(currentSelected	,(int)ENUM_UPGRADE_STATE.SELECTED,1) ;
		//換選框
		currentSelected = target ;
		//顯示選框
		SetTalentTreeState(currentSelected	,(int)ENUM_UPGRADE_STATE.SELECTED,0) ;
		//內容說明
		int iTargetGroup = 0 ;
		int iTargetLevel = 0 ;
		//找出 被框選的
		SubFunctionUpgradeSelected(ref iTargetGroup , ref iTargetLevel) ;
		SubFunctionTalentDescription(iTargetGroup , iTargetLevel) ;
	}
	//
	void onSelectedMove( int val )
	{
		if( Time.time < lastMoveTime )
			return;
		lastMoveTime = Time.time + moveDelayTime;
		
		JSK_SoundProcess.PlaySound("MenuMove");
		
		if( JSK_GlobalProcess.g_ModuleVerson == 0 )
			return;
		//temp for current
		JSK_ENUM_TALENT_TREE_STATE temp = JSK_ENUM_TALENT_TREE_STATE.NONE ;
		//是否可選
		if (false == SubFunctionEnableSelected( val , ref temp)) return ;
		//可選,先清除上一個按鈕壓下
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.UP		,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.DOWN	,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.LEFT	,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.RIGHT	,0) ;
		//再清除上一個選框
		SetTalentTreeState(currentSelected	,(int)ENUM_UPGRADE_STATE.SELECTED,1) ;
		//壓下按鈕,記錄現在按鈕位置
		SubFunctionButtonSelected(ref currentButton) ;
		//選到哪個選框? 移過去
		currentSelected = temp ;
		//顯示選框
		SetTalentTreeState(currentSelected	,(int)ENUM_UPGRADE_STATE.SELECTED,0) ;
		//按鈕壓下
		SetTalentTreeState(currentButton	,val	,1) ;
		//內容說明
		int iTargetGroup = 0 ;
		int iTargetLevel = 0 ;
		//找出 被框選的
		SubFunctionUpgradeSelected(ref iTargetGroup , ref iTargetLevel) ;
		//選框所選 名稱 說明
		SubFunctionTalentDescription(iTargetGroup , iTargetLevel) ;
		
	}
	//
	bool SubFunctionEnableSelected( int val , ref JSK_ENUM_TALENT_TREE_STATE Succeed)
	{
		bool IsAllowAction = false ;
		//
		if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL)//橫一 只能右
		{
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL)//橫二 只能左右
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL)//橫三 只能左右
		{
			if (val == (int)ENUM_DIRECTION.UP)    Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL ;
			if (val == (int)ENUM_DIRECTION.RIGHT) Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL ;
			if (val == (int)ENUM_DIRECTION.DOWN)  Succeed = JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY ;
			if (val == (int)ENUM_DIRECTION.LEFT)  Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL ;
			IsAllowAction = true ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL)//橫四縱一
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL)//橫五縱一
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL)//橫六縱一
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL)//橫四縱二
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL ; }
			IsAllowAction = true ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL)//橫五縱二
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL ; }
			IsAllowAction = true ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL)//橫六縱二
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ;Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { IsAllowAction = true ;Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { IsAllowAction = true ;Succeed = JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL)//橫四縱三
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL ; }
			IsAllowAction = true ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL)//橫五縱三
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL ; }
			IsAllowAction = true ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL)//橫六縱三
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.DOWN)  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL)//橫四縱四
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL ; }		
			if( val == (int)ENUM_DIRECTION.UP)	  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL)//橫五縱四
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.RIGHT) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL)//橫六縱四
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL ; }
			if( val == (int)ENUM_DIRECTION.UP)	  { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL ; }
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY)//縱五
		{
			if( val == (int)ENUM_DIRECTION.LEFT ) { IsAllowAction = true ; Succeed = JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL ; }//只能左
		}
		//
		return IsAllowAction ;
	}
	//Selected是移動後的選框,Button是移動前壓下箭頭按紐
	void SubFunctionButtonSelected(ref JSK_ENUM_TALENT_TREE_STATE Button)
	{
		if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL)//橫一
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.STAR_1_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL)//橫二
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.STAR_2_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL)//橫三
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.STAR_3_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL)//橫四縱一
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL)//橫五縱一
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL)//橫六縱一
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL)//橫四縱二
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.POWER_1_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL)//橫五縱二
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.POWER_2_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL)//橫六縱二
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.POWER_3_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL)//橫四縱三
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL)//橫五縱三
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL)//橫六縱三
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL)//橫四縱四
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.SPEED_1_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL)//橫五縱四
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.SPEED_2_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL)//橫六縱四
		{
			Button	= JSK_ENUM_TALENT_TREE_STATE.SPEED_3_BUTTON;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY)//縱五
		{
			Button  = JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY_BUTTON ;
		}
		else
			Button  = JSK_ENUM_TALENT_TREE_STATE.NONE ;
	}
	//
	void SubFunctionUpgradeSelected(ref int iTargetGroup , ref int iTargetLevel)
	{
		//iTargetLevel for array Index,value = 0~2
        //iTargetGroup for ENUM_UPGRADE_ATTRIBUTE
		if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL)//橫一
		{
			iTargetLevel = 0 ;//1星
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.STAR ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL)//橫二
		{
			iTargetLevel = 1 ;//2星
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.STAR ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL)//橫三
		{
			iTargetLevel = 2 ;//3星
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.STAR ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL)//橫四縱一
		{
			iTargetLevel = 0 ;//1級
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL)//橫五縱一
		{
			iTargetLevel = 1 ;//2級
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL)//橫六縱一
		{
			iTargetLevel = 2 ;//3級
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL)//橫四縱二
		{
			iTargetLevel = 0 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.POWER ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL)//橫五縱二
		{
			iTargetLevel = 1 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.POWER ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL)//橫六縱二
		{
			iTargetLevel = 2 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.POWER ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL)//橫四縱三
		{
			iTargetLevel = 0 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL)//橫五縱三
		{
			iTargetLevel = 1 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL)//橫六縱三
		{
			iTargetLevel = 2 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL)//橫四縱四
		{
			iTargetLevel = 0 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.SPEED ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL)//橫五縱四
		{
			iTargetLevel = 1 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.SPEED ;
		}
		else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL)//橫六縱四
		{
			iTargetLevel = 2 ;
			iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.SPEED ;
		}
        else if (currentSelected == JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY)//縱五
        {
            iTargetLevel = 0;
            iTargetGroup = (int)ENUM_UPGRADE_ATTRIBUTE.NEXT_MOTO;
        }
        else
        {
            iTargetLevel = -1;
            iTargetGroup = -1;
            return;//怎麼來的?
        }
	}
	//
	void SubFunctionUpgradeAndUnlock(int iValue , int iIndex)
	{
		if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.STAR)
		{
			//所圈選的選框是可購買的
			if (iStarLevel[iIndex] == (int)ENUM_UPGRADE_STATE.ENABLE)
			{
				iStarLevel[iIndex] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
				//unlock next
				if((iIndex+1) < 3)//最大三級
				{				
					iStarLevel[iIndex+1] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購買的
				}
				//三星解鎖重力/爆發/敏捷/速度/買車
				if(iIndex == 2)//最大三級,所以三星要特別處理
				{
					iGravityLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
					iPowerLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
					iDexterityLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
					iSpeedLevel[0] = (int)ENUM_UPGRADE_STATE.ENABLE ;
					if (iNextMotoBuy != (int)ENUM_UPGRADE_STATE.UPGRADED) iNextMotoBuy = (int)ENUM_UPGRADE_STATE.ENABLE ;
                    iPlayerGravity = 0;// 重力等級
                    iPlayerSpeed = 0;// 速度等級
                    iPlayerPower = 0;// 爆發力等級
                    iPlayerDexterity = 0; // 敏捷度等級
				}
			}
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY)
		{
			//所圈選的選框是可購買的
			if (iGravityLevel[iIndex] == (int)ENUM_UPGRADE_STATE.ENABLE)
			{			
				iGravityLevel[iIndex] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
                iPlayerGravity = iIndex + 1;
				//unlock next
				if((iIndex+1) < 3)//最大三級
				{
					iGravityLevel[iIndex+1] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購買的
				}			
			}	
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.NEXT_MOTO)
		{
			//所圈選的選框是可購買的
			if (iNextMotoBuy == (int)ENUM_UPGRADE_STATE.ENABLE)
			{
				iNextMotoBuy = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
				//車不用解鎖
			}
		}

		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.SPEED)
		{
			//所圈選的選框是可購買的
			if (iSpeedLevel[iIndex] == (int)ENUM_UPGRADE_STATE.ENABLE)
			{			
				iSpeedLevel[iIndex] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
                iPlayerSpeed = iIndex + 1;
				//unlock next
				if((iIndex+1) < 3)//最大三級
				{
					iSpeedLevel[iIndex+1] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購買的
				}
			}
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.POWER)
		{
			//所圈選的選框是可購買的
			if (iPowerLevel[iIndex] == (int)ENUM_UPGRADE_STATE.ENABLE)
			{			
				iPowerLevel[iIndex] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
                iPlayerPower = iIndex + 1;
				//unlock next
				if((iIndex+1) < 3)//最大三級
				{
					iPowerLevel[iIndex+1] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購買的
				}			
			}	
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY)
		{
			//所圈選的選框是可購買的
			if (iDexterityLevel[iIndex] == (int)ENUM_UPGRADE_STATE.ENABLE)
			{			
				iDexterityLevel[iIndex] = (int)ENUM_UPGRADE_STATE.UPGRADED ;//購買
                iPlayerDexterity = iIndex + 1;
				//unlock next
				if((iIndex+1) < 3)//最大三級
				{
					iDexterityLevel[iIndex+1] = (int)ENUM_UPGRADE_STATE.ENABLE ;//可購買的
				}
			}	
		}
	}
	//
	void SubFunctionTalentDescription(int iValue , int iLevel)
    {
        string sDescription = "" ;
		if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.STAR)
		{
			if (iLevel == 0) sDescription = "快艇一星" ;
			if (iLevel == 1) sDescription = "快艇二星" ;
			if (iLevel == 2) sDescription = "快艇三星" ;
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.GRAVITY)
		{
			if (iLevel == 0) sDescription = "重力一級" ;
			if (iLevel == 1) sDescription = "重力二級" ;
			if (iLevel == 2) sDescription = "重力三級" ;
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.NEXT_MOTO)
		{
			sDescription = "" ;//不是買技
            //
            int iBoatIndexX = JSK_MatchedRivalList.Instance().GetMotoUpgradeSelectedBoat();//1~9
            int iTypeX = ((iBoatIndexX - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevelX = ((iBoatIndexX - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            int iNextBoat = iLevelX + 1;
            //同系列下一輛
            if (iNextBoat <= (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL)
            {
                //get Data from WebServer' BoatManager
/*
                string sOut = "";
                BoatData data = (BoatData)BoatManager.GetBoat(iTypeX, iNextBoat , out sOut);
                if (data == null) return;
                TextMesh mTextMeshX = menuRoot.FindChild("TextDescription").GetComponent<TextMesh>();
                mTextMeshX.text = data.Description;
                mTextMeshX = menuRoot.FindChild("TextData").GetComponent<TextMesh>();
                mTextMeshX.text = data.Name;
 */ 
            }
            return;
		}		
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.SPEED)
		{
			if (iLevel == 0) sDescription = "速度一級" ;
			if (iLevel == 1) sDescription = "速度二級" ;
			if (iLevel == 2) sDescription = "速度三級" ;
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.POWER)
		{
			if (iLevel == 0) sDescription = "爆發一級" ;
			if (iLevel == 1) sDescription = "爆發二級" ;
			if (iLevel == 2) sDescription = "爆發三級" ;
		}
		else if (iValue == (int)ENUM_UPGRADE_ATTRIBUTE.DEXTERITY)
		{
			if (iLevel == 0) sDescription = "敏捷一級" ;
			if (iLevel == 1) sDescription = "敏捷二級" ;
			if (iLevel == 2) sDescription = "敏捷三級" ;
		}
        else
        {
            //..?
        }
        TextMesh mTextMesh = menuRoot.FindChild("TextTalent").GetComponent<TextMesh>();
        mTextMesh.text = sDescription;
        int iBoatIndex = JSK_MatchedRivalList.Instance().GetMotoUpgradeSelectedBoat();//1~9
        int iType = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
        int iLevelR = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
        string sOutX = "";
        //get Data from WebServer' BoatManager
/*
        BoatData dataX = (BoatData)BoatManager.GetBoat(iType, iLevelR, out sOutX);
        if (dataX == null) return;
        mTextMesh = menuRoot.FindChild("TextDescription").GetComponent<TextMesh>();
        mTextMesh.text = dataX.Description;
        mTextMesh = menuRoot.FindChild("TextData").GetComponent<TextMesh>();
        mTextMesh.text = dataX.Name;
 */
	}
	//
	void SetTalentTreeState(JSK_ENUM_TALENT_TREE_STATE state, int iIndex , int iOption)
	{
		switch( state )
		{
			case JSK_ENUM_TALENT_TREE_STATE.NONE: break;
			case JSK_ENUM_TALENT_TREE_STATE.STAR_1_LEVEL:SubFunctionSetTalentTreeStat(PlayerStar1 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.STAR_2_LEVEL:SubFunctionSetTalentTreeStat(PlayerStar2 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.STAR_3_LEVEL:SubFunctionSetTalentTreeStat(PlayerStar3 , iIndex	,iOption) ; break ; 
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_LEVEL:SubFunctionSetTalentTreeStat(PlayerGravity1 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_LEVEL:SubFunctionSetTalentTreeStat(PlayerGravity2 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_LEVEL:SubFunctionSetTalentTreeStat(PlayerGravity3 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_1_LEVEL:SubFunctionSetTalentTreeStat(PlayerPower1 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_2_LEVEL:SubFunctionSetTalentTreeStat(PlayerPower2 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_3_LEVEL:SubFunctionSetTalentTreeStat(PlayerPower3 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_LEVEL:SubFunctionSetTalentTreeStat(PlayerDexterity1 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_LEVEL:SubFunctionSetTalentTreeStat(PlayerDexterity2 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_LEVEL:SubFunctionSetTalentTreeStat(PlayerDexterity3 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_1_LEVEL:SubFunctionSetTalentTreeStat(PlayerSpeed1 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_2_LEVEL:SubFunctionSetTalentTreeStat(PlayerSpeed2 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_3_LEVEL:SubFunctionSetTalentTreeStat(PlayerSpeed3 , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY:SubFunctionSetTalentTreeStat(PlayerNextMotoBuy , iIndex	,iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.CASH:
			{
				int iMaxDigit	= 10 ;//位數上限
				int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE ;//行為
				int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CASH_WHITE ;//圖庫
				JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(TalentCash , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
				break ;
			}
			case JSK_ENUM_TALENT_TREE_STATE.LEVEL:
			{
				int iMaxDigit	= 3 ;//位數上限
				int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
				int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE ;//圖庫
				JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(TalentLevel , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
				break ;
			}
			case JSK_ENUM_TALENT_TREE_STATE.MONEY:
			{
				int iMaxDigit	= 10 ;//位數上限
				int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
				int	iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.MONEY_YELLOW ;//圖庫
				JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(TalentMoney , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
				break ;
			}
			case JSK_ENUM_TALENT_TREE_STATE.EXP:
			{
				int iMaxDigit	= 10 ;//位數上限
				int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
				int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.EXP_BLUE ;//圖庫
				JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(TalentExp , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
				break ;
			}
			case JSK_ENUM_TALENT_TREE_STATE.DY_COIN:
			{
				int iMaxDigit	= 10 ;//位數上限
				int iBehaviour	= (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.LEFT_ZERO_DISABLE ;//行為
				int iSpriteSet	= (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE ;//圖庫
				JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(TalentDYCoin , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
				break ;
			}
			case JSK_ENUM_TALENT_TREE_STATE.BUTTON:
			{
				break;
			}	
			case JSK_ENUM_TALENT_TREE_STATE.STAR_1_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonStar1Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.STAR_2_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonStar2Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.STAR_3_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonStar3Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_1_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonGravity1Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_2_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonGravity2Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.GRAVITY_3_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonGravity3Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_1_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonPower1Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_2_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonPower2Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.POWER_3_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonPower3Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_1_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonDexterity1Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_2_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonDexterity2Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.DEXTERITY_3_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonDexterity3Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_1_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonSpeed1Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_2_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonSpeed2Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.SPEED_3_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonSpeed3Menu,	iIndex,	iOption) ; break ;
			case JSK_ENUM_TALENT_TREE_STATE.NEXT_MOTO_BUY_BUTTON:SubFunctionSetTalentTreeButtonPressed(ButtonNextMotoBuy,	iIndex,	iOption) ; break ;
			default:break ;
		}
	}
	//
	void SubFunctionSetTalentTreeStat(GameObject[] pTargetObject,int iIndex,int iOption)
	{
		//4狀態(已購買,可購買,不能買,圈選中)
		if ((iIndex < 0) || (iIndex > 3)) return ;//out of range				
		//圈選中 為附加狀態
		if (iIndex == 3)
		{
			if (iOption == 0) pTargetObject[iIndex].SetActive(true) ;
			if (iOption == 1) pTargetObject[iIndex].SetActive(false);
		}
		else
		{
			//已購買,可購買,不能買 為互斥,3選1
			for (int i = 0 ; i < 3 ; i++) pTargetObject[i].SetActive(false) ;//reset
			pTargetObject[iIndex].SetActive(true) ;
		}
	}
	//
	void SubFunctionSetTalentTreeButtonPressed(GameObject[] pTargetObject,int iIndex,int iOption)
	{
		if (iIndex == (int)ENUM_DIRECTION.LEFT) 
		{	
			if (iOption == 0) pTargetObject[2].SetActive(true) ; //press button active
			if (iOption == 1) pTargetObject[2].SetActive(false); //press button down
		}
		if (iIndex == (int)ENUM_DIRECTION.RIGHT) 
		{	
			if (iOption == 0) pTargetObject[3].SetActive(true) ; //press button active
			if (iOption == 1) pTargetObject[3].SetActive(false); //press button down
		}
		if (iIndex == (int)ENUM_DIRECTION.UP) 
		{	
			if (iOption == 0) pTargetObject[0].SetActive(true) ; //press button active
			if (iOption == 1) pTargetObject[0].SetActive(false); //press button down
		}
		if (iIndex == (int)ENUM_DIRECTION.DOWN)
		{	
			if (iOption == 0) pTargetObject[1].SetActive(true) ; //press button active
			if (iOption == 1) pTargetObject[1].SetActive(false); //press button down
		}
		//
		if (iOption == 1) 
		{
			IsButtonPressDown = true ;
			lastButtonTime = Time.time + 0.1f ;
		}
	}
	//
	void SetRestoreButtonState()
	{
		if( Time.time < lastButtonTime ) return;
		IsButtonPressDown = false ;
		//
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.UP		,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.DOWN	,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.LEFT	,0) ;
		SetTalentTreeState(currentButton	,(int)ENUM_DIRECTION.RIGHT	,0) ;
	}
}
//
public enum JSK_ENUM_TALENT_TREE_STATE
{
	NONE,
    [Description("A1")]
	STAR_1_LEVEL,
    [Description("A2")]
	STAR_2_LEVEL,
    [Description("A3")]
	STAR_3_LEVEL,
    [Description("B1")]
	GRAVITY_1_LEVEL,
    [Description("B2")]
	GRAVITY_2_LEVEL,
    [Description("B3")]
	GRAVITY_3_LEVEL,
    [Description("C1")]
	POWER_1_LEVEL,
    [Description("C2")]
	POWER_2_LEVEL,
    [Description("C3")]
	POWER_3_LEVEL,
    [Description("D1")]
	DEXTERITY_1_LEVEL,
    [Description("D2")]
	DEXTERITY_2_LEVEL,
    [Description("D3")]
	DEXTERITY_3_LEVEL,
    [Description("E1")]
	SPEED_1_LEVEL,
    [Description("E2")]
	SPEED_2_LEVEL,
    [Description("E3")]
	SPEED_3_LEVEL,
    [Description("")]
	NEXT_MOTO_BUY,
	CASH,
	LEVEL,
	MONEY,
	EXP,
	DY_COIN,
	BUTTON,
	STAR_1_BUTTON,
	STAR_2_BUTTON,
	STAR_3_BUTTON,
	GRAVITY_1_BUTTON,
	GRAVITY_2_BUTTON,
	GRAVITY_3_BUTTON,
	POWER_1_BUTTON,
	POWER_2_BUTTON,
	POWER_3_BUTTON,
	DEXTERITY_1_BUTTON,
	DEXTERITY_2_BUTTON,
	DEXTERITY_3_BUTTON,
	SPEED_1_BUTTON,
	SPEED_2_BUTTON,
	SPEED_3_BUTTON,
	NEXT_MOTO_BUY_BUTTON,
	MAX
}
