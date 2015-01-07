using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(iTweenPath))]
public class iTweenPathCustomEditor : Editor {

	private static Tool tmpTool;

	private iTweenPath _target;
	private GUIStyle style = new GUIStyle();

	private SerializedProperty _pathName;
	private SerializedProperty _nodes;
	private SerializedProperty _pathColor;

	private Vector3 OldPosition;	// 紀錄物件位置


	void OnEnable(){

		this._target = (iTweenPath)base.target;

		this.style.fontStyle = FontStyle.Bold;
		this.style.normal.textColor = Color.white;

		this._pathName = base.serializedObject.FindProperty("pathName");
		this._nodes = base.serializedObject.FindProperty("nodes");
		this._pathColor = base.serializedObject.FindProperty("pathColor");

		if(!this._target.initialized){
			
			this._target.initialized = true;
			this._target.pathName = string.Format("New Path {0}" , GameObject.FindObjectsOfType<iTweenPath>().Length);
			this._target.initialName = this._target.pathName;
			this._target.pathColor = new Color(Random.value , Random.value , Random.value);
			this._nodes.arraySize = 2;
		}

		if(Tools.current != Tool.None){
			tmpTool = Tools.current;
			Tools.current = Tool.None;
		}

		OldPosition = this._target.transform.position ;
	}

	void OnDisable(){

		if(tmpTool == Tool.None) return;
		Tools.current = tmpTool;
	}

	public override void OnInspectorGUI ()
	{
		base.serializedObject.Update();

		EditorGUILayout.PropertyField(this._pathName , new GUIContent("路徑名稱","每條移動路徑名稱不可重複"));
		EditorGUILayout.PropertyField(this._pathColor , new GUIContent("顏色"));

		if(string.IsNullOrEmpty(this._pathName.stringValue)) this._pathName.stringValue = this._target.initialName;

		EditorGUILayout.BeginHorizontal();
		this._nodes.isExpanded = EditorGUILayout.Foldout(this._nodes.isExpanded , new GUIContent(string.Format("節點 ({0})" , this._nodes.arraySize)));
		if(GUILayout.Button("+" , GUILayout.ExpandWidth(false))){
			this._nodes.arraySize++;
		}
		EditorGUILayout.EndHorizontal();

		if(this._nodes.isExpanded){

			EditorGUI.indentLevel = 1;
			for(int i = 0 ; i < this._nodes.arraySize ; i++){
				EditorGUILayout.BeginHorizontal("box");
				//EditorGUILayout.PropertyField(this._nodes.GetArrayElementAtIndex(i) , new GUIContent((i+1).ToString()));
				EditorGUILayout.PropertyField(this._nodes.GetArrayElementAtIndex(i) , new GUIContent(i.ToString())); // 修改：起始點為編號0
				if(GUILayout.Button("-" , GUILayout.ExpandWidth(false)) && this._nodes.arraySize > 2){
					this._nodes.DeleteArrayElementAtIndex(i);
					break;
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		base.serializedObject.ApplyModifiedProperties();
	}

	void OnSceneGUI(){

		if(!this._target.enabled) return;
		if(this._target.nodes.Count < 2) return;

		// 物件移動時，調整節點位置
		Vector3 Delat = this._target.transform.position - OldPosition;
		if(Delat.x!=0 || Delat.z!=0)
		{
			OldPosition = this._target.transform.position;
			for (int i = 0; i < _target.nodes.Count; i++)
				_target.nodes[i] = _target.nodes[i] + Delat;
		}

		Undo.RecordObject(_target,"Adjust iTween Path");

		string _label = string.Empty;

		for (int i = 0; i < _target.nodes.Count; i++) {

			_target.nodes[i] = Handles.PositionHandle(_target.nodes[i], Quaternion.identity);

			if(i == 0) _label = string.Format("{0} 起點" , this._target.pathName);
			else if(i == this._target.nodes.Count - 1) _label = string.Format("{0} 終點" , this._target.pathName);
			else _label = i.ToString();

			Handles.Label(this._target.nodes[i] , _label, style);
		}
	}
}
