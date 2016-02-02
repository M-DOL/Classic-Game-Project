using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinCollider : MonoBehaviour
{
    bool rotating = false, flying = false;
    float rotTime, rotDur = 3f;
    float flyTime, flyDur = 2f;
    void FixedUpdate()
    {
        if (rotating)
        {
            Crash.S.rigid.velocity = Vector3.zero;
            Crash.S.transform.Rotate(new Vector3(0, 10f, 0));
            if (Time.time - rotTime > rotDur)
            {
                rotating = false;
                flying = true;
                flyTime = Time.time;
            }
        }
        if(flying)
        {
            Crash.S.rigid.velocity = Vector3.up * 10;
            if(Time.time - flyTime > flyDur)
            {
                SceneManager.LoadScene("_Scene_Title");
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Crash")
        {
            rotating = true;
            rotTime = Time.time;
        }
    }
}
