using UnityEngine;
using System.Collections;

public class HSV {

	public float h;//0~360
	public float s;
	public float v;
	
	public static HSV white{
		get { return new HSV(0,0,0); }
	}

	public HSV(){
		
		this.h = 0;
		this.s = 0;
		this.v = 0;

	}
	public HSV(float h, float s, float v){
		
		this.h = h;
		this.s = s;
		this.v = v;
	
	}
}
