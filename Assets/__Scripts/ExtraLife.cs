using UnityEngine;
using System.Collections;

public class ExtraLife : MonoBehaviour
{
    public bool flying = false;
    public Vector3 lifeDir, follow;
    public Rigidbody rigid;
    public SpriteRenderer rend;
    public bool flickering = false;
    public int flickerTimes = 6;
    public float countSpeed = 150f;
    public float sizeCorrection = .97f;
    public float lifeIconOffsetX = -90f, lifeIconOffsetY = -70f, terminalDistance = 12f;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rend = gameObject.transform.FindChild("extraLife_0").GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (flying)
        {
            return;
        }
        if (col.gameObject.tag == "Crash")
        {
            FlyToCounter();
        }
    }
    public void FlyToCounter()
    {
        Display.S.lifeFly(transform.position);
        flying = true;
        Crash.S.PlaySound("ExtraLife");
        Display.S.Show();
        Destroy(gameObject);
    }
}
