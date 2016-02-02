using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public float speed = 2f;
	public int turnSpeed;
	public int timer;
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Rigidbody>().velocity = Vector3.right * speed;
		timer++;
		if (timer >= turnSpeed) {
			speed *= -1;
			timer = 0;
		}
	}
}
