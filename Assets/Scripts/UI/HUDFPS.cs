using UnityEngine;
using System.Collections;
public class HUDFPS : MonoBehaviour 
{
	
	// Attach this to a GUIText to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.
	
	public  float updateInterval = 0.5F;
	
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private float fps = 0f;
    VersionCodeScriptObject PH;
	void Start()
	{
		timeleft = updateInterval;
        PH = (VersionCodeScriptObject)Resources.Load("UI/VersionCode", typeof(VersionCodeScriptObject));
	}

	void OnGUI()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			fps = accum/frames;
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
		
		Vector2	posIdx	= new Vector2(10f, 100f);
		int 	iLine	= 0;
		float	XL = 10f, YL = 30f ;
		
		GUIStyle aStyle = new GUIStyle();
		aStyle.fontSize = 20;
		aStyle.normal.textColor = Color.white;

		if(fps < 30)
			aStyle.normal.textColor = Color.red;
		else
			aStyle.normal.textColor = Color.green;
		
		GUI.Label(new Rect(posIdx.x, posIdx.y+iLine*YL, XL, YL),"FPS : "+fps.ToString(), aStyle);
		iLine++;

        aStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(XL, 3, XL, YL), PH.BundleVersionName, aStyle);

        GUI.Label(new Rect(XL, 3 + iLine * YL, XL, YL), PH.BundleVersionCode, aStyle);		
	}
}