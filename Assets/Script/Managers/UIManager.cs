using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

	public GameObject ScorePopup;
	public GameObject FadeImage;

	public GameObject[] Sliders;
	public GameObject Eyedropper;

	public GameObject LeftArrow;
	public GameObject RightArrow;
	public GameObject CMYButton;
	public GameObject EDButton;

	bool CMYClicked;
	bool EDClicked;
	bool LeftClicked = false;
	bool RightClicked = false;

	GameObject Quit;
	GameObject Restart;

	public GameObject CurrentPopup = null; // handle score popup & fade




	/////////////////////////////
	// singletone ft
	/////////////////////////////

	private static UIManager instance = null;
	public static UIManager I { get { return instance; } }

	// Use this for initialization
	public void Start () {

		//initialization


		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if ( (Character.I != null) && (LeftClicked ^ RightClicked)) {

			if( RightClicked ){

				Field.I.RotateCounterClockwise();

			}
			else{

				Field.I.RotateClockwise();

			}
		}

	}


	/////////////////////////////
	// In-Game functions
	/////////////////////////////


	public void Init(){
	
	
			GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Button");

			foreach( GameObject b in Buttons){
		
				b.GetComponent<Button>().interactable = true;

			}

			//설정대로초기화 --> 언제..?
			CMYClicked = false;
			EDClicked = false;

			Debug.Log ("UI Manager :: UI Init end");
	}

	public void ClickStageStartButton(){

		StartCoroutine (ListToGameRoutine ());

	}

	IEnumerator ListToGameRoutine(){

		StartCoroutine (WorldManager.I.Fade (true, 1.0f, "black"));
		
		yield return new WaitForSeconds (1.1f);

		GameManager.I.Flow = GameManager.GameFlow.GameInit;

	}

	public void ClickStageLeftButton(){

		StartCoroutine (StageArrowButtonRoutine (true));

	}

	public void ClickStageRightButton(){

		StartCoroutine (StageArrowButtonRoutine (false));
	}

	IEnumerator StageArrowButtonRoutine( bool _left){

		GameObject.Find ("Selected").GetComponent<Button> ().interactable = false;
		GameObject.Find ("Helper").GetComponent<Button> ().interactable = false;

		WorldManager.I.rotdirection = _left;
		WorldManager.I.cnt =0;
		InvokeRepeating ("CallRotate", (Time.deltaTime/2f), (Time.deltaTime/2f));

		yield return new WaitForSeconds (Time.deltaTime * 60f);

		CancelInvoke ();

		GameObject.Find ("Selected").GetComponent<Button> ().interactable = true;
		GameObject.Find ("Helper").GetComponent<Button> ().interactable = true;


		if (_left) {
			WorldManager.I.stageNum --;
			
			if( WorldManager.I.stageNum < 0 )
				WorldManager.I.stageNum = 5;
			
		}
		else {
			WorldManager.I.stageNum ++;
		}
	
	}

	void CallRotate(){
	
		if (WorldManager.I.cnt < 240) {

			WorldManager.I.RotateStageFountain ();
			WorldManager.I.cnt ++;
		}

	}
	
	//---------------------
	// In-Game UI
	//---------------------



	public void ClickLeftArrowButton(){

		LeftClicked = !LeftClicked;
	}

	public void ClickRightArrowButton(){
		
		RightClicked = !RightClicked;
	}

	public void ClickRestartButton(){

		StartCoroutine (RestartRoutine ());

	}

	IEnumerator RestartRoutine(){

		StartCoroutine(WorldManager.I.Fade (true, 1f, "black"));
		yield return new WaitForSeconds (1f + Time.deltaTime*2f );

		GameManager.I.Flow = GameManager.GameFlow.GameInit;

	}

	public void ClickQuitButton(){

		StartCoroutine (QuitRoutine());

	}

	IEnumerator QuitRoutine(){

		StartCoroutine(WorldManager.I.Fade (true, 1f, "white"));
		yield return new WaitForSeconds (1f + Time.deltaTime * 2f );
		
		GameManager.I.Flow = GameManager.GameFlow.StageSelect;
	}

	public void ClickEDHelperButton(){

		if (EDClicked) {
			
			EDButton.GetComponent<Image>().color = Color.white;
			
		}
		
		else{
			
			EDButton.GetComponent<Image>().color = Color.gray;
		}

		EDClicked = !EDClicked;

		Eyedropper.SetActive (!Eyedropper.activeSelf);

	}

	public void ClickCMYHelperButton(){

		if (CMYClicked) {

			CMYButton.GetComponent<Image>().color = Color.white;

		}

		else{

			CMYButton.GetComponent<Image>().color = Color.gray;
		}

		CMYClicked = !CMYClicked;

		foreach( GameObject s in Sliders){
			
			string name = s.name;
			if( name != "Brightness"){

				s.SetActive(!s.activeSelf);
			}
		}
	}

	void SlidersInit(Color dest){

		foreach( GameObject s in Sliders){

			s.GetComponent<Slider>().value = 1f;

			string name = s.name;
			CMYK _dest = ColorManager.I.RGBtoCMYK (dest); 

			switch( name ){
			case "Brightness" : 
				break;
			case "Cyan" : 
				break;
			case "Magenta" : 
				break;
			case "Yellow" : 
				break;
			default : Debug.Log ("Who are you, slider? " + s.name);
				break;
			}

			s.SetActive(true);
		}

	}

	public void ChangeSliders(){

		CMYK _cmyk = Character.I.cmyk;

		foreach (GameObject s in Sliders) {

			string name = s.name;

			switch(name){
			case "Brightness":
				HSV _hsv = ColorManager.I.RGBtoHSV (Character.I.color);
				s.GetComponent<Slider> ().value = _hsv.v;
				Debug.Log ("hsv : "+_hsv.h+", "+_hsv.s+", "+_hsv.v);
				_hsv.v = 1f;
				Color c = ColorManager.I.HSVtoRGB (_hsv);
				Debug.Log ("RGB : "+c);
				s.transform.FindChild ("Color").GetComponent<Image>().color = c;
				break;
			case "Cyan" :
				s.GetComponent<Slider> ().value = 1f - _cmyk.c;
				break;
			case "Magenta":
				s.GetComponent<Slider> ().value = 1f - _cmyk.m;
				break;
			case "Yellow" :
				s.GetComponent<Slider> ().value = 1f - _cmyk.y;
				break;
			}

		}
	}

	public void ChangeEyedropperText( CMYK _t ){
		if(Eyedropper != null && Eyedropper.activeSelf){

			if( _t == null ){
		//		Debug.Log ("_Target == null");
				
				Eyedropper.transform.FindChild ("Cyan").GetComponent<Text> ().text = "+?";
				Eyedropper.transform.FindChild ("Magenta").GetComponent<Text> ().text = "+?";
				Eyedropper.transform.FindChild ("Yellow").GetComponent<Text> ().text = "+?";

			} 
			else { 
		//		Debug.Log ("_Target != null");
				
				CMYK _crt = Character.I.cmyk;
				
				float _c = (_crt.c - _t.c) * -255f;
				float _m = (_crt.m - _t.m) * -255f;
				float _y = (_crt.y - _t.y) * -255f;

				string buho;

				if(_c >=0 ) buho = "+";
				else buho = "";

				Eyedropper.transform.FindChild ("Cyan").GetComponent<Text> ().text = buho + _c.ToString ("0");

				if(_m >=0 ) buho = "+";
				else buho = "";

				Eyedropper.transform.FindChild ("Magenta").GetComponent<Text> ().text = buho + _m.ToString ("0");

				if(_y >=0 ) buho = "+";
				else buho = "";

				Eyedropper.transform.FindChild ("Yellow").GetComponent<Text> ().text = buho + _y.ToString ("0");


			}
		}
	}


	public void StopUI(){

		Eyedropper = null;
		
		LeftArrow.SetActive(false);
		RightArrow.SetActive(false);
		CMYButton.SetActive(false);
		EDButton.SetActive (false);


	}




	//---------------------
	// Score Popup & Button
	//---------------------



	public void CallScorePopup(float percent){

		if (CurrentPopup == null) {

			CurrentPopup = (GameObject) Instantiate (ScorePopup, Vector3.zero, Quaternion.identity);

			Transform t = CurrentPopup.transform;

			t.SetParent ( GameObject.Find ("Canvas").transform);
			CurrentPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			t.localScale = new Vector3 (1.0f, 1.0f, 1.0f);


			if( percent % 10 == 0 ) t.FindChild("percent").GetComponent<Text>().text = percent.ToString() + "%";
			else t.FindChild("percent").GetComponent<Text>().text = percent.ToString("0.0") + "%";

			Sprite title = null;

			if( percent < 70) {

				title = Resources.Load <Sprite> ("UI/Result/failed");

			}
			else if ( percent < 100) {
				title = Resources.Load <Sprite> ("UI/Result/clear");

			}
			else if( percent == 100 ) {

				title = Resources.Load <Sprite> ("UI/Result/complete");
			}

			t.FindChild("Title").GetComponent<Image>().sprite = title;
			t.FindChild ("Restart").GetComponent<Button>().onClick.AddListener( ()=> ClickRestartButton());
			t.FindChild ("List").GetComponent<Button>().onClick.AddListener( ()=> ClickQuitButton());
		
		}
	}
	

}
