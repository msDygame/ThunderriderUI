using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class JSK_SceneLoad : MonoBehaviour
{
	public Material[] 	SkyboxMaterials			= null;
	public Material[] 	SkyboxMaterialsAndroid1 = null;//改过的天空盒材质.
	public Material[] 	SkyboxMaterialsAndroid2 = null;//unity的移动天空盒材质.
	public Texture2D[]	LightmapMaterials1		= null;
	public Texture2D[]	LightmapMaterials2		= null;
	public Texture2D[]	LightmapMaterials3		= null;
	public Texture2D[]	LightmapMaterials4		= null;
	public Texture2D[]	LightmapMaterials5		= null;
	
	public Material[] 	SkyboxAndroidRing 		= null;
	public Texture2D[]	LightmapMaterialsRing	= null;
	
	private GameObject	SceneObject 			= null;
	
    public GameObject LoadScene( int index )//加载场景.
    {
		string sLoadedScenePath = "";
		
		if( JSK_GlobalProcess.g_IsAndroidRing )
		{
			sLoadedScenePath = "JSK/SceneAndroid/Scene" + index.ToString();
			
			SceneObject = JSK_GlobalProcess.CreatSceneObject("JSK/SceneAndroid/Scene" + index.ToString(), Vector3.zero, Quaternion.identity);
			//SceneObject.name = "JSK_GameScene";
			SceneObject.name = "JSK_GameScene_Ring";
			RenderSettings.skybox = SkyboxAndroidRing[index-1];
			LightmapSettings.lightmapsMode = LightmapsMode.Single;
	        LightmapData[] maps0 = LightmapSettings.lightmaps;
	        maps0[0].lightmapFar = LightmapMaterialsRing[index - 1];
	        LightmapSettings.lightmaps = maps0;
			RenderSettings.ambientLight = Color256(255, 255, 255, 255);
			SceneObject.transform.Find("SceneEffect").gameObject.SetActiveRecursively(false);
			RenderSettings.fog = false;
			return SceneObject;
		}
		else
		{
			sLoadedScenePath = "JSK/Scene/Scene" + index.ToString();
			SceneObject = JSK_GlobalProcess.CreatSceneObject("JSK/Scene/Scene" + index.ToString(), Vector3.zero, Quaternion.identity);
			//SceneObject.name = "JSK_GameScene";
			SceneObject.name = "Scene"+ index.ToString();
		}
		
		Debug.Log("LoadScene:" + SceneObject.name);
		//Debug.Log("LoadScene path:" + sLoadedScenePath);
		
		//天空盒.
		#if UNITY_ANDROID
		//RenderSettings.skybox = SkyboxMaterialsAndroid1[index-1];
		RenderSettings.skybox = SkyboxMaterialsAndroid2[index-1];
		#else
		RenderSettings.skybox = SkyboxMaterials[index-1];
		#endif
		
		LightmapSettings.lightmapsMode = LightmapsMode.Single;
		LightmapData[] maps = LightmapSettings.lightmaps;
		
		if( index == 1 )
		{
			for( int i = 0; i < LightmapMaterials1.Length; i++ )
				maps[i].lightmapFar = LightmapMaterials1[i];
			RenderSettings.fogColor = Color256(97, 189, 255, 255);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogDensity = 0.01f; 
            RenderSettings.fogStartDistance = 100;
            RenderSettings.fogEndDistance = 800;
            RenderSettings.ambientLight = Color256(179, 173, 232, 255);
		}
        else if( index == 2 )
        {
			for( int i = 0; i < LightmapMaterials2.Length; i++ )
				maps[i].lightmapFar = LightmapMaterials2[i];
            RenderSettings.fogColor = Color256(33, 25, 53, 255);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.005f; 
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 800;
            RenderSettings.ambientLight = Color256(99, 83, 158, 255);
        }
        else if( index == 3 )
        {
			for( int i = 0; i < LightmapMaterials3.Length; i++ )
				maps[i].lightmapFar = LightmapMaterials3[i];
//            RenderSettings.fogColor = Color256(179, 200, 184, 255);
//            RenderSettings.fogMode = FogMode.ExponentialSquared;
//            RenderSettings.fogDensity = 0.002f;
//            RenderSettings.fogStartDistance = 10;
//            RenderSettings.fogEndDistance = 2000;
//            RenderSettings.ambientLight = Color256(149, 175, 136, 255);
			RenderSettings.fogColor = Color256(33, 25, 53, 255);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.005f; 
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 800;
            RenderSettings.ambientLight = Color256(99, 83, 158, 255);
        }
        else if( index == 4 )
        {
			for( int i = 0; i < LightmapMaterials4.Length; i++ )
				maps[i].lightmapFar = LightmapMaterials4[i];
            RenderSettings.fogColor = Color256(13, 38, 31, 255);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogDensity = 0.005f;
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 200;
            RenderSettings.ambientLight = Color256(139, 143, 93, 255);
        }
		else if( index == 5 )
        {
			for( int i = 0; i < LightmapMaterials5.Length; i++ )
				maps[i].lightmapFar = LightmapMaterials5[i];
            RenderSettings.fogColor = Color256(158, 186, 206, 255);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogDensity = 0.005f;
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 700;
            RenderSettings.ambientLight = Color256(150, 137, 238, 255);
        }
		LightmapSettings.lightmaps = maps;
		initQualityForPlatform();
		
		return SceneObject;
    }
	
	public void initQualityForPlatform()
	{
		//雾效.
		#if UNITY_ANDROID
			SceneObject.transform.Find("SceneEffect").gameObject.SetActiveRecursively(false);
			RenderSettings.fog = false;
		#else
			RenderSettings.fog = true;
		#endif
	}
	
    Color Color256( int r, int g, int b, int a )
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }
	
}