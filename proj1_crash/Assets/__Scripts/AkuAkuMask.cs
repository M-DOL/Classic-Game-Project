using UnityEngine;
using System.Collections;

public class AkuAkuMask : MonoBehaviour {
	public bool follow = false;
	//Vector3 initialRotate = Vector3.zero;

	//void Start(){
	//	initialRotate.x = 270;
	//	initialRotate.y = 180;
	//	this.transform.Rotate (initialRotate);
	//}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Crash"){
			Crash.S.numMasks++;
			follow = true;
		}
	}

	void Update () {
		if (follow) {
			Vector3 pos = this.transform.position;
			pos.x = Crash.S.transform.position.x + 0.5f;
			pos.y = Crash.S.transform.position.y + 2f;
			pos.z = Crash.S.transform.position.z - 3f;
			this.transform.position = pos;
		}
	}

}
