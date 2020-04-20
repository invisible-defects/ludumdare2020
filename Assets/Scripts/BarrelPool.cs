using UnityEngine;
using System.Collections;

public class BarrelPool : BaseObjectPool
{
    [SerializeField]
    float minDistance;
    [SerializeField]
    float deviation = 2f;
    [SerializeField]
    float doubleChance = 0.2f;
    [SerializeField]
    float doubleSplit = 0.5f;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.gameMode.OnChanged += this.GameModeOnChanged;
        GameManager.Instance.state.OnChanged += this.StateOnChanged;
    }

    private void GameModeOnChanged()
    {
        switch (GameManager.Instance.gameMode.Value)
        {
            case GameManager.GameMode.Barrels:
                this.shouldGenerate = true;
                break;
            case GameManager.GameMode.Drones:
                this.shouldGenerate = false;
                break;
        }
    }

    private void StateOnChanged()
    {
        if (GameManager.Instance.state.Value == GameManager.State.MainMenu)
        {
            // Reset
            this.shouldGenerate = true;
            CleanUp();
        }
    }

    protected override void Generate()
    {
        int idx = activeObjects.Count - 1;
        float distance = idx >= 0 ?
            transform.position.x + generateDistance - activeObjects[activeObjects.Count - 1].transform.position.x :
            Mathf.Infinity;
        if (distance > minDistance)
        {
            Vector3 position = transform.position;
            position.x += generateDistance + Random.Range(-deviation, deviation);
            if (Random.Range(0f, 1f) < doubleChance)
            {
                var left  = position - Vector3.right * doubleSplit * 0.5f;
                var right = position + Vector3.right * doubleSplit * 0.5f;
                GetRandom(left);
                GetRandom(right);
            }
            else
            {
                GetRandom(position);
            }
        }
    }

    private void CleanUp()
    {
        while (activeObjects.Count > 0)
        {
            Remove(activeObjects.Count - 1);
        }
    }
}
