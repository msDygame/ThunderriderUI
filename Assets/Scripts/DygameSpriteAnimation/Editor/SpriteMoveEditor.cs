using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	[CustomEditor(typeof(SpriteMove))]
	public class SpriteMoveEditor : BaseSpriteEditor {

		private SerializedProperty _orientToPath;
		private string[] pathNames;

		void OnEnable(){

			base.SetCommonProperty();

			this._orientToPath = base.serializedObject.FindProperty("orientToPath");

			iTweenPath[] _iTweenPath = GameObject.FindObjectsOfType<iTweenPath>();
			this.pathNames = new string[_iTweenPath.Length];
			for(int i = 0 ; i < _iTweenPath.Length ; i++){

				this.pathNames[i] = _iTweenPath[i].pathName;
			}
		}

		public override void OnInspectorGUI ()
		{
			base.serializedObject.Update();

			base.PlayAutoGUI();
			EditorGUILayout.PropertyField(this._orientToPath , new GUIContent("面向路徑" , "gameObject 的 z 軸朝向路徑前進方向移動"));

			if(this.pathNames.Length == 0){

				EditorGUILayout.LabelField("請先建立移動路徑，才可開始建立移動設置");

			}else if(base._hashtables.arraySize == 0){

				base.SettingsForEmpty("建立移動設置");

			}else{

				base.SettingsFoldout("移動設置");
				if(base._hashtables.isExpanded){

					EditorGUI.indentLevel = 1;

					for(int i = 0 ; i < base._hashtables.arraySize ; i++){

						SerializedProperty _item = base._hashtables.GetArrayElementAtIndex(i);
						SerializedProperty _pathName = _item.FindPropertyRelative("pathName");

						EditorGUILayout.BeginVertical("box");

						EditorGUILayout.BeginHorizontal();
						int _pathNameIndex = EditorGUILayout.Popup("路徑" , this.GetPathNameIndex(_pathName.stringValue) , this.pathNames);
						_pathName.stringValue = this.pathNames[_pathNameIndex];
						if(base.DeleteSettingButton(i)) break;
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.PropertyField(_item.FindPropertyRelative("moveToPath") , new GUIContent("移至路徑" , "當開始移動時，先從 gameObject 目前位置移動到路徑起點再依照路徑移動"));
						base.CommonSettingGUI(_item);

						EditorGUILayout.EndVertical();
					}
				}
			}

			base.NextRequeatGUI();

			base.serializedObject.ApplyModifiedProperties();
		}

		private int GetPathNameIndex(string name){

			for(int i = 0 ; i < this.pathNames.Length ; i++){

				if(name == this.pathNames[i]) return i;
			}

			return 0;
		}
	}
}