using UnityEngine;
using System.Collections;

public class CMYK{

	public float c;
	public float m;
	public float y;
	public float k;
	
	public static CMYK white{
		get { return new CMYK(0,0,0,0); }
	}


	public CMYK(){
		
		this.c = 0;
		this.m = 0;
		this.y = 0;
		this.k = 0;
	}
	public CMYK(float c, float m, float y, float k){

		this.c = c;
		this.m = m;
		this.y = y;
		this.k = k;
	}

}
