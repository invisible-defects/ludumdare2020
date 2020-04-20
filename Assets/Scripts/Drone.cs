using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SpriteSheetController))]
public class Drone : MonoBehaviour
{
    public enum State
    {
        Appear,
        Attack,
        Death,
        Flee
    }

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

    [SerializeField]
    private float countdownThreshold = 0.3f;
    [SerializeField]
    private float countdownTime = 3f;
    [SerializeField]
    private float countdownDeviation = 1f;

    private PlayerController player;

    private Rigidbody rb;

    private SpriteSheetController ssc;

    private State state = State.Appear;

    public Vector2 hoverTarget;

    private float? countdownEnd = null;

    private Vector2 spawnPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        ssc = GetComponent<SpriteSheetController>();
        ssc.OnAnimationEnd += this.OnAnimationEnd;
        spawnPoint = transform.position;
        GameManager.Instance.RegisterDrone();
    }

    private void Update()
    {
        if (state != State.Death)
        {
            Vector2 target = hoverTarget;
            float currentAcc = acceleration;
            switch (state)
            {
                case State.Appear:
                    target = hoverTarget;
                    break;
                case State.Attack:
                    target = player.transform.position;
                    currentAcc = 500;
                    break;
                case State.Flee:
                    target = spawnPoint;
                    break;
                default:
                    break;
            }

            UpdatePosition(target, currentAcc);
            UpdateTilt();
        }
        if (state == State.Appear)
        {
            UpdateSpotlight();
            CheckForAttack();
        }
    }

    private void UpdatePosition(Vector2 target, float currentAcc)
    {
        var direction = (target - (Vector2)transform.position).normalized;
        var velocity = new Vector2(
            Mathf.Lerp(rb.velocity.x, direction.x * maxSpeed, Mathf.Clamp01(currentAcc * Time.deltaTime)),
            Mathf.Lerp(rb.velocity.y, direction.y * maxSpeed, Mathf.Clamp01(currentAcc * Time.deltaTime * 3))
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
        Vector2 direction = player.transform.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x);
        var deg = angle * Mathf.Rad2Deg + 90;
        spotlight.rotation = Quaternion.Slerp(spotlight.rotation, Quaternion.Euler(0, 0, deg), spotlightSpeed * Time.deltaTime);
    }

    private void CheckForAttack()
    {
        if (player.State == PlayerController.PlayerState.Dead)
        {
            spotlight.gameObject.SetActive(false);
            state = State.Flee;
        }
        if (countdownEnd.HasValue)
        {
            if (countdownEnd.Value >= Time.time)
            {
                spotlight.gameObject.SetActive(false);
                state = State.Attack;
            }
        }
        else
        {
            var distance = ((Vector2)transform.position - hoverTarget).magnitude;
            if (distance < countdownThreshold)
            {
                countdownEnd = Time.time + countdownTime + Random.Range(-countdownDeviation, countdownDeviation);
            }
        }
    }

    private void Death()
    {
        spotlight.gameObject.SetActive(false);
        state = State.Death;
        ssc.Play("Explosion");
        rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Death)
        {
            Death();
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player.Death();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != State.Death)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                Death();
            }
        }
    }

    private void OnAnimationEnd(string name)
    {
        if (name == "Explosion")
        {
            GameManager.Instance.UnRegisterDrone();
            Destroy(gameObject);
        }
    }
}
