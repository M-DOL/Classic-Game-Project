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
    public LayerMask crateLayerMask, groundLayerMask;
    public Vector3 fruitDir;
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        startTime = Time.time;
        crateLayerMask = LayerMask.GetMask("Crate");
        groundLayerMask = LayerMask.GetMask("Ground");
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
            if (transform.position.z > Display.S.fruitDest.z)
            {
                Display.S.OrderIncrementFruit();
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(flying)
        {
            return;
        }
        if(col.gameObject.layer == groundLayerMask)
        {
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;
        }
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning && Time.time - startTime > invincibleDur) ||
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
        if (flying)
        {
            return;
        }
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning && Time.time - startTime > invincibleDur) ||
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
        Crash.S.PlaySound("WumpaCollect");
        Display.S.Show();
        flying = true;
        fruitDir = Display.S.fruitDest - transform.position;
        fruitDir = Vector3.Normalize(fruitDir);
        rigid.velocity = fruitDir * countSpeed;
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
