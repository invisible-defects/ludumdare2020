using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(SpriteSheetController))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Dead
    }

    public PlayerState State { get; private set; } = PlayerState.Idle;

    private Rigidbody rb;
    private SpriteSheetController ssc;
    private SoundManager sm;

    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float freeSpeed;

    [SerializeField]
    private float topThreshold;

    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float bulletSpeed = 5f;

    public ReactiveProperty<float> cooldown = new ReactiveProperty<float>(1);

    [SerializeField]
    private float cooldownSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ssc = GetComponent<SpriteSheetController>();
        sm = SoundManager.Instance;
        ssc.OnAnimationEnd += this.OnAnimationEnd;
        GameManager.Instance.state.OnChanged += this.OnStateChange;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (State == PlayerState.Running)
            {
                State = PlayerState.Jumping;
                ssc.Play("JumpStart");
                sm.playJump();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (State == PlayerState.Jumping)
            {
                rb.velocity = Vector2.up * (Mathf.Min(rb.velocity.y, freeSpeed));
            }
        }

        if (State == PlayerState.Jumping)
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

        if (State == PlayerState.Running || State == PlayerState.Jumping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Mathf.Approximately(cooldown.Value, 1))
                {
                    sm.playShot();
                    cooldown.Value = 0;
                    var spawnOnScreen = Camera.main.WorldToScreenPoint(bulletSpawn.position);
                    var direction = (Input.mousePosition - spawnOnScreen).normalized;
                    var angle = Mathf.Atan2(direction.y, direction.x);
                    var rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                    var newBullet = Instantiate(bullet, bulletSpawn.position, rotation);
                    var bulletRb = newBullet.GetComponent<Rigidbody>();
                    bulletRb.velocity = newBullet.transform.right * bulletSpeed;
                    Destroy(newBullet, 5);
                }
            }

            if (cooldown.Value < 1)
            {
                cooldown.Value = Mathf.Clamp01(cooldown.Value + cooldownSpeed * Time.deltaTime);
            }
        }
    }

    public void Death()
    {
        if (State == PlayerState.Dead)
            return;

        State = PlayerState.Dead;
        GameManager.Instance.state.Value = GameManager.State.GameOver;
        ssc.Play("Death");
        sm.playDeath();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default") && State == PlayerState.Jumping)
        {
            State = PlayerState.Running;
            ssc.Play("JumpEnd");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            Death();
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

    private void OnStateChange()
    {
        switch(GameManager.Instance.state.Value)
        {
            case GameManager.State.Playing:
                this.State = PlayerState.Running;
                ssc.Play("Run");
                break;
            case GameManager.State.MainMenu:
                //Reset
                this.State = PlayerState.Idle;
                ssc.Play("Idle");
                cooldown.Value = 1;
                break;
            default:
                break;
        }
    }
}
