using UnityEngine;
using System.Collections;

public class DiabloIII_HP : MonoBehaviour 
{
    public float XP = 10;   //貼圖出現的X位置(%)
    public float YP = 10;   //貼圖出現的Y位置(%)
    public Texture2D Texture_SPCircle_Current ;//要顯示的血條 (在前)
    public Texture2D Texture_SPCircle_Dark ;   //當遮色片的血條 (在後)
    public float CurrentValue = 10;   //目前的血量百分比 (這邊之後可以調調看，效果會馬上出現)
    protected float ValueMin = 8;     //血量最小值  (照著你的圖調整誤差用)
    protected float ValueMax = 89;    //血量最大值  (照著你的圖調整誤差用)
	// Use this for initialization
	void Start ()
    {
	
	}	
	// Update is called once per frame
	void Update () 
    {
	
	} 
    void OnGUI() 
    {
        //顯示血條
        GUIFunction.DrawBar(1, XP, YP, 128, 128, Texture_SPCircle_Current, Texture_SPCircle_Dark, CurrentValue, ValueMin, ValueMax);
    }
}

public class GUIFunction 
{
    //GUI遮色片
    //使用格式：GUIFunction.DrawBar(縮放比例, X位置(百分比), Y位置(百分比), 貼圖寬度, 貼圖高度, 底貼圖, 前貼圖, 目前填滿的比例, 填滿的最大值, 填滿的最小值)
    public static void DrawBar(float MaxRatio , float WidthPositionPercent , float HeightPostionPercent , float GUIWidth , float GUIHeight , Texture2D BG , Texture2D FG , float CurrentValue , float ValueMin , float ValueMax)
    {
        float CurrentWidth ;
        float CurrentHeight ;
        string str = "" + Screen.width ;
        float Ratio = float.Parse(str) / 2000;   //先計算螢幕目前的寬度比例
        if (Ratio > MaxRatio)   //大於最大比例就讓比例強制等於
            Ratio = MaxRatio;
        if (Ratio < 0.3f)    //最小比例不會小於原始的30%
            Ratio = 0.3f;
        CurrentWidth = GUIWidth * Ratio;  //縮放比例
        CurrentHeight = GUIHeight * Ratio;  //縮放比例

        //控制百分比，不讓它超過100%與低於0%
        if (WidthPositionPercent > 100)
            WidthPositionPercent = 100;
        if (HeightPostionPercent > 100)
            HeightPostionPercent = 100;
        if (WidthPositionPercent < 0 )
            WidthPositionPercent = 0;
        if (HeightPostionPercent < 0) 
            HeightPostionPercent = 0;

        //計算數值間距
        //EX：最大值(ValueMax)為100，最小值(ValueMin)為0，間隔(Interval) = 100 - 0 = 100
        float Interval = ValueMax - ValueMin;

        //目前的血量比例；以數學公式導出得結果，得出來的結果是百分比
        //Ex：最小值(ValueMin)為0，目前填滿的比例(CurrentValue)為50，間隔(Interval)為100
        //目前血量比例(Value) = 0 + (100 - 50) * 100 * 0.01 = 50
        float Value = ValueMin + (100.0f - CurrentValue) * Interval * 0.01f;  

        //計算螢幕座標位置 (使物件對齊置中)
        float CurrentXPosition = Screen.width * WidthPositionPercent * 0.01f - CurrentWidth * 0.5f;
        float CurrentYPosition = Screen.height * HeightPostionPercent * 0.01f - CurrentHeight * 0.5f;

        //開啟GUI群組功能
        GUI.BeginGroup(new Rect(CurrentXPosition, CurrentYPosition, CurrentWidth, CurrentHeight));
        if (BG) GUI .DrawTexture ( new Rect (0, 0, CurrentWidth, CurrentHeight), BG);  //確認有無底貼圖，有貼圖則在群組的正中央繪製貼圖
        GUI.BeginGroup(new Rect(0.0f, 0.0f, CurrentWidth, CurrentHeight * Value * 0.01f));
        if (FG) GUI.DrawTexture(new Rect (0, 0, CurrentWidth, CurrentHeight), FG);  //確認有無前貼圖，有貼圖則在群組的正中央繪製貼圖
        GUI.EndGroup();
        GUI.EndGroup();
    }
}
