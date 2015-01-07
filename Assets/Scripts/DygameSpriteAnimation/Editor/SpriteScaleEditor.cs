using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	[CustomEditor(typeof(SpriteScale))]
	public class SpriteScaleEditor : BaseSpriteEditor {

		private SpriteScale _target;
		private Transform _transform;
		
		private Vector3 originalScale;
		private Vector3 tmpScale;

		void OnEnable(){
			
			base.SetCommonProperty();
			
			this._target = (SpriteScale)base.target;
			this._transform = this._target.transform;
		}

		void OnDisable(){
			
			this.ReuseValues();
		}

		public override void OnInspectorGUI ()
		{
			base.serializedObject.Update();
			
			base.PlayAutoGUI();
			
			if(base._hashtables.arraySize == 0){
				
				base.SettingsForEmpty("建立縮放設置");
				
			}else{
				
				base.SettingsFoldout("縮放設置");
				if(base._hashtables.isExpanded){
					
					EditorGUI.indentLevel = 1;
					
					for(int i = 0 ; i < base._hashtables.arraySize ; i++){
						
						GUI.enabled = !base.editing || (base.editing && base.editId == i);
						
						SerializedProperty _item = base._hashtables.GetArrayElementAtIndex(i);
						SerializedProperty _scale = _item.FindPropertyRelative("scale");
						
						EditorGUILayout.BeginVertical("box");
						
						EditorGUILayout.BeginHorizontal();
						
						if(base.editing && base.editId == i){
							
							EditorGUILayout.PropertyField(_scale , new GUIContent("縮放目標"));
							if(GUILayout.Button("確定" , GUILayout.ExpandWidth(false))){
								
								this.ReuseValues();
								base.editing = false;
							}
							if(GUILayout.Button("取消" , GUILayout.ExpandWidth(false))){
								
								this.ReuseValues();
								_scale.vector3Value = this.tmpScale;
								
								base.editing = false;
							}
							
						}else{
							
							EditorGUILayout.LabelField("縮放目標" , _scale.vector3Value.ToString());
							if(GUILayout.Button("設定" , GUILayout.ExpandWidth(false))){
								base.HideTool();
								this.tmpScale = _scale.vector3Value;
								this.originalScale = this._transform.localScale;
								this._transform.localScale = _scale.vector3Value == Vector3.zero ? this._transform.localScale : _scale.vector3Value;
								base.editId = i;
								base.editing = true;
							}
							
							if(base.DeleteSettingButton(i)) break;
						}
						
						EditorGUILayout.EndHorizontal();
						
						base.CommonSettingGUI(_item);
						
						EditorGUILayout.EndVertical();
					}
				}
			}
			
			GUI.enabled = true;
			base.NextRequeatGUI();
			
			base.serializedObject.ApplyModifiedProperties();
		}
		
		void OnSceneGUI(){
			
			if(!this._target.enabled) return;
			if(!base.editing) return;
			
			Undo.RecordObject(_target,"Adjust Sprite Scale");
			
			this._transform.localScale = Handles.ScaleHandle(this._target.hashtables[base.editId].scale == Vector3.zero ? this._transform.localScale : this._target.hashtables[base.editId].scale , this._transform.position , Quaternion.identity , HandleUtility.GetHandleSize(this._transform.position));
			this._target.hashtables[base.editId].scale = this._transform.localScale;
			
			Handles.Label(this._transform.position , this._target.hashtables[base.editId].scale.ToString() , base.style);
		}
		
		private void ReuseValues(){
			
			if(!base.editing) return;
			
			base.ReuseTool();
			this._transform.localScale = this.originalScale;
		}
	}
}