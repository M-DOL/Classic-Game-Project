using UnityEngine;
using System.Collections;

public class TriggerCrate : Crate
{
    public bool triggered = false;
    public override void BreakBox(bool crushed)
    {
        if(!triggered)
        {
            triggered = true;
            Trigger();
        }
    }
    public void Trigger()
    {

    }
}
