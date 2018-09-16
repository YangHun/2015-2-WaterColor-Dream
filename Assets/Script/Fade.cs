using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	GameObject fade;
	bool isStart = false;
	bool fadeout = false;

	public float speed;
	float time = 0;

	string colorname;

	void Update(){

		if (!isStart) {

			FadeFunction();
		}

	}

	public void setStart(bool _fadeout, float _speed, string _name){

		isStart = false;
		time = 0;
		fadeout = _fadeout;
		speed = _speed;
		colorname = _name;

		SetFadeImage ();

	}

	void SetFadeImage(){ //initialization

		fade = this.gameObject;

		RectTransform rect = fade.GetComponent<RectTransform> ();

		rect.anchoredPosition = Vector2.zero;
		rect.sizeDelta = new Vector2 (Screen.width, Screen.height);

		if(colorname == "black") fade.GetComponent<Image> ().material.SetColor ("_Color", Color.black); //(0,0,0,1);
		else if (colorname == "white") fade.GetComponent<Image> ().material.SetColor ("_Color", Color.white); //(1,1 1,1);
		else fade.GetComponent<Image> ().material.SetColor ("_Color", Color.clear);

		if (fadeout) {
			Color col = fade.GetComponent<Image>().material.color;
			col.a = 0.0f;
			fade.GetComponent<Image>().material.color = col;
		} else {
			Color col= fade.GetComponent<Image>().material.color;
			col.a = 1.0f;
			fade.GetComponent<Image>().material.color = col;
		}
	}

	void FadeFunction(){

		if( fade == null) fade = this.gameObject;


		if (fadeout) { // fade out start
			time += Time.deltaTime;
			if( time >= speed){
				isStart = true;
				time = 0f;
				//Destroy (this.gameObject);
			}
			else{

				if(fade.GetComponent<Image>().material.color != null){
				Color col = fade.GetComponent<Image>().material.color;
				col.a += 1.0f/(speed / Time.deltaTime);
				fade.GetComponent<Image>().material.SetColor("_Color", col);
				}
			}
		} 
		else { //fade in start
			time += Time.deltaTime;
			
			if( time >= speed){ //end
				
				//		fade.GetComponent<Image>().material.SetColor("_Color", Color.clear);
				isStart = true;
				Destroy (this.gameObject);
				//this.enabled = false;
				
			}
			else{

				if( fade.GetComponent<Image>().material.color != null){
				Color col = fade.GetComponent<Image>().material.color;
				col.a -= 1.0f/(speed / Time.deltaTime);
				fade.GetComponent<Image>().material.SetColor("_Color", col);
				}
			}
		}
	}
}
