using UnityEngine;
using System.Collections;

public class JSK_LogoFlag : MonoBehaviour
{
	private	float	animSpeed	= 2.0f;
	
	void Awake()
	{
		Transform flagLeft = transform.Find("Flag_Left");
		AnimationState left = flagLeft.animation["Ae_FlagLoop"];
		left.speed = animSpeed;
		flagLeft.animation.Play("Ae_FlagLoop");
		
		Transform flagRight = transform.Find("Flag_Right");
		AnimationState right = flagRight.animation["Ae_FlagLoop"];
		right.speed = animSpeed;
		flagRight.animation.Play("Ae_FlagLoop");
		
        //flagLeft.gameObject.SetActiveRecursively(false);
        //flagRight.gameObject.SetActiveRecursively(false);
	}
}
