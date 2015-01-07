using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour 
{
    protected int iCount = 0;
	// Use this for initialization
	void Start () 
    {
	
	}	
	// Update is called once per frame
	void Update () 
    {
	
	}
    //
    public void onClickOn() { Debug.Log(name + " is Clicked! " + iCount); iCount++; }
    public void onClickIn(int iInput)
    {
        Debug.Log(name + " is Clicked! " + iCount); iCount++;
        if (iInput == 1) Application.LoadLevel("UI_Motion");
        else if (iInput == 2) Application.LoadLevel("JSK_MarketMenu");
        else if (iInput == 3) Application.LoadLevel("JSK_ThunderRankMenu");
        else if (iInput == 4) Application.Quit();
        else return;
    }
}
