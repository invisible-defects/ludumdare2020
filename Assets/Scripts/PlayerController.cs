using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Running,
    Jumping
}

[RequireComponent(typeof(Rigidbody), typeof(SpriteSheetController))]
public class PlayerController : MonoBehaviour
{
    private State state = State.Running;
    private Rigidbody rb;
    private SpriteSheetController ssc;

    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float freeSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ssc = GetComponent<SpriteSheetController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == State.Running)
            {
                state = State.Jumping;
                rb.velocity = Vector2.up * jumpSpeed;
                ssc.Play("JumpTop");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (state == State.Jumping)
            {
                rb.velocity = Vector2.up * (Mathf.Min(rb.velocity.y, freeSpeed));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == State.Jumping)
        {
            state = State.Running;
            ssc.Play("Run");
        }
    }
}
