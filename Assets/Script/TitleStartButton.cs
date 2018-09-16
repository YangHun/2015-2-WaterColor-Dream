using UnityEngine;
using System.Collections;

public class TitleStartButton : MonoBehaviour {

	void OnTouchDown(){
		
		WorldManager.I.ClickStartButton ();
		
	}
}
