using UnityEngine;
using System.Collections;

public class ExtraLife : MonoBehaviour {
    public bool flying = false;
    public Vector3 lifeDir;
    public Rigidbody rigid;
    public float countSpeed = 300f;
    public float sizeCorrection = .35f;
    void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Crash"){
            Display.S.Show();
            FlyToCounter();
		}
	}
    void FixedUpdate()
    {
        if(flying)
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
        lifeDir = Display.S.fruitDest - transform.position;
        lifeDir = Vector3.Normalize(lifeDir);
        rigid.velocity = lifeDir * countSpeed;
        Crash.S.PlaySound("WumpaCollect");
    }
}
