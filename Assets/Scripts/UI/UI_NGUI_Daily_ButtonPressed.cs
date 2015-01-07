using UnityEngine;
using System.Collections;

public class UI_NGUI_Daily_ButtonPressed : MonoBehaviour 
{
    void Awake()
    {
        GameObject button = GameObject.Find("UI_NGUI_Daily/Sprite_Wall_Dn_Button_A");
        UIEventListener.Get(button).onClick = OnClick1;
    }
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    //
    void OnClick1(GameObject button)
    {
        Application.LoadLevel("UI_NGUI_Main");
    }
}
