using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public struct DestroyedElement
{
    public string tag;
    public Vector3 placement;
    public Quaternion rotation;
}
public class CameraFollow : MonoBehaviour
{
    public ArrayList crateTags = new ArrayList{ "Crate", "MultiCrate", "BounceCrate", "LifeCrate", "TriggerCrate", "MaskCrate", "RandomCrate" };
    public List<DestroyedElement> destroyed;
    public float frontFollowDistance = 4.8f;
    public float backFollowDistance = 7.5f;
    public bool frontFacing = true;
    public float speed = 2f;
    public float distThres = .01f;
    Vector3 newPos;
    public static CameraFollow S;
    void Awake()
    {
        S = this;
        destroyed = new List<DestroyedElement>();
    }
    void Start()
    {
        newPos = Camera.main.transform.position;
        newPos.z = Crash.S.transform.position.z - frontFollowDistance;
        Camera.main.transform.position = newPos;
    }
    void Update()
    {
        newPos = Camera.main.transform.position;
        if(!Crash.S.jumping)
        {
            newPos.y = Crash.S.transform.position.y + 3f;
        }
        newPos.z = Crash.S.rigid.velocity.z * Time.deltaTime;
        if (Crash.S.rigid.velocity.z > .01f)
        {
            frontFacing = true;
        }
        else if (Crash.S.rigid.velocity.z < -.01f)
        {
            frontFacing = false;
        }
        newPos.z = Crash.S.transform.position.z - (frontFacing ? frontFollowDistance : backFollowDistance);
        if (Mathf.Abs(newPos.z - Camera.main.transform.position.z) > distThres)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPos, Time.deltaTime * speed + Mathf.Abs(Crash.S.rigid.velocity.z) / 30f);
        }
        else
        {
            Camera.main.transform.position = newPos;
        }
    }
    public void RespawnItems()
    {
        GameObject toInst = null;
        foreach (DestroyedElement gone in destroyed)
        {
            toInst = Resources.Load("Prefabs/" + gone.tag) as GameObject;
            if(toInst != null)
            {
                Instantiate(toInst, gone.placement, gone.rotation);
            }
        }
        destroyed.Clear();
    }
    public void AddToRespawn(GameObject g)
    {
        DestroyedElement dest = new DestroyedElement();
        dest.placement = Vector3.zero;
        dest.placement.x = g.transform.position.x;
        dest.placement.y = g.transform.position.y;
        dest.placement.z = g.transform.position.z;
        dest.rotation.w = g.transform.rotation.w;
        dest.rotation.x = g.transform.rotation.x;
        dest.rotation.y = g.transform.rotation.y;
        dest.rotation.z = g.transform.rotation.z;
        dest.tag = g.tag;
        destroyed.Add(dest);
    }
}
