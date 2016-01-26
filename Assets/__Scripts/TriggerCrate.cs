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
    public IEnumerator Trigger()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material = metalMat;
        foreach(GameObject invisCrate in invisCrates)
        {
            invisCrate.GetComponent<InvisCrate>().ReplaceBox();
            yield return new WaitForSeconds(.6f); 
        }
    }
}
