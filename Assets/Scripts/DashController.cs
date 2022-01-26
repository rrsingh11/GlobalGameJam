using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float enableTime;
    [SerializeField, Range(0, 50)] float dashVelocity;

    private Rigidbody rb;
    private PlayerController playerController;

    public bool canDash;
 
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && playerController.moveHorizontal != 0 && canDash)
        {
            rb.velocity += new Vector3(playerController.moveHorizontal * dashVelocity, 0f);
            playerController.enabled = false;
            StartCoroutine(EnableScript());
        }
        
        IEnumerator EnableScript()
        {
            canDash = false;
            yield return new WaitForSeconds(enableTime);
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerController.enabled = true;
            canDash = true;
        }
    }
}
