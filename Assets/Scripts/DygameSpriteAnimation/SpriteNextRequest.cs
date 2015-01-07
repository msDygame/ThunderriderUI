using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	[System.Serializable]
	public class SpriteNextRequest {

		public GameObject target;
		public bool all;
		public bool move;
		public bool rotate;
		public bool scale;
		public bool color;
	}
}