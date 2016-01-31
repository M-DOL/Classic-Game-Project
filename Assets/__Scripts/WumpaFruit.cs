using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour
{
    public Rigidbody rigid;
    public float rotSpeed = 3f;
    public float countSpeed = 20f;
    public float fruitFlySpeed = 100f;
    public bool flying = false, knocked = false;
    public float knockStart;
    public float knockDur = 1f;
    public float fruitDelay = .1f;
    public float startTime;
    public float invincibleDur = .5f;
    public float fruitFlyHeight = .5f;
    public float sizeCorrection = .98f;
    public float terminalDistance = 7f;
    public float fruitIconOffsetX = 30f, fruitIconOffsetY = -30f;
    public int crateLayerMask, groundLayerMask;
    public Vector3 fruitDir;
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        startTime = Time.time;
        crateLayerMask = LayerMask.NameToLayer("Crate");
        groundLayerMask = LayerMask.NameToLayer("Ground");
    }
    void FixedUpdate()
    {
        if (knocked && Time.time - knockStart > knockDur)
        {
            Destroy(gameObject);
        }
        if (!flying)
        {
            transform.RotateAround(transform.position, Vector3.up, rotSpeed);
        }
        else
        {
            transform.localScale *= sizeCorrection;
            rigid.velocity = Vector3.Normalize((new Vector3(fruitIconOffsetX, Screen.height + fruitIconOffsetY, 0) - Camera.main.WorldToScreenPoint(transform.position))) * countSpeed;
            if (Vector3.Magnitude(new Vector3(fruitIconOffsetX, Screen.height + fruitIconOffsetY, 0) - Camera.main.WorldToScreenPoint(transform.position)) < terminalDistance)
            {
                Display.S.OrderIncrementFruit();
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (flying)
        {
            return;
        }
        if (col.gameObject.layer == groundLayerMask)
        {
            Vector3 newPos = transform.position;
            newPos.y += .2f;
            transform.position = newPos;
            rigid.velocity = Vector3.zero;
            rigid.useGravity = false;
        }
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning) ||
       (col.gameObject.layer == crateLayerMask && col.gameObject.GetComponent<Rigidbody>().velocity.y < 0))
        {
            FlyToCounter();
        }
        //Prevents fruit from crates being spun away immediately
        else if (col.gameObject.tag == "Crash" && Time.time - startTime > invincibleDur)
        {
            FlyAway();
        }
    }
    void OnTriggerStay(Collider col)
    {
        if(flying)
        {
            return;
        }
        if ((col.gameObject.tag == "Crash" && !Crash.S.spinning) ||
       (col.gameObject.layer == crateLayerMask && col.gameObject.GetComponent<Rigidbody>().velocity.y < 0))
        {
            FlyToCounter();
        }
    }
    public void FlyToCounter()
    {
        Crash.S.PlaySound("WumpaCollect");
        Display.S.Show();
        flying = true;
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
