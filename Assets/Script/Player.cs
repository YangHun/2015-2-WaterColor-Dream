using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	Vector3 pos;

	////////////////////////////
	// singletone ft
	/////////////////////////////
	
	private static Player instance = null;
	public static Player I { get { return instance; } }

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
	
	// Update is called once per frame
	void Update () {
	
		updatePosition ();

	}


	public void Init(){

		transform.position = Vector3.zero;
		pos = transform.position;


		transform.FindChild ("Character").GetComponent<Renderer> ().material.SetColor ("_Color", Color.white);
		transform.FindChild ("Character").GetComponent<Rigidbody> ().velocity = Vector3.zero;
		transform.FindChild ("Character").GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		transform.FindChild ("Character").GetComponent<Rigidbody> ().useGravity = true;

		if (transform.FindChild ("Character").GetComponent<Character> () == null) {

			//if this script is missed after restart game or start new game
			//must be initialized
			Debug.Log ("Script ??????");

		}

		Debug.Log ("Player :: Init End");

	}

	void updatePosition(){

		if (Character.I != null) {

			Vector3 v = Character.I.transform.position;

			pos.x = v.x;
			pos.z = v.z;

			if( pos.y - v.y > 5.25f) pos.y = v.y + 5.25f;
			else if ( pos.y - v.y < 5.25f) pos.y = v.y + 5.25f;

			transform.position = pos;

			Character.I.transform.position = v;
		}
	}

}
