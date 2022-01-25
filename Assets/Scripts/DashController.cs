using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float dashSpeed;
    private float startDashTime;
    public float dashTime;
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = 2;
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = 4;
            }
        }

        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if(direction == 1)
                {
                    rigidBody.velocity = Vector3.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rigidBody.velocity = Vector3.right * dashSpeed;
                }
                else if (direction == 3)
                {
                    rigidBody.velocity = Vector3.up * dashSpeed;
                }
                else if (direction==4) 
                {
                    rigidBody.velocity = Vector3.down * dashSpeed;
                }
            }
        }
    }
}
