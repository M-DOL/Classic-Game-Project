using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public float speed = 2f;
    public bool launched;
    public float launchSpeed;
    public float launchTime;
    public float launchDuration;

    protected BoxCollider boxCol;
    protected Rigidbody rigid;

    public static Enemy S;
    void Awake()
    {
        S = this;
    }
    // Use this for initialization
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        boxCol = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {

    }

    public void LaunchEnemy()
    {
        launched = true;
        launchTime = Time.time;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.velocity = Vector3.forward * launchSpeed;
        RailMover.S.AddToRespawn(gameObject);
    }

}
