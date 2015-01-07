using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	public class BaseSpriteEditor : Editor {

		protected GUIStyle style = new GUIStyle();

		protected SerializedProperty _playAutomatically;
		protected SerializedProperty _hashtables;
		protected SerializedProperty _next;
		
		protected bool editing;
		protected int editId;

		private Tool tmpTool;

		protected void SetCommonProperty(){

			this.style.fontStyle = FontStyle.Bold;
			this.style.normal.textColor = Color.white;

			this._playAutomatically = base.serializedObject.FindProperty("playAutomatically");
			
			this._hashtables = base.serializedObject.FindProperty("hashtables");
			this._next = base.serializedObject.FindProperty("next");
		}

		protected void PlayAutoGUI(){
			EditorGUILayout.PropertyField(this._playAutomatically , new GUIContent("自動播放" , "當 gameObject 被產生時或場景開始時即自動執行"));
		}

		protected void NextRequeatGUI(){

			if(this._next.arraySize == 0){

				if(GUILayout.Button("建立結束後接續的動作")) this._next.arraySize++;

			}else{

				EditorGUILayout.BeginHorizontal();
				this._next.isExpanded = EditorGUILayout.Foldout(this._next.isExpanded , new GUIContent(string.Format("接續動作 ({0})" , this._next.arraySize)));
				if(GUILayout.Button("+" , GUILayout.ExpandWidth(false))) this._next.arraySize++;
				EditorGUILayout.EndHorizontal();

				if(!this._next.isExpanded) return;

				for(int i = 0 ; i < this._next.arraySize ; i++){

					SerializedProperty _properties = this._next.GetArrayElementAtIndex(i);
					SerializedProperty _all = _properties.FindPropertyRelative("all");

					EditorGUILayout.BeginVertical("box");

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(_properties.FindPropertyRelative("target") , new GUIContent("目標物件", "請求執行後續動作的目標物件 gameObject"));
					if(GUILayout.Button("-" , GUILayout.ExpandWidth(false))){
						this._next.DeleteArrayElementAtIndex(i);
						break;
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.PropertyField(_all , new GUIContent("全部", "目標物件 gameObject 如果包含移動、旋轉、縮放、變色 等動作則全部執行"));

					if(!_all.boolValue){

						EditorGUILayout.PropertyField(_properties.FindPropertyRelative("move") , new GUIContent("移動", "目標物件 gameObject 如果包含移動的動作則執行"));
						EditorGUILayout.PropertyField(_properties.FindPropertyRelative("rotate") , new GUIContent("旋轉", "目標物件 gameObject 如果包含旋轉的動作則執行"));
						EditorGUILayout.PropertyField(_properties.FindPropertyRelative("scale") , new GUIContent("縮放", "目標物件 gameObject 如果包含縮放的動作則執行"));
						EditorGUILayout.PropertyField(_properties.FindPropertyRelative("color") , new GUIContent("改變顏色", "目標物件 gameObject 如果包含改變顏色的動作則執行"));
					}

					EditorGUILayout.EndVertical();
				}
			}

		}

		protected void SettingsForEmpty(string label){

			if(GUILayout.Button(label)) this._hashtables.arraySize++;
		}

		protected void SettingsFoldout(string label){

			EditorGUILayout.BeginHorizontal();
			this._hashtables.isExpanded = EditorGUILayout.Foldout(this._hashtables.isExpanded , new GUIContent(string.Format("{0} ({1})" , label , this._hashtables.arraySize)));

			if(this.editing){
				GUI.enabled = false;
				this.SettingsAddButton();
				GUI.enabled = true;
			}else this.SettingsAddButton();

			EditorGUILayout.EndHorizontal();
		}

		private void SettingsAddButton(){
			if(GUILayout.Button("+" , GUILayout.ExpandWidth(false))) this._hashtables.arraySize++;
		}

		protected bool DeleteSettingButton(int index){

			if(GUILayout.Button("-" , GUILayout.ExpandWidth(false))){
				this._hashtables.DeleteArrayElementAtIndex(index);
				return true;
			}

			return false;
		}

		protected void CommonSettingGUI(SerializedProperty property){

			SerializedProperty _time = property.FindPropertyRelative("time");
			SerializedProperty _delay = property.FindPropertyRelative("delay");

			EditorGUILayout.PropertyField(_time , new GUIContent("時間","動作開始到結束所需花費的時間"));
			EditorGUILayout.PropertyField(_delay , new GUIContent("延遲","指定延遲多少時間後才開始動作"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("easeType") , new GUIContent("緩和種類" , "動作過程的緩和方式"));
			EditorGUILayout.PropertyField(property.FindPropertyRelative("loopType") , new GUIContent("循環方式" , "重複循環的方式"));
			
			if(_time.floatValue <= 0) _time.floatValue = 0.5f;
			if(_delay.floatValue < 0) _delay.floatValue = 0;
		}

		protected void HideTool(){

			this.tmpTool = Tools.current;
			Tools.current = Tool.None;
		}

		protected void ReuseTool(){
			Tools.current = this.tmpTool;
		}
	}
}