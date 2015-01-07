using UnityEngine;
using System.Collections;


public class MessageDisplayer : MonoBehaviour
{
	public static int iMaxLineCount = 10;
	private float fMaxShowMessageTime = 5f;
	private float fRaiseSpeed = 40f;
	private string [] MessageTable = new string[iMaxLineCount];
	private float [] ShowTimeTable = new float[iMaxLineCount];
	
	private float fLineHeight;
	private float fLineWidth;
	private float BasePosX;
	private float BasePosY;
	
	void Start () 
	{
		fLineHeight	= 20f;
		fLineWidth	= 100f;
		BasePosX	= Screen.width/2f+50f;
		BasePosY	= Screen.height/2f;
		for(int i=0; i<iMaxLineCount; i++)
			ShowTimeTable[i]=0f;
		
	}
	
	public void DisplayMessage(string message)// add a message to show
	{
		if(enabled==false)
			enabled = true;
		for(int i=(iMaxLineCount-1); i>=0; i--)
		{
			if(i==0)
			{
				MessageTable[i]=message;
				ShowTimeTable[i]=Time.time;
			}
			else
			{
				MessageTable[i]=MessageTable[i-1];
				ShowTimeTable[i]=ShowTimeTable[i-1];
			}
		}
	}
	void SetMessagePos(Vector2 aVect2)
	{
		BasePosX = aVect2.x;
		BasePosY = aVect2.y;
	}
	void OnGUI()
	{
		if(ShowTimeTable[0]==0)
		{
			enabled = false;
			return;
		}
		for(int i=0; i<iMaxLineCount; i++)
		{
			if(ShowTimeTable[i]==0)
				break;

			float fTimeLast = ShowTimeTable[i]+fMaxShowMessageTime-Time.time;
			if(fTimeLast<=0)
			{
				fTimeLast = 0f;
				ShowTimeTable[i]=0;
				break;
			}
			GUIStyle aStyle = new GUIStyle();
			float fShowScale = fTimeLast/fMaxShowMessageTime;
			aStyle.normal.textColor = new Color(1f, 1f, 1f, fShowScale);
			//aStyle.normal.textColor.a = fShowScale;
			GUI.Label(new Rect(BasePosX, BasePosY-fLineHeight*i-fRaiseSpeed/fShowScale, fLineWidth, fLineHeight), MessageTable[i], aStyle);
		}
	}
}
