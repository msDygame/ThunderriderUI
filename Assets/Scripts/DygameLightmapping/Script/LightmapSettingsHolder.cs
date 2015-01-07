using UnityEngine;
using System.Collections;

public class LightmapSettingsHolder : ScriptableObject {

	public ColorSpace bakedColorSpace;
	public LightmapDataHolder[] lightmaps;
	public LightmapsMode lightmapsMode;
	public LightProbes lightProbes;
}

[System.Serializable]
public class LightmapDataHolder{

	public Texture2D lightmapFar;
	public Texture2D lightmapNear;
}
