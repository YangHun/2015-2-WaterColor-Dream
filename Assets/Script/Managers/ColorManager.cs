using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {

	////////////////////////////
	// singletone ft
	/////////////////////////////


	private static ColorManager instance = null;
	public static ColorManager I { get { return instance; } }
	
	// Use this for initialization
	void Start () {
		
		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} 
		else {
			instance = this;
			DontDestroyOnLoad (this.transform.root.gameObject); //Assure Uniqueness regardless of Scene #
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public Color SubtractiveMix (Color a){

		Color result = Color.white;
	
		CMYK a_ = RGBtoCMYK (a);
		CMYK b_ = Character.I.cmyk;

		CMYK r_ = a_;

		r_.c = Mathf.Max (a_.c, b_.c);
		r_.m = Mathf.Max (a_.m, b_.m);
		r_.y = Mathf.Max (a_.y, b_.y);
		r_.k = Mathf.Max (a_.k, b_.k);

		if( r_.k != 1.0f) {
			result = CMYKtoRGB (r_);
		}
		else {
			result = Color.black;
		}

		result.a = 1.0f;
		Character.I.cmyk = r_;

		Debug.Log (" Subtractive Mixture ; c, m, y = " + r_.c + ", "+ r_.m + ", "+ r_.y);
		return result;
	}


	public CMYK RGBtoCMYK (Color rgb){
		
		CMYK cmyk = CMYK.white;

		if (rgb != null) {
			cmyk.k = 1 - Mathf.Max (rgb.r, rgb.g, rgb.b);

			if (cmyk.k != 1) {
				cmyk.c = (1 - rgb.r - cmyk.k) / (1 - cmyk.k);
				cmyk.m = (1 - rgb.g - cmyk.k) / (1 - cmyk.k);
				cmyk.y = (1 - rgb.b - cmyk.k) / (1 - cmyk.k);
			} else {
				cmyk.c = cmyk.m = cmyk.y = 1f;
				cmyk.k = 0f;

			}
		}

		return cmyk;
		
	}

	public HSV RGBtoHSV (Color rgb){

		HSV hsv = HSV.white;

		float c_max = Mathf.Max (rgb.r, rgb.g, rgb.b);
		float delta = c_max - Mathf.Min (rgb.r, rgb.g, rgb.b);


		//Hue
		if (delta == 0)
			hsv.h = 0;
		else if (c_max == rgb.r)
			hsv.h = 60 * (((rgb.g - rgb.b) / delta) % 6);
		else if (c_max == rgb.g)
			hsv.h = 60 * (((rgb.b - rgb.r) / delta) + 2);
		else if (c_max == rgb.b)
			hsv.h = 60 * (((rgb.r - rgb.g) / delta) + 4);

		//Saturation
		if (c_max == 0)
			hsv.s = 0;
		else
			hsv.s = (delta / c_max);

		//value
		hsv.v = c_max;

		return hsv;
	}

	public Color CMYKtoRGB(CMYK cmyk){
		
		Color rgb = Color.white;
		
		rgb.r = (1 - cmyk.c) * (1 - cmyk.k);
		rgb.g = (1 - cmyk.m) * (1 - cmyk.k);
		rgb.b = (1 - cmyk.y) * (1 - cmyk.k);
		rgb.a = 1.0f;
		
		return rgb;
	}

	public Color HSVtoRGB(HSV hsv){
		
		Color rgb = Color.white;

		float c, x, m;

		c = hsv.v * hsv.s;
		x = c * (1 - Mathf.Abs ( (hsv.h/60) % 2 - 1 ) );
		m = hsv.v - c;

		float h = hsv.h;
		if (0 <= h && h < 60) rgb = new Color (c, x, 0);
		else if ( 60 <= h && h < 120) rgb = new Color (x, c, 0);
		else if ( 120 <= h && h < 180) rgb = new Color (0, c, x);
		else if ( 180 <= h && h < 240) rgb = new Color (0, x, c);
		else if ( 240 <= h && h < 300) rgb = new Color (x, 0, c);
		else if ( 300 <= h && h < 360) rgb = new Color (c, 0, x);
		
		rgb.r += m;
		rgb.g += m;
		rgb.b += m;
		rgb.a = 1.0f;
		
		return rgb;
	}

	public float CalculateScore(Color crt, Color dest){
	
		float percent = 100f;

		CMYK _dest = ColorManager.I.RGBtoCMYK (dest);
		CMYK _crt = ColorManager.I.RGBtoCMYK (crt); //character's color
		
		Vector4 __dest = new Vector4 (_dest.c, _dest.m, _dest.y, _dest.k);
		Vector4 __crt = new Vector4 (_crt.c, _crt.m, _crt.y, _crt.k);

		__crt -= __dest;

		float dc =__crt.magnitude;
		float dd = __dest.magnitude;


		if (dd != 0f) {
			if (dc < dd) {
				percent = 100f * (1f - (dc / dd));
			} 
			else
				percent = 0f;
		} 
		else {
			percent = 100f * ( 1f - dc / Mathf.Pow (3f,0.5f) ) ;
		}

		return percent;
	}
}
