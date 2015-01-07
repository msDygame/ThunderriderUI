using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class SpriteRotate : BaseSpriteAnimation {

		public SpriteRotateHash[] hashtables;

		public override void PlaySingle (int target, bool toNext)
		{
			if(base.current >= this.hashtables.Length) return;

			Hashtable hash = base.SetBaseHash<SpriteRotateHash>(this.hashtables , toNext);

			hash.Add("from" , transform.localEulerAngles);
			hash.Add("to" , this.hashtables[base.current].rotation);
			hash.Add("onupdate" , "OnUpdateValue");

			iTween.ValueTo(gameObject , hash);
		}

		public void PlayAllRotate(){

			base.PlayAll();
		}

		private void OnUpdateValue(Vector3 localeulerAngles){

			transform.localEulerAngles = localeulerAngles;
		}
	}
}