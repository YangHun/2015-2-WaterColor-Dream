using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

	public Color[] Colors; //(r,g,b) , a = 1;

	// Use this for initialization
	void Start () {

		Transform obj;

		for( int i = 0 ; i < 7 ; i ++){

			obj = this.gameObject.transform.FindChild ("Tile" + i);
			obj.GetComponent<Tile>().rgb = Colors[i] ;
			obj.GetComponent<Renderer> ().material.SetColor ("_Color", Colors[i]);


		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
