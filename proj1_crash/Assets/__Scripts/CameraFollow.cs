using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public float frontFollowDistance = 4.8f;
	public float backFollowDistance = 7.5f;
    public bool frontFacing = true;
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 pos = Camera.main.transform.position;
        if (Crash.S.rigid.velocity.z > .01f)
        {
            frontFacing = true;
        }
        else if(Crash.S.rigid.velocity.z < -.01f)
        {
            frontFacing = false;
        }
        pos.z = Crash.S.transform.position.z - (frontFacing ? frontFollowDistance : backFollowDistance);
        Camera.main.transform.position = pos;
	}
}
