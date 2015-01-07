using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_ChooseMotoProcess : MonoBehaviour
{   
	private int 		maxMotoNum     		= 0 ;  	//最大的摩托数量.
    private int         iCurrentMotoIndex   = 0 ;   //當前的摩托索引.
	private int 		iCurrentMotoIndexRow= 0 ;	//當前的摩托橫方向索引,換車,001->004->007
	private int 		iCurrentMotoIndexColumn=2;	//當前的摩托直方向索引,換色,001->002->003
	private int			iMaxMotoNumberRow   = 3 ;	//橫方向最大的摩托數量.
	private int			iMaxMotoNumberColumn= 3 ;	//直方向最大的摩托數量.
    public Transform MotoListRoot;//選車用
    private GameObject[] MotoList;//選車用

	void Awake()
	{
		JSK_GlobalProcess.InitGlobal();
        MotoList = new GameObject[9];//九輛車
        MotoList[0] = MotoListRoot.Find("tr_moto_001").gameObject;
        MotoList[1] = MotoListRoot.Find("tr_moto_002").gameObject;
        MotoList[2] = MotoListRoot.Find("tr_moto_003").gameObject;
        MotoList[3] = MotoListRoot.Find("tr_moto_004").gameObject;
        MotoList[4] = MotoListRoot.Find("tr_moto_005").gameObject;
        MotoList[5] = MotoListRoot.Find("tr_moto_006").gameObject;
        MotoList[6] = MotoListRoot.Find("tr_moto_007").gameObject;
        MotoList[7] = MotoListRoot.Find("tr_moto_008").gameObject;
        MotoList[8] = MotoListRoot.Find("tr_moto_009").gameObject;
	}
	
	void Start()
	{
		JSK_SoundProcess.PlayMusic("MainMenu");
		JSK_GlobalProcess.SetFifoScene(1,1);
		JSK_GlobalProcess.SetFifoScene(2,1);
		JSK_GlobalProcess.ClearFifoMessage();		
	}
	
	void Update()
	{
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 30), "old version ChooseMoto"))
        {
            Application.LoadLevel("JSK_MotoMenu");
        }
    }

    //壓下四個方向鍵
	void onMotoMove( int val )
	{
		if( val == (int)ENUM_DIRECTION.UP)//上
		{
			iCurrentMotoIndexColumn-- ;
			if (iCurrentMotoIndexColumn < 0) iCurrentMotoIndexColumn = iMaxMotoNumberColumn -1  ;
		}
		else if( val == (int)ENUM_DIRECTION.DOWN)//下
		{
			iCurrentMotoIndexColumn++ ;
			if (iCurrentMotoIndexColumn >= iMaxMotoNumberColumn) iCurrentMotoIndexColumn = 0 ;
		}
		else if( val == (int)ENUM_DIRECTION.LEFT)//左
		{
			iCurrentMotoIndexRow-- ;
			if (iCurrentMotoIndexRow < 0) iCurrentMotoIndexRow = 0 ;//跟據GDD:向左切換1輛快艇。不LOOP。
		}
		else if( val == (int)ENUM_DIRECTION.RIGHT)//右
		{
			iCurrentMotoIndexRow++ ;
			if (iCurrentMotoIndexRow >= iMaxMotoNumberRow) iCurrentMotoIndexRow = iMaxMotoNumberRow - 1 ;//跟據GDD:向右切換1輛快艇。不LOOP。
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
	}   
}
