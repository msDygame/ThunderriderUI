using UnityEngine;
using System.Collections;
/// <summary>
/// 3D文字專用腳本
/// </summary>
public class M3DTEXT : MonoBehaviour 
{
	/// <summary>
	/// 是否要黑邊
	/// </summary>
	//public bool mBlackBG;
	/// <summary>
	/// 是否使用翻譯文字內容
	/// </summary>
	public bool mUseContent=false;
	public int mContentIndex;
	public Color32 mFontColor;
	//public Color32 mFontBACKColor;
	TextMesh mTextMesh;
	
	int mLaquageIndex=0;//0 繁中,1 英文
	/// <summary>
	/// 翻譯文字內容
	/// </summary>
	string[][] mContentAy=new string[][]
	{
		new string[]{"開戰","Play Now"}
		
	};
	
	// Use this for initialization
	void Start () 
	{
		mTextMesh=this.transform.GetComponentInChildren<TextMesh>();
		//mTextMesh.anchor=TextAnchor.UpperCenter;
		//mTextMesh.renderer.material.color=mFontColor;
		if(mUseContent)
			mTextMesh.text=mContentAy[mContentIndex][mLaquageIndex];
		
//		if(mBlackBG)
//		{
//			GameObject aBlackBG=Instantiate(mTextMesh.transform.gameObject,mTextMesh.transform.localPosition,Quaternion.identity) as GameObject;
//			aBlackBG.transform.parent=this.transform;
//			//aBlackBG.transform.localPosition=TextMesh.transform.localPosition;
//			//aBlackBG.transform.localScale=mTextMesh.transform.localScale*1f;
//			aBlackBG.transform.localPosition=new Vector3(mTextMesh.transform.localPosition.x,mTextMesh.transform.localPosition.y,mTextMesh.transform.localPosition.z+0f);
//			TextMesh aTextMesh=aBlackBG.transform.GetComponent<TextMesh>();
//			aTextMesh.characterSize=1.5f;
//			
//			aTextMesh.renderer.material.color=mFontBACKColor;
//			if(mUseContent)
//				aTextMesh.text=mContentAy[mContentIndex][mLaquageIndex];
//		
//		}
	}
	
	public void Text(int iContect)
	{
		mTextMesh=this.transform.GetComponent<TextMesh>();
		mTextMesh.text=mContentAy[iContect][mLaquageIndex];
	}
	
	public void Text(string iContect)
	{
		mTextMesh=this.transform.GetComponent<TextMesh>();
		mTextMesh.text=iContect;
	}
	
	public void SetTextColor(Color32 iColor)
	{
		mTextMesh=this.transform.GetComponent<TextMesh>();
		mTextMesh.renderer.material.color=iColor;
	}
	
	//0x1199FF沒有ALPHA
	public void SetTextColor(uint iColor24)
	{
		uint aR=iColor24>>16;
		uint aG=iColor24>>8;
		aG=aG & 0xff;
		uint aB= iColor24 & 0xff;
		mTextMesh=this.transform.GetComponent<TextMesh>();
		//mTextMesh.renderer.material.color=new Color32((byte)aR,(byte)aG,(byte)aB,255);
		//mTextMesh.renderer.material.SetColor("_Color",new Color32((byte)aR,(byte)aG,(byte)aB,255));
		if(mTextMesh==null)
		Debug.Log("85 mTextMesh= null ,"+this.transform.parent.name);
		mTextMesh.renderer.material.color=new Color32((byte)aR,(byte)aG,(byte)aB,255); 
	}
	
	//0xff0000ff有ALPHA
	public void SetTextColorAlpha(uint iColor32)
	{
		uint aA=iColor32>>24 & 0xff;
		uint aR=iColor32>>16& 0xff;
		uint aG=iColor32>>8& 0xff;
		uint aB= iColor32 & 0xff;
		mTextMesh=this.transform.GetComponent<TextMesh>();
		//mTextMesh.renderer.material.color=new Color32((byte)aR,(byte)aG,(byte)aB,255);
		//mTextMesh.renderer.material.SetColor("_Color",new Color32((byte)aR,(byte)aG,(byte)aB,(byte)aA));
		mTextMesh.renderer.material.color=new Color32((byte)aR,(byte)aG,(byte)aB,(byte)aA);
	}
	// Update is called once per frame
//	void Update () 
//	{
//	
//	}
}
