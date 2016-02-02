using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct ObjectSet
{
    public int numObjects;
    public GameObject type;
    public ObjectSet(GameObject kind, int num)
    {
        numObjects = num;
        type = kind;
    }
}
public class Crate : MonoBehaviour
{
    public List<ObjectSet> items;
    public float bounceVel = 3f;
    public float fruitVelY = 3f;
    public float fruitVelXZ = 1.5f;
    public float itemOffset = .5f;
    public bool invincible = false;
    public Rigidbody rigid;
    public LayerMask crateLayerMask;
    protected BoxCollider boxCol;
    static System.Random rand = new System.Random();
    public ObjectSet obj;
    // Use this for initialization
    void Start()
    {
        boxCol = gameObject.GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        crateLayerMask = LayerMask.GetMask("Crate");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Crab")
        {
            if (col.gameObject.GetComponent<CrabEnemy>().launched)
            {
                BreakBox(false);
            }
        }
        if (col.gameObject.tag == "Turtle")
        {
            if (col.gameObject.GetComponent<TurtleEnemy>().launched)
            {
                BreakBox(false);
            }
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.invincible)
            {
                BreakBox(true);
                return;
            }
            if (Crash.S.spinning &&
                boxCol.bounds.max.y < Crash.S.collider.bounds.center.y + .2f)
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y > boxCol.bounds.max.y - .1f;
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping && (Crash.S.toBreak == boxCol || Crash.S.toBreak == null))
                {
                    //The box cannot be crushed if Crash is jumping on it
                    BreakBox(false);
                    Crash.S.Bounce(bounceVel);
                }
                else
                {
                    Crash.S.LandOnCrate();
                }
            }
            else if(Crash.S.jumping && Crash.S.collider.bounds.max.y < boxCol.bounds.min.y + .1f)
            {
                BreakBox(true);
            }
        }
    }
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Crash")
        {
            if ((Crash.S.spinning &&
                boxCol.bounds.max.y < Crash.S.collider.bounds.center.y + .2f))
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }
        }
    }

    public virtual void BreakBox(bool crushed)
    {
        Destroy(this.gameObject);
        Crash.S.PlaySound("CrateBreak");
        if (items == null || items.Count == 0)
        {
            return;
        }
        int choice = rand.Next(items.Count);
        ObjectSet objects = items[choice];
        if (objects.type != null)
        {
            Quaternion rot = Quaternion.identity;
            Vector3 pos = transform.position;
            if (objects.type.name == "AkuAkuMask")
            {
                rot = Quaternion.Euler(270, 180, 0);
                pos.z -= .5f;
            }
            if (objects.numObjects == 1 && objects.type.name != "WumpaFruit")
            {
                pos.y += itemOffset + .25f;
                Instantiate(objects.type, pos, rot);
                return;
            }
            if (!crushed)
            {
                //Instantiates all crate objects
                pos.y += .5f;
                for (int i = 0; i < objects.numObjects; ++i)
                {
                    Vector3 fruitVel = Vector3.zero;
                    fruitVel = Random.insideUnitSphere * fruitVelXZ;
                    fruitVel.y = fruitVelY;
                    GameObject fruit = Instantiate(objects.type, pos, rot) as GameObject;
                    fruit.GetComponent<Rigidbody>().velocity = fruitVel;
                }
            }
            else
            {
                FruitGen.S.ManageFruits(objects.numObjects, transform.position.x, transform.position.y, transform.position.z);
            }

        }
    }

}
