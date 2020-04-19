using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : Singleton<SpeedManager>
{
    public float SpeedMultiplier { get; private set; } = 1f;

    public float FrameMultiplier
    {
        get
        {
            return SpeedMultiplier > 0 ? 1 / SpeedMultiplier : Mathf.Infinity;
        }
    }

    public float Speed
    {
        get
        {
            return SpeedMultiplier * playerSpeed;
        }
    }

    [SerializeField]
    private float acceleration = 0.1f;

    [SerializeField]
    private float deathAnimLength = 1.25f;

    [SerializeField]
    private float playerSpeed = 2f;

    private float decelerationStart;
    private float decelerationStartSpeed;

    private void Start()
    {
        GameManager.Instance.state.OnChanged += OnStateChange;
    }

    private void Update()
    {
        if (GameManager.Instance.state.Value == GameManager.State.Playing)
        {
            SpeedMultiplier += acceleration * Time.deltaTime;
        }
        else if (GameManager.Instance.state.Value == GameManager.State.GameOver)
        {
            var t = Mathf.Clamp01((Time.time - decelerationStart) / deathAnimLength);
            SpeedMultiplier = Mathf.Lerp(decelerationStartSpeed, 0, t);
        }
        else
        {
            if (SpeedMultiplier != 1)
                SpeedMultiplier = 1;
        }
    }

    private void OnStateChange()
    {
        if (GameManager.Instance.state.Value == GameManager.State.GameOver)
        {
            decelerationStart = Time.time;
            decelerationStartSpeed = SpeedMultiplier;
        }
    }
}
