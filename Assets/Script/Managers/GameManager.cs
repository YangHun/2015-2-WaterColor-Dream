using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	public float pct;

	public enum GameFlow { 
		AppStart, 
		Title, 
		TutorialPlay,
		StageSelect, 
		GameInit, 
		GamePlay, 
		GameEnd, 
		GameOver
	}
	// Appstart : App이 시작되었을 때 단 한번 될 수 있는 state


	private GameFlow _Flow = GameFlow.AppStart;

	public GameFlow Flow {

		get{
			return _Flow;
		}

		set{

			switch( value ){

			case GameFlow.AppStart:

				FlowAppStart ();
				break;

			case GameFlow.Title:

				FlowTitle();
				break;
			
			case GameFlow.StageSelect:
				StartCoroutine(FlowStageSelect());
				break;
			
			case GameFlow.GameInit:
				FlowGameInit();
				break;

			case GameFlow.GamePlay:
				FlowGamePlay ();
				break;

			case GameFlow.GameOver :
				FlowGameOver ();
				break;

			case GameFlow.GameEnd :
				FlowGameEnd ();
				break;

			case GameFlow.TutorialPlay:

				break;
			}

			_Flow = value;
		}
	}


	//Singleton

	private static GameManager instance;
	public static GameManager I {
		get { return instance; }
	}

	void Start(){

		Flow = GameFlow.AppStart;

		if (instance != null) {

			Destroy (this.gameObject);
			return;

		} else {

			instance = this;
			DontDestroyOnLoad(this.gameObject);

		}


	}

	void Update(){

		#if UNITY_ANDROID
		// Back 버튼
		if (Input.GetKey(KeyCode.Escape)) {
			
			Debug.LogError(" ::: Quit Game ::: ");

			if( Flow == GameFlow.StageSelect ){
		
				Flow = GameFlow.Title;

			}
			else if ( Flow == GameFlow.Title){

				Application.Quit();
				return;
			}
		}
		#endif
		
		#if UNITY_EDITOR
		
		#endif
		
	}

	void FlowAppStart(){

		// Title Scene init & Load Data
		Debug.Log ("1. App Start");

		StartCoroutine (TitleInitRoutine ());

	}

	IEnumerator TitleInitRoutine(){

		yield return new WaitForSeconds (Time.deltaTime * 2f );
		Flow = GameFlow.Title;
	}

	void FlowTitle(){
		// at the "Title" Scene, waiting for click start button
		StartCoroutine(WorldManager.I.Fade (false, 1.5f, "white"));
		WorldManager.I.SetWaterColor ();

		Debug.Log ("2. Title");

	}

	IEnumerator FlowStageSelect(){

		//at the "List" Scene, waiting for select stage
		Debug.Log ("3. Stage Select");
		Application.LoadLevel ("List");

		yield return null;

		StartCoroutine(WorldManager.I.Fade (false, 1.5f, "white"));
		WorldManager.I.SetWaterColor();
		//fade gogo

	}

	void FlowGameInit(){

		// Enter Game!
		Debug.Log ("4. Game Init");
		StartCoroutine (GameInitRoutine());

	}
	
	IEnumerator GameInitRoutine(){

		Application.LoadLevel ("Game");
		yield return null;

		WorldManager.I.Init ();
		UIManager.I.Init ();


		Flow = GameFlow.GamePlay;
	}


	void FlowGamePlay(){

		// init End! Start Game play
		Debug.Log ("5. Start Game Play!");

	}

	void FlowGameEnd(){

		Debug.Log ("6. Game End");
		// save & call score popup after pause game

		UIManager.I.CallScorePopup (pct);

	}

	void FlowGameOver(){
		Debug.Log ("6. Game Over ㅠㅠ");

		//Fade...

		//restart game!
		Flow = GameFlow.GameInit;

	}


}


/*
public delegate void Dele();

public class GameManager : MonoBehaviour {

	//About the Flow of This Game


	public string StageNum = null;
	public string selectedButton = null;

	public GameObject[] stages;


	//Delegates about flow function
	Dele _app_start;
	Dele _data_load;

	Dele _stage_select;

	Dele _game_init;
	Dele _map_init;
	Dele _player_init;
	Dele _ui_init;

	Dele _game_start;

	Dele _game_end;
	Dele _save_data;
	Dele _button_select;

	//Delegates List
	//public List< Dele > DelegateList = new List<Dele> ();
	public Dele CurrentDele = null;
	public bool isGameStart = false;

	public List < Dele > DelegateList = new List < Dele >();


	/////////////////////////////
	// singleton
	/////////////////////////////
	private static GameManager instance = null;
	public static GameManager I { 
		get {
			return instance; 
		} 
	}

	// Use this for initialization
	void Start () {

		//initialization
		_app_start = ApplicationStart;
		_data_load = DataLoad;
		_stage_select = StageSelect;
		_game_init = GameInitRoutineStart;
		_map_init = GameInitLoadingMap;
		_player_init = GameInitPlayerInit;
		_ui_init = GameInitUIInit;
		_game_start = StartGame;
		_game_end = EndGameRoutineStart;
		_save_data = SaveGameData;
		_button_select = ButtonSelect;

		//singleton
		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
			DontDestroyOnLoad (this.transform.root.gameObject);
		}

		ApplicationStart ();
	}

	// when Scene is restart just after paused
	void Awake() {

	}

	// Update is called once per frame
	void Update () {
	
		if (CurrentDele == null) {
			
			if (DelegateList.Count > 0) {

				CurrentDele = DelegateList [DelegateList.Count - 1];

				DelegateList.RemoveAt (DelegateList.Count - 1);

				CurrentDele ();

				//	Debug.Log ("Next Dele = "+ DelegateList[ DelegateList.Count - 1 ].Method.Name );
			}
		}// else
		//	Debug.Log ("Current Delegate's name = "+ CurrentDele.Method.Name );


	#if UNITY_ANDROID
		// Back 버튼
		if (Input.GetKey(KeyCode.Escape)) {

			Debug.LogError(" ::: Quit Game ::: ");
			Application.Quit();
			
			return;
		}
		#endif
		
		#if UNITY_EDITOR
		
		#endif

	}

	public void ResetCurrentDelegate(){

		CurrentDele = null;

	}

	//1. Application이 실행된다.

	void ApplicationStart(){

		Debug.Log ("1. Application이 실행된다.");
		CurrentDele = _app_start;

		StartCoroutine(SetTitleScene());

	}

	IEnumerator SetTitleScene(){

		yield return new WaitForSeconds (Time.deltaTime * 2f);

		if (UIManager.I != null) {
			UIManager.I.Fade (false, 1.0f, "black");
			//Debug.Log ("Changed");
		}
		else
			Debug.Log ("UIManager is null");


		if (WorldManager.I != null) {
			WorldManager.I.SetWaterColor ();
			//Debug.Log ("Changed");
		}
		else
			Debug.Log ("WorldManager is null");

		DelegateList.Add (_data_load);
	}

	// 2. Data Load : 데이터를 불러온다

	void DataLoad(){

		Debug.Log ("2. Data Load : 데이터를 불러온다");

		DelegateList.Add (_stage_select);

		ResetCurrentDelegate ();

	}


	// 3. Stage Select : 스테이지 신에서 스테이지를 선택한다.

	void StageSelect(){

		if (Application.loadedLevelName == "List") {

			Debug.Log ("3. Stage Select : 스테이지 신에서 스테이지를 선택한다.");

			WorldManager.I.SetWaterColor();
			UIManager.I.Init ();

			Debug.Log ("Stage Selecting.....");
		
			//	DelegateList.Add (
		}

	}

	// 4. Game Init : 게임 신으로 들어가서, 초기화를 한다

	public void GameInitRoutineStart(){

		Debug.Log ("GameInit Routine Start");
		
		//	Debug.Log (GameManager.I.DelegateList);
		//		Debug.Log (_map_init);

		Application.LoadLevel ("Game");
		StartCoroutine(Delay (Time.deltaTime * 2 ));
		Debug.Log ("Delay end :" + Time.time);
		DelegateList.Add (_map_init);


	}
	
	void GameInitLoadingMap(){
		
		Debug.Log ("Map Loading Start");


		//to-do
		Debug.Log ("this stage is"+ StageNum);

		WorldManager.I.Init();

		//if this f.t is end
		DelegateList.Add (_player_init);
		Debug.Log ("add 'Player Init' to list");
		ResetCurrentDelegate ();
		Debug.Log ("f.t. 1 end");
	}
	
	void GameInitPlayerInit(){
		
		Debug.Log ("Player Init start");


		//to-do
		try{

			if(Player.I == null) throw new System.Exception();
			WorldManager.I.PlayerInit ();
			DelegateList.Add (_ui_init);

			Debug.Log ("add 'UI Init' to list");
			ResetCurrentDelegate ();
			Debug.Log ("f.t. 2 end");
		

		}catch {
			Debug.LogError ("Player is null");
			ResetCurrentDelegate (); //shut down this function
		}

	}
	
	void GameInitUIInit(){
		
		Debug.Log ("UI Init start");

		//to-do
		UIManager.I.Init ();
		UIManager.I.Fade (false, 1.0f, "black");

		StartCoroutine(AddAfterDelay (0.9f, _game_start));

		//if this f.t is end
		Debug.Log ("Init Callback End - 'Game Start' to list");
	//	DelegateList.Add (_game_start);

	}


	// 5. 게임이 시작된다 ( 4가 끝나고 시작되어야 함)
	void StartGame(){

		Debug.Log ("Game Start function start!");
		isGameStart = true;

		// if this function is end
		//GameManager.I.CurrentDele = null;
	}

	// 6. 게임이 끝나고, Score가 보여지며 저장이 된다 ( 5가 끝나고 시작되어야 함)
	public void EndGameRoutineStart(){

		Debug.Log ("Stop Game Playing! EndGame Routine Start!");

		DelegateList.Add (_save_data);

		WorldManager.I.StopGame ();

		ResetCurrentDelegate ();
	}

	void SaveGameData(){

		Debug.Log ("Save Play Data...");

		DelegateList.Add (_button_select);

	}

	void ButtonSelect(){


		if (selectedButton == "Restart") {

			Application.LoadLevel ("Game");
			GameInitRoutineStart();
			ResetCurrentDelegate();

		} 
		else if (selectedButton == "Return") {

			DelegateList.Add (_data_load);

			Application.LoadLevel ("List");
			ResetCurrentDelegate();

		} 
		else {

			Debug.Log ("selectedButton = "+selectedButton);
		}
		
		selectedButton = null;
	}

	public void GameOver(){

		Debug.Log ("Game is Over --> Restart!");

		UIManager.I.Fade (true, 0.7f, "black");

		StartCoroutine(Delay (0.6f, _game_init));

		// Delay --> 1.5f
	}



	IEnumerator Delay( float t ){

		Debug.Log ("Delay start :" + Time.time);
		yield return new WaitForSeconds(t);
		ResetCurrentDelegate ();
		Debug.Log ("Delay end :" + Time.time);
	}

	IEnumerator Delay( float t, Dele _dele){

		yield return new WaitForSeconds (t);

		_dele ();

	}

	IEnumerator AddAfterDelay( float t, Dele _dele){

		yield return new WaitForSeconds(t);
		DelegateList.Add (_dele);

		ResetCurrentDelegate ();
	}
}

*/

