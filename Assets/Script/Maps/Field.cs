using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {

	//public float radius;
	public float angle; // change amount
	
	public Vector3 pivot;
	public Vector3 angleVector; 

	////////////////////////////
	// singletone ft
	/////////////////////////////

	private static Field instance = null;
	public static Field I{ get { return instance; } }

	// Use this for initialization
	void Start () {
	
		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} 
		else {
			instance = this;
		}

		angleVector = transform.rotation.eulerAngles;
		angleVector.y += angle;
	}

	public void RotateClockwise(){

		SetPivotVector (Character.I.transform.position);
		FieldRotate (1);

		Scope.I.SetNextScope();
		
	}
	
	public void RotateCounterClockwise(){

		SetPivotVector (Character.I.transform.position);
		FieldRotate (-1);

		Scope.I.SetNextScope();
	}
	
	void FieldRotate (int direction){


		SetPivot ( GameObject.Find ("Pivot") );

		transform.parent.transform.Rotate (angleVector * direction);

	}
	

	public void SetPivotVector(Vector3 v){

		pivot = v;
		pivot.y = 0f;

	}

	void SetPivot(GameObject Pivot){

		if (Pivot.transform.childCount > 0) {

			Pivot.transform.DetachChildren();

		} 
			Pivot.transform.position = pivot;
			transform.SetParent (Pivot.transform);

	
	}
}
