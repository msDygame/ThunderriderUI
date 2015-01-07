using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	void Awake(){

		if(renderer is SpriteRenderer) this.spriteRenderer = (SpriteRenderer)renderer;

		Debug.Log(this.spriteRenderer);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
