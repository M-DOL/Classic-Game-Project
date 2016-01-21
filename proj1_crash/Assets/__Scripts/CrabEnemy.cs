using UnityEngine;
using System.Collections;

public class CrabEnemy : Enemy {
	
	public override void Move () {
		if (launched) {
			if (Time.time - launchTime > launchDuration) {
				Destroy (this.gameObject);
			} else {
				rigid.velocity = Vector3.forward * launchSpeed;
			}
		} else {
			rigid.velocity = Vector3.right * speed;
		}
	}

	void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Enemy")
        {
            if (launched)
            {
                Destroy(col.gameObject);
            }
        }
        if (col.gameObject.tag == "Wall"){
			speed *= -1;
		}
		else if(col.gameObject.tag == "Crash"){
			if(Crash.S.spinning){
				LaunchEnemy ();
				return;
			}

			bool killEnemy = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;

			if(Crash.S.falling && killEnemy){
				Destroy (this.gameObject);
				Crash.S.Bounce (3f);
			}

			if (!killEnemy) {
                if (Crash.S.numMasks > 0)
                {
                    Destroy(this.gameObject);
                    AkuAkuMask.mask.LoseMask();
                }
                else
                {
                    Display.S.DecrementLives();
                    //Display.S.Restart ();
                    Crash.S.Respawn();
                }
			}
		}
	}
}
