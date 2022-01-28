using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0,10)] float jumpForce;
    [SerializeField, Range(0, 10)] float airSpeed;
    [SerializeField, Range(0, 10)] float groundSpeed;
    [SerializeField, Range(0, 10)] float wallThrust;
    [SerializeField, Range(0, 100)] float glitchStartTime;
    [SerializeField, Range(1, 2)] int rotationMultiplier = 1;

    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float moveHorizontal;
    public float moveVertical;
    public bool canStick;
    public bool onGround;
    public bool onRoof;
    public bool onWall;
    public bool glitch;
    public bool spacePressed;
    public float rotationSpeed;
    public float xVelocity;
    public float yVelocity;
    public float speed;

    public Transform rotationReference;
    Vector2 jumpDirection;

    //public Animator anim;
    
    RaycastHit hit;


    private Rigidbody rb;
    private BoxCollider boxCollider;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        if (glitch)
        {
            InvokeRepeating("Flip", glitchStartTime, Random.Range(5,10));
        }
    }

    void Update()
    {
        yVelocity = rb.velocity.y;
        xVelocity = rb.velocity.x;
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (onGround)
            jumpDirection = new Vector2(rb.velocity.x, jumpForce);
        else if (onRoof)
        {
            jumpDirection = new Vector2(rb.velocity.x, -jumpForce / 2);
            if (Mathf.Abs(rb.velocity.x) < 10 && Mathf.Abs(rb.velocity.x) > 0)
                rb.velocity = Vector2.zero;
        }
        else if (onWall)
        {
            if (Mathf.Abs(moveHorizontal) > 0.1)
            {
                jumpDirection = new Vector2(jumpForce * moveHorizontal, wallThrust * moveVertical);
            }
            if (!spacePressed || (moveHorizontal == 0 && spacePressed) || Mathf.Abs(rb.velocity.x) < 3)
                rb.velocity = Vector2.zero;
        }

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
    
        if (Input.GetKeyDown(KeyCode.C))
        {
            Flip();
            //rotationReference.Rotate(0, 0, -90f * rotationMultiplier);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationReference.rotation, rotationSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PressTime());
            spacePressed = true;
            if (IsGrounded() || onRoof || onWall)
                rb.velocity = jumpDirection;
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveHorizontal) > 0.1f && IsGrounded())
        {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }
        else if (Mathf.Abs(moveHorizontal) > 0.1f && (!onWall && !onRoof))
        {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }
        else if (moveHorizontal == 0f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //else if (Mathf.Abs(moveHorizontal) < 0.1 && (onWall))
        //    rb.velocity = Vector2.zero;
        //else if (Mathf.Abs(moveVertical) > 0.1 && (onRoof) && !spacePressed)
        //    rb.velocity = Vector2.zero;

        if (onRoof || onWall)
        {
            rb.useGravity = false;
        }
        else
            rb.useGravity = true;

    }
    
    IEnumerator PressTime()
    {
        yield return new WaitForSeconds(.2f);
        spacePressed = false;
    }

    public void Flip()
    {
        rotationReference.Rotate(0, 0, -90f * rotationMultiplier);
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
                onWall = false;
                onRoof = false;
            }
            else if (hit.collider.CompareTag("White Button"))
                onGround = true;
            else if (hit.collider.CompareTag("WhiteRoof"))
            {
                if (canStick)
                {
                    onRoof = true;
                    onGround = false;
                    onWall = false;
                }
            }
            else if (hit.collider.CompareTag("WhiteWall"))
            {
                if (canStick)
                {
                    onWall = true;
                    onRoof = false;
                    onGround = false;
                }
            }
        }
        else if (Physics.Raycast(blackRay1, out hit, .51f) || Physics.Raycast(blackRay2, out hit, .51f))
        {
            if (hit.collider.CompareTag("Black"))
            {
                onGround = true;
                onWall = false;
                onRoof = false;
            }
            else if (hit.collider.CompareTag("Black Button"))
                onGround = true;
            else if (hit.collider.CompareTag("BlackRoof"))
            {
                if (canStick)
                { 
                    onRoof = true;
                    onGround = false;
                    onWall = false;
                }
            }
            else if (hit.collider.CompareTag("BlackWall"))
            {
                if (canStick)
                {
                    onWall = true;
                    onRoof = false;
                    onGround = false;
                }
            }
        }
        else
        { 
            onGround = false;
            onRoof = false;
            onWall = false;
        }
        return onGround;
    }

    private void OnDrawGizmos()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * .6f);
        Gizmos.DrawRay(transform.position, transform.right * .6f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.up * .6f);
        Gizmos.DrawRay(transform.position, -transform.right * .6f);
    }
}