using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public Color rgb;

	void OnCollisionEnter(Collision col){

		Debug.Log ("Tile cols! at "+ this.gameObject.name +" in "+this.gameObject.transform.parent.name);

		if (col.rigidbody.detectCollisions) {

		}

	}
}
