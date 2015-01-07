using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ToolsMenu {

	[MenuItem("Tools/Dygame/Lightmapping/儲存 Lightmap Settings")]
	static void SaveLightmapSettings(){

		LightmapSettingsHolder obj = ScriptableObject.CreateInstance<LightmapSettingsHolder>();

		obj.bakedColorSpace = LightmapSettings.bakedColorSpace;
		obj.lightmapsMode = LightmapSettings.lightmapsMode;
		obj.lightProbes = LightmapSettings.lightProbes;

		obj.lightmaps = new LightmapDataHolder[LightmapSettings.lightmaps.Length];
		for(int i = 0 ; i < LightmapSettings.lightmaps.Length ; i++){

			obj.lightmaps[i] = new LightmapDataHolder(){ lightmapFar = LightmapSettings.lightmaps[i].lightmapFar , lightmapNear = LightmapSettings.lightmaps[i].lightmapNear};
		}

		AssetDatabase.CreateAsset(obj , "Assets/LightmapSettings.asset");

		Debug.Log("Lightmap Settings 儲存在 Assets/LightmapSettings.asset");
	}

	[MenuItem("Tools/Dygame/Lightmapping/載入 Lightmap Settings")]
	static void LoadLightmapSettings(){

		Object obj = Selection.activeObject;

		if(obj == null){

			Debug.Log("未選擇任何檔案");
			return;
		}

		if(!(obj is LightmapSettingsHolder)){

			Debug.Log("不是 Lightmap 資料檔");
			return;
		}

		LightmapSettingsHolder holder = (LightmapSettingsHolder)obj;

		LightmapSettings.bakedColorSpace = holder.bakedColorSpace;
		LightmapSettings.lightmapsMode = holder.lightmapsMode;
		LightmapSettings.lightProbes = holder.lightProbes;

		List<LightmapData> _lightmaps = new List<LightmapData>();
		for(int i = 0; i < holder.lightmaps.Length; i++){

			_lightmaps.Add(new LightmapData(){ lightmapFar = holder.lightmaps[i].lightmapFar , lightmapNear = holder.lightmaps[i].lightmapNear});
		}

		LightmapSettings.lightmaps = _lightmaps.ToArray();
	}
}
