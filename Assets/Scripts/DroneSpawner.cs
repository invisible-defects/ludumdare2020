using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : Singleton<DroneSpawner>
{
    [SerializeField]
    private GameObject dronePrefab;

    private BoxCollider box;
    [SerializeField]
    private BoxCollider targetBox;

    [SerializeField]
    private int deviation = 1;
    [SerializeField]
    private int maxWave = 20;

    [SerializeField]
    private float delay = 10f;

    private float? startTime = null;

    private int toSpawn = 0;

    private int dronesCount = 0;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
        GameManager.Instance.gameMode.OnChanged += this.GameModeOnChanged;
    }

    private void Update()
    {
        if (startTime.HasValue && Time.time >= startTime.Value)
        {
            CheckSpawn();
            startTime = null;
        }
    }

    private void GameModeOnChanged()
    {
        if (GameManager.Instance.gameMode.Value == GameManager.GameMode.Drones)
        {
            startTime = Time.time + delay;
            toSpawn = GameManager.Instance.toSpawn;
        }
    }

    private void CheckSpawn()
    {
        if (toSpawn > 0)
        {
            var willSpawn = Mathf.Min(toSpawn, maxWave);
            Spawn(willSpawn);
            toSpawn -= willSpawn;
        }
        else
        {
            GameManager.Instance.SetGameMode(GameManager.GameMode.Barrels);
        }
    }

    private void Spawn(int count)
    {
        var spawnCount = Random.Range(count - deviation, count + deviation + 1);
        for (int i = 0; i < count; i++)
        {
            var spawnPoint = box.RandomPoint(transform.position.z);
            var droneGo = Instantiate(dronePrefab, spawnPoint, Quaternion.identity);
            var drone = droneGo.GetComponent<Drone>();
            Vector2 targetPoint = targetBox.RandomPoint();
            drone.hoverTarget = targetPoint;
        }
    }

    public void RegisterDrone()
    {
        ++dronesCount;
    }

    public void UnRegisterDrone()
    {
        --dronesCount;

        if (dronesCount == 0)
        {
            CheckSpawn();
        }
    }
}
