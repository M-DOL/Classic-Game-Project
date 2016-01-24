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
public class Crate : MonoBehaviour {
	public List<ObjectSet> items;
    public float bounceVel = 3f;
    public float itemOffset = .5f;
    public bool invincible = false;
    public Rigidbody rigid;
    public LayerMask crateLayerMask;
	protected BoxCollider boxCol;
    static System.Random rand = new System.Random();

	// Use this for initialization
	void Start () {
		boxCol = gameObject.GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
        crateLayerMask = LayerMask.GetMask("Crate");
	}
	
	void OnCollisionEnter (Collision col) {
        if (col.gameObject.tag == "Crab" || col.gameObject.tag == "Turtle")
        {
            if (Enemy.S.launched)
            {
                BreakBox(false);
            }
        }
            if (rigid.velocity.y < 0f)
        {
            invincible = true;
            return;
        }
		if(col.gameObject.tag == "Crash"){
            if (Crash.S.invincible)
            {
                BreakBox(true);
            }
            if ((Crash.S.spinning && 
				boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f)))
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
			if(Crash.S.falling && landed){
				if (Crash.S.jumping) {
                    //The box cannot be crushed if Crash is jumping on it
					BreakBox(false);
					Crash.S.Bounce (bounceVel);
				} else {
					Crash.S.LandOnCrate();
				}
			}
		}
	}

	void OnCollisionStay(Collision col){
        if (rigid.velocity.y < 0f)
        {
            invincible = true;
            return;
        }
        if (col.gameObject.tag == "Crash")
        {
            if (Crash.S.spinning &&
                boxCol.bounds.max.y < (Crash.S.collider.bounds.center.y + .01f))
            {
                bool crateAbove = Physics.Raycast(transform.position, Vector3.up, transform.localScale.y, crateLayerMask);
                BreakBox(crateAbove);
                return;
            }

            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
            if (Crash.S.falling && landed)
            {
                if (Crash.S.jumping)
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
        }
    }
	public virtual void BreakBox(bool crushed)
    {
		Vector3 pos = transform.position;
		Quaternion rot = Quaternion.identity;
        CameraFollow.S.AddToRespawn(gameObject);
        Destroy (this.gameObject);
        Crash.S.PlaySound("CrateBreak");
        if(items == null || items.Count == 0)
        {
            return;
        }
        int choice = rand.Next(items.Count);
        ObjectSet objects = items[choice];
		if(objects.type != null){
            if (objects.type.name == "AkuAkuMask")
            {
                rot = Quaternion.Euler(270, 180, 0);
            }
            if(objects.numObjects == 1)
            {
                pos.y += itemOffset;
                Instantiate(objects.type, pos, rot);
                return;
            }
            if(!crushed)
            {
                //Instantiates all crate objects
                for (int i = 0; i < objects.numObjects; ++i)
                {
                    Vector3 initPoint = Random.insideUnitSphere;
                    initPoint.y = 0;
                    initPoint += pos;
                    Instantiate(objects.type, initPoint, rot);
                }
            }
            else
            {
                for (int i = 0; i < objects.numObjects; ++i)
                {
                    Instantiate(objects.type, pos, rot);
                }
            }

		}
	}
}
