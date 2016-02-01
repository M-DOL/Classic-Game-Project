using UnityEngine;
using System.Collections;

public class CheckpointCrate : Crate {

	public override void BreakBox(bool crushed){
        PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
        PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
        PlayerPrefs.SetFloat("CheckpointZ", transform.position.z);
        Crash.S.PlaySound("CrateBreak");
		Destroy (this.gameObject);
	}
}
