using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class JSK_EffectResource : MonoBehaviour
{
	public static 	GameObject		g_EffectResourceObj = null;
	public 			GameObject[]	EffectResourceList 	= null;
	private 		Hashtable		EffectTable			= new Hashtable();
	
	void Awake()
	{
		EffectTable.Clear();
		foreach( GameObject obj in EffectResourceList )
		{
			EffectTable.Add(obj.name, obj);
		}
	}
	
	public GameObject getEffectByName( string name )
	{
		if( EffectTable.Contains(name) )
			return (GameObject)EffectTable[name];
		else
			return null;
	}
}
