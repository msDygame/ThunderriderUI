using UnityEngine;
using System.Collections;
using UnityEditor;
//Unity 3D : 腳本化物件 ScriptableObject 設置資料成為 Asset
public class VersionCodeScriptAsset : ScriptableObject
{
    public string BundleVersionCode;//android versionCode
    public string BundleVersionName;//android versionName
	[MenuItem("Tools/AutoBuild/reset VersionCode Asset")]
	public static void BuildAsset()
	{
        VersionCodeScriptObject asset = ScriptableObject.CreateInstance<VersionCodeScriptObject>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/UI/VersionCode.asset");
        //get
//      asset.BundleVersionCode = string.Copy("" + PlayerSettings.Android.bundleVersionCode);//無法儲存?
//      asset.BundleVersionName = string.Copy(PlayerSettings.bundleVersion);//無法儲存?
		AssetDatabase.SaveAssets();      
        EditorUtility.DisplayDialog("create Asset", "VersionCode.asset", "OK", "");	
	}
}