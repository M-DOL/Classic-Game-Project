using UnityEngine;
using System.Collections;

public class BounceCrate : Crate {

	void OnCollisionEnter (Collision col)
    {
        if (rigid.velocity.y < 0f)
        {
            invincible = true;
            return;
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.spinning &&
                boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f))
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .05f;
            if (Crash.S.falling && landed)
            {
                   Crash.S.Bounce(2 * bounceVel);
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (rigid.velocity.y < 0f)
        {
            invincible = true;
            return;
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.spinning &&
                boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f))
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
            if (Crash.S.falling && landed)
            {
                Crash.S.Bounce(2 * bounceVel);
            }
        }
    }

}
