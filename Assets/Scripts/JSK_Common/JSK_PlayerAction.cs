using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
	
public class JSK_PlayerAction : MonoBehaviour
{
	public string 	AnimName		= "";	//动画名称.
	public int 		ActionLayer 	= 0;	//动作的播放等级 0:Loop 1:Once 2:ClampForever.
	public string 	ActionSound		= "";	//播放的声音.
	public bool		ActionRestart	= false;//持续动画被中断之后是否重新播放.
}
