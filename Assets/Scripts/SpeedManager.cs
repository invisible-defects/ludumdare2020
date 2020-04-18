using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : Singleton<SpeedManager>
{
    public float Speed { get; private set; } = 1f;

    [SerializeField]
    private float acceleration = 0.1f;

    private void Update()
    {
        Speed += acceleration * Time.deltaTime;
    }
}
