using UnityEngine;
using System.Collections;

public class MultiCrate : Crate
{
    public int fruitsRemaining = 9;
    void Start()
    {
        bounceVel = 4f;
        boxCol = gameObject.GetComponent<BoxCollider>();
        //Selects 8 or 10 fruits with a 50:50 chance
        fruitsRemaining = Random.value > .5f ? 9 : 7;
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.spinning)
            {
                item = null;
                BreakBox();
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping)
                {
                    if (fruitsRemaining > 0)
                    {
                        --fruitsRemaining;
                        Crash.S.Bounce(bounceVel);
                        Display.S.IncrementFruit();
                    }
                    else
                    {
                        BreakBox();
                    }
                }
                else
                {
                    Crash.S.LandOnCrate();
                }
            }
        }
    }
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Crash" && Crash.S.spinning)
        {
            item = null;
            BreakBox();
        }
    }
}
