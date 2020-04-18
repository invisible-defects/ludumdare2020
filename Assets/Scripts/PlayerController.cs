using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Running,
    Jumping
}

[RequireComponent(typeof(Rigidbody), typeof(SpriteSheetController))]
public class PlayerController : MonoBehaviour
{
    private State state = State.Idle;
    private Rigidbody rb;
    private SpriteSheetController ssc;

    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float freeSpeed;

    [SerializeField]
    private float topThreshold;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ssc = GetComponent<SpriteSheetController>();
        ssc.OnAnimationEnd += this.OnAnimationEnd;
        GameManager.Instance.OnStateChange += this.OnStateChange;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == State.Running)
            {
                state = State.Jumping;
                ssc.Play("JumpStart");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (state == State.Jumping)
            {
                rb.velocity = Vector2.up * (Mathf.Min(rb.velocity.y, freeSpeed));
            }
        }

        if (state == State.Jumping)
        {
            if (ssc.Current == "JumpUp" && Mathf.Abs(rb.velocity.y) < topThreshold)
            {
                ssc.Play("JumpTop");
            }
            else if (ssc.Current == "JumpTop" && Mathf.Abs(rb.velocity.y) > topThreshold)
            {
                ssc.Play("JumpDown");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == State.Jumping)
        {
            state = State.Running;
            ssc.Play("JumpEnd");
        }
    }

    private void OnAnimationEnd(string name)
    {
        switch (name)
        {
            case "JumpStart":
                rb.velocity = Vector2.up * jumpSpeed;
                ssc.Play("JumpUp");
                break;
            case "JumpEnd":
                ssc.Play("Run");
                break;
            default:
                break;
        }
    }

    private void OnStateChange(GameManager.State state)
    {
        switch(state)
        {
            case GameManager.State.Playing:
                this.state = State.Running;
                ssc.Play("Run");
                break;
            case GameManager.State.MainMenu:
                this.state = State.Idle;
                ssc.Play("Idle");
                break;
            default:
                break;
        }
    }
}
