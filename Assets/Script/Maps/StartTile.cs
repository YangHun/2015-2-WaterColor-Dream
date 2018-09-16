using UnityEngine;
using System.Collections;

public class StartTile : MonoBehaviour {

	public Sprite wall;

	bool isStart;

	// Use this for initialization
	void Start () {
	
		isStart = false;
		wall =  Resources.Load <Sprite> ("wall");


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeWall(){

		if (!isStart) {

			Transform w = this.transform.FindChild ("wall");
			Transform child;

			for (int i = 0; i < 6; i ++) {

				child = w.FindChild ("wall" + i);

				child.GetComponent<SpriteRenderer> ().sprite = wall;
				child.tag = "Wall";
				child.GetComponent<Collider> ().isTrigger = false;
			}

			w.FindChild("ceiling").gameObject.SetActive (true);

			isStart = true;

		}
	}
}
