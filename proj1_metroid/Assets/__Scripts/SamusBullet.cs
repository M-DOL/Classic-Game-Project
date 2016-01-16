using UnityEngine;
using System.Collections;
//1. Stop the bullet after distance
//2. Stop the bullet at walls
public class SamusBullet : MonoBehaviour
{
    public float bulletStopDist = 3f;
    protected Vector3 bulletOrigin;
    void Start()
    {
        bulletOrigin = transform.position;
    }
    void FixedUpdate()
    {
        float dist = (transform.position - bulletOrigin).magnitude;
        if (dist >= bulletStopDist)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
