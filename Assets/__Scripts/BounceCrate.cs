using UnityEngine;
using System.Collections;

public class BounceCrate : Crate
{
    public bool metal = false;
    void OnCollisionEnter(Collision col)
    {
        if (!metal)
        {
            if (Crash.S.invincible)
            {
                BreakBox(true);
                return;
            }
            if (col.gameObject.tag == "Crab")
            {
                if (col.gameObject.GetComponent<CrabEnemy>().launched)
                {
                    BreakBox(false);
                }
            }
            if (col.gameObject.tag == "Turtle")
            {
                if (col.gameObject.GetComponent<TurtleEnemy>().launched)
                {
                    BreakBox(false);
                }
            }
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.invincible || (Crash.S.spinning &&
                boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f)) && !metal)
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y > boxCol.bounds.max.y - .1f;
            if (Crash.S.falling && landed && Crash.S.jumping && (Crash.S.toBreak == boxCol || Crash.S.toBreak == null))
            {
                Crash.S.Bounce(2 * bounceVel);
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (Crash.S.invincible && !metal)
        {
            BreakBox(true);
            return;
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.spinning &&
                boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f) && !metal)
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }
        }
    }

}