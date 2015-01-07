using UnityEngine;
using System.Collections;

public class JSK_PlayerEffect : MonoBehaviour
{
	private	GameObject			MotoShield				= null;	// 防護罩
	private	GameObject			MotoTurbo				= null;	// 加速

	private	ParticleEmitter		WaterEffect_Tail		= null;	// 車尾水花
	private	ParticleEmitter		WaterEffect_BottonBig	= null;	// 底部水花
	private	ParticleEmitter		WaterEffect_BottonSmall	= null;	// 底部水花
	private	ParticleEmitter		WaterEffect_Fail		= null;	// 落水水花
	
	private	ParticleSystem		WaterEffect_Side		= null;	// 側邊水花

	void Start()
	{
//		MotoShield				=  transform.Find("Effect/SpeedGuard").gameObject;
//
//		WaterEffect_Tail 		= transform.Find("Effect/Water_back").particleEmitter;
//		WaterEffect_BottonBig 	= transform.Find("Effect/Water_Botton").particleEmitter;
//		WaterEffect_BottonSmall	= transform.Find("Effect/Water_Botton_stop").particleEmitter;
//
//		WaterEffect_Side		= transform.Find("Effect/WaterEffect_1").particleSystem;

//		WaterEffect_L1 			= transform.Find("WaterEffect/WaterSplash_Side/WaterEffect_L1").particleEmitter;
//		
//		WaterEffect_L2 			= transform.Find("WaterEffect/WaterSplash_Side/WaterEffect_L2").particleEmitter;
//		WaterEffect_L2_Ani		= WaterEffect_L2.GetComponent<ParticleAnimator>();
//		
//		WaterEffect_R1 			= transform.Find("WaterEffect/WaterSplash_Side/WaterEffect_R1").particleEmitter;
//			
//		WaterEffect_R2 			= transform.Find("WaterEffect/WaterSplash_Side/WaterEffect_R2").particleEmitter;
//		WaterEffect_R2_Ani		= WaterEffect_R2.GetComponent<ParticleAnimator>();
		
//		WaterEffect_Left 		= transform.Find("WaterEffect/WaterSplash_Bottom/WaterEffect_Left").particleEmitter;
//		WaterEffect_Left_Ani	= WaterEffect_Left.GetComponent<ParticleAnimator>();
//			
//		WaterEffect_Right 		= transform.Find("WaterEffect/WaterSplash_Bottom/WaterEffect_Right").particleEmitter;
//		WaterEffect_Right_Ani	= WaterEffect_Right.GetComponent<ParticleAnimator>();
			
//		WaterEffect_Side		= transform.Find("WaterEffect2/WaterSplash_Side/WaterEffect_1").particleSystem;
//		WaterEffect_Side_1		= transform.Find("WaterEffect2/WaterSplash_Side/WaterEffect_1/WaterEffect_2").particleSystem;
	}
	
	public void SetSpecialEffectEnable( bool enable )
	{
//		specialEffectEnable = enable;
//		
//		//specialEffectEnable = false;
	}
	
	float iScale = 0.0f;
//	float iScale2 = 1.0f;
	
	public void UpdateWaterEffect( float percent, Vector2 dir )
	{
//		if( percent < 0.1 || !specialEffectEnable )
//		{
//			WaterEffect_Tail.enableEmission = false;
//			
//			if( JSK_GlobalProcess.g_UseNewActor )
//			{
//				WaterEffect_Side.enableEmission = false;
//				WaterEffect_Side_1.enableEmission = false;
//			}
////			if( JSK_GlobalProcess.g_UseNewActor )
////			{
////				WaterEffect2_L1.emit = false;
////				WaterEffect2_L2.emit = false;
////				WaterEffect2_R1.emit = false;
////				WaterEffect2_R2.emit = false;
////			}
//		}
//		else
//		{
////			if( dir.x > 0.05f )
////			{
////				WaterEffect_L2.emit = false;
////				WaterEffect_L2.minSize = percent * (1.8f + iScale);
////				WaterEffect_L2.maxSize = percent * (2.2f + iScale);
////				WaterEffect_L2.minEnergy = percent * (0.6f + iScale);
////				WaterEffect_L2.maxEnergy = percent * (1.2f + iScale);
////				WaterEffect_L2.minEmission = percent * (20 + iScale);
////				WaterEffect_L2.maxEmission = percent * (25 + iScale);
////				WaterEffect_L2_Ani.damping = percent * (0.8f + iScale);
////			}
////			else
////			{
////				//WaterEffect_L2.emit = false;
////			}
////			
////			if( dir.x < -0.05f )
////			{
////				WaterEffect_R2.emit = false;
////				WaterEffect_R2.minSize = percent * (1.8f + iScale);
////				WaterEffect_R2.maxSize = percent * (2.2f + iScale);
////				WaterEffect_R2.minEnergy = percent * (0.6f + iScale);
////				WaterEffect_R2.maxEnergy = percent * (1.2f + iScale);
////				WaterEffect_R2.minEmission = percent * (20 + iScale);
////				WaterEffect_R2.maxEmission = percent * (25 + iScale);
////				WaterEffect_R2_Ani.damping = percent * (0.8f + iScale);
////			}
////			else
////			{
////				//WaterEffect_R2.emit = false;
////			}
//			
////			WaterEffect_Left.emit = false;
////			WaterEffect_Left.minSize = percent * (0.4f + iScale);
////			WaterEffect_Left.maxSize = percent * (1 + iScale);
////			WaterEffect_Left.minEnergy = percent * (1.8f + iScale);
////			WaterEffect_Left.maxEnergy = percent * (2.2f + iScale);
////			WaterEffect_Left.minEmission = percent * (20 + iScale);
////			WaterEffect_Left.maxEmission = percent * (25 + iScale);
////			WaterEffect_Left_Ani.damping = percent * (1.5f + iScale);
////			
////			WaterEffect_Right.emit = false;
////			WaterEffect_Right.minSize = percent * (0.4f + iScale);
////			WaterEffect_Right.maxSize = percent * (1 + iScale);
////			WaterEffect_Right.minEnergy = percent * (1.8f + iScale);
////			WaterEffect_Right.maxEnergy = percent * (2.2f + iScale);
////			WaterEffect_Right.minEmission = percent * (20 + iScale);
////			WaterEffect_Right.maxEmission = percent * (25 + iScale);
////			WaterEffect_Right_Ani.damping = percent * (1.5f + iScale);
//
//			if( JSK_GlobalProcess.g_UseNewActor )
//			{
//				if( dir.y >= 0 )
//				{
//					WaterEffect_Side.enableEmission = true;
//					
//					//WaterEffect_Side.emissionRate =  ( percent + 0.5f ) * 20.0f ;
//					//WaterEffect_Side.startSize = ( percent + 0.2f ) * 1.2f ;
//					
//					WaterEffect_Side_1.enableEmission = true;
//					//WaterEffect_Side_1.emissionRate = ( percent + 0.5f ) * 80.0f ;
//					//WaterEffect_Side_1.startSize = ( percent + 0.5f ) * 4.0f ;
//				}
//				else
//				{
//					//Debug.Log("WaterEffect_Side false@@@@@@@@@@@@@s");
//					WaterEffect_Side.enableEmission = false;
//					WaterEffect_Side_1.enableEmission = false;
//				}
//			}
//		}
	}

}
