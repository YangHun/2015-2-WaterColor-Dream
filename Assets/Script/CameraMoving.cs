using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour {

	Vector3 pos;

	float height;

	// Use this for initialization
	void Start () {
		height = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	
		// pos = ????;
	
		pos.y = height;

		transform.position = pos;


	}
}
