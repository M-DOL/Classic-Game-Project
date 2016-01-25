using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Crash : MonoBehaviour {
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
	int groundLayerMask;
	public float spinStartTime;
	public static Crash S;

	void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
		originalPosition = S.transform.position;
		originalRotation = S.transform.rotation;

		rigid = gameObject.GetComponent<Rigidbody> ();
		collider = gameObject.GetComponent<BoxCollider> ();

		distToGround = gameObject.GetComponent<BoxCollider>().bounds.extents.y;
		groundedOffset = collider.size.x / 2f;

		groundLayerMask = LayerMask.GetMask ("Ground");
        crashSound = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        iH = Input.GetAxis("Horizontal");
        iV = Input.GetAxis("Vertical");
        spin = Input.GetKeyDown(KeyCode.S);
        jumpStart = Input.GetKeyDown(KeyCode.A);
        if (!jumping && jumpStart)
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
    }
    void FixedUpdate () {
        //Spinning
        if (spin && !spinning && Time.time - spinEnd > spinCooldown)
        {
            spin = false;
            prespinRotation = transform.rotation;
			spinning = true;
			spinStartTime = Time.time;
            PlaySound("Spin");
		}
		else if(spinning)
        {
            if(((jumping || falling) && Time.time - spinStartTime > airSpinDuration) || Time.time - spinStartTime > spinDuration)
            {
                spinning = false;
                transform.rotation = prespinRotation;
                spinEnd = Time.time;
            }
		}

		// Set the x and z values of new velocity
		vel = Vector3.zero;
		vel.z += iV * speed;
		vel.x += iH * speed;

		if(spinning)
        {
			transform.Rotate (Vector3.up, spinSpeed * Time.fixedDeltaTime);
		}
		else if(GetArrowInput() && vel != Vector3.zero)
        {
			transform.rotation = Quaternion.LookRotation(vel);
		}
        //-.01f because of floating number calculations.
		falling = rigid.velocity.y < 0f;
		grounded = (grounded && !jumping) || OnGround ();
		if (jumpCont)
        {
                vel.y = jumpVel;
		}
        else
        {
			if (grounded) {
				jumping = false;
			}
            vel.y = rigid.velocity.y;
        }
		// Apply our new velocity
		rigid.velocity = vel;
	}

	public void LandOnCrate(){
		grounded = true;
	}

	public void Bounce(float bounceVel){
		Vector3 vel = rigid.velocity;
		vel.y = bounceVel;
		rigid.velocity = vel;
        PlaySound("CrateBounce");
	}

	bool OnGround(){
		return Physics.Raycast (transform.position, Vector3.down, distToGround, groundLayerMask)
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
	bool GetArrowInput(){
		return Input.GetKey (KeyCode.LeftArrow)
			|| Input.GetKey (KeyCode.RightArrow)
			|| Input.GetKey (KeyCode.UpArrow)
			|| Input.GetKey (KeyCode.DownArrow);
	}

	public void Respawn(){
        ScreenFader.S.EndScene();
        if (checkpoint != Vector3.zero) {
            transform.position = checkpoint;
			transform.rotation = originalRotation;
		} else {
            transform.position = originalPosition;
			transform.rotation = originalRotation;
		}
        CameraFollow.S.RespawnItems();
        Vector3 cameraPos = transform.position;
        cameraPos.z -= CameraFollow.S.frontFollowDistance;
        CameraFollow.S.transform.position = cameraPos;
    }

    public void PlaySound(string soundName)
    {
        crashSound.PlayOneShot(Resources.Load("Sounds/" + soundName) as AudioClip);
    }
    public void KnockBack()
    {
        Vector3 knockVel = rigid.velocity;
        knockVel.y = jumpVel;
        rigid.velocity = knockVel; 
    }
	public IEnumerator Invincible(){
        Crash.S.PlaySound("AkuAkuInvincible");
		invincible = true;
		yield return new WaitForSeconds (20);
		invincible = false;
		AkuAkuMask.mask.LoseMask ();
	}
}