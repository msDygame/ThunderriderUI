using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class SpriteScale : BaseSpriteAnimation {

		public SpriteScaleHash[] hashtables;

		public override void PlaySingle (int target, bool toNext)
		{
			if(base.current >= this.hashtables.Length) return;

			Hashtable hash = base.SetBaseHash<SpriteScaleHash>(this.hashtables , true);
			
			hash.Add("scale" , this.hashtables[base.current].scale);
			
			iTween.ScaleTo(gameObject , hash);
		}

		public void PlayAllScale(){

			base.PlayAll();
		}
	}
}