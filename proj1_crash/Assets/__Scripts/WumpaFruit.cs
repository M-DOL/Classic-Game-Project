using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Crash"){
			Display.S.IncrementFruit ();
			Destroy (gameObject);
		}
	}
}
