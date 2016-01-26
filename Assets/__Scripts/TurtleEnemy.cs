using UnityEngine;
using System.Collections;

public class TurtleEnemy : Enemy
{

    public int turnTime;
    public int maxTurnTime;

    public float progress = 0;
    public float height = 5f;
    public bool flip = false;
    private Vector3 origin;
    private Vector3 origRot;
    private Vector3 flipRot;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        boxCol = gameObject.GetComponent<BoxCollider>();

        origin = transform.position;
        flipRot = origRot = transform.rotation.eulerAngles;
        flipRot.z += 180;
    }

    public override void Move()
    {

        if (progress < 90 && flip)
        {
            Flip();
            progress++;
        }
        else if (progress >= 90)
        {
            progress = 0;
            flip = false;
        }

        turnTime++;
        if (launched)
        {
            if (Time.time - launchTime > launchDuration)
            {
                CameraFollow.S.AddToRespawn(gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                rigid.velocity = Vector3.forward * launchSpeed;
            }
        }
        else
        {
            Vector3 towards = Vector3.MoveTowards(transform.position, Crash.S.transform.position, .05f);
            towards.y = transform.position.y;
            Vector3 away = Vector3.MoveTowards(transform.position, Crash.S.transform.position, -.05f);
            away.y = transform.position.y;
            if (turnTime >= maxTurnTime)
            {
                transform.position = away;
                if (turnTime >= 2 * maxTurnTime)
                {
                    turnTime = 0;
                }
            }
            else transform.position = towards;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Crab" || col.gameObject.tag == "Turtle")
        {
            if (launched)
            {
                Destroy(col.gameObject);
            }
        }
        if (col.gameObject.tag == "Crash")
        {
			if (Crash.S.spinning || Crash.S.invincible)
            {
                LaunchEnemy();
                return;
            }

            bool killEnemy = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;

            if (Crash.S.falling && killEnemy)
            {
                flip = true;
                Crash.S.Bounce(3f);
            }

            if (!killEnemy)
            {
                if (Crash.S.numMasks > 0)
                {
                    Crash.S.KnockBack();
                    CameraFollow.S.AddToRespawn(gameObject);
                    Destroy(this.gameObject);
                    AkuAkuMask.mask.LoseMask();
                }
                Display.S.DecrementLives();
                //Display.S.Restart ();
                Crash.S.Respawn();
            }
        }
    }

    void Flip()
    {

        Vector3 pos = origin;
        pos.y = (Mathf.Sin(Mathf.Deg2Rad * progress) * Mathf.Cos(Mathf.Deg2Rad * progress) * height) + origin.y;

        transform.position = pos; // handle moving upwards
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(flipRot), progress / 90); // handle rotation
    }
}
