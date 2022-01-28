using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    [SerializeField] private Transform rotationReference;
    [SerializeField] private Vector3 pushStrength;
    [SerializeField] private float speed;
    [SerializeField] private PlayerMeshGenerator playerMeshGenerator;
    [SerializeField] private float factor;
    
    private Rigidbody rb;
    private bool grounded;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
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
        var vel = rb.velocity;
        if (!Input.GetKey(KeyCode.D) || !grounded)
        {
            vel.x = 0f;
            return;
        }
        vel.x = speed * 2f;
        rb.velocity = vel;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
