using UnityEngine;
using System.Collections;

public class ExtraLife : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Crash"){
			Display.S.IncrementLives();
			Destroy (gameObject);
		}
	}
}
