using UnityEngine;
using System.Collections;

public class JSK_PlayerAnimation : MonoBehaviour
{
	private	string				animStringVal	= "";
	private JSK_PlayerAction 	lastAction		= null;	//持续性状态的动作.
	private JSK_PlayerAction 	onceAction		= null;	//一次性状态的动作.
	private int					moveAnimType	= 0;	//当前的移动状态:0空闲,1前进,2左转,3右转,4左后转,5右后转.
	private int					animState		= 0;	//0移动判定状态,1一次性动画状态,2持续动画状态.
	
	public void InitAnimation( string val )
	{
		animStringVal = val;
		
		foreach( JSK_PlayerAction act in JSK_GlobalProcess.g_Actions )
		{
			//Debug.Log(act.AnimName);
			AnimationState anim = animation[animStringVal + act.AnimName];
			if(anim==null)
				continue; // 缺動作...要注意
			if( act.ActionLayer == 0 )
				anim.wrapMode = WrapMode.Loop;
			else if( act.ActionLayer == 1 )
				anim.wrapMode = WrapMode.Once;
			else if( act.ActionLayer == 2 )
				anim.wrapMode = WrapMode.ClampForever;
		}
		setStateAction("ready", 1.0f);
	}
	
	public void AnimationInput( Vector2 dir )
	{
		if( animState == 0 )
		{
			int lastAnimType = moveAnimType;
			moveAnimType = 0;
			
			if( dir.y >= 0 )
			{
				if( dir.x < -0.25f )
					moveAnimType = 2;
				else if( dir.x > 0.25f )
					moveAnimType = 3;
				else if( dir.y > 0 )
					moveAnimType = 1;
			}
			else
			{
				if( dir.x < 0 )
					moveAnimType = 4;
				else
					moveAnimType = 5;
			}
			
			if( lastAnimType != moveAnimType )
			{
				if( moveAnimType == 0 )
					setAction("ready", 1.0f);
				else if( moveAnimType == 1 )
					setAction("up_and_down", 1.0f);
				else if( moveAnimType == 2 )
					setAction("left", 1.0f);
				else if( moveAnimType == 3 )
					setAction("right", 1.0f);
				else if( moveAnimType == 4 )
					setAction("parking_left", 1.0f);
				else if( moveAnimType == 5 )
					setAction("parking_right", 1.0f);
			}
		}
	}
	
	public bool IsCanPlayIdleAction()
	{
		if( animState == 0 && moveAnimType == 0 && animation.IsPlaying(animStringVal + "ready") )
			return true;
		return false;
	}
	
	public void PlayIdleAction( string sAnimName, float speed )
	{
		if( animation.IsPlaying(animStringVal + sAnimName) )
			return;
		
		AnimationState anim = animation[animStringVal + sAnimName];
		anim.speed = speed;
		animation.CrossFade(animStringVal + sAnimName, 0.3f, PlayMode.StopSameLayer);
		setStateAction("ready", 1.0f);//保证做完一次性动画之后平滑过渡到IDLE状态.
	}
	//第三个参数,是否等待之前的一次性动画播放完毕才能播放下一个.
	public void PlayOnceAction( string sAnimName, float speed, bool wait )
	{
		//Debug.Log("PlayOnceAction " + sAnimName);
		if( wait && onceAction && animation.IsPlaying(animStringVal + onceAction.AnimName) )
			return;

		if( animation.IsPlaying(animStringVal + sAnimName) )
			return;
		
		onceAction = JSK_GlobalProcess.GetAction(sAnimName);
		animState = 1;
		
		AnimationState anim = animation[animStringVal + sAnimName];
		anim.speed = speed;
		animation.CrossFade(animStringVal + sAnimName, 0.3f, PlayMode.StopSameLayer);
		setStateAction("ready", 1.0f);//保证做完一次性动画之后平滑过渡到IDLE状态.
	}
	
	public void PlayLastAction( string sAnimName, float speed, bool repeat )
	{
		//Debug.Log("PlayLastAction " + sAnimName);
		onceAction = null;
		animState = 2;
		
		lastAction = JSK_GlobalProcess.GetAction(sAnimName);
		AnimationState anim = null;
		if( repeat )
		{
			anim = animation.CrossFadeQueued(animStringVal + sAnimName, 0.3f, QueueMode.PlayNow, PlayMode.StopSameLayer);
		}
		else
		{
			if( animation.IsPlaying(animStringVal + sAnimName) )
				return;
			anim = animation[animStringVal + sAnimName];
			animation.CrossFade(animStringVal + sAnimName, 0.3f, PlayMode.StopSameLayer);
		}
		anim.speed = speed;
	}
	
	public void StopLastAction( string sAnimName )
	{
		if( animation.IsPlaying(animStringVal + sAnimName) )
		{
			animState = 0;
			moveAnimType = 0;
			setAction("ready", 1.0f);
		}
		if( lastAction && lastAction.AnimName == sAnimName )
		{
			//animState = 0;
			//moveAnimType = 0;
			//setAction("ready", 1.0f);
			lastAction = null;
		}
	}
	
	void setAction( string sAnimName, float speed )
	{
		//Debug.Log("setAction " + sAnimName);
		AnimationState anim = animation[animStringVal + sAnimName];
		if(anim==null)	return;
		animation.CrossFade(animStringVal + sAnimName, 0.3f);
		anim.speed = speed;
		JSK_PlayerAction act = JSK_GlobalProcess.GetAction(sAnimName);
		playActionSound(act.ActionSound);
	}
	
	void setStateAction( string sAnimName, float speed )
	{
		//Debug.Log("setStateAction " + sAnimName);
		AnimationState anim = animation.CrossFadeQueued(animStringVal + sAnimName, 0.3f, QueueMode.CompleteOthers, PlayMode.StopSameLayer);
		anim.speed = speed;
		JSK_PlayerAction act = JSK_GlobalProcess.GetAction(sAnimName);
		playActionSound(act.ActionSound);
	}
	
	void playActionSound( string name )
	{
		if( name != "" )
		{
			//name = name.Replace("@", animStringVal);
			JSK_SoundProcess.PlaySound(name);
		}
	}
	
	void Update()
	{
		//if( Input.GetKeyDown(KeyCode.Q) )
			//PlayLastAction("restart", 1.0f, true);
		
		//if( Input.GetKeyDown(KeyCode.W) )
			//PlayLastAction("jump_1", 1.0f, false);
		
		//if( Input.GetKeyDown(KeyCode.E) )
			//PlayOnceAction("collide_front", 1.0f, true);
		
		//if( Input.GetKeyDown(KeyCode.R) )
			//StopLastAction("restart");
		
		//if( Input.GetKeyDown(KeyCode.T) )
			//StopLastAction("jump_1");
		
		if( onceAction && !animation.IsPlaying(animStringVal + onceAction.AnimName) )
		{
			if( lastAction && lastAction.ActionRestart )
			{
				PlayLastAction(lastAction.AnimName, 1.0f, false);//这里面把onceAction设置为null了.
			}
			else
			{
				onceAction = null;
				animState = 0;
				moveAnimType = 0;//由于此时已经切换到IDLE状态,所以移动状态为0
			}
		}
	}
}
