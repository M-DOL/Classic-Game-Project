using UnityEngine;
using System.Collections;

public class WumpaFruit : MonoBehaviour
{
    public Rigidbody rigid;
    public float rotSpeed = 3f;
    public float flySpeed = 300f;
    public bool flying = false;
    public float flyDuration = 1f;
    public float flyStartTime;
    public float sizeCorrection = .35f;
    public Vector3 fruitDir;
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (!flying)
        {
            transform.RotateAround(transform.position, Vector3.up, rotSpeed);
        }
        else
        {
            transform.localScale += sizeCorrection * Vector3.one;
            if(transform.position.z > Display.S.fruitDest.z)
            {
                Display.S.IncrementFruit();
                Destroy(gameObject);
            }
            /*if (Time.time - flyStartTime > flyDuration)
            {
                Display.S.IncrementFruit();
                Destroy(gameObject);
            }*/
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Crash")
        {
            FlyToCounter();
        }
    }
    void FlyToCounter()
    {
        flying = true;
        fruitDir = Display.S.fruitDest - transform.position;
        fruitDir = Vector3.Normalize(fruitDir);
        flyStartTime = Time.time;
        rigid.velocity = fruitDir * flySpeed;
    }
}
