using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class JSK_PictureResource : MonoBehaviour
{
	public static GameObject	g_PictureResourceObj 	= null;
	public 	Texture2D[] 		TextureResourceList 	= null;
	private Hashtable 			TextureTable			= new Hashtable();
	
	void Awake()
	{
		TextureTable.Clear();
		foreach( Texture2D tex in TextureResourceList )
		{
			TextureTable.Add(tex.name, tex);
		}
	}
	
	public Texture2D getTextureByName( string name )
	{
		if( TextureTable.Contains(name) )
			return (Texture2D)TextureTable[name];
		else
		{
			Debug.Log(name);
			return null;
		}
	}
	
}
