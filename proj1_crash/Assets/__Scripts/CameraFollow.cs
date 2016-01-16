using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float followDistance = 7.5f;

	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Camera.main.transform.position;
		pos.z = Crash.S.transform.position.z - followDistance;
		Camera.main.transform.position = pos;
	}
}
