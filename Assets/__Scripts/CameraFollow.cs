using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public float frontFollowDistance = 4.8f;
    public float backFollowDistance = 7.5f;
    public bool frontFacing = true;
    public float speed = 2f;
    public float distThres = .01f;
    public Vector3 newPos;
    public static CameraFollow S;
    void Awake()
    {
        S = this;
    }
    void Start()
    {
        newPos = Camera.main.transform.position;
        newPos.y = Crash.S.sceneCenter.y + 4f; ;
        newPos.z = Crash.S.transform.position.z - frontFollowDistance;
        Camera.main.transform.position = newPos;
    }
    void Update()
    {
        newPos = Camera.main.transform.position;
        newPos.z = Crash.S.rigid.velocity.z * Time.deltaTime;
        if (Crash.S.rigid.velocity.z > .01f)
        {
            frontFacing = true;
        }
        else if (Crash.S.rigid.velocity.z < -.01f)
        {
            frontFacing = false;
        }
        newPos.x = Crash.S.sceneCenter.x;
        if(Crash.S.set2D)
        {
            newPos.y = Crash.S.transform.position.y + 3f;
            newPos.z = Crash.S.pos2D;
        }
        else
        {
            newPos.y = Crash.S.sceneCenter.y + 4f;
            newPos.z = Crash.S.transform.position.z - (frontFacing ? frontFollowDistance : backFollowDistance);
        }
        if (Mathf.Abs(newPos.z - Camera.main.transform.position.z) > distThres)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPos, Time.deltaTime * speed + Mathf.Abs(Crash.S.rigid.velocity.z) / 30f);
        }
        else
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPos, Time.deltaTime * speed);
        }
        transform.rotation = Quaternion.Euler(0, Vector3.RotateTowards(Crash.S.lastSceneCenter, Crash.S.sceneCenter, .2f, 0.0f).x, 0);
    }
}
