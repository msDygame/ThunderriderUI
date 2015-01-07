using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
	
public class JSK_GUIProcess : MonoBehaviour
{
	private static Hashtable Pictures = new Hashtable(); //图片列表.
	
	void Awake()
	{
		Pictures.Clear();
	}
	
	public static Texture2D GetPicture( string key )
    {
		if( !Pictures.Contains(key) )
		{
			//Pictures[key] = JSK_PictureResource.g_PictureResourceObj.GetComponent<JSK_PictureResource>().getTextureByName(key);
			Pictures[key] = (Texture2D)Resources.Load("JSK/Picture/" + key);
		}
		return (Texture2D)Pictures[key];
	}
	
	//语言版本 1简中 2繁中 3英语 4日语.
	public static Texture2D GetLanguagePicture( string key )
    {
		if( JSK_GlobalProcess.g_Language == 1 )
			key += "_cn";
		else if( JSK_GlobalProcess.g_Language == 2 )
			key += "_en";
		
		if( !Pictures.Contains(key) )
		{
			//Pictures[key] = JSK_PictureResource.g_PictureResourceObj.GetComponent<JSK_PictureResource>().getTextureByName(key);
			Pictures[key] = (Texture2D)Resources.Load("JSK/Language/" + key);
		}
		return (Texture2D)Pictures[key];
	}
	
}
