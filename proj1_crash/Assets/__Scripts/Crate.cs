using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public GameObject item;

	BoxCollider boxCol;

	// Use this for initialization
	void Start () {
		boxCol = gameObject.GetComponent<BoxCollider>();
	}
	
	void OnCollisionEnter (Collision col) {
		if(col.gameObject.tag == "Crash"){
			if(Crash.S.spinning){
				BreakBox ();
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
			if(Crash.S.falling && landed){
				if (Crash.S.jumping) {
					BreakBox ();
					Crash.S.Bounce (3f);
				} else {
					Crash.S.LandOnCrate ();
				}
			}
		}
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Crash" && Crash.S.spinning) {
			BreakBox ();
		}
	}

	void BreakBox(){
		Vector3 pos = transform.position;
		Destroy (this.gameObject);
		if(item != null){
			Instantiate (item, pos, Quaternion.identity);
		}
	}
}
