using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MultiCrate : Crate
{
    public GameObject fruitPrefab;
    //Fruit sequences last a certain duration with max 10 fruits
    public float startTime;
    public float fruitDuration = 8f;
    public int fruitsRemaining = 9;
    void Start()
    {
        bounceVel = 4f;
        boxCol = gameObject.GetComponent<BoxCollider>();
        crateLayerMask = LayerMask.GetMask("Crate");
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Crab")
        {
            if (col.gameObject.GetComponent<CrabEnemy>().launched)
            {
                items = null;
                BreakBox(false);
            }
        }
        if (col.gameObject.tag == "Turtle")
        {
            if (col.gameObject.GetComponent<TurtleEnemy>().launched)
            {
                items = null;
                BreakBox(false);
            }
        }
        if (col.gameObject.tag == "Crash")
        {
            if(Crash.S.invincible)
            {
                items = new List<ObjectSet>();
                items.Add(new ObjectSet(fruitPrefab, fruitsRemaining + 1));
                BreakBox(true);
                return;
            }
            if (Crash.S.spinning &&
               boxCol.bounds.max.y < Crash.S.collider.bounds.center.y + .2f)
            {
                items = null;
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = (Crash.S.collider.bounds.center.y + Crash.S.collider.bounds.min.y) / 2f > boxCol.bounds.max.y;
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping && (Crash.S.toBreak == boxCol || Crash.S.toBreak == null))
                {
                    if (fruitsRemaining == 9)
                    {
                        startTime = Time.time;
                    }
                    if (Time.time - startTime > fruitDuration)
                    {
                        items = null;
                    }
                    else if (fruitsRemaining > 0)
                    {
                        --fruitsRemaining;
                        Vector3 fruitPos = transform.position;
                        fruitPos.y += .5f;
                        fruitPos.z -= .5f;
                        GameObject fruit = Instantiate(fruitPrefab, fruitPos, Quaternion.identity) as GameObject;
                        fruit.GetComponent<WumpaFruit>().FlyToCounter();
                        Crash.S.Bounce(bounceVel);
                        return;
                    }
                    bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                    BreakBox(crateAbove);
                }
                else
                {
                    Crash.S.LandOnCrate();
                }
            }
            else if (Crash.S.jumping && Crash.S.collider.bounds.max.y < boxCol.bounds.min.y + .1f)
            {
                if (fruitsRemaining == 9)
                {
                    startTime = Time.time;
                }
                if (Time.time - startTime > fruitDuration)
                {
                    items = null;
                }
                else if (fruitsRemaining > 0)
                {
                    --fruitsRemaining;
                    Vector3 fruitPos = transform.position;
                    fruitPos.y += .5f;
                    fruitPos.z -= .5f;
                    GameObject fruit = Instantiate(fruitPrefab, fruitPos, Quaternion.identity) as GameObject;
                    fruit.GetComponent<WumpaFruit>().FlyToCounter();
                    Crash.S.Bounce(bounceVel);
                    return;
                }
                BreakBox(true);
            }
        }
    }
    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Crash")
        {
            if (Crash.S.invincible)
            {
                items = new List<ObjectSet>();
                items.Add(new ObjectSet(fruitPrefab, fruitsRemaining + 1));
                BreakBox(true);
                return;
            }
            if (Crash.S.spinning &&
                   boxCol.bounds.max.y < Crash.S.collider.bounds.center.y + .2f)
            {
                items = null;
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
            }
        }
    }
}
