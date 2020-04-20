using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField]
    private Transform spotlight;
    [SerializeField]
    private float spotlightSpeed = 1f;
    [SerializeField]
    private float upComponent = 1f;

    [SerializeField]
    private float maxSpeed = 1f;
    [SerializeField]
    private float acceleration = 0.1f;

    private Transform player;

    private Rigidbody rb;

    public Vector2 hoverTarget;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateTilt();
        UpdateSpotlight();
    }

    private void UpdatePosition()
    {
        var direction = (hoverTarget - (Vector2)transform.position).normalized;
        var velocity = new Vector2(
            Mathf.Lerp(rb.velocity.x, direction.x * maxSpeed, acceleration * Time.deltaTime),
            Mathf.Lerp(rb.velocity.y, direction.y * maxSpeed, acceleration * Time.deltaTime * 3)
            );
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        rb.velocity = velocity;
        Debug.DrawRay(transform.position, rb.velocity, Color.cyan);
    }

    private void UpdateTilt()
    {
        var force = (Vector2)rb.velocity + Vector2.up * upComponent;
        force = new Vector2(force.x, Mathf.Abs(force.y));
        var angle = Mathf.Atan2(force.y, force.x);
        var deg = angle * Mathf.Rad2Deg - 90;
        transform.localRotation = Quaternion.Euler(0, 0, deg);
        Debug.DrawRay(transform.position, transform.up, Color.yellow);
    }

    private void UpdateSpotlight()
    {
        Vector2 direction = player.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x);
        var deg = angle * Mathf.Rad2Deg + 90;
        spotlight.rotation = Quaternion.Slerp(spotlight.rotation, Quaternion.Euler(0, 0, deg), spotlightSpeed * Time.deltaTime);
    }
}
