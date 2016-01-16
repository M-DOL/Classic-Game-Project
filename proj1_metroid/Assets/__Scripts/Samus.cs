using UnityEngine;
using System.Collections;
public enum Facing
{
    R, L, RU, LU, U, D
}
public class Samus : MonoBehaviour {
    public float speedX = 4f;
    public float speedJump = 10f;
    public Sprite spRight, spUp;
    public GameObject bulletPrefab;
    public float speedBullet = 10f;
    public Transform bulletOrigin, bulletOriginUp;
    public bool _____________________;
    public Rigidbody rigid;
    public SpriteRenderer spRend;
    public bool grounded = false;
    public CapsuleCollider feet;
    public int groundPhysicsLayerMask;
    public Vector3 groundedCheckOffset;
    private Facing _face = Facing.R;
    protected RigidbodyConstraints noRotZ, noRotYZ;
    Quaternion turnLeft = Quaternion.Euler(0, 180, 0);
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
        //Get sprite
        spRend = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        feet = transform.Find("Feet").GetComponent<CapsuleCollider>();
        //Set groundPhysicsLayerMask
        groundPhysicsLayerMask = LayerMask.GetMask("Ground");
        groundedCheckOffset = new Vector3(feet.height * .4f, 0, 0);
        noRotZ = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        noRotYZ = RigidbodyConstraints.FreezePositionY | noRotZ;
    }
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Fire();
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 checkLoc = feet.transform.position + Vector3.up * (feet.radius * 0.9f);
        grounded = (Physics.Raycast(checkLoc, Vector3.down, feet.radius, groundPhysicsLayerMask)
            || Physics.Raycast(checkLoc + groundedCheckOffset, Vector3.down, feet.radius, groundPhysicsLayerMask)
            || Physics.Raycast(checkLoc - groundedCheckOffset, Vector3.down, feet.radius, groundPhysicsLayerMask));
        //If grounded, add constraints.
        rigid.constraints = grounded ? noRotYZ : noRotZ;
        Vector3 vel = rigid.velocity;
        if(Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            vel.x = -speedX;
            face = Facing.L;
        }
        else if(!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            vel.x = speedX;
            face = Facing.R;
        }
        else
        {
            vel.x = 0;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            face = Facing.U;
        }
        else
        {
            face = Facing.D;
        }
        if(Input.GetKeyDown(KeyCode.A) && grounded)
        {
            rigid.constraints = noRotZ;
            vel.y = speedJump;
        }
        rigid.velocity = vel;
	}
    void Fire()
    {
        GameObject go = Instantiate<GameObject>(bulletPrefab);
        if(face == Facing.R || face == Facing.L)
        {
            go.transform.position = bulletOrigin.position;
            go.GetComponent<Rigidbody>().velocity = bulletOrigin.right * speedBullet;
        }
        else
        {
            go.transform.position = bulletOriginUp.position;
            go.GetComponent<Rigidbody>().velocity = bulletOriginUp.up * speedBullet;
        }
    }
    public Facing face
    {
        get { return _face; }
        set {
            if(_face == value)
            {
                return;
            }
            switch(value)
            {
                case Facing.U:
                    if(_face == Facing.R || _face == Facing.RU)
                    {
                        _face = Facing.RU;
                    }
                    else
                    {
                        _face = Facing.LU;
                    }
                    break;
                case Facing.D:
                    if (_face == Facing.R || _face == Facing.RU)
                    {
                        _face = Facing.R;
                    }
                    else
                    {
                        _face = Facing.L;
                    }
                    break;
                default:
                    _face = value;
                    break;
            }
            if(_face == Facing.R || _face == Facing.L)
            {
                spRend.sprite = spRight;
            }
            else
            {
                spRend.sprite = spUp;
            }
            if(_face == Facing.R || _face == Facing.RU)
            {
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = turnLeft;
            }
        }
    }
}
