using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct ObjectSet
{
    public int numObjects;
    public GameObject type;
}
public class Crate : MonoBehaviour {
	public List<ObjectSet> items;
    public float bounceVel = 3f;
	protected BoxCollider boxCol;
    static System.Random rand = new System.Random();
	// Use this for initialization
	void Start () {
		boxCol = gameObject.GetComponent<BoxCollider>();
	}
	
	void OnCollisionEnter (Collision col) {
		if(col.gameObject.tag == "Crash"){
			if(Crash.S.spinning){
				BreakBox ();
				return;
			}

			bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .01f;
			if(Crash.S.falling && landed){
				if (Crash.S.jumping) {
					BreakBox ();
					Crash.S.Bounce (bounceVel);
				} else {
					Crash.S.LandOnCrate ();
				}
			}
		}
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Crash" && Crash.S.spinning) {
			BreakBox ();
		}
	}

	public virtual void BreakBox()
    {
		Vector3 pos = transform.position;
		Quaternion rot = Quaternion.identity;
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
                Instantiate(objects.type, pos, rot);
                return;
            }
            //For bunches of Wumpa Fruits
            for(int i = 0; i < objects.numObjects; ++i)
            {
                Vector3 initPoint = Random.insideUnitSphere;
                initPoint.y = 0;
                initPoint += pos;
                Instantiate(objects.type, initPoint, rot);
            }
		}
	}
}
