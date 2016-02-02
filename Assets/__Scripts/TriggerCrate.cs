using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerCrate : Crate
{
    public bool triggered = false;
    public Material metalMat;
    public List<GameObject> invisCrates;
    public override void BreakBox(bool crushed)
    {
        if(!triggered)
        {
            triggered = true;
            StartCoroutine(Trigger());
        }
    }
    void OnCollisionEnter(Collision col)
    {
        bool landed = Crash.S.collider.bounds.min.y > boxCol.bounds.max.y - .1f;
        if (triggered)
        {
            if (Crash.S.falling && landed)
            {
                Crash.S.LandOnCrate();
            }
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.invincible)
            {
                BreakBox(true);
                return;
            }
            if (Crash.S.spinning &&
               boxCol.bounds.max.y < Crash.S.collider.bounds.center.y + .05f)
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping && (Crash.S.toBreak == boxCol || Crash.S.toBreak == null))
                {
                    //The box cannot be crushed if Crash is jumping on it
                    BreakBox(false);
                    Crash.S.Bounce(bounceVel);
                }
                else
                {
                    Crash.S.LandOnCrate();
                }
            }
        }
    }
    public IEnumerator Trigger()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = metalMat;
        foreach(GameObject invisCrate in invisCrates)
        {
            invisCrate.GetComponent<InvisCrate>().ReplaceBox();
            Crash.S.PlaySound("Bounce");
            yield return new WaitForSeconds(.5f); 
        }
    }
}
