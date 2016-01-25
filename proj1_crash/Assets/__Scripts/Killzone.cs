using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Crash")
        {
            Display.S.DecrementLives();
            Crash.S.Respawn();
        }
    }
}
