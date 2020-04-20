using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject dronePrefab;

    private BoxCollider box;
    [SerializeField]
    private BoxCollider targetBox;

    [SerializeField]
    private int minSpawn = 1;
    [SerializeField]
    private int maxSpawn = 3;

    [SerializeField]
    private float delay = 10f;

    private float? startTime = null;

    private void Start()
    {
        box = GetComponent<BoxCollider>();
        GameManager.Instance.gameMode.OnChanged += this.GameModeOnChanged;
    }

    private void Update()
    {
        if (startTime.HasValue && Time.time >= startTime.Value)
        {
            Spawn();
            startTime = null;
        }
    }

    private void GameModeOnChanged()
    {
        if (GameManager.Instance.gameMode.Value == GameManager.GameMode.Drones)
        {
            startTime = Time.time + delay;
        }
    }

    private void Spawn()
    {
        var count = Random.Range(minSpawn, maxSpawn + 1);
        for (int i = 0; i < count; i++)
        {
            var spawnPoint = box.RandomPoint(transform.position.z);
            var droneGo = Instantiate(dronePrefab, spawnPoint, Quaternion.identity);
            var drone = droneGo.GetComponent<Drone>();
            Vector2 targetPoint = targetBox.RandomPoint();
            drone.hoverTarget = targetPoint;
        }
    }
}
