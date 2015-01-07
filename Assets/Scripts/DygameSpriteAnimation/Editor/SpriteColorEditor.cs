using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	[CustomEditor(typeof(SpriteColor))]
	public class SpriteColorEditor : BaseSpriteEditor {

//		private SpriteColor _target;

		void OnEnable(){
			
			base.SetCommonProperty();
			
//			this._target = (SpriteColor)base.target;
		}
		
		public override void OnInspectorGUI ()
		{
			base.serializedObject.Update();
			
			base.PlayAutoGUI();
			
			if(base._hashtables.arraySize == 0){
				
				base.SettingsForEmpty("建立顏色設置");
				
			}else{
				
				base.SettingsFoldout("縮放設置");
				if(base._hashtables.isExpanded){
					
					EditorGUI.indentLevel = 1;
					
					for(int i = 0 ; i < base._hashtables.arraySize ; i++){

						SerializedProperty _item = base._hashtables.GetArrayElementAtIndex(i);
						SerializedProperty _color = _item.FindPropertyRelative("color");
						
						EditorGUILayout.BeginVertical("box");
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PropertyField(_color , new GUIContent("變色目標"));
						if(base.DeleteSettingButton(i)) break;
						EditorGUILayout.EndHorizontal();
						
						base.CommonSettingGUI(_item);
						
						EditorGUILayout.EndVertical();
					}
				}
			}

			base.NextRequeatGUI();
			
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
