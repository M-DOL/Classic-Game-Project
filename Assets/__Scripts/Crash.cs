﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Crash : MonoBehaviour
{
    public float speed = 6f;

    public float spinDuration = .4f;
    public float airSpinDuration = .2f;
    public float spinSpeed = 5000f;
    public float spinEnd;
    public float spinCooldown = .7f;
    public bool spin;

    public float jumpVel = 3.5f;
    public float jumpStartTime;
    public float jumpDur = .5f;
    public bool jumpStart, jumpCont;

    public float invincibleDur = 20f;
    public float invincibleStart = 1f;
    public bool invincible = false;
    public bool grounded = true;
    public bool jumping = false;
    public bool falling = false;
    public bool spinning = false;
    public new BoxCollider collider;

    public int numMasks;

    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public Quaternion prespinRotation;
    public Vector3 checkpoint = Vector3.zero;

    //Sound Clips
    AudioSource crashSound;

    public Rigidbody rigid;
    float iH, iV;
    Vector3 vel;
    float distToGround;
    float groundedOffset;
    int fakeGroundLayerMask, groundLayerMask, crateLayerMask;
    RaycastHit[] hits = new RaycastHit[9];
    public Collider toBreak;
    Vector3 origin;
    private float[] offsets;
    private LayerMask wallLayerMask;
    private RaycastHit sceneLeft, sceneRight, sceneFloor;
    public float maxWallDist = 10f, maxGroundDist = 20f, max2DDist = 3f;
    public float spinStartTime;
    public Vector3 sceneCenter = Vector3.zero, lastSceneCenter = Vector3.zero;
    public bool set2D = false;
    public float pos2D;
    Color origColor;
    Renderer rend, rendL, rendR;
    public bool ignoreInput = false;
    public static Crash S;

    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<BoxCollider>();
        rend = gameObject.GetComponent<Renderer>();
        rendR = transform.FindChild("ArmR").GetComponent<Renderer>();
        rendL = transform.FindChild("ArmL").GetComponent<Renderer>();
        origColor = rend.material.color;
        checkpoint.x = PlayerPrefs.GetFloat("CheckpointX");
        checkpoint.y = PlayerPrefs.GetFloat("CheckpointY");
        checkpoint.z = PlayerPrefs.GetFloat("CheckpointZ");
        if (checkpoint != Vector3.zero)
        {
            transform.position = checkpoint;
            Destroy(Display.S.checkPointCrate);
        }
        distToGround = gameObject.GetComponent<BoxCollider>().bounds.extents.y;
        groundedOffset = collider.size.x / 2f;

        groundLayerMask = LayerMask.GetMask("Ground");
        fakeGroundLayerMask = LayerMask.GetMask("FakeGround");
        crateLayerMask = LayerMask.GetMask("Crate");
        crashSound = GetComponent<AudioSource>();
        wallLayerMask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        iH = Input.GetAxis("Horizontal");
        iV = Input.GetAxis("Vertical");
        spin = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Q);
        jumpStart = Input.GetKeyDown(KeyCode.A);
        if (ignoreInput)
        {
            iH = 0;
            iV = 0;
            jumpStart = false;
        }
        if (spin && !spinning && Time.time - spinEnd > spinCooldown)
        {
            spin = false;
            prespinRotation = transform.rotation;
            spinning = true;
            spinStartTime = Time.time;
            PlaySound("Spin");
        }
        if (!jumping && jumpStart && grounded)
        {
            PlaySound("Jump");
            jumping = true;
            jumpCont = true;
            jumpStartTime = Time.time;
        }
        if ((jumpCont &&
            Input.GetKeyUp(KeyCode.A)) ||
            Time.time - jumpStartTime > jumpDur)
        {
            jumpCont = false;
        }
        if(Input.GetKeyDown(KeyCode.I) && !invincible)
        {
            invincible = true;
            PlaySound("Invincible");
            rend.material.color = Color.yellow;
            rendL.material.color = Color.yellow;
            rendR.material.color = Color.yellow;
            Camera.main.GetComponent<AudioSource>().Pause();
        }
        else if(Input.GetKeyDown(KeyCode.I) && numMasks < 3 && invincible)
        {
            invincible = false;
            rend.material.color = origColor;
            rendL.material.color = origColor;
            rendR.material.color = origColor;
            crashSound.Stop();
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
    void FixedUpdate()
    {
        Physics.Raycast(transform.position, Vector3.left, out sceneLeft, maxWallDist, wallLayerMask);
        Physics.Raycast(transform.position, Vector3.right, out sceneRight, maxWallDist, wallLayerMask);
        lastSceneCenter = sceneCenter;
        sceneCenter = Vector3.zero;
        sceneCenter = (sceneLeft.point + sceneRight.point) / 2f;
        if (!set2D)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out sceneFloor, maxGroundDist, groundLayerMask | fakeGroundLayerMask))
            {
                sceneCenter.y = sceneFloor.point.y;
            }
        }
        set2D = Physics.Raycast(collider.bounds.max, Vector3.forward, max2DDist, groundLayerMask);
        pos2D = Crash.S.transform.position.z - 7f;
        //Spinning

        if (spinning)
        {
            collider.size = new Vector3(1.5f, 1f, 1.5f);
            if (((jumping || falling) && Time.time - spinStartTime > airSpinDuration) || Time.time - spinStartTime > spinDuration)
            {
                collider.size = new Vector3(1f, 1f, 1f);
                spinning = false;
                transform.rotation = prespinRotation;
                spinEnd = Time.time;
            }
        }

        // Set the x and z values of new velocity
        vel = Vector3.zero;
        vel.z += iV * speed;
        vel.x += iH * speed;

        if (spinning)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.fixedDeltaTime);
        }
        else if (GetArrowInput() && vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(vel);
        }
        falling = rigid.velocity.y < -.01f;
        grounded = (grounded && !jumping) || OnGround();
        if (jumpCont)
        {
            vel.y = jumpVel;
        }
        else
        {
            if (grounded)
            {
                jumping = false;
            }
            vel.y = rigid.velocity.y;
        }
        // Apply our new velocity
        rigid.velocity = vel;
        if (jumping && falling)
        {
            origin = transform.position;
            origin.y = collider.bounds.min.y;
            Physics.Raycast(origin, Vector3.down, out hits[0], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * Vector3.left, Vector3.down, out hits[1], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * Vector3.right, Vector3.down, out hits[2], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * Vector3.forward, Vector3.down, out hits[3], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * Vector3.back, Vector3.down, out hits[4], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * (Vector3.back + Vector3.left),  Vector3.down, out hits[5], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * (Vector3.back + Vector3.right), Vector3.down, out hits[6], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * (Vector3.forward + Vector3.left), Vector3.down, out hits[7], distToGround, crateLayerMask);
            Physics.Raycast(origin + groundedOffset * (Vector3.forward + Vector3.right), Vector3.down, out hits[8], distToGround, crateLayerMask);
            Dictionary<Collider, int> colliderMap = new Dictionary<Collider, int>();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    if (colliderMap.ContainsKey(hit.collider))
                    {
                        ++colliderMap[hit.collider];
                    }
                    else
                    {
                        colliderMap.Add(hit.collider, 0);
                    }
                }
            }
            int maxHits = 0;
            foreach (KeyValuePair<Collider, int> entry in colliderMap)
            {
                if (entry.Value > maxHits)
                {
                    toBreak = entry.Key;
                    maxHits = entry.Value;
                }
            }
        }
    }

    public void LandOnCrate()
    {
        grounded = true;
        jumping = false;
    }

    public void Bounce(float bounceVel)
    {
        Vector3 vel = rigid.velocity;
        vel.y = bounceVel;
        rigid.velocity = vel;
        PlaySound("CrateBounce");
    }

    bool OnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * Vector3.left, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * Vector3.right, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * Vector3.forward, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * Vector3.back, Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * (Vector3.back + Vector3.left), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * (Vector3.back + Vector3.right), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * (Vector3.forward + Vector3.left), Vector3.down, distToGround, groundLayerMask)
            || Physics.Raycast(transform.position + groundedOffset * (Vector3.forward + Vector3.right), Vector3.down, distToGround, groundLayerMask);
    }

    // Returns true if an arrow key is being pressed
    bool GetArrowInput()
    {
        return Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow)
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.DownArrow);
    }

    public void Respawn()
    {
        PlaySound("Woah");
        PlayerPrefs.SetInt("Fruits", Display.S.numFruit);
        PlayerPrefs.SetInt("Lives", Display.S.numLives);
        StartCoroutine(Restart());
    }

    public void PlaySound(string soundName)
    {
        crashSound.PlayOneShot(Resources.Load("Sounds/" + soundName) as AudioClip);
    }
    public void KnockBack()
    {
        PlaySound("Woah");
        Vector3 knockVel = rigid.velocity;
        knockVel.y = jumpVel;
        rigid.velocity = knockVel;
    }
    public IEnumerator Invincible()
    {
        Crash.S.PlaySound("AkuAkuInvincible");
        invincible = true;
        yield return new WaitForSeconds(20);
        invincible = false;
        Crash.S.LoseMask();
    }
    public void LoseMask()
    {
        if (numMasks == 1)
        {
            Destroy(transform.FindChild("AkuAkuMask").gameObject);
        }
        numMasks--;
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(.2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}