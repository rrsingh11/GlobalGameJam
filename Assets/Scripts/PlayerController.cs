using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0,10)] float jumpForce;
    [SerializeField, Range(0, 10)] float airSpeed;
    [SerializeField, Range(0, 10)] float groundSpeed;
    [SerializeField, Range(0, 20)] float maxVelocity;
    [SerializeField, Range(0, 10)] float maxAirVelocity;

    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool onGround;
    
    public LayerMask WhitePlatformLayerMask;

    //public Animator anim;
    RaycastHit hit;

    private float speed;
    private float moveVertical;
    public float moveHorizontal;
    
    private Rigidbody rb;
    private BoxCollider boxCollider;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

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
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxVelocity && IsGrounded())
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode.Impulse);
        }
        else if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxAirVelocity)
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode.Impulse);
            //rb.MovePosition(transform.forward * speed);
            //rb.velocity = new Vector2(speed * moveHorizontal, 0f);
        }


        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);
        }

        else if (rb.velocity.x > 0 && moveHorizontal == 0 && IsGrounded())
            rb.velocity = new Vector2(0f,0f);
        else if (rb.velocity.x < -0 && moveHorizontal == 0 && IsGrounded())
            rb.velocity = new Vector2(0f, 0f);
    }

    public bool IsGrounded()
    {
        //RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size * .5f, 0f, Vector2.down, 5f, platformLayerMask);
        //onGround = Physics.BoxCast(boxCollider.bounds.center, transform.localScale/2, -transform.up, out hit, transform.rotation, 10f, platformLayerMask);
        onGround = Physics.Raycast(transform.position,-transform.up, .6f,WhitePlatformLayerMask);
        //RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down * 5f, platformLayerMask);
        //Debug.Log(raycastHit2D.collider);
        //Debug.DrawRay(transform.position, Vector3.down*.6f,Color.red);
        //onGround = raycastHit2D.collider;
        return onGround;
    }

    private void OnDrawGizmos()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boxCollider.bounds.center - new Vector3(0, .05f, 0), boxCollider.bounds.size + new Vector3(0, .1f, 0));
    }
}
