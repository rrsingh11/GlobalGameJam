using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float enableTime;
    [SerializeField, Range(0, 50)] float dashVelocity;

    private PlayerController playerController;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            rb.velocity += new Vector3(playerController.moveHorizontal * dashVelocity, rb.velocity.y);
            playerController.enabled = false;
            StartCoroutine(EnableScript());
        }
        
        IEnumerator EnableScript()
        {
            yield return new WaitForSeconds(enableTime);
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerController.enabled = true;
            
        }
    }
}
