using UnityEngine;
using System.Collections;
using System;

public class Scope : MonoBehaviour {

	//public float radius;
//	public float angle; // change amount

	Vector3 pivot;
	Vector3 position;

	public Vector3 speed = new Vector3(0.04f, 0.04f, 0f);

	public float radius;

	public bool isreset;

	////////////////////////////
	// singletone ft
	/////////////////////////////
	
	private static Scope instance = null;
	public static Scope I { get { return instance; } }
	
	// Use this for initialization
	void Start () {
		
		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} 
		else {
			instance = this;
		}	
	}

	void Update(){

		ScaleNextScope ();
		Eyedropping ();

	}

	void ScaleNextScope(){

		Transform _next = transform.FindChild ("Next").transform;

		if (_next.localScale.x > 0) {
		
			_next.localScale -= speed;

		}

	}

	public void SetNextScope(){

		if (isreset) {
			
			transform.FindChild ("Next").GetComponent<Renderer>().enabled = false;
			transform.FindChild ("Expected").GetComponent<Renderer>().enabled = false;

		} 
		else {
		
			GameObject _next = transform.FindChild("Next").gameObject;
			GameObject _exp = transform.FindChild ("Expected").gameObject;

			_next.GetComponent<Renderer>().enabled = true;
			_exp.GetComponent<Renderer>().enabled = true;

			position = Player.I.transform.position;
			position.y = 0f;
			
			position.Normalize ();
			
			position *= radius;

			Vector3 dir = pivot + position;

			_next.transform.position = dir;

			dir.z -= 0.1f;

			_exp.transform.position = dir;
		}
	}

	public void SetPivotVector(Vector3 _pivot){
		
		pivot = _pivot;
		pivot.y += 0.1f;
		transform.position = pivot;

		SetScope (this.gameObject);

	}

	void SetScope(GameObject Scope){

		Scope.transform.position = pivot;
		transform.FindChild ("Prev").transform.position = pivot;

		transform.FindChild("Next").transform.localScale = new Vector3 (10f,10f,1f);

	}
	void Eyedropping(){

		Ray ray = new Ray (transform.FindChild ("Next").transform.position, Vector3.down);
		RaycastHit hit;
		LayerMask mask = 9;
		if (Physics.Raycast (ray, out hit, mask)) {
		
			try{
				CMYK t = ColorManager.I.RGBtoCMYK (hit.transform.gameObject.GetComponent<Renderer>().material.color);
				UIManager.I.ChangeEyedropperText(t);
			}
			catch( Exception ex ) {
			Debug.LogError (ex);
				
			}

				//UIManager.I.ChangeEyedropperText( t.c, t.m, t.y);
			

		} //else {

		//	UIManager.I.ChangeEyedropperText( null );

	//	}
	}
}
