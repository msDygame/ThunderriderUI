using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public enum JSK_MenuState
{
	None,
	Render,
	NoRender,
	Active,
	DisActive,
	Down,
	TextureIndex,
	ENUM_SPRITE_INDEX
}

public class JSK_MenuObject : MonoBehaviour
{
	public  Material 		materialActive 	  		= null;
	public  Material 		materialDisActive		= null;
	public  Material 		materialDown			= null;
	public  Texture[] 		textureList				= null;
	public  Sprite[]		SpriteList				= null; 
	private JSK_MenuState	curState				= JSK_MenuState.None;
	
	public JSK_MenuState getMenuState()
	{
		return curState;
	}

	public void setMenuState( JSK_MenuState state, int index )
	{
		curState = state;
		
		switch( state )
		{
		case JSK_MenuState.Render:
			renderer.enabled = true;
			break;
		case JSK_MenuState.NoRender:
			renderer.enabled = false;
			break;
		case JSK_MenuState.Active:
			renderer.material = materialActive;
			break;
		case JSK_MenuState.DisActive:
			renderer.material = materialDisActive;
			break;
		case JSK_MenuState.Down:
			renderer.material = materialDown;
			break;
		case JSK_MenuState.TextureIndex:
			renderer.material.mainTexture = textureList[index];
			break;
		case JSK_MenuState.ENUM_SPRITE_INDEX:
			this.transform.GetComponent<SpriteRenderer>().sprite = SpriteList[index];
			break ;	
		default:break ;
		}
	}
	
}
