using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Crash")
        {
            Display.S.DecrementLives();
            if (Display.S.numLives >= 0)
            {
                Crash.S.Respawn();
            }
        }
    }
}
