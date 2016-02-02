using UnityEngine;
using System.Collections;

public class SpikePole : Enemy {

	public bool dropping;
	public bool raising;
	public bool waitUp;
	public bool waitDown;
	public int timer;
	public int dropTime;
	public int raiseTime;
	public int waitUpTime;
	public int waitDownTime;

	public override void Move () {
		if (dropping) {
			rigid.velocity = Vector3.down * speed;
			if (timer >= dropTime) {
				dropping = false;
				waitDown = true;
				timer = 0;
			}
		}
		if (raising){
			rigid.velocity = Vector3.up * speed;
			if(timer >= raiseTime){
				raising = false;
				waitUp = true;
				timer = 0;
			}
		}
		if (waitDown){
			rigid.velocity = Vector3.zero;
			if (timer >= waitDownTime) {
				waitDown = false;
				raising = true;
				timer = 0;
			}
		}
		if (waitUp){
			rigid.velocity = Vector3.zero;
			if (timer >= waitUpTime) {
				waitUp = false;
				dropping = true;
				timer = 0;
			}
		}
		timer++;
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Crash") {
			if (Crash.S.numMasks > 0) {
				Crash.S.KnockBack ();
				Crash.S.LoseMask ();
			} else {
				Display.S.DecrementLives ();
				if (Display.S.numLives >= 0) {
					Crash.S.Respawn ();
				}
			}
		}
	}
}
