using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	Rigidbody rig;

	public Vector3 startPos;
	Vector3 colPos;

	public Collider[] Cols;
	public bool iscolstart = false;

	public Color color = Color.white;
	public CMYK cmyk = CMYK.white;

	////////////////////////////
	// singletone ft
	/////////////////////////////

	private static Character instance = null;
	public static Character I { get { return instance; } }

	// Use this for initialization
	void Start () {
	
		//	move = false;
		rig = GetComponent<Rigidbody> ();
		
		startPos = transform.position;

		if (instance != null) {
			Destroy (this.gameObject);
			return;
		} 
		else {
			instance = this;
		}
	}

	void Update(){

		if (colPos.y - transform.position.y > 100.0f)
			WorldManager.I.Over ();

	}

	void OnCollisionEnter(Collision col){

		colPos = col.contacts[0].point;
		Collider collider = col.collider;
		Collider _tile = null;
		bool _wall = false;

		Cols = Physics.OverlapSphere (transform.position, 5.00f * 0.50f * 2.48f);

		int leng = Cols.Length;
		
		for (int i = 0; i < leng; i ++) {
			
		//	Debug.Log ("Cols[" + i + "] = " + Cols [i].name);
			
			if ((_tile == null) && (Cols [i].tag == "Tile")) {
				
				_tile = Cols [i];
			}

			if ( (_wall == false) && (Cols [i].tag == "Wall")) {
				_wall = true;
			}
		}

		Debug.Log ("_wall == "+ _wall);
		if (_wall) {
			Scope.I.isreset = true;
			Scope.I.SetNextScope ();
		}
		else Scope.I.isreset = false;

		if (collider.tag == "Tile") {

			// change Character's Color to Tile's Color
			ChangeCharacterColor (collider);

			// set new Pivot of Scope
			Scope.I.SetPivotVector (colPos);
			Scope.I.SetNextScope ();

			// Add force to character
			if (!iscolstart) {
				
				StartCoroutine (setColBoolean ());	
				AddforceToCharacter ();
				StartCoroutine (WorldManager.I.CallParticle (collider.GetComponent<Tile> ().rgb, col.contacts [0].point));
			}
		} else if (collider.tag == "Start") {

			Scope.I.isreset = true;
			Scope.I.SetNextScope();

			AddforceToCharacter ();
			Scope.I.SetPivotVector (colPos);
			
		} 

	}

	void AddforceToCharacter(){

		//velocity init to zero
		rig.velocity = Vector3.zero;
		//rig.angularVelocity = Vector3.zero;
	
		//set direction
		Vector3 dir = Player.I.transform.forward;
		dir.y = 0f;
		dir.Normalize ();
		
		dir *= -1.659f;
		//dir *= - 1.106f * 1.5f;

		dir.y = 1.3278f;

		//dir.y = 4.426f * 0.3f;

		dir *= 620;

		rig.AddForce (dir);
		//Debug.Log ("addForce");
	}

	void OnTriggerExit (Collider col) {
	
		if (col.tag == "StartWall") {

			col.transform.parent.parent.GetComponent<StartTile>().ChangeWall ();

		}
	}


	void OnTriggerEnter (Collider col) {

		if (col.tag == "DestWall") {
			WorldManager.I.End ();
		}
	}

	void ChangeCharacterColor(Collider collider){

		if (collider.GetComponent<Tile> () != null) {
			//		Debug.Log ("col.collider.GetComponent<Tile>().rgb = " + col.collider.GetComponent<Tile>().rgb);
			
			Color chg = ColorManager.I.SubtractiveMix (collider.GetComponent<Tile> ().rgb);
			GetComponent<Renderer> ().material.SetColor ("_Color", chg); 
			color = chg;

			UIManager.I.ChangeSliders();

		//	Debug.Log ("character's color => " + chg);

			if( chg.Equals( Color.black) ){

				WorldManager.I.Over();

			}
		} 
		else
			Debug.Log ("this isn't a tile");
	}

	IEnumerator setColBoolean(){

		iscolstart = true;
		yield return new WaitForSeconds (1.0f);
	
		iscolstart = false;
	}
}