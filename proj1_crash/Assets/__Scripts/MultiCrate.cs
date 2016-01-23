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
        if (col.gameObject.tag == "Crash")
        {
			if (Crash.S.invincible || Crash.S.spinning)
            {
                if (Crash.S.invincible)
                {
                    for (int i = 0; i < fruitsRemaining; ++i)
                    {
                        items.Add(items[0]);
                    }
                }
                else
                {
                    items = null;
                }
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping)
                {
                    if (fruitsRemaining > 0)
                    {
                        if (fruitsRemaining == 9)
                        {
                            startTime = Time.time;
                        }
                        --fruitsRemaining;
                        Vector3 fruitPos = transform.position;
                        fruitPos.y += .5f;
                        fruitPos.z -= .5f;
                        Instantiate(fruitPrefab, fruitPos, Quaternion.identity);
                        Crash.S.Bounce(bounceVel);
                        if (Time.time - startTime < fruitDuration)
                        {
                            return;
                        }
                        else
                        {
                            items = null;
                        }
                    }
                    bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                    BreakBox(crateAbove);
                }
                else
                {
                    Crash.S.LandOnCrate();
                }
            }
        }
    }
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Crash" && Crash.S.spinning)
        {
            items = null;
            bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
            BreakBox(crateAbove);
        }
    }
}
