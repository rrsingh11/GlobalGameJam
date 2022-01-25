using System.Collections;
using System.Collections.Generic;
using Buttons;
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
    public float flipDistance;
    public float rotationSpeed;

    public Transform rotationReference;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    Vector3 m_EulerAngleVelocity;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        m_EulerAngleVelocity = new Vector3(0, 0, -90f);
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
        Quaternion deltaRotation = Quaternion.Lerp(transform.rotation, rotationReference.rotation, rotationSpeed);

        if (Input.GetKeyDown(KeyCode.C))
        {
            rotationReference.Rotate(0, 0, -180f);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rotationReference.rotation, rotationSpeed);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxVelocity && onGround)
        {
            //rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode.Impulse);
            rb.velocity = new Vector2(moveHorizontal * speed,rb.velocity.y);
        }
        else if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxAirVelocity)
        {
            //rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode.Impulse);
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
            //rb.MovePosition(transform.forward * speed);
            //rb.velocity = new Vector2(speed * moveHorizontal, 0f);
        }


        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);
        }

        else if (rb.velocity.x > 0 && moveHorizontal == 0 && onGround)
            rb.velocity = new Vector2(0f,0f);
        else if (rb.velocity.x < -0 && moveHorizontal == 0 && onGround)
            rb.velocity = new Vector2(0f, 0f);
    }

    public bool IsGrounded()
    {
        var whiteRay1 = new Ray(transform.position, -transform.up);
        var blackRay1 = new Ray(transform.position, transform.up);
        var whiteRay2 = new Ray(transform.position, transform.right);
        var blackRay2 = new Ray(transform.position, -transform.right);
        if (Physics.Raycast(whiteRay1, out hit, .6f) || Physics.Raycast(whiteRay2, out hit, .6f))
        {
            if (hit.collider.CompareTag("White") || hit.collider.CompareTag("White Button"))
            {
                onGround = true;
            }
            if (hit.collider.CompareTag("White Button"))
                hit.transform.GetComponent<ButtonController>().Trigger();
        }
        else if (Physics.Raycast(blackRay1, out hit, .6f) || Physics.Raycast(blackRay2, out hit, .6f))
        {
            if (hit.collider.CompareTag("Black") || hit.collider.CompareTag("Black Button"))
            {
                onGround = true;
            }
            if (hit.collider.CompareTag("Black Button"))
                hit.transform.GetComponent<ButtonController>().Trigger();
        }
        else
            onGround = false;
        //onGround = Physics.Raycast(transform.position,-transform.up, .6f,WhitePlatformLayerMask);
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