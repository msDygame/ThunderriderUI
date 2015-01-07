using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
	
public class JSK_CharacterData : MonoBehaviour
{
    public string	Name 		= "";	//人物名称.
	public string	AnimString	= "";	//动作档前缀名称.
	public string	SoundString	= "";	//声音档前缀名称.
    public string	PrefabName 	= ""; 	//模型名称.
	public float	MassWater	= 0;	//漂浮质量.
	public float	MassFall	= 0;	//落水质量.
	public float	IdleTime	= 0;	//空闲间隔时间.
	public float	TurboPow	= 0;	//加速力.
}
