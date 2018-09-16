using UnityEngine;
using System.Collections;


//<summary>
//각 Scene 안 World 관련 함수를 담당한다
//</summary>
public class WorldManager : MonoBehaviour {

	Color WaterColor = Color.clear;

	public GameObject[] maps;
	public GameObject splash;
	public GameObject ripple;

	public GameObject fade;

	public int cnt=0;

	GameObject field;

	int _stagenum = 0;

	public int stageNum {
		get{ return _stagenum; }
		set { 
			_stagenum = value; 
		
			if( _stagenum >= 6) _stagenum = 0;
			else if ( _stagenum <0) _stagenum = 5;		
		}
	}

	public bool rotdirection;

	
	/////////////////////////////
	// singletone ft
	/////////////////////////////

	private static WorldManager instance = null;
	public static WorldManager I {
		get { return instance;}
	}

	// Use this for initialization
	public void Start () {
	
		if (instance != null) {
			Destroy (this.gameObject);
			return;

		} else {
			instance = this;
			DontDestroyOnLoad(this.transform.root.gameObject);
		}

	}

	public void SetWaterColor(){

		if (Application.loadedLevelName == "Title") {
		
			GameObject water = GameObject.Find ("WaterProDaytime");

			HSV hsv = ColorManager.I.RGBtoHSV( water.GetComponent<Renderer>().material.GetColor ("_RefrColor"));
			Random.seed = System.DateTime.Now.Millisecond;
			hsv.h = Random.Range(0,180);

			WaterColor = ColorManager.I.HSVtoRGB(hsv);
			water.GetComponent<Renderer>().material.SetColor("_RefrColor", WaterColor);

		}
		else if (Application.loadedLevelName == "List" && WaterColor != Color.clear){

			GameObject water = GameObject.Find ("WaterProDaytime");
			
			HSV hsv = ColorManager.I.RGBtoHSV( WaterColor );
			water.GetComponent<Renderer>().material.SetColor("_RefrColor", WaterColor);
		}

	}

	public void Init(){


		field = GameObject.Find ("Field");
		if (field != null) {
			Destroy (field);
			return;
		}
		else {

			field = (GameObject)Instantiate ( maps[stageNum] , Vector3.zero, Quaternion.identity);
			field.transform.SetParent( GameObject.Find ("Pivot").transform);
			//field.GetComponent<Field>().Start();
			Debug.Log ("Field instantiate success?");

		}

	}

	/////////////////////////////
	// Title Scene functions
	/////////////////////////////
	
	public void ClickStartButton(){

		StartCoroutine (TitletoListRoutine ());
		
	}

	IEnumerator TitletoListRoutine(){

		StartCoroutine(WorldManager.I.Fade (true, 1f, "white"));
		yield return new WaitForSeconds (1f + Time.deltaTime*2f );
	
		GameManager.I.Flow = GameManager.GameFlow.StageSelect;

	}
	
	/////////////////////////////
	// List Scene functions
	/////////////////////////////
	
	public void ClickStageButton(GameObject b){
		
		StartCoroutine (ListToGameRoutine (b));
		
	}

	IEnumerator ListToGameRoutine(GameObject b){

		StartCoroutine(WorldManager.I.Fade (true, 1f, "black"));
		yield return new WaitForSeconds (1f + Time.deltaTime*2f );
	
		WorldManager.I.stageNum = int.Parse ( b.name);
		GameManager.I.Flow = GameManager.GameFlow.GameInit;

	}

	public void RotateStageFountain(){

		Vector3 angle = Vector3.zero;
		angle.y = 0.5f;
		
		if (rotdirection)
			angle *= -1f;
		
		GameObject obg = GameObject.Find ("Stage");

		obg.transform.Rotate (angle);

		cnt ++;
	
	}

	/////////////////////////////
	// Game Scene functions
	/////////////////////////////

	public void PlayerInit(){

		Player.I.Init ();

	}

	public void Over(){

		StartCoroutine (GameOverRoutine ());

	}

	IEnumerator GameOverRoutine(){

		Destroy (Scope.I.gameObject);
		Destroy (Character.I);

		StartCoroutine(WorldManager.I.Fade (true, 1f, "black"));

		yield return new WaitForSeconds (1f + Time.deltaTime );

		GameManager.I.Flow = GameManager.GameFlow.GameOver;


	}

	public void End(){

		Destroy (Scope.I.gameObject);

		Color dest = GameObject.Find ("Dest Tile").GetComponent<DestTile> ().DestColor;
		float score = ColorManager.I.CalculateScore (Character.I.color, dest);

		Destroy (Character.I);

		GameManager.I.pct = score;
		GameManager.I.Flow = GameManager.GameFlow.GameEnd;

	}



	public IEnumerator CallParticle(Color col, Vector3 startpos){

		Vector3 pos = startpos;
		//pos.y -= 3.0f;

		GameObject obj = null;
		obj = (GameObject)Instantiate (splash, startpos, Quaternion.identity);

		obj.GetComponent<ParticleSystemRenderer> ().material.SetColor ("_EmisColor", col);

		yield return new WaitForSeconds(1.0f);

		Destroy (obj);
		Debug.Log (obj);

	}

	public IEnumerator Fade(bool fadeout, float speed, string color){
		
			
			GameObject obj = (GameObject)Instantiate (fade, Vector3.zero, Quaternion.identity);
			obj.transform.SetParent (GameObject.Find ("Canvas").transform);
			obj.GetComponent<Fade> ().setStart (fadeout, speed, color);
			
		yield return new WaitForSeconds( speed + 1f);

		Destroy (obj);

	}
}
