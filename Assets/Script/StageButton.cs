using UnityEngine;
using System.Collections;

public class StageButton : MonoBehaviour {

	public Vector3 pos;
	public Vector3 rot;
	public float maxheight;
	public float minheight;

	float buho = 1f;

	void Update(){

		RotateAnimation ();


	}

	void OnTouchDown(){
		
		WorldManager.I.ClickStageButton(this.gameObject);
		
	}

	void RotateAnimation(){

		transform.Rotate (rot);


		Vector3 _pos = transform.position;

		if (_pos.y > maxheight) {

			buho = -1f;

		} else if (_pos.y < minheight) {

			buho = 1f;
		} 

		transform.position += (buho * pos);
	}

}
