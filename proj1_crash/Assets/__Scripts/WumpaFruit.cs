using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour
{
    public Rigidbody rigid;
    public float rotSpeed = 3f;
    public float countSpeed = 300f;
    public float fruitFlySpeed = 100f;
    public bool flying = false, knocked = false;
    public float knockStart;
    public float knockDur = 1f;
    public float fruitDelay = .1f;
    public float sizeCorrection = .35f;
    public float startTime;
    public float invincibleDur = .5f;
    public float fruitFlyHeight = .5f;
    public LayerMask crateLayerMask;
    public Vector3 fruitDir;
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        startTime = Time.time;
        crateLayerMask = LayerMask.GetMask("Crate");
    }
    void FixedUpdate()
    {
        if(knocked && Time.time - knockStart > knockDur)
        {
            Destroy(gameObject);
        }
        if (!flying)
        {
            transform.RotateAround(transform.position, Vector3.up, rotSpeed);
        }
        else
        {
            transform.localScale += sizeCorrection * Vector3.one;
            if(transform.position.z > Display.S.fruitDest.z)
            {
                Display.S.OrderIncrementFruit();
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning) ||
        CameraFollow.S.crateTags.Contains(col.gameObject.tag))
        {
            FlyToCounter();
        }
        //Prevents fruit from crates being spun away immediately
        else if(col.gameObject.tag == "Crash" && Time.time - startTime > invincibleDur)
        {
            FlyAway();
        }
    }
    void OnTriggerStay(Collider col)
    {
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning) ||
        CameraFollow.S.crateTags.Contains(col.gameObject.tag))
        {
            FlyToCounter();
        }
        //Prevents fruit from crates being spun away immediately
        else if (col.gameObject.tag == "Crash" && Time.time - startTime > invincibleDur)
        {
            FlyAway();
        }
    }
    public void FlyToCounter()
    {
        Display.S.Show();
        Display.S.lastLaunch = Time.time;
        flying = true;
        fruitDir = Display.S.fruitDest - transform.position;
        fruitDir = Vector3.Normalize(fruitDir);
        rigid.velocity = fruitDir * countSpeed;
        Crash.S.PlaySound("WumpaCollect");
    }
    void FlyAway()
    {
        knocked = true;
        knockStart = Time.time;
        Vector3 randDir = Random.insideUnitSphere;
        randDir.y = fruitFlyHeight;
        rigid.velocity = Vector3.Normalize(randDir) * fruitFlySpeed;
        Crash.S.PlaySound("WumpaHit");
    }
}
