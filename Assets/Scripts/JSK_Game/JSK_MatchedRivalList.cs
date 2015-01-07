using UnityEngine;
using System.Collections;

//記錄由 配對頁面(Jsk_MatchMenu.unity)所產生的配對玩家(from WebServerProcess.GetRivalData(i);)
public class JSK_MatchedRivalList : MonoBehaviour
{
	public static int	iMaxRivalPlayer ;//public for debug
	// data for MatchMenu
	protected static RandomDisplayRatio[] RivalList ;
	// data for MotoUpgradeMenu
	protected static int iMotoUpgradeSelectedBoat ;//選了那一臺車要進行升級,車在MotoUpgradeMenu.unity選的,升級介面在MotoTalentTreeMenu.unity裡處理,故保留其值.
	// self
	protected	static	JSK_MatchedRivalList Self;
	// Use this for initialization
	void Start () 
	{
		Self = this;
		iMaxRivalPlayer = 0 ;
		RivalList = new RandomDisplayRatio[12] ;
		for (int i = 0 ; i < 12 ; i++) RivalList[i] = new RandomDisplayRatio() ;
		iMotoUpgradeSelectedBoat = 0 ;
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
	//Instance
	public static JSK_MatchedRivalList Instance()
	{
		return Self;
	}
	//
	public void SetRivalList(int iIndex , RandomDisplayRatio data)
	{
		RivalList[iIndex].iShipLevel= data.iShipLevel ;
		RivalList[iIndex].iShipType = data.iShipType ;
		RivalList[iIndex].sShipType = data.sShipType ;
		RivalList[iIndex].iShipStar = data.iShipStar ;
		RivalList[iIndex].sPlayerName = data.sPlayerName ;
		RivalList[iIndex].iRationNumber = data.iRationNumber ;
	}
	public void GetRivalList(int iIndex , ref RandomDisplayRatio data)
	{
		if (iIndex >= iMaxRivalPlayer) return ;
		data.iShipLevel = RivalList[iIndex].iShipLevel ;
		data.iShipType =  RivalList[iIndex].iShipType ;
		data.sShipType =  RivalList[iIndex].sShipType ;
		data.iShipStar =  RivalList[iIndex].iShipStar ;
		data.sPlayerName= RivalList[iIndex].sPlayerName ;
		data.iRationNumber=RivalList[iIndex].iRationNumber ;
	}
	//
	public void SetMaxRivalPlayer(int i) { iMaxRivalPlayer = i ; }
	//
	public void Reset()
	{
		if (iMaxRivalPlayer == 0) return ;
		for (int i = 0 ; i < 12 ; i++)
		{
			RivalList[i].iShipLevel= 0 ;
			RivalList[i].iShipType = 0 ;
			RivalList[i].sShipType = "" ;
			RivalList[i].iShipStar = 0 ;
			RivalList[i].sPlayerName = "" ;
			RivalList[i].iRationNumber = 0 ;
		}
		SetMaxRivalPlayer(0) ;
	}
	//
	public void SetMotoUpgradeSelectedBoat(int i) { iMotoUpgradeSelectedBoat = i ; }
	public int  GetMotoUpgradeSelectedBoat() { return iMotoUpgradeSelectedBoat ; } 
}