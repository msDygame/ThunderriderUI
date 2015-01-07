using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class SpriteMove : BaseSpriteAnimation {
		
		public bool orientToPath;
		public SpriteMoveHash[] hashtables;

		public override void PlaySingle (int target, bool toNext)
		{
			if(base.current >= this.hashtables.Length) return;

			Hashtable hash = base.SetBaseHash<SpriteMoveHash>(this.hashtables , toNext);
			
			hash.Add("path", iTweenPath.GetPath(this.hashtables[base.current].pathName));
			hash.Add("movetopath", this.hashtables[base.current].moveToPath);
			hash.Add("orienttopath", this.orientToPath);
			
			iTween.MoveTo(gameObject , hash);
		}

		public void PlayAllMove(){

			base.PlayAll();
		}
	}
}
