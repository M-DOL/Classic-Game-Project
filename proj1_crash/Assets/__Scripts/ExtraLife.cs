using UnityEngine;
using System.Collections;

public class ExtraLife : MonoBehaviour
{
    public bool flying = false;
    public Vector3 lifeDir;
    public Rigidbody rigid;
    public float countSpeed = 300f;
    public float sizeCorrection = .35f;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Crash")
        {
            FlyToCounter();
        }
    }
    void FixedUpdate()
    {
        if (flying)
        {
            transform.localScale += sizeCorrection * Vector3.one;
            if (transform.position.z > Display.S.fruitDest.z)
            {
                Display.S.IncrementLives();
                Destroy(gameObject);
            }
        }
    }
    public void FlyToCounter()
    {
        flying = true;
        Display.S.Show();
        lifeDir = Display.S.lifeDest - transform.position;
        lifeDir = Vector3.Normalize(lifeDir);
        rigid.velocity = lifeDir * countSpeed;
    }
}
