using UnityEngine;
using System.Collections;

public class TestButton : MonoBehaviour {

	void OnTouchDown(){

		Debug.Log ("OnTouchDown! at "+ this.gameObject.name);

	}

	void OnTouchUp(){
		Debug.Log ("OnTouchUp! at "+ this.gameObject.name);
	}

	void OnTouchStay(){
		Debug.Log ("OnTouchStay! at "+ this.gameObject.name);
	}

	void OnTouchExit(){
		Debug.Log ("OnTouchExit! at "+ this.gameObject.name);
	}
}
