using UnityEngine;
using System.Collections;

public class ButtonOnPressed : MonoBehaviour 
{
    GameObject Male = null;
    GameObject Female = null;
    void Awake()
    {
        GameObject button = GameObject.Find("UI_NGUI_Main/Sprite_1_InGame");
        UIEventListener.Get(button).onClick = ButtonClick1;

        button = GameObject.Find("UI_NGUI_Main/Sprite_2_Market");
        UIEventListener.Get(button).onClick = ButtonClick2;

        button = GameObject.Find("UI_NGUI_Main/Sprite_3_Action");
        UIEventListener.Get(button).onClick = ButtonClick3;

        button = GameObject.Find("UI_NGUI_Main/Sprite_4_Ranking");
        UIEventListener.Get(button).onClick = ButtonClick4;

        button = GameObject.Find("UI_NGUI_Main/Sprite_5_Exit");
        UIEventListener.Get(button).onClick = ButtonClick5;

        button = GameObject.Find("UI_NGUI_Main/Sprite_Player_Nickname");
        UIEventListener.Get(button).onClick = ButtonClickMale;

        button = GameObject.Find("UI_NGUI_Main/Sprite_Player_Level");
        UIEventListener.Get(button).onClick = ButtonClickFemale;
    }
	// Use this for initialization
	void Start () 
    {
        Female = GameObject.Find("UI_NGUI_Main/Character/tr2_female");
        Male = GameObject.Find("UI_NGUI_Main/Character/tr2_male_Prefab");
        Male.SetActive(false);
	}	
	// Update is called once per frame
	void Update () 
    {
	
	}
    //
    void ButtonClick1(GameObject button)
    {
        Application.LoadLevel("UI_Motion");
    }
    //
    void ButtonClick2(GameObject button)
    {
        Application.LoadLevel("JSK_MarketMenu");
    }
    //
    void ButtonClick3(GameObject button)
    {
        return;
    }
    //
    void ButtonClick4(GameObject button)
    {
        Application.LoadLevel("JSK_ThunderRankMenu");
    }
    void ButtonClick5(GameObject button)
    {
        Application.Quit();
    }
    //
    void ButtonClickMale(GameObject button)
    {        
        Female.SetActive(false);
        Male.SetActive(true);
    }
    //
    void ButtonClickFemale(GameObject button)
    {
        Female.SetActive(true);
        Male.SetActive(false);
    }
}
