using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [SerializeField] private Transform rotationReference;
    [SerializeField] private Vector3 pushStrength;
    
    
    private Rigidbody rb;
    private bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        grounded = false;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.GetChild(0).rotation, rotationReference.rotation, 10f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.C) && grounded)
        {
            rotationReference.Rotate(0f, 0f, 180f);
            
            rb.AddForce(pushStrength);
        }
    }
}
