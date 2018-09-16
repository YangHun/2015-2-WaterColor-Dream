using UnityEngine;
using System.Collections;

public class DestTile : MonoBehaviour {

	public Color DestColor;

	// Use this for initialization
	void Start () {
	
		GetComponent<Renderer> ().material.SetColor ("_Color", DestColor);

	}
	
	// Update is called once per frame
	void Update () {
		
	}




}
