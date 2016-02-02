using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinCollider : MonoBehaviour
{
    bool rotating = false, flying = false;
    float rotTime, rotDur = 3f;
    float flyTime, flyDur = 2f;
    bool playing = false;
    void FixedUpdate()
    {
        if (rotating)
        {
            Crash.S.rigid.velocity = Vector3.zero;
            Crash.S.transform.Rotate(new Vector3(0, 20f, 0));
            if (Time.time - rotTime > rotDur)
            {
                rotating = false;
                flying = true;
                flyTime = Time.time;
            }
        }
        if(flying)
        {
            Display.S.congrats.enabled = true;
            Crash.S.transform.rotation = Quaternion.Euler(0, 180f, 0);
            Crash.S.rigid.velocity = Vector3.up * 10;
            if(!playing)
            {
                playing = true;
                AudioSource camSound = Camera.main.GetComponent<AudioSource>();
                camSound.Stop();
                camSound.PlayOneShot(Resources.Load("Sounds/Win") as AudioClip);
            }
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
            Crash.S.transform.parent = gameObject.transform;
            Crash.S.transform.localPosition = new Vector3(0f, -1f, 0f);
            Crash.S.ignoreInput = true;
            rotTime = Time.time;
        }
    }
}
