using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dygame.SpriteAnimation{

	[CustomEditor(typeof(SpriteRotate))]
	public class SpriteRotateEditor : BaseSpriteEditor {

		private SpriteRotate _target;
		private Transform _transform;

		private Quaternion originalRotation;
		private Vector3 tmpRotation;

		void OnEnable(){

			base.SetCommonProperty();

			this._target = (SpriteRotate)base.target;
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
				
				base.SettingsForEmpty("建立旋轉設置");
				
			}else{
				
				base.SettingsFoldout("旋轉設置");
				if(base._hashtables.isExpanded){
					
					EditorGUI.indentLevel = 1;
					
					for(int i = 0 ; i < base._hashtables.arraySize ; i++){

						GUI.enabled = !base.editing || (base.editing && base.editId == i);

						SerializedProperty _item = base._hashtables.GetArrayElementAtIndex(i);
						SerializedProperty _rotation = _item.FindPropertyRelative("rotation");
						
						EditorGUILayout.BeginVertical("box");
						
						EditorGUILayout.BeginHorizontal();

						if(base.editing && base.editId == i){

							EditorGUILayout.PropertyField(_rotation , new GUIContent("旋轉目標"));
							if(GUILayout.Button("確定" , GUILayout.ExpandWidth(false))){
								
								this.ReuseValues();
								base.editing = false;
							}
							if(GUILayout.Button("取消" , GUILayout.ExpandWidth(false))){
								
								this.ReuseValues();
								_rotation.vector3Value = this.tmpRotation;

								base.editing = false;
							}
							
						}else{

							EditorGUILayout.LabelField("旋轉目標" , _rotation.vector3Value.ToString());
							if(GUILayout.Button("設定" , GUILayout.ExpandWidth(false))){
								base.HideTool();
								this.tmpRotation = _rotation.vector3Value;
								this.originalRotation = this._transform.localRotation;
								this._target.transform.rotation = Quaternion.Euler(_rotation.vector3Value);
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

			Undo.RecordObject(_target,"Adjust Sprite Rotate");

			this._transform.localRotation = Handles.RotationHandle(Quaternion.Euler(this._target.hashtables[base.editId].rotation) , this._transform.position);
			this._target.hashtables[base.editId].rotation = this._transform.localEulerAngles;

			Handles.Label(this._transform.position , this._target.hashtables[base.editId].rotation.ToString() , base.style);
		}

		private void ReuseValues(){

			if(!base.editing) return;

			base.ReuseTool();
			this._transform.localRotation = this.originalRotation;
		}
	}
}