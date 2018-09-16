using UnityEngine;
using System.Collections;

public class test : MonoBehaviour{

	bool left = false;

	bool right = false;

	void Start(){


		Debug.Log ("Start");

	}

	void Update(){
		if( left ^ right ){
			
			Clicked ();
		}
	}

	public void changeleftvalue(){

		left = !left;

	}

	public void changerightvalue(){

		right = !right;
	}

	void Clicked(){

		Debug.Log ("Clicked~");

	}
}
