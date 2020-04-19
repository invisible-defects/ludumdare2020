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
            return 1 / SpeedMultiplier;
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
    private float playerSpeed = 2f;

    private void Update()
    {
        if (GameManager.Instance.state.Value == GameManager.State.Playing)
        {
            SpeedMultiplier += acceleration * Time.deltaTime;
        }
        else
        {
            if (SpeedMultiplier != 1)
                SpeedMultiplier = 1;
        }
    }
}
