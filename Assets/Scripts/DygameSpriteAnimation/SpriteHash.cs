using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public abstract class SpriteHash {

		public float time = 1;
		public float delay = 0;
		public iTween.EaseType easeType = iTween.EaseType.linear;
		public iTween.LoopType loopType = iTween.LoopType.none;
	}

	[System.Serializable]
	public class SpriteMoveHash : SpriteHash{
		
		public string pathName;
		public bool moveToPath;
	}

	[System.Serializable]
	public class SpriteRotateHash : SpriteHash{

		public Vector3 rotation;
	}

	[System.Serializable]
	public class SpriteScaleHash : SpriteHash{

		public Vector3 scale;
	}

	[System.Serializable]
	public class SpriteColorHash : SpriteHash{

		public Color color;
	}
}