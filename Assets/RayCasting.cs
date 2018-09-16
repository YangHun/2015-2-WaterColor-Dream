using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCasting : MonoBehaviour {

	public LayerMask touchInputMask;

	private List<GameObject> touchList = new List<GameObject> ();
	private GameObject[] touchesOld;

	RaycastHit hit;

	Camera camera;

	void Start(){

		camera = this.gameObject.GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update () {
	
#if UNITY_EDITOR
		if( Input.GetMouseButton (0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0) ) {

			touchesOld = new GameObject[ touchList.Count];
			touchList.CopyTo( touchesOld);
			touchList.Clear ();

			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			Debug.Log ("Input.mousePosition = "+Input.mousePosition);
			Debug.Log ("Ray Cast!");

			if( Physics.Raycast ( ray, out hit, touchInputMask)){

				GameObject reciplent = hit.transform.gameObject;
				Debug.Log ("hit point : " + camera.WorldToScreenPoint(hit.point));

				Debug.Log ("hitted object name : "+reciplent.name);

				if( Input.GetMouseButtonDown(0)){
				
					reciplent.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);

				}
			/*	if( Input.GetMouseButtonUp(0)){
					
					reciplent.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					
				}
				if( Input.GetMouseButton (0)){
					
					reciplent.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					
				} */
			}

			foreach ( GameObject g in touchesOld){
				if( !touchList.Contains(g)){
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
#endif
		if( Input.touchCount >0 ) {
			
			touchesOld = new GameObject[ touchList.Count];
			touchList.CopyTo( touchesOld);
			touchList.Clear ();
			
			foreach (Touch touch in Input.touches) {
				Ray ray = camera.ScreenPointToRay(touch.position);
			//	Debug.Log ("touch.position = "+touch.position);
				
				if( Physics.Raycast ( ray, out hit, touchInputMask)){
					
					GameObject reciplent = hit.transform.gameObject;
					Debug.Log ("hit point : " + hit.point);

					if( touch.phase == TouchPhase.Began){
						
						reciplent.SendMessage ("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
						
					}
				/*	if( touch.phase == TouchPhase.Ended){
						
						reciplent.SendMessage ("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
						
					}
					if( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
						
						reciplent.SendMessage ("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
						
					}
					if( touch.phase == TouchPhase.Canceled){
						
						reciplent.SendMessage ("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
						
					}
					*/
				}
			}
			foreach ( GameObject g in touchesOld){
				if( !touchList.Contains(g)){
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
