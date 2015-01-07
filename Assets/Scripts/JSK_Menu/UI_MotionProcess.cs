using UnityEngine;
using System.Collections;
//當前的選擇,選車=0,選油=1,選場景=2
public enum ENUM_SELECT_MOTION__STATE : int
{
    SELECTED_MOTO  = 0,//選車
    SELECTED_OIL   = 1,//選油
    SELECTED_STAGE = 2,//選場景
	MAX,
}
public class UI_MotionProcess : MonoBehaviour 
{
    //Select Motion 
    protected int iCurrentMotionIndex = 0;//當前的動作索引,選車/待機=0,車選油=1,油選車=2,油選場景=3,場景選油=4 (Idle=0,Moto_to_Oil=1,Oil_to_moto=2,Oil_to_Scene=3,Scene_to_Oil=4)
    protected int iCurrentMotionSelected = (int)ENUM_SELECT_MOTION__STATE.SELECTED_MOTO ;//當前的,選車=0,選油=1,選場景=2
    //For animator,因為playmaker沒有animator action選項...
    protected Animator animator;
    public Transform menuRoot;//for find Animator
    protected GameObject objectAnimator = null;
    protected AnimatorStateInfo currentState;
    protected AnimatorStateInfo previousState;
    //Select Moto
    protected int maxMotoNum        = 0;  	//最大的摩托数量.
    protected int iCurrentMotoIndex = 0;   //當前的摩托索引.
    protected int iCurrentMotoIndexRow = 0;	//當前的摩托橫方向索引,換車,001->004->007
    protected int iCurrentMotoIndexColumn = 2;	//當前的摩托直方向索引,換色,001->002->003
    protected int iMaxMotoNumberRow = 3;	//橫方向最大的摩托數量.
    protected int iMaxMotoNumberColumn = 3;	//直方向最大的摩托數量.
    public Transform menuRootMoto;//選車用
    protected GameObject[] MotoList;//車
    //Select Oil
    protected int iCurrentOilIndex  = 1;//當前的,一般=2,高汽=1,超汽=0
    protected const int iMaxOilCount= 3;//目前最多三種油
    //Select Stage/Map
    protected int iCurrentStageIndex= 0;//當前的,阿茲特克=1,江南=2?
    protected const int iMaxMapCount= 3;//目前最多三場景
    protected int iTemporaryIndex   = 0;//為了證明左右鍵沒壞...
    protected const int iMaxLapCount= 3;//目前最多三場景
    protected int iStageRoundCount  = 1;//當前所選場景的圈數
    protected int iStageDifficulty  = 1;//當前所選場景的難度
    //moto state
    protected GameObject MenuUpgradeStar1 = null;//升級介面一顆星;顯示快艇目前升級的狀態(0級不亮星星，3級亮3顆星)滿足3星之後才可以解鎖下一級的快艇。(條件1)
    protected GameObject MenuUpgradeStar2 = null;//升級介面兩顆星;//同上
    protected GameObject MenuUpgradeStar3 = null;//升級介面參顆星;//同上
    protected GameObject MenuUpgradeBadge1 = null;//升級介面車系列金色
    protected GameObject MenuUpgradeBadge2 = null;//升級介面車系列銅色
    protected GameObject MenuUpgradeBadge3 = null;//升級介面車系列紫色
    protected GameObject UpgradeBuyCar1 = null;//升級介面已購買左1 (三狀態互斥:可購買/已購買/不能買)(五系列:動力/控制/噴射/資料片/吱廖騙)
    protected GameObject UpgradeBuyCar2 = null;//升級介面已購買左2
    protected GameObject UpgradeBuyCar3 = null;//升級介面已購買中3
    protected GameObject UpgradeBuyCar4 = null;//升級介面已購買右4
    protected GameObject UpgradeBuyCar5 = null;//升級介面已購買右5
    protected GameObject UpgradeCanBuy1 = null;//升級介面可購買左1
    protected GameObject UpgradeCanBuy2 = null;//升級介面可購買左2
    protected GameObject UpgradeCanBuy3 = null;//升級介面可購買中3
    protected GameObject UpgradeCanBuy4 = null;//升級介面可購買右4
    protected GameObject UpgradeCanBuy5 = null;//升級介面可購買右5
    protected GameObject UpgradeSuspend1 = null;//升級介面不能買左1
    protected GameObject UpgradeSuspend2 = null;//升級介面不能買左2
    protected GameObject UpgradeSuspend3 = null;//升級介面不能買中3
    protected GameObject UpgradeSuspend4 = null;//升級介面不能買右4
    protected GameObject UpgradeSuspend5 = null;//升級介面不能買右5
    protected GameObject[] UpgradeBuyCarStar;		//升級介面五系列三顆星 (五系列:0級不亮星星，3級亮3顆星)
    protected GameObject[] UpgradeCarStarBackground;//升級介面五系列三顆星的背景(三顆空星)
    protected GameObject[] UpgradeCarSelected  ;//升級介面五系列背景選框(藍底)
    protected GameObject UpgradeSquares1 = null;//升級介面背景小車左1
    protected GameObject UpgradeSquares2 = null;//升級介面背景小車左2
    protected GameObject UpgradeSquares3 = null;//升級介面背景小車中3
    protected GameObject UpgradeSquares4 = null;//升級介面背景小車右4
    protected GameObject UpgradeSquares5 = null;//升級介面背景小車右5
    protected GameObject[] UpgradeDYCoin;		//升級介面動游幣
    protected GameObject[] UpgradeLevel;		//升級介面Level;顯示目前玩家的等級及經驗值條。
    protected GameObject[] UpgradeMoney;		//升級介面Money;
    protected GameObject[] UpgradeExp;          //升級介面Exp  ;顯示目前玩家的等級及經驗值條。
    protected GameObject[] UpgradeCarLevel;		//升級介面車Level;顯示目前車等級。
    //oil state
    protected GameObject[] MenuOilLightBar;		//選油光棒
    protected GameObject[] MenuOilCheckBox;     //選油打勾
    //stage state
    public  Transform   menuRootStage ;//選場景用
    protected GameObject[] MenuDifficulty = null;//難度,0~5星
    protected GameObject MenuRoundCount = null;//圈數,1~9
    protected GameObject[] MenuSceneName = null;//場景名,Ex:"阿茲特克","江南",etc
    protected GameObject[] MenuSceneMap = null;//場景圖
    protected GameObject[] MenuMiniMap = null;//小地圖
    //stage ranking 排行榜 前10名玩家
    protected static RandomDisplayRank[] RankList = new RandomDisplayRank[10];
    //Cutscene
    protected float fTimer = 0.0f;
    protected GameObject CutsceneObject = null;
    protected bool IsPlayingCutscene = false;
    //
    void Awake()
    {
        JSK_GlobalProcess.InitGlobal();
        //
        objectAnimator = menuRoot.Find("ui_motion/tr_ui_camera").gameObject;
        //moto        
        MotoList = new GameObject[9];//九輛車
        MotoList[0] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_001").gameObject;
        MotoList[1] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_002").gameObject;
        MotoList[2] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_003").gameObject;
        MotoList[3] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_004").gameObject;
        MotoList[4] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_005").gameObject;
        MotoList[5] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_006").gameObject;
        MotoList[6] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_007").gameObject;
        MotoList[7] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_008").gameObject;
        MotoList[8] = menuRootMoto.Find("tr_MotoMenuUpgrade 1/tr_moto_009").gameObject;
        //moto state
        MenuUpgradeStar1  = menuRootMoto.Find("tr-car_levelup/star_a_01/tr_mo_mu_carlevel_star_b01").gameObject;
        MenuUpgradeStar2  = menuRootMoto.Find("tr-car_levelup/star_a_01/tr_mo_mu_carlevel_star_b02").gameObject;
        MenuUpgradeStar3  = menuRootMoto.Find("tr-car_levelup/star_a_01/tr_mo_mu_carlevel_star_b03").gameObject;
        MenuUpgradeBadge1 = menuRootMoto.Find("tr-car_levelup/badge/tr_mo_mu_carlevel_badge01").gameObject;
        MenuUpgradeBadge2 = menuRootMoto.Find("tr-car_levelup/badge/tr_mo_mu_carlevel_badge02").gameObject;
        MenuUpgradeBadge3 = menuRootMoto.Find("tr-car_levelup/badge/tr_mo_mu_carlevel_badge03").gameObject;
        UpgradeLevel = new GameObject[3];//3位數
        UpgradeLevel[0] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_00").gameObject;
        UpgradeLevel[1] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_01").gameObject;
        UpgradeLevel[2] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_02").gameObject;
        UpgradeMoney = new GameObject[8];//8位數
        UpgradeMoney[0] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money1").gameObject;
        UpgradeMoney[1] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money2").gameObject;
        UpgradeMoney[2] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money3").gameObject;
        UpgradeMoney[3] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money4").gameObject;
        UpgradeMoney[4] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money5").gameObject;
        UpgradeMoney[5] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money6").gameObject;
        UpgradeMoney[6] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money7").gameObject;
        UpgradeMoney[7] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_Money8").gameObject;
        UpgradeDYCoin = new GameObject[8];//8位數
        UpgradeDYCoin[0] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin1").gameObject;
        UpgradeDYCoin[1] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin2").gameObject;
        UpgradeDYCoin[2] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin3").gameObject;
        UpgradeDYCoin[3] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin4").gameObject;
        UpgradeDYCoin[4] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin5").gameObject;
        UpgradeDYCoin[5] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin6").gameObject;
        UpgradeDYCoin[6] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin7").gameObject;
        UpgradeDYCoin[7] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_DYCoin8").gameObject;
        UpgradeExp = new GameObject[8];//8位數
        UpgradeExp[0] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP1").gameObject;
        UpgradeExp[1] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP2").gameObject;
        UpgradeExp[2] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP3").gameObject;
        UpgradeExp[3] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP4").gameObject;
        UpgradeExp[4] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP5").gameObject;
        UpgradeExp[5] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP6").gameObject;
        UpgradeExp[6] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP7").gameObject;
        UpgradeExp[7] = menuRootMoto.Find("game_coin/tr_mk_mu_bkground_06_EXP8").gameObject;
        UpgradeCarLevel = new GameObject[3];//3位數
        UpgradeCarLevel[0] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_00").gameObject;
        UpgradeCarLevel[1] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_01").gameObject;
        UpgradeCarLevel[2] = menuRootMoto.Find("tr_mk_lv02/tr_mk_mu_bklv_02").gameObject;
        UpgradeBuyCar1 = menuRootMoto.Find("squares/Group_yes/tr_mo_mu_yes01").gameObject;
        UpgradeBuyCar2 = menuRootMoto.Find("squares/Group_yes/tr_mo_mu_yes02").gameObject;
        UpgradeBuyCar3 = menuRootMoto.Find("squares/Group_yes/tr_mo_mu_yes03").gameObject;
        UpgradeBuyCar4 = menuRootMoto.Find("squares/Group_yes/tr_mo_mu_yes04").gameObject;
        UpgradeBuyCar5 = menuRootMoto.Find("squares/Group_yes/tr_mo_mu_yes05").gameObject;
        UpgradeCanBuy1 = menuRootMoto.Find("squares/Group_no/tr_mo_mu_no05").gameObject;
        UpgradeCanBuy2 = menuRootMoto.Find("squares/Group_no/tr_mo_mu_no04").gameObject;
        UpgradeCanBuy3 = menuRootMoto.Find("squares/Group_no/tr_mo_mu_no03").gameObject;
        UpgradeCanBuy4 = menuRootMoto.Find("squares/Group_no/tr_mo_mu_no02").gameObject;
        UpgradeCanBuy5 = menuRootMoto.Find("squares/Group_no/tr_mo_mu_no01").gameObject;
        UpgradeSquares1 = menuRootMoto.Find("littel_car/tr_mk_mu_moto_001").gameObject;
        UpgradeSquares2 = menuRootMoto.Find("littel_car/tr_mk_mu_moto_002").gameObject;
        UpgradeSquares3 = menuRootMoto.Find("littel_car/tr_mk_mu_moto_003").gameObject;
        UpgradeSquares4 = menuRootMoto.Find("littel_car/tr_mk_mu_moto_004").gameObject;
        UpgradeSquares5 = menuRootMoto.Find("littel_car/tr_mk_mu_moto_005").gameObject;
        UpgradeSuspend1 = menuRootMoto.Find("squares/squares_stop/tr_mo_mu_carlevel_squares_stop01").gameObject;
        UpgradeSuspend2 = menuRootMoto.Find("squares/squares_stop/tr_mo_mu_carlevel_squares_stop02").gameObject;
        UpgradeSuspend3 = menuRootMoto.Find("squares/squares_stop/tr_mo_mu_carlevel_squares_stop03").gameObject;
        UpgradeSuspend4 = menuRootMoto.Find("squares/squares_stop/tr_mo_mu_carlevel_squares_stop04").gameObject;
        UpgradeSuspend5 = menuRootMoto.Find("squares/squares_stop/tr_mo_mu_carlevel_squares_stop05").gameObject;
        UpgradeBuyCarStar = new GameObject[15];//5系列3顆星最多15顆
        UpgradeBuyCarStar[0] = menuRootMoto.Find("squares/star_b/tr_mo_mu_carlevel_star_b01").gameObject;
        UpgradeBuyCarStar[1] = menuRootMoto.Find("squares/star_b/tr_mo_mu_carlevel_star_b02").gameObject;
        UpgradeBuyCarStar[2] = menuRootMoto.Find("squares/star_b/tr_mo_mu_carlevel_star_b03").gameObject;
        UpgradeBuyCarStar[3] = menuRootMoto.Find("squares/star_c/tr_mo_mu_carlevel_star_b01").gameObject;
        UpgradeBuyCarStar[4] = menuRootMoto.Find("squares/star_c/tr_mo_mu_carlevel_star_b02").gameObject;
        UpgradeBuyCarStar[5] = menuRootMoto.Find("squares/star_c/tr_mo_mu_carlevel_star_b03").gameObject;
        UpgradeBuyCarStar[6] = menuRootMoto.Find("squares/star_d/tr_mo_mu_carlevel_star_b01").gameObject;
        UpgradeBuyCarStar[7] = menuRootMoto.Find("squares/star_d/tr_mo_mu_carlevel_star_b02").gameObject;
        UpgradeBuyCarStar[8] = menuRootMoto.Find("squares/star_d/tr_mo_mu_carlevel_star_b03").gameObject;
        UpgradeBuyCarStar[9] = menuRootMoto.Find("squares/star_e/tr_mo_mu_carlevel_star_b01").gameObject;
        UpgradeBuyCarStar[10] = menuRootMoto.Find("squares/star_e/tr_mo_mu_carlevel_star_b02").gameObject;
        UpgradeBuyCarStar[11] = menuRootMoto.Find("squares/star_e/tr_mo_mu_carlevel_star_b03").gameObject;
        UpgradeBuyCarStar[12] = menuRootMoto.Find("squares/star_f/tr_mo_mu_carlevel_star_b01").gameObject;
        UpgradeBuyCarStar[13] = menuRootMoto.Find("squares/star_f/tr_mo_mu_carlevel_star_b02").gameObject;
        UpgradeBuyCarStar[14] = menuRootMoto.Find("squares/star_f/tr_mo_mu_carlevel_star_b03").gameObject;
        UpgradeCarStarBackground = new GameObject[15];//5系列星數背景
        UpgradeCarStarBackground[0] = menuRootMoto.Find("squares/star_b_01/tr_mo_mu_carlevel_star_a01").gameObject;
        UpgradeCarStarBackground[1] = menuRootMoto.Find("squares/star_b_01/tr_mo_mu_carlevel_star_a02").gameObject;
        UpgradeCarStarBackground[2] = menuRootMoto.Find("squares/star_b_01/tr_mo_mu_carlevel_star_a03").gameObject;
        UpgradeCarStarBackground[3] = menuRootMoto.Find("squares/star_c_01/tr_mo_mu_carlevel_star_a01").gameObject;
        UpgradeCarStarBackground[4] = menuRootMoto.Find("squares/star_c_01/tr_mo_mu_carlevel_star_a02").gameObject;
        UpgradeCarStarBackground[5] = menuRootMoto.Find("squares/star_c_01/tr_mo_mu_carlevel_star_a03").gameObject;
        UpgradeCarStarBackground[6] = menuRootMoto.Find("squares/star_d_01/tr_mo_mu_carlevel_star_a01").gameObject;
        UpgradeCarStarBackground[7] = menuRootMoto.Find("squares/star_d_01/tr_mo_mu_carlevel_star_a02").gameObject;
        UpgradeCarStarBackground[8] = menuRootMoto.Find("squares/star_d_01/tr_mo_mu_carlevel_star_a03").gameObject;
        UpgradeCarStarBackground[9] = menuRootMoto.Find("squares/star_e_01/tr_mo_mu_carlevel_star_a01").gameObject;
        UpgradeCarStarBackground[10] = menuRootMoto.Find("squares/star_e_01/tr_mo_mu_carlevel_star_a02").gameObject;
        UpgradeCarStarBackground[11] = menuRootMoto.Find("squares/star_e_01/tr_mo_mu_carlevel_star_a03").gameObject;
        UpgradeCarStarBackground[12] = menuRootMoto.Find("squares/star_f_01/tr_mo_mu_carlevel_star_a01").gameObject;
        UpgradeCarStarBackground[13] = menuRootMoto.Find("squares/star_f_01/tr_mo_mu_carlevel_star_a02").gameObject;
        UpgradeCarStarBackground[14] = menuRootMoto.Find("squares/star_f_01/tr_mo_mu_carlevel_star_a03").gameObject;
        UpgradeCarSelected = new GameObject[5];//5系列背景選框
        UpgradeCarSelected[0] = menuRootMoto.Find("squares/squares/tr_mo_mu_carlevel_squares_a01").gameObject;
        UpgradeCarSelected[1] = menuRootMoto.Find("squares/squares/tr_mo_mu_carlevel_squares_a02").gameObject;
        UpgradeCarSelected[2] = menuRootMoto.Find("squares/squares/tr_mo_mu_carlevel_squares_a03").gameObject;
        UpgradeCarSelected[3] = menuRootMoto.Find("squares/squares/tr_mo_mu_carlevel_squares_a04").gameObject;
        UpgradeCarSelected[4] = menuRootMoto.Find("squares/squares/tr_mo_mu_carlevel_squares_a05").gameObject;
        //oil state
        MenuOilLightBar = new GameObject[3] ;//光棒
        MenuOilLightBar[0] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_4_1").gameObject;
        MenuOilLightBar[1] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_4_2").gameObject;
        MenuOilLightBar[2] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_4_3").gameObject;
        MenuOilCheckBox = new GameObject[3];//打勾
        MenuOilCheckBox[0] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_31").gameObject;
        MenuOilCheckBox[1] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_32").gameObject;
        MenuOilCheckBox[2] = menuRoot.Find("ui_motion/tr_uicamera-motion/gassoline/tr_mk_mu_gassoline_33").gameObject;
        //stage state
        MenuDifficulty = new GameObject[5] ;//難度,0~5星
        MenuDifficulty[0] = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_hardstar_a01").gameObject;
        MenuDifficulty[1] = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_hardstar_a01-1").gameObject;
        MenuDifficulty[2] = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_hardstar_a01-2").gameObject;
        MenuDifficulty[3] = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_hardstar_a01-3").gameObject;
        MenuDifficulty[4] = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_hardstar_a01-4").gameObject;
        MenuRoundCount = menuRootStage.Find("tr_menu_background_002/tr_mo_mu_lap_01").gameObject;//圈數,1~9
        MenuSceneName   = new GameObject[5] ;//場景名
        MenuSceneName[0] = menuRootStage.Find("tr_menu_background_003/sprite_tr_mk_mu_bkground_Area2").gameObject;//阿茲特克
        MenuSceneName[1] = menuRootStage.Find("tr_menu_background_003/sprite_tr_mk_mu_bkground_Area1").gameObject;//江南
        MenuSceneName[2] = menuRootStage.Find("tr_menu_background_003/sprite_tr_mk_mu_bkground_Area5").gameObject;//水都
        MenuSceneName[3] = menuRootStage.Find("tr_menu_background_003/sprite_tr_mk_mu_bkground_Area3").gameObject;//極地
        MenuSceneName[4] = menuRootStage.Find("tr_menu_background_003/sprite_tr_mk_mu_bkground_Area4").gameObject;//東京
        MenuSceneMap = new GameObject[5] ;//場景
        MenuSceneMap[0] = menuRootStage.Find("map/tr_menu_china_map/tr_mo_mu_Chile_01").gameObject;//阿茲特克
        MenuSceneMap[1] = menuRootStage.Find("map/tr_menu_china_map/tr_mo_mu_China_02").gameObject;//江南
        MenuSceneMap[2] = menuRootStage.Find("map/tr_menu_china_map/tr_mo_mu_Venice_a01").gameObject;//水都
        MenuSceneMap[3] = menuRootStage.Find("map/tr_menu_china_map/tr_mo_mu_Siberia_a01").gameObject;//極地
        MenuSceneMap[4] = menuRootStage.Find("map/tr_menu_china_map/tr_mo_mu_tokyo_a01").gameObject;//東京
        MenuMiniMap = new GameObject[5];//小地圖	
        MenuMiniMap[0] = menuRootStage.Find("map/tr_menu_china_map/littlemap/tr_mk_mu_MimiMap_01").gameObject;//阿茲特克
        MenuMiniMap[1] = menuRootStage.Find("map/tr_menu_china_map/littlemap/tr_mk_mu_MimiMap_02").gameObject;//江南
        MenuMiniMap[2] = menuRootStage.Find("map/tr_menu_china_map/littlemap/tr_mk_mu_MimiMap_03").gameObject;//水都
        MenuMiniMap[3] = menuRootStage.Find("map/tr_menu_china_map/littlemap/tr_mk_mu_MimiMap_04").gameObject;//極地
        MenuMiniMap[4] = menuRootStage.Find("map/tr_menu_china_map/littlemap/tr_mk_mu_MimiMap_05").gameObject;//東京
        for (int i = 1; i < 5; i++) MenuSceneMap[i].SetActive(false);
        for (int i = 1; i < 5; i++) MenuMiniMap[i].SetActive(false);
        for (int i = 0; i < 10; i++) RankList[i] = new RandomDisplayRank();
    }
	// Use this for initialization
	void Start () 
    {     
        animator = objectAnimator.GetComponent<Animator>();	
        //
        JSK_SoundProcess.PlayMusic("MainMenu");
        JSK_GlobalProcess.SetFifoScene(1, 1);
        JSK_GlobalProcess.SetFifoScene(2, 1);
        JSK_GlobalProcess.ClearFifoMessage();
        //get default data
        onMotoMove(0);
        //set default Stage
        JSK_GlobalProcess.GamePlace = iCurrentStageIndex + 1;//加1處理.
	}	
	// Update is called once per frame
	void Update () 
    {
        checkFifoMenu();
        //Animator
        if (animator)
        {
            // "IsNext"flag is true , NEXT or BACK button Pressed
            if (animator.GetBool("IsNext"))
            {
                // check state if state name is not same => already run next animaiton! reset "Next" flag = false
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
        //Cutscene
        if (IsPlayingCutscene)
        {
            float delta = Time.deltaTime;
            fTimer += delta;
            if (fTimer > 2.0001f)
            {
                IsPlayingCutscene = false;
                Destroy(CutsceneObject);
            }
        }
	}
    //
    void OnGUI()
    {    
    }
    //壓下 下一步/回上層 鍵(pressed NEXT/BACK button)
    void onMotionMove(int val)
    {
        if (val == 0)//BACK
        {
                 if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_MOTO)  { return; }
            else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_OIL)   { iCurrentMotionIndex = 2; iCurrentMotionSelected = (int)ENUM_SELECT_MOTION__STATE.SELECTED_MOTO; }
            else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_STAGE) { iCurrentMotionIndex = 4; iCurrentMotionSelected = (int)ENUM_SELECT_MOTION__STATE.SELECTED_OIL; }
            else return;
        }
        else if (val == 1)//NEXT
        {
                 if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_MOTO)  { iCurrentMotionIndex = 1; iCurrentMotionSelected = (int)ENUM_SELECT_MOTION__STATE.SELECTED_OIL; }
            else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_OIL) { iCurrentMotionIndex = 3; iCurrentMotionSelected = (int)ENUM_SELECT_MOTION__STATE.SELECTED_STAGE; SetStageForOilCheck(); }
            else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_STAGE) { return; }
            else return;
        }
        else
        {
            return;
        }
        //
        if (iCurrentMotionIndex == 0) return;//happen?
        //20141111 Fix:使用新版Cutscene的插件動畫取代UnityAnimation
        //MecAnima
        if (animator)
        {
            //設定物件上Animator變數，播放相對應動畫
            animator.SetInteger("iRotate", iCurrentMotionIndex);
            animator.SetBool("IsNext", true);//run the animation
        }
        //20141128Fix.
        //PlayingCutscene(iCurrentMotionIndex);
    }

    //called form Playmake
    void onLoadLevel()
    {
        int iTargetUIVersion = JSK_GlobalProcess.g_iDemoUIVersion;
        if (iTargetUIVersion == 1)
        {
            /* do nothing */
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

    //壓下 選車介面 四個方向鍵
    void onMotoMove(int val)
    {
        if (val == (int)ENUM_DIRECTION.UP)//上
        {
            iCurrentMotoIndexColumn--;
            if (iCurrentMotoIndexColumn < 0) iCurrentMotoIndexColumn = iMaxMotoNumberColumn - 1;
        }
        else if (val == (int)ENUM_DIRECTION.DOWN)//下
        {
            iCurrentMotoIndexColumn++;
            if (iCurrentMotoIndexColumn >= iMaxMotoNumberColumn) iCurrentMotoIndexColumn = 0;
        }
        else if (val == (int)ENUM_DIRECTION.LEFT)//左
        {
            iCurrentMotoIndexRow--;
            if (iCurrentMotoIndexRow < 0) iCurrentMotoIndexRow = 0;//跟據GDD:向左切換1輛快艇。不LOOP。
        }
        else if (val == (int)ENUM_DIRECTION.RIGHT)//右
        {
            iCurrentMotoIndexRow++;
            if (iCurrentMotoIndexRow >= iMaxMotoNumberRow) iCurrentMotoIndexRow = iMaxMotoNumberRow - 1;//跟據GDD:向右切換1輛快艇。不LOOP。
        }
        else
        {
            //reset
            iCurrentMotoIndexRow = 0;
            iCurrentMotoIndexColumn = 0;
        }
        //        
        MotoList[iCurrentMotoIndex].SetActive(false);
        iCurrentMotoIndex = iCurrentMotoIndexColumn * iMaxMotoNumberColumn + iCurrentMotoIndexRow;
        MotoList[iCurrentMotoIndex].SetActive(true);
        JSK_GlobalProcess.GameMoto1P = iCurrentMotoIndex + 1;
        //
        GetDataFromWebServerForMoto(iCurrentMotoIndex) ;      
    }   
    //壓下 選油介面 光棒
    void onOilMove(int val)
    {
        if (val == (int)ENUM_DIRECTION.UP)//上
        {
            iCurrentOilIndex--;
            if (iCurrentOilIndex < 0) iCurrentOilIndex = iMaxOilCount - 1;
        }
        else if (val == (int)ENUM_DIRECTION.DOWN)//下
        {
            iCurrentOilIndex++;
            if (iCurrentOilIndex >= iMaxOilCount) iCurrentOilIndex = 0;
        }
        else if (val == (int)ENUM_DIRECTION.LEFT)//左
        {
            iCurrentOilIndex--;
            if (iCurrentOilIndex < 0) iCurrentOilIndex = iMaxOilCount - 1;
        }
        else if (val == (int)ENUM_DIRECTION.RIGHT)//右
        {
            iCurrentOilIndex++;
            if (iCurrentOilIndex >= iMaxOilCount) iCurrentOilIndex = 0;
        }
        else
        {
                 if (val == (int)ENUM_SELECT_OIL_STATE.SELECTED_SUPERIOR_OIL) iCurrentOilIndex = 0;
            else if (val == (int)ENUM_SELECT_OIL_STATE.SELECTED_ADVANCED_OIL) iCurrentOilIndex = 1;
            else if (val == (int)ENUM_SELECT_OIL_STATE.SELECTED_JUNIOR_OIL)   iCurrentOilIndex = 2;
            else return;
        }
         //state Changed!
        SetStateForOil(iCurrentOilIndex);
        //
        //iCurrentOilIndex:一般=2,高汽=1,超汽=0
        if (iCurrentOilIndex == 0) JSK_GlobalProcess.GameOil = 3;//汽油     1:一般 2:高級 3:超級 (add at 20141104)
        if (iCurrentOilIndex == 1) JSK_GlobalProcess.GameOil = 2;
        if (iCurrentOilIndex == 2) JSK_GlobalProcess.GameOil = 1;
         
    }
    //
    //壓下 選場景介面 左右方向鍵
    void onStageMove(int val)
    {
        if (val == (int)ENUM_DIRECTION.UP)//上
        {
            iCurrentStageIndex--;
            if (iCurrentStageIndex < 0) iCurrentStageIndex = iMaxMapCount - 1;

            iTemporaryIndex--;
            if (iTemporaryIndex < 0) iTemporaryIndex = iMaxLapCount - 1;
        }
        else if (val == (int)ENUM_DIRECTION.DOWN)//下
        {
            iCurrentStageIndex++;
            if (iCurrentStageIndex >= iMaxMapCount) iCurrentStageIndex = 0;

            iTemporaryIndex++;
            if (iTemporaryIndex >= iMaxLapCount) iTemporaryIndex = 0;
        }
        else if (val == (int)ENUM_DIRECTION.LEFT)//左
        {
            iCurrentStageIndex--;
            if (iCurrentStageIndex < 0) iCurrentStageIndex = iMaxMapCount - 1;

            iTemporaryIndex--;
            if (iTemporaryIndex < 0) iTemporaryIndex = iMaxLapCount - 1;
        }
        else if (val == (int)ENUM_DIRECTION.RIGHT)//右
        {
            iCurrentStageIndex++;
            if (iCurrentStageIndex >= iMaxMapCount) iCurrentStageIndex = 0 ;

            iTemporaryIndex++;
            if (iTemporaryIndex >= iMaxLapCount) iTemporaryIndex = 0;
        }
        else
        {
            return;
        }
        //        
        JSK_GlobalProcess.GamePlace = iCurrentStageIndex + 1;//加1處理.
        //
        GetDateFromWebServerForStage(iCurrentStageIndex);
        //state Changed!
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_DIFFICULT,iStageDifficulty,    0);
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_NAME,     iCurrentStageIndex,  0);
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_ROUND,    iStageRoundCount,    0);
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_SCENE,    iCurrentStageIndex,  0);
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_MINI,     iCurrentStageIndex,  0);
        SetStateForStage(ENUM_UI_STAGE_STATE.MAP_RANKING,  iCurrentStageIndex,  0);
        SetStateForStage(ENUM_UI_STAGE_STATE.BUTTON,   0,  0);//由PlayMaker取代,但...
    } 
    //
    void SetStateForMoto(ENUM_UI_MOTO_STATE state, int iIndex, int iOption)
    {
        switch( state )
		{
            case ENUM_UI_MOTO_STATE.NONE: break;
            case ENUM_UI_MOTO_STATE.STAR:
			{
				//default 0 star
				if ((iIndex > 3) || (iIndex < 0)) iIndex = 0 ;
                if (iIndex == 0) { MenuUpgradeStar1.SetActive(false); MenuUpgradeStar2.SetActive(false); MenuUpgradeStar3.SetActive(false); }
                if (iIndex == 1) { MenuUpgradeStar1.SetActive(true); MenuUpgradeStar2.SetActive(false); MenuUpgradeStar3.SetActive(false); }
                if (iIndex == 2) { MenuUpgradeStar1.SetActive(true); MenuUpgradeStar2.SetActive(true); MenuUpgradeStar3.SetActive(false); }
                if (iIndex == 3) { MenuUpgradeStar1.SetActive(true); MenuUpgradeStar2.SetActive(true); MenuUpgradeStar3.SetActive(true); }
	        	break;
			}
            case ENUM_UI_MOTO_STATE.BADGE:
            {
                //default
                if ((iIndex > 2) || (iIndex < 0)) break;//out of range
                if (iIndex == 0) { MenuUpgradeBadge1.SetActive(true); MenuUpgradeBadge2.SetActive(false); MenuUpgradeBadge3.SetActive(false); }
                if (iIndex == 1) { MenuUpgradeBadge1.SetActive(false); MenuUpgradeBadge2.SetActive(true); MenuUpgradeBadge3.SetActive(false); }
                if (iIndex == 2) { MenuUpgradeBadge1.SetActive(false); MenuUpgradeBadge2.SetActive(false); MenuUpgradeBadge3.SetActive(true); }
                break;
            }          
            case ENUM_UI_MOTO_STATE.CAR_LEVEL:
            {
                if (iIndex < 0) break;//out of range
                int iMaxDigit = 3;//位數上限
                int iMaxLimit = 1;
                for (int i = 0; i < iMaxDigit; i++) iMaxLimit *= 10;
                // max
                if (iIndex >= iMaxLimit) iIndex = iMaxLimit - 1;//max
                //
                int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE;//行為
                int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE;//圖庫
                JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(UpgradeCarLevel, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                break;
            }            
            case ENUM_UI_MOTO_STATE.LEVEL:
            {   
                if (iIndex < 0) break;//out of range
                int iMaxDigit	= 3 ;//位數上限
                int iMaxLimit   = 1 ;
                for (int i = 0 ; i < iMaxDigit ; i++) iMaxLimit *= 10 ;
                // max
                if (iIndex >= iMaxLimit) iIndex = iMaxLimit - 1 ;//max
                //
			    int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE ;//行為
                int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.LEVEL_BLUE;//圖庫
			    JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(UpgradeLevel , iIndex , iMaxDigit , iSpriteSet , iBehaviour) ;
                break;
            }
            case ENUM_UI_MOTO_STATE.MONEY:
            {
                if (iIndex < 0) break;//out of range
                int iMaxDigit = 8;//位數上限
                int iMaxLimit = 1;
                for (int i = 0; i < iMaxDigit; i++) iMaxLimit *= 10;
                // max
                if (iIndex >= iMaxLimit) iIndex = iMaxLimit - 1;//max
                //
                int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE;//行為
                int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MONEY_YELLOW;//圖庫
                JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(UpgradeMoney, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                break;
            }
            case ENUM_UI_MOTO_STATE.DYCOIN:
            {
                if (iIndex < 0) break;//out of range
                int iMaxDigit = 8;//位數上限
                int iMaxLimit = 1;
                for (int i = 0; i < iMaxDigit; i++) iMaxLimit *= 10;
                // max
                if (iIndex >= iMaxLimit) iIndex = iMaxLimit - 1;//max
                //
                int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE;//行為
                int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.EXP_BLUE;//圖庫
                JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(UpgradeDYCoin, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                break;
            }
            case ENUM_UI_MOTO_STATE.GOLD:
            {
                break;
            }
            case ENUM_UI_MOTO_STATE.EXP:
            {
                if (iIndex < 0) break;//out of range
                int iMaxDigit = 8;//位數上限
                int iMaxLimit = 1;
                for (int i = 0; i < iMaxDigit; i++) iMaxLimit *= 10;
                // max
                if (iIndex >= iMaxLimit) iIndex = iMaxLimit - 1;//max
                //
                int iBehaviour = (int)ENUM_SPRITE_RENDERER_BEHAVIOUR.RIGHT_ZERO_DISABLE;//行為
                int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.EXP_BLUE;//圖庫
                JSK_SpriteRendererUtility.Instance().SetGameObjectNumber(UpgradeExp, iIndex, iMaxDigit, iSpriteSet, iBehaviour);
                break;
            }
            case ENUM_UI_MOTO_STATE.MOTO_BUY:
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
            case ENUM_UI_MOTO_STATE.SQUARES_MOTO:
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
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(UpgradeSquares1, iOption, iSpriteSet);
				}
				if (iIndex == 1) 
				{
					UpgradeSquares2.SetActive(IsEnable);
					if (IsEnable == false) break ;
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(UpgradeSquares2, iOption, iSpriteSet);
				}
				if (iIndex == 2) 
				{	
					UpgradeSquares3.SetActive(IsEnable);
					if (IsEnable == false) break ;
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(UpgradeSquares3, iOption, iSpriteSet);
				}
				if (iIndex == 3)
				{	
					UpgradeSquares4.SetActive(IsEnable);
					if (IsEnable == false) break ;
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(UpgradeSquares4, iOption, iSpriteSet);
				}
				if (iIndex == 4)
				{	
					UpgradeSquares5.SetActive(IsEnable);
					if (IsEnable == false) break ;
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.MOTO_UPGRADE_CAR_ICON;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(UpgradeSquares5, iOption, iSpriteSet);
				}
				break ;
			}
            case ENUM_UI_MOTO_STATE.MINI_CAR_STAR:
			{
				//default moto
				if ((iIndex > 4) || (iIndex < 0)) break ;//out of range				
				//default 3 star
				bool IsEnable = true ;
				if ((iOption > 3) || (iOption < 0)) { IsEnable = false ; iOption = 0 ;}
				//				
				int i = iIndex * 3 ;				
				if (iOption == 0) {UpgradeBuyCarStar[i+0].SetActive(false);	UpgradeBuyCarStar[i+1].SetActive(false); UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 1) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(false); UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 2) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(true);  UpgradeBuyCarStar[i+2].SetActive(false); }
				if (iOption == 3) {UpgradeBuyCarStar[i+0].SetActive(true);	UpgradeBuyCarStar[i+1].SetActive(true);  UpgradeBuyCarStar[i+2].SetActive(true);  }
                //
                for(int j = 0 ; j < 3 ; j++)
                {
                    UpgradeCarStarBackground[i+j].SetActive(IsEnable);
                }
				break;
			}
            case ENUM_UI_MOTO_STATE.MINI_CAR_SELECTED:
            {
				if ((iIndex > 4) || (iIndex < 0)) break ;//out of range		
                for (int i = 0 ; i < 5 ; i++) UpgradeCarSelected[i].SetActive(false) ;//5 mini car
                if (iIndex == 1) UpgradeCarSelected[0].SetActive(true);//同系列一星
                if (iIndex == 2) UpgradeCarSelected[1].SetActive(true);//同系列二星
                if (iIndex == 3) UpgradeCarSelected[2].SetActive(true);//同系列三星
                break ;
            }
           default:break ;
		}
	}
    //
    void SetStateForOil(int iIndex)
    {
        //for keyboard pressed
        for (int i = 0; i < 3; i++) MenuOilLightBar[i].SetActive(false);
        //高般汽油=0,超般汽油=1,一般汽油=2
        MenuOilLightBar[iIndex].SetActive(true);
    }
    //
    void SetStageForOilCheck()
    {
        for (int i = 0; i < 3; i++) MenuOilCheckBox[i].SetActive(false);
        //綠色勾勾
        MenuOilCheckBox[iCurrentOilIndex].SetActive(true);
    }
    //
    void SetStateForStage(ENUM_UI_STAGE_STATE state, int iIndex, int iOption)
    {
        switch (state)
        {
            case ENUM_UI_STAGE_STATE.NONE: break;
            case ENUM_UI_STAGE_STATE.MAP_DIFFICULT:
                {
                    if ((iIndex > 5) || (iIndex < 0)) break;//out of range
                    for (int i = 0; i < 5; i++) MenuDifficulty[i].SetActive(false);
                    //
                    if (iIndex == 0)
                    {
                    }
                    else if (iIndex == 1)
                    {
                        MenuDifficulty[0].SetActive(true);
                    }
                    else if (iIndex == 2)
                    {
                        MenuDifficulty[0].SetActive(true);
                        MenuDifficulty[1].SetActive(true);
                    }
                    else if (iIndex == 3)
                    {
                        MenuDifficulty[0].SetActive(true);
                        MenuDifficulty[1].SetActive(true);
                        MenuDifficulty[2].SetActive(true);
                    }
                    else if (iIndex == 4)
                    {
                        MenuDifficulty[0].SetActive(true);
                        MenuDifficulty[1].SetActive(true);
                        MenuDifficulty[2].SetActive(true);
                        MenuDifficulty[3].SetActive(true);
                    }
                    else
                    {
                        MenuDifficulty[0].SetActive(true);
                        MenuDifficulty[1].SetActive(true);
                        MenuDifficulty[2].SetActive(true);
                        MenuDifficulty[3].SetActive(true);
                        MenuDifficulty[4].SetActive(true);
                    }
                    break;
                }
            case ENUM_UI_STAGE_STATE.MAP_NAME:
                {
                    if ((iIndex > 5) || (iIndex < 0)) break;//out of range
                    for (int i = 0; i < 5; i++) MenuSceneName[i].SetActive(false);
                    MenuSceneName[iIndex].SetActive(true);
                    break;
                }
            case ENUM_UI_STAGE_STATE.MAP_ROUND:
                {
                    if ((iIndex > 9) || (iIndex < 0)) break;//out of range
                    int iSpriteSet = (int)ENUM_SPRITE_PACKAGE.STAGE_LAP_WHITE_BLUE;//圖庫
                    JSK_SpriteRendererUtility.Instance().SetGameObjectSprite(MenuRoundCount, iIndex, iSpriteSet);
                    break;
                }
            case ENUM_UI_STAGE_STATE.MAP_SCENE:
                {
                    if ((iIndex > 5) || (iIndex < 0)) break;//out of range
                    for (int i = 0; i < 5; i++) MenuSceneMap[i].SetActive(false);
                    MenuSceneMap[iIndex].SetActive(true);
                    break;
                }
            case ENUM_UI_STAGE_STATE.MAP_MINI:
                {
                    if ((iIndex > 5) || (iIndex < 0)) break;//out of range
                    for (int i = 0; i < 5; i++) MenuMiniMap[i].SetActive(false);
                    MenuMiniMap[iIndex].SetActive(true);                    
                    break;
                }
            case ENUM_UI_STAGE_STATE.MAP_RANKING:
                {
                    if ((iIndex > 5) || (iIndex < 0)) break;//out of range
                    int iLineIndex = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        string sIndex = "" + (iLineIndex + 1);//從01開始
                        string sTemp = sIndex.PadLeft(2, '0');//兩位數,不足補0
                        string sTarget = "";
                        //資料錯誤或成積不存在
                        if (RankList[i].iRanlIndex == 0) continue;
                        else
                        {
                            //玩家名
                            sTarget = "tr_menu_background_002/TextPlayerName/Text" + sTemp;
                            TextMesh mTextMesh = menuRootStage.FindChild(sTarget).GetComponent<TextMesh>();
                            mTextMesh.text = RankList[i].sPlayerName;
                            //名次
                            sTarget = "tr_menu_background_002/TextRankIndex/Text" + sTemp;
                            mTextMesh = menuRootStage.FindChild(sTarget).GetComponent<TextMesh>();
                            mTextMesh.text = "" + RankList[i].iRanlIndex;
                            //場次
                            sTarget = "tr_menu_background_002/TextTotalRound/Text" + sTemp;
                            mTextMesh = menuRootStage.FindChild(sTarget).GetComponent<TextMesh>();
                            mTextMesh.text = "" + RankList[i].iTotalRound;
                            //時間
                            sTarget = "tr_menu_background_002/TextTotalTimer/Text" + sTemp;
                            mTextMesh = menuRootStage.FindChild(sTarget).GetComponent<TextMesh>();
                            int iSecond = RankList[i].iFinishTime % 60;
                            int iMinute = (RankList[i].iFinishTime / 60) % 60;
                            int iHour = RankList[i].iFinishTime / 3600;
                            string sTimer = "" + iHour + ":" + iMinute + ":" + iSecond;
                            mTextMesh.text = "" + sTimer;
                            iLineIndex++;
                        }
                    }
                    break;
                }
            default: break;
        }
	}
    //
    void GetDataFromWebServerForMoto(int Target)
    {
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            //取得快艇資料
            int iBoatTarget = Target+1;//1~9
            string sBoatTarget = "tr_moto_00" + iBoatTarget ;
            int iBoatIndex = SubFunctionSelectedMotoByFile(sBoatTarget);
            int iType = ((iBoatIndex - 1) / (int)JSK_ENUM_MOTO_LIST.MAX_TYPE) + 1;//1~3
            int iLevelX = ((iBoatIndex - 1) % (int)JSK_ENUM_MOTO_LIST.MAX_LEVEL) + 1;//1~3
            string sOut = "";
            BoatData data = (BoatData)BoatManager.GetBoat(iType, iLevelX, out sOut);
            if (data == null) return;
            //記錄所選
            JSK_MatchedRivalList.Instance().SetMotoUpgradeSelectedBoat(iBoatIndex);
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
            //快艇系列ICON	用來表示目前選擇的快艇是哪一系列的 (未來系,重機系,軍武系)
            SetStateForMoto(ENUM_UI_MOTO_STATE.BADGE, data.Type, 0);
            //快艇星數 1~3星ICON	顯示快艇目前升級的狀態(0級不亮星星，3級亮3顆星) 滿足3星之後才可以解鎖下一級的快艇。(條件1)
            SetStateForMoto(ENUM_UI_MOTO_STATE.STAR, data.Lv, 0);
            //快艇LV
            SetStateForMoto(ENUM_UI_MOTO_STATE.CAR_LEVEL, 0, 0);
            //快艇資料	用來顯示當前快艇的數值 (顯示內容及方式未定)
            TextMesh mTextMesh = menuRootMoto.FindChild("TextDescription1").GetComponent<TextMesh>();
            mTextMesh.text = data.Description;
            //debug:
//          mTextMesh = menuRootMoto.FindChild("TextDescription2").GetComponent<TextMesh>();
//          mTextMesh.text = data.Model;
//          mTextMesh = menuRootMoto.FindChild("TextDescription3").GetComponent<TextMesh>();
//          mTextMesh.text = "Status:" + data.Status;
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
            int iMoney = WebServerProcess.User.Money;
            int iDYCoin= WebServerProcess.User.DYMoney;
            int iLevel = WebServerProcess.User.UserLevel;
            int iExp   = WebServerProcess.User.Exp;
            int iGold  = WebServerProcess.User.GoldMoney;
            SetStateForMoto(ENUM_UI_MOTO_STATE.LEVEL,   iLevel, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.MONEY,   iMoney, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.DYCOIN,  iDYCoin,0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.EXP,     iExp,   0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.GOLD,    iGold,  0);
            //display same type boat
            int iTarget = data.Type;
            int iCount = 0;
            int[] iSeriesStatus = new int[5];//因為畫面上只有五個Icon,最多只取五個
            int[] iSeriesIcon = new int[5];
            int[] iSeriesStar = new int[5];
            for (int i = 0; i < BoatManager.BoatLists.Count; i++)
            {
                if (iCount >= 5) break;//最多只取五個
                BoatData dataTemp = (BoatData)BoatManager.BoatLists[i];
                if (iTarget != dataTemp.Type) continue;//同一系列
                iSeriesStatus[iCount] = dataTemp.Status;//狀態
                int iTemp = SubFunctionSelectedMotoByID(dataTemp.ID);//1~9
                iSeriesIcon[iCount] = iTemp-1;//1~9 to 0~8 
                iSeriesStar[iCount] = dataTemp.Lv;//星數
                iCount++;
            }
            for (int i = 0; i < 5; i++)
            {
                if (i < iCount)
                {
                    //五車狀態: data.Status = 0; // 狀態 0:無法購買 1:已購買 2:可購買 
                    SetStateForMoto(ENUM_UI_MOTO_STATE.MOTO_BUY, i, iSeriesStatus[i]);
                    //五車(小車)Icon
                    SetStateForMoto(ENUM_UI_MOTO_STATE.SQUARES_MOTO, i, iSeriesIcon[i]);
                    //五車星數
                    SetStateForMoto(ENUM_UI_MOTO_STATE.MINI_CAR_STAR, i, iSeriesStar[i]);
                }
                else
                {
                    SetStateForMoto(ENUM_UI_MOTO_STATE.MOTO_BUY, i, 0);//無法購買
                    SetStateForMoto(ENUM_UI_MOTO_STATE.SQUARES_MOTO, i, -1);//
                    SetStateForMoto(ENUM_UI_MOTO_STATE.MINI_CAR_STAR, i, 0);//0星
                }
            }
            SetStateForMoto(ENUM_UI_MOTO_STATE.MINI_CAR_SELECTED, data.Lv, 0);
            //Update RadarChart
            DrawPolygon RadarChartScript = menuRootMoto.FindChild("tr_mk_market_radar/tr_mk_market_radar02/RadarChart").GetComponent<DrawPolygon>();
            RadarChartScript.value[0] = data.LevelGRV ;//上:重力等級
            RadarChartScript.value[1] = data.LevelPOW ;//右:爆發力等級
            RadarChartScript.value[2] = data.LevelSPD ;//下:敏捷度等級
            RadarChartScript.value[3] = data.LevelDEX ;//左:速度等級
 */
        }
        else
        {
            int iBoatTarget = UnityEngine.Random.Range(1, 10);//1~9
            string sBoatTarget = "tr_moto_00" + iBoatTarget;
            int iBoatIndex = SubFunctionSelectedMotoByFile(sBoatTarget);            
            //記錄所選
            JSK_MatchedRivalList.Instance().SetMotoUpgradeSelectedBoat(iBoatIndex);

            int iType = UnityEngine.Random.Range(1, 4);//1~3
            int iLevelX = UnityEngine.Random.Range(1, 4);//1~3
            //快艇系列ICON	用來表示目前選擇的快艇是哪一系列的 (未來系,重機系,軍武系)
            SetStateForMoto(ENUM_UI_MOTO_STATE.BADGE, iType, 0);
            //快艇星數 1~3星ICON	顯示快艇目前升級的狀態(0級不亮星星，3級亮3顆星) 滿足3星之後才可以解鎖下一級的快艇。(條件1)
            SetStateForMoto(ENUM_UI_MOTO_STATE.STAR, iLevelX , 0);
            //快艇LV
            SetStateForMoto(ENUM_UI_MOTO_STATE.CAR_LEVEL, 0, 0);
            //快艇資料	用來顯示當前快艇的數值 (顯示內容及方式未定)
            TextMesh mTextMesh = menuRootMoto.FindChild("TextDescription1").GetComponent<TextMesh>();
            mTextMesh.text ="HelloWorld";
            int iMoney = UnityEngine.Random.Range(1, 10000);
            int iDYCoin = UnityEngine.Random.Range(1, 1000);
            int iLevel = UnityEngine.Random.Range(1, 100);
            int iExp = iLevel * 10000 + UnityEngine.Random.Range(1, 10000);
            int iGold = UnityEngine.Random.Range(1, 10000);
            SetStateForMoto(ENUM_UI_MOTO_STATE.LEVEL, iLevel, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.MONEY, iMoney, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.DYCOIN, iDYCoin, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.EXP, iExp, 0);
            SetStateForMoto(ENUM_UI_MOTO_STATE.GOLD, iGold, 0);
            int iCount = 0;
            int[] iSeriesStatus = new int[5];//因為畫面上只有五個Icon,最多只取五個
            int[] iSeriesIcon = new int[5];
            int[] iSeriesStar = new int[5];
            //display same type boat
            for (int i = 0; i < 5; i++)//最多只取五個
            {
                iSeriesStatus[iCount] = UnityEngine.Random.Range(0, 3);//狀態0~2                
                iSeriesIcon[iCount] = UnityEngine.Random.Range(0, 9);//0~8 
                iSeriesStar[iCount] = UnityEngine.Random.Range(1, 4);//星數
                //五車狀態: data.Status = 0; // 狀態 0:無法購買 1:已購買 2:可購買 
                SetStateForMoto(ENUM_UI_MOTO_STATE.MOTO_BUY, i, iSeriesStatus[i]);
                //五車(小車)Icon
                SetStateForMoto(ENUM_UI_MOTO_STATE.SQUARES_MOTO, i, iSeriesIcon[i]);
                //五車星數
                SetStateForMoto(ENUM_UI_MOTO_STATE.MINI_CAR_STAR, i, iSeriesStar[i]);
            }
            SetStateForMoto(ENUM_UI_MOTO_STATE.MINI_CAR_SELECTED, iLevelX, 0);
            //Update RadarChart
            DrawPolygon RadarChartScript = menuRootMoto.FindChild("tr_mk_market_radar/tr_mk_market_radar02/RadarChart").GetComponent<DrawPolygon>();
            RadarChartScript.value[0] = UnityEngine.Random.Range(1, 100);//上:重力等級
            RadarChartScript.value[1] = UnityEngine.Random.Range(1, 100);//右:爆發力等級
            RadarChartScript.value[2] = UnityEngine.Random.Range(1, 100);//下:敏捷度等級
            RadarChartScript.value[3] = UnityEngine.Random.Range(1, 100);//左:速度等級
        }
    }
    //
    void GetDateFromWebServerForStage(int iTarget)
    {
        if (JSK_GlobalProcess.g_IsWebServer)
        {
/*
            int iSceneID = iTarget + 1;//加1處理.
            SceneData scene = SceneManager.GetScene(iSceneID);//webServer的
            if (scene == null) return;
            iStageRoundCount = scene.RoundCount ;//當前所選場景的圈數
        //  iStageDifficulty = scene.Difficulty ;//當前所選場景的難度
            //
            SceneData sceneT = SceneManager.GetScene(iTemporaryIndex + 1);//webServer的
            if (sceneT == null) return;
            iStageDifficulty = sceneT.Difficulty ;//當前所選場景的難度
            //排行榜
            for (int i = 0; i < 10; i++)
            {
                int iID = i + 1;
                RankData data = WebServerProcess.GetTimeRank(iSceneID , iID);
                if (data == null)
                {
                    RankList[i].iRanlIndex = 0;
                    RankList[i].iTotalRound = 0;
                    RankList[i].iTotalScore = 0;
                    RankList[i].iFinishTime = 0;
                    RankList[i].sPlayerName = "";
                }
                else
                {
                    //int Rank; //排行
                    //int UserID; //玩家代號
                    //string Name; //玩家姓名
                    //int Score; //分數
                    //int PlayCount; //玩的次數
                    RankList[i].iRanlIndex = data.Rank;
                    RankList[i].iTotalRound = data.PlayCount;
                    RankList[i].iTotalScore = data.Score;
                    RankList[i].iFinishTime = (data.Score / 100);//單位:秒
                    RankList[i].sPlayerName = data.Name;
                }
            }
*/
        }
        else
        {
            int iRandom = Random.Range(1, 10);
            int iRandomDefficultStar = Random.Range(0, 6);//0,1,2,3,4,5 star
            iStageRoundCount  = iRandom ;
            iStageDifficulty = iRandomDefficultStar;
        }
    }
    //
    void checkFifoMenu()
    {
        string sInputMsg = JSK_GlobalProcess.GetFifoMsg();

        if (sInputMsg.Length > 0) Debug.Log("sInputMsg:" + sInputMsg);
        //
        if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_MOTO) 
        { 
            if (sInputMsg.IndexOf("Left") >= 0) onMotoMove((int)ENUM_DIRECTION.LEFT);
            else if (sInputMsg.IndexOf("Right") >= 0) onMotoMove((int)ENUM_DIRECTION.RIGHT);
            else if (sInputMsg.IndexOf("Up") >= 0) onMotoMove((int)ENUM_DIRECTION.UP);
            else if (sInputMsg.IndexOf("Down") >= 0) onMotoMove((int)ENUM_DIRECTION.DOWN);
            else if (sInputMsg.IndexOf("Esc") >= 0) Application.LoadLevel("JSK_MotoTalentTreeMenu");
            else if (sInputMsg.IndexOf("Back") >= 0) Application.LoadLevel("JSK_MotoTalentTreeMenu");
            else if (sInputMsg.IndexOf("Confirm") >= 0) onMotionMove(1) ;//NEXT
            else if (Input.GetKeyDown(KeyCode.X)) Application.LoadLevel("UI_MainMenu");
        }
        else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_OIL) 
        {
            if (sInputMsg.IndexOf("Left") >= 0) onOilMove((int)ENUM_DIRECTION.LEFT);
            else if (sInputMsg.IndexOf("Right") >= 0) onOilMove((int)ENUM_DIRECTION.RIGHT);
            else if (sInputMsg.IndexOf("Up") >= 0) onOilMove((int)ENUM_DIRECTION.UP);
            else if (sInputMsg.IndexOf("Down") >= 0) onOilMove((int)ENUM_DIRECTION.DOWN);
            else if (sInputMsg.IndexOf("Esc") >= 0) onMotionMove(0) ;
            else if (sInputMsg.IndexOf("Back") >= 0) onMotionMove(0);
            else if (sInputMsg.IndexOf("Confirm") >= 0) onMotionMove(1) ;//NEXT
            else if (Input.GetKeyDown(KeyCode.X)) onMotionMove(0);
        }
        else if (iCurrentMotionSelected == (int)ENUM_SELECT_MOTION__STATE.SELECTED_STAGE) 
        { 
            if (sInputMsg.IndexOf("Left") >= 0) onStageMove((int)ENUM_DIRECTION.LEFT);
            else if (sInputMsg.IndexOf("Right") >= 0) onStageMove((int)ENUM_DIRECTION.RIGHT);
            else if (sInputMsg.IndexOf("Up") >= 0) onStageMove((int)ENUM_DIRECTION.UP);
            else if (sInputMsg.IndexOf("Down") >= 0) onStageMove((int)ENUM_DIRECTION.DOWN);
            else if (sInputMsg.IndexOf("Esc") >= 0) onMotionMove(0) ;//NEXT
            else if (sInputMsg.IndexOf("Back") >= 0) onMotionMove(0);//NEXT
            else if (sInputMsg.IndexOf("Confirm") >= 0) Application.LoadLevel("JSK_MatchMenu");
            else if (Input.GetKeyDown(KeyCode.X)) onMotionMove(0);//NEXT
        }
        else 
            return;
    }
    //
    public int SubFunctionSelectedMoto(int iTarget)
    {
        //以下片斷 相同於 iValue = iTarget + 1 ;
        int iValue = 0;
        switch (iTarget)
        {   //                                                              ID ServerID FileName    ItemName          ItemDescripte 
            case 0: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_3; break;//1 tr_moto_001	未來系冰河	
            case 1: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_2; break;//2 tr_moto_002	未來系火焰 
            case 2: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_3_LEVEL_1; break;//3 tr_moto_003	未來系飛船         --
            case 3: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_1; break;//4 tr_moto_004	重機-紅            紅色新星
            case 4: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_2; break;//5 tr_moto_005	重機-白            白色流星
            case 5: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_2_LEVEL_3; break;//6 tr_moto_006	重機-火焰          火焰慧星
            case 6: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_1; break;//7 tr_moto_007	軍武系的鯊魚機     鯊魚艦
            case 7: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_2; break;//8 tr_moto_008	軍武系的旗魚機     旗魚艦
            case 8: iValue = (int)JSK_ENUM_MOTO_LIST.TYPE_1_LEVEL_3; break;//9 tr_moto_009	軍武系的鎚頭鯊機   鎚頭鯊艦
            default: break;//what happen?
        }
        return iValue;
    }
    public int SubFunctionSelectedMotoByFile(string sTarget)
    {
             if (0 == string.Compare(sTarget, "tr_moto_001", true)) return 9;//
        else if (0 == string.Compare(sTarget, "tr_moto_002", true)) return 8;//
        else if (0 == string.Compare(sTarget, "tr_moto_003", true)) return 7;//
        else if (0 == string.Compare(sTarget, "tr_moto_004", true)) return 4;//
        else if (0 == string.Compare(sTarget, "tr_moto_005", true)) return 5;//
        else if (0 == string.Compare(sTarget, "tr_moto_006", true)) return 6;//
        else if (0 == string.Compare(sTarget, "tr_moto_007", true)) return 1;//
        else if (0 == string.Compare(sTarget, "tr_moto_008", true)) return 2;//
        else if (0 == string.Compare(sTarget, "tr_moto_009", true)) return 3;//
        else return -1;
    }
    public int SubFunctionSelectedMotoByID(string sTarget)
    {
             if (0 == string.Compare(sTarget, "B0101", true)) return 3;//tr_moto_003
        else if (0 == string.Compare(sTarget, "B0102", true)) return 2;//tr_moto_002
        else if (0 == string.Compare(sTarget, "B0103", true)) return 1;//tr_moto_001
        else if (0 == string.Compare(sTarget, "B0201", true)) return 4;//tr_moto_004
        else if (0 == string.Compare(sTarget, "B0202", true)) return 5;//tr_moto_005
        else if (0 == string.Compare(sTarget, "B0203", true)) return 6;//tr_moto_006
        else if (0 == string.Compare(sTarget, "B0301", true)) return 7;//tr_moto_007
        else if (0 == string.Compare(sTarget, "B0302", true)) return 8;//tr_moto_008
        else if (0 == string.Compare(sTarget, "B0303", true)) return 9;//tr_moto_009
        else return -1;
    }
    //
    void PlayingCutscene(int iIndex)
    {
        //Cutscene用法:
        //1.把CutScene拖成prefab
        //2.在此prefab中新增GameObjectEmpty
        //3.掛載CutsceneTrigger.cs到GameObject (在.\Cinema Suite\Cinema Director\System\Runtime\Helpers\)
        //4.拖曳Cutscene到GameObject裡的public Cutscene上,Ex:"BG2_To_BG3".此cutscene prefab在Instantiate後就會自動執行.
        if (CutsceneObject != null) return;
        if (IsPlayingCutscene == true) return;
        //BG1_to_BG2 || BG2_to_BG1
        if ((iIndex == 1) || (iIndex == 2))
        {
            CutsceneObject = (GameObject)Instantiate(Resources.Load("JSK/Common/Moto_CutScene_BG1_To_BG2"));
            IsPlayingCutscene = true;
            fTimer = 0.0f;        
        }
        //BG2_to_BG3 || /BG3_to_BG2
        if ((iIndex == 3) || (iIndex == 4))
        {            
            CutsceneObject = (GameObject)Instantiate(Resources.Load("JSK/Common/Moto_CutScene_BG2_To_BG3"));
            IsPlayingCutscene = true;
            fTimer = 0.0f;
        }
    }
}
//
public enum ENUM_UI_MOTO_STATE
{
	NONE,
	STAR,
	BADGE,
	MOTO_BUY,
	SQUARES_MOTO,//小車ICON
	CAR_LEVEL,
	LEVEL,
    MONEY,//遊戲幣
    DYCOIN,//動游幣
    GOLD,//金幣
	EXP,
	MINI_CAR_STAR,
    MINI_CAR_SELECTED,
	MAX
}
//
public enum ENUM_UI_STAGE_STATE
{
    NONE,
    MAP_DIFFICULT,//難度
    MAP_NAME ,
    MAP_ROUND,//圈數
    MAP_SCENE,//大地圖
    MAP_MINI ,//小地圖
    MAP_RANKING,//排行榜
    BUTTON,
    MAX
}
public enum ENUM_SELECT_OIL_STATE : int
{
    SELECTED_SUPERIOR_OIL = 5,//超級汽油
    SELECTED_ADVANCED_OIL = 6,//高級汽油
    SELECTED_JUNIOR_OIL   = 7,//一般棄油
    MAX,
}