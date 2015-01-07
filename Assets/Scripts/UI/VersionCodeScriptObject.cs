using UnityEngine;
using System.Collections;
//Unity 3D : 腳本化物件 ScriptableObject 設置資料成為 Asset
public class VersionCodeScriptObject : ScriptableObject
{
    public string BundleVersionCode;//android versionCode
    public string BundleVersionName;//android versionName
    void Start()
    {
        BundleVersionCode = "1";
        BundleVersionName = "1.0.0.0";
    }
}