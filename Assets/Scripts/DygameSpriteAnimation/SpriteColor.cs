using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class SpriteColor : BaseSpriteAnimation {

		public SpriteColorHash[] hashtables;
		private SpriteRenderer spriteRenderer;

		void Awake(){

			if(renderer is SpriteRenderer) this.spriteRenderer = (SpriteRenderer)renderer;
		}

		public override void PlaySingle (int target, bool toNext)
		{
			if(base.current >= this.hashtables.Length) return;
			
			Hashtable hash = base.SetBaseHash<SpriteColorHash>(this.hashtables , toNext);

			if(this.spriteRenderer == null){

				hash.Add("color" , this.hashtables[base.current].color);

			}else{

				hash.Add("from" , this.spriteRenderer.color);
				hash.Add("to" , this.hashtables[base.current].color);
				hash.Add("onupdate" , "OnUpdateValue");
				
				iTween.ValueTo(gameObject , hash);
			}
		}
		
		public void PlayAllColor(){
			
			base.PlayAll();
		}
		
		private void OnUpdateValue(Color color){
			
			if(this.spriteRenderer != null) this.spriteRenderer.color = color;
		}
	}
}