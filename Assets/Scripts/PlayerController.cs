using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0,10)] float jumpForce;
    [SerializeField, Range(0, 10)] float airSpeed;
    [SerializeField, Range(0, 10)] float groundSpeed;

    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float moveHorizontal;
    public bool onGround;
    public float rotationSpeed;
    public float dashVelocity;

    public Transform rotationReference;
    
    public LayerMask WhitePlatformLayerMask;

    //public Animator anim;
    
    RaycastHit hit;

    private float speed;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        if (IsGrounded())
        {
            speed = groundSpeed;
            //anim.SetBool("onGround", true);
        }
        else
        {
            speed = airSpeed;
            //anim.SetBool("onGround", false);
        }
        //anim.SetFloat("Horizontal", Mathf.Abs(moveHorizontal));
        //anim.SetFloat("Vertical", rb.velocity.y);
        Quaternion deltaRotation = Quaternion.Lerp(transform.rotation, rotationReference.rotation, rotationSpeed);

        if (Input.GetKeyDown(KeyCode.X))
        {
            rb.velocity += new Vector3(moveHorizontal * dashVelocity, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            rotationReference.Rotate(0, 0, -90f);
        }
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationReference.rotation, rotationSpeed);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveHorizontal) > 0.1f && IsGrounded())
        {
            rb.velocity = new Vector2(moveHorizontal * speed,rb.velocity.y);
        }
        else if (Mathf.Abs(moveHorizontal) > 0.1f)
        {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }


        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public bool IsGrounded()
    {
        Ray whiteRay1 = new Ray(transform.position, -transform.up);
        Ray blackRay1 = new Ray(transform.position, transform.up);
        Ray whiteRay2 = new Ray(transform.position, transform.right);
        Ray blackRay2 = new Ray(transform.position, -transform.right);
        if (Physics.Raycast(whiteRay1, out hit, .6f) || Physics.Raycast(whiteRay2, out hit, .6f))
        {
            if (hit.collider.CompareTag("White"))
            {
                onGround = true;
            }
        }
        else if (Physics.Raycast(blackRay1, out hit, .6f) || Physics.Raycast(blackRay2, out hit, .6f))
        {
            if (hit.collider.CompareTag("Black"))
            {
                onGround = true;
            }
        }
        else
            onGround = false;
        return onGround;
    }

    private void OnDrawGizmos()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boxCollider.bounds.center - new Vector3(0, .05f, 0), boxCollider.bounds.size + new Vector3(0, .1f, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * .6f);
        Gizmos.DrawRay(transform.position, transform.right * .6f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.up * .6f);
        Gizmos.DrawRay(transform.position, -transform.right * .6f);
    }
}
