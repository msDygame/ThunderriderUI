using UnityEngine;
using System.Collections;

public class nGUIBehaviourScript : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    //
    void onClick()
    {
        Destroy(GameObject.Find("OrcSprite"));
    }
}
