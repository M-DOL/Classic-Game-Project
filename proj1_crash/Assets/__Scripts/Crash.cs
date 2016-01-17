using UnityEngine;
using System.Collections;

public class Crash : MonoBehaviour {

	public float speed = 10f;
	public float jumpVel = 5f;
	public float spinDuration;
	public float spinSpeed;

	public float jumpCount;
	public float maxJumpCount;

	public bool grounded = true;
	public bool jumping = false;
	public bool falling = false;
	public bool spinning = false;
	public new BoxCollider collider;

	public int numMasks;

	public Vector3 originalPosition;
	public Quaternion originalRotation;
	public Vector3 checkpoint = Vector3.zero;

	float iH, iV;
	Vector3 vel;
	float distToGround;
	float groundedOffset;
	Rigidbody rigid;
	int groundLayerMask;
	float spinStartTime;

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
	}

	// Update is called once per frame
	void FixedUpdate () {
		// Get movement input
		iH = Input.GetAxis("Horizontal");
		iV = Input.GetAxis ("Vertical");
		float jump = Input.GetAxis ("Jump");
		float spin = Input.GetAxis ("Fire1");

		if(!spinning && spin > 0){
			spinning = true;
			spinStartTime = Time.time;
		}

		if(Time.time - spinStartTime > spinDuration){
			spinning = false;
		}

		// Set the x and z values of new velocity
		vel = Vector3.zero;
		vel.z += iV * speed;
		vel.x += iH * speed;

		if(spinning){
			transform.Rotate (Vector3.up, spinSpeed * Time.fixedTime);
		}
		else if(GetArrowInput() && vel != Vector3.zero){
			transform.rotation = Quaternion.LookRotation (vel);
		}

		falling = rigid.velocity.y < 0;
		grounded = (grounded && !jumping) || OnGround ();

		if (jump > 0 && jumpCount < maxJumpCount) {
			jumpCount++;
			vel.y = jumpVel;
			jumping = true;
		} else {
			if (grounded) {
				jumping = false;
				jumpCount = 0;
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
		if (checkpoint != Vector3.zero) {
			transform.position = checkpoint;
			transform.rotation = originalRotation;
		} else {
			transform.position = originalPosition;
			transform.rotation = originalRotation;
		}
	}
}