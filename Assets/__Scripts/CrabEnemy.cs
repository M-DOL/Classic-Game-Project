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
        if (col.gameObject.tag == "Crab" || col.gameObject.tag == "Turtle")
        {
            if (launched)
            {
                Destroy(col.gameObject);
            }
        }
		if (col.gameObject.tag == "Wall" || col.gameObject.layer == 11){//11 == crate layer
			speed *= -1;
		}
		else if(col.gameObject.tag == "Crash"){
            if(launched)
            {
                return;
            }
			if(Crash.S.spinning || Crash.S.invincible){
				LaunchEnemy ();
				return;
			}

			bool killEnemy = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;

			if(Crash.S.jumping && Crash.S.falling && killEnemy){
                Destroy (this.gameObject);
				Crash.S.Bounce (3f);
			}

			if (!killEnemy) {
                if (Crash.S.numMasks > 0)
                {
                    Crash.S.KnockBack();
                    Destroy(this.gameObject);
                    Crash.S.LoseMask();
                }
                else
                {
                    Display.S.DecrementLives();
                    if(Display.S.numLives >= 0)
                    {
                        Crash.S.Respawn();
                    }
                }
			}
		}
	}
}
