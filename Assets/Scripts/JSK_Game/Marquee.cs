using UnityEngine;
using System.Collections;

public class Marquee : MonoBehaviour
{
	public string message ;		// 跑馬燈文字
	public float scrollSpeed; 	// 捲動速度
	Rect messageRect;			// 初始位置與文字寬度設定

	public void init(Rect aRect, string aStr, float aSpeed)
	{
		messageRect = aRect;
		message = aStr;
		scrollSpeed = aSpeed;
		this.enabled = false;
	}

	public void Start()
	{
		enabled = true;
	}

	public void End()
	{
		enabled = false;
	}

	void OnGUI ()
	{
		if(scrollSpeed==0)
			return;
		// Set up the message's rect if we haven't already
		if (messageRect.width == 0) {
			Vector2 dimensions = GUI.skin.label.CalcSize(new GUIContent(message));
			
			// Start the message past the left side of the screen
			messageRect.x      = -dimensions.x;
			messageRect.width  =  dimensions.x;
			messageRect.height =  dimensions.y;
		}
		
		messageRect.x += Time.deltaTime * scrollSpeed;
		
		// If the message has moved past the right side, move it back to the left
		if (messageRect.x > Screen.width) {
			messageRect.x = -messageRect.width;
		}
		
		GUI.Label(messageRect, message);
	}
}