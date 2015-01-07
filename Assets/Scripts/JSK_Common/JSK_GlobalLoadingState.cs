using UnityEngine;
using System.Collections;

public enum JSK_LoadState : int
{
	LoadNone = 0 ,	//无加载状态.
	LoadIng  = 1 ,	//加载中.
	LoadWait = 2 ,	//网络模式加载完毕并等待其他玩家.
	LoadEnd  = 3 ,	//加载完毕.
	WaitMatchResult = 4,
	ProcessMatchResultA = 5,
	ENUM_WAIT_PLAYER_RESULT = 6,
	ProcessMatchResultB = 7,
	ENUM_WAIT_WEB_SERVER_TIMEOUT = 8,
	ENUM_MATCH_PLAYER_RESULT = 9,
	ENUM_READY_GAMEPLAY = 10
}

public class JSK_GlobalLoadingState : MonoBehaviour
{
	public JSK_LoadState Enum_JSK_LoadState = JSK_LoadState.LoadNone;
	//self
	private static JSK_GlobalLoadingState Self;
	//
	void Awake()
	{
		Enum_JSK_LoadState = JSK_LoadState.LoadNone;
	}
	// Use this for initialization
	void Start () 
	{
		Self=this;	
	}	
	// Update is called once per frame
	void Update () 
	{
		
	}
	//
	public static JSK_GlobalLoadingState Instance()
	{
		return Self;
	}
	public bool IsLoading()
	{
		if( Enum_JSK_LoadState != JSK_LoadState.LoadNone )
			return true;
		else
			return false;
	}
	public void SetLoadState(JSK_LoadState state)
	{
		Enum_JSK_LoadState = state ;
	}
	public JSK_LoadState GetLoadState()
	{
		return Enum_JSK_LoadState ;
	}
	public void LoadingEnd()
	{
		Instance().SetLoadState( JSK_LoadState.LoadNone ) ;
		JSK_GlobalProcess.StartGameLevel() ;
	}
}