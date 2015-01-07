using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class ToolsMenu {

		[MenuItem("Tools/Dygame/SpriteAnimation/建立 or 新增 移動路徑")]
		private static void CreateMovePath(){

			string name = "_SpriteMovePath";

			GameObject gameObject = GameObject.Find(name);
			if(gameObject == null) gameObject = new GameObject(name , typeof(iTweenPath));
			else{
				gameObject.AddComponent<iTweenPath>();
			}

			Selection.activeGameObject = gameObject;
		}

		[MenuItem("Tools/Dygame/SpriteAnimation/加入移動")]
		private static void Move(){

			AddSpriteAnimation<SpriteMove>("移動");
		}

		[MenuItem("Tools/Dygame/SpriteAnimation/加入旋轉")]
		private static void Rotate(){
			AddSpriteAnimation<SpriteRotate>("旋轉");
		}

		[MenuItem("Tools/Dygame/SpriteAnimation/加入縮放")]
		private static void Scale(){
			AddSpriteAnimation<SpriteScale>("縮放");
		}

		[MenuItem("Tools/Dygame/SpriteAnimation/加入變色")]
		private static void ChangeColor(){
			AddSpriteAnimation<SpriteColor>("變色");
		}

		private static void AddSpriteAnimation<T>(string tag) where T : BaseSpriteAnimation{

			GameObject gameObject = Selection.activeGameObject;
			
			if(gameObject == null){
				Debug.LogWarning("未選擇任何 gameObject");
				return;
			}
			
			T spriteAnim = gameObject.GetComponent<T>();
			
			if(spriteAnim == null) gameObject.AddComponent<T>();
			else Debug.Log(string.Format("{0} 已經被加入過 {1}" , gameObject.name , tag));
		}
	}
}