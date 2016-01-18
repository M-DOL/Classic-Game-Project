using UnityEngine;
using System.Collections;

public class CheckpointCrate : Crate {

	public override void BreakBox(){
		Crash.S.checkpoint = transform.position;
        Crash.S.PlaySound("CrateBreak");
		Destroy (this.gameObject);
	}
}
