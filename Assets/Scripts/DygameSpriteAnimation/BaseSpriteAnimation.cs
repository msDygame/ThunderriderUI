using UnityEngine;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public abstract class BaseSpriteAnimation : MonoBehaviour {

		public bool playAutomatically;
		public SpriteNextRequest[] next;
		
		protected int current;

		private const string FUNC_NEXT = "PlayNext";
		private const string FUNC_NEXT_GAMEOBJECTS = "PlayNextGameObjects";
		private const string FUNC_PLAYALL = "PlayAll";
		private const string FUNC_PLAYALL_MOVE = "PlayAllMove";
		private const string FUNC_PLAYALL_ROTATE = "PlayAllRotate";
		private const string FUNC_PLAYALL_SCALE = "PlayAllScale";
		private const string FUNC_PLAYALL_COLOR = "PlayAllColor";

		void Start () {
			
			if(this.playAutomatically) this.PlayAll();
		}

		public abstract void PlaySingle(int target , bool toNext);

		public void PlayAll ()
		{
			this.PlaySingle(this.current = 0 , true);
		}

		protected Hashtable SetBaseHash<T>(T[] hashtables , bool toNext) where T : SpriteHash{

			Hashtable hash = iTween.Hash(
				"time" , hashtables[this.current].time ,
				"delay" , hashtables[this.current].delay,
				"easetype" , hashtables[this.current].easeType,
				"looptype" , hashtables[this.current].loopType
				);

			if(this.current == hashtables.Length - 1){

				hash.Add("oncompletetarget", gameObject);
				hash.Add("oncomplete" , FUNC_NEXT_GAMEOBJECTS);

			}else if(toNext){

				hash.Add("oncompletetarget", gameObject);
				hash.Add("oncomplete" , FUNC_NEXT);
			}

			return hash;
		}

		private void PlayNext(){

			this.PlaySingle(++this.current , true);
		}

		private void PlayNextGameObjects(){

			foreach(SpriteNextRequest request in this.next){

				if(request.target == null) continue;

				if(request.all) request.target.SendMessage(FUNC_PLAYALL);
				else{

					if(request.move) request.target.SendMessage(FUNC_PLAYALL_MOVE);
					if(request.rotate) request.target.SendMessage(FUNC_PLAYALL_ROTATE);
					if(request.scale) request.target.SendMessage(FUNC_PLAYALL_SCALE);
					if(request.color) request.target.SendMessage(FUNC_PLAYALL_COLOR);
				}
			}
		}
	}
}