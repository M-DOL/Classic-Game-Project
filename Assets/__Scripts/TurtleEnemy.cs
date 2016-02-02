using UnityEngine;
using System.Collections;

public class TurtleEnemy : Enemy
{
    public int turnTime;
    public int maxTurnTime;
    public float trampTime, trampDur = 6f, alertDur = 4f;
    public float progress = 0;
    public float height = 4f;
    public bool flip = false;
    public bool trampoline = false, alert = false;
    private Vector3 origin;
    private Vector3 origRot;
    private Vector3 flipRot;
    private Renderer EyeL, EyeR;
    private Vector3 trampLoc;
    Quaternion trampRot;
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        boxCol = gameObject.GetComponent<BoxCollider>();

        origin = transform.position;
        flipRot = origRot = transform.rotation.eulerAngles;
        flipRot.z += 180;
        EyeL = transform.FindChild("EyeL").GetComponent<Renderer>();
        EyeR = transform.FindChild("EyeR").GetComponent<Renderer>();
    }

    public override void Move()
    {
        if(!boxCol.bounds.Contains(Crash.S.transform.position))
        {
            Physics.IgnoreCollision(Crash.S.collider, boxCol, false);
        }
        if (trampoline && Time.time - trampTime > trampDur)
        {
            alert = false;
            trampoline = false;
            transform.eulerAngles = origRot;
        }
        else if(trampoline && Time.time - trampTime > alertDur && !alert)
        {
            alert = true;
            StartCoroutine(Alert());
        }
        else if(trampoline && !launched)
        {
            transform.position = trampLoc;
            transform.rotation = trampRot;
            return;
        }
        if (progress < 90 && flip)
        {
            Flip();
            progress++;
        }
        else if (progress >= 90)
        {
            progress = 0;
            flip = false;
            trampoline = true;
            trampTime = Time.time;
            trampLoc = transform.position;
            trampRot = transform.rotation;
        }

        turnTime++;
        if (launched)
        {
            if (Time.time - launchTime > launchDuration)
            {
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
            if (launched)
            {
                return;
            }
            if (Crash.S.spinning || Crash.S.invincible)
            {
                LaunchEnemy();
                return;
            }
            bool killEnemy = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .4f;

            if (trampoline && Crash.S.jumping && killEnemy)
            {
                Crash.S.Bounce(6f);
                trampTime = Time.time;
                return;
            }
            else if(trampoline)
            {
                Physics.IgnoreCollision(col.collider, boxCol, true);
                trampTime = Time.time;
                return;
            }
            else if (Crash.S.falling && Crash.S.jumping && killEnemy)
            {
                flip = true;
            }
            if (flip && Crash.S.falling && Crash.S.jumping && killEnemy)
            {
                Crash.S.Bounce(6f);
            }
            if (!killEnemy && !flip && !trampoline)
            {
                if (Crash.S.numMasks > 0)
                {
                    Crash.S.KnockBack();
                    Destroy(this.gameObject);
                    Crash.S.LoseMask();
                }
                else
                {
                    Display.S.DecrementLives();
                    if (Display.S.numLives >= 0)
                    {
                        Crash.S.Respawn();
                    }
                }
            }
        }
    }
    void Flip()
    {

        Vector3 pos = origin;
        pos.y = (Mathf.Sin(Mathf.Deg2Rad * progress) * Mathf.Cos(Mathf.Deg2Rad * progress) * height) + origin.y;

        transform.position = pos; // handle moving upwards
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(flipRot), progress / 20f); // handle rotation
    }
    IEnumerator Alert()
    {
        while(Time.time - trampTime < trampDur)
        {
            EyeL.material.color = Color.red;
            EyeR.material.color = Color.red;
            yield return new WaitForSeconds(.3f);
            EyeL.material.color = Color.white;
            EyeR.material.color = Color.white;
            yield return new WaitForSeconds(.3f);
        }
    }
}
