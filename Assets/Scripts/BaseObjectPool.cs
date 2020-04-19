using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseObjectPool : MonoBehaviour
{
    [SerializeField]
    float despawnDistance;
    [SerializeField]
    protected List<GameObject> activeObjects;
    [SerializeField]
    protected GameObject[] poolObjects;
    [SerializeField]
    protected float generateDistance;

    protected ObjectPool objectPool = new ObjectPool();

    void Start()
    {
        objectPool = new ObjectPool(poolObjects);
    }

    void Update()
    {
        if (GameManager.Instance.state.Value == GameManager.State.Playing ||
            GameManager.Instance.state.Value == GameManager.State.GameOver)
        {
            CheckDispawn();
            Generate();
            Move();
        }
    }

    protected void Move()
    {
        foreach (GameObject obj in activeObjects)
        {
            obj.transform.position -= Vector3.right * SpeedManager.Instance.Speed * Time.deltaTime;
        }
    }

    protected void CheckDispawn()
    {
        if (activeObjects.Count > 0)
        {
            float distance = Mathf.Abs(transform.position.x - activeObjects[0].transform.position.x);
            if (distance > despawnDistance)
            {
                Remove(0);
            }
        }
    }
    protected abstract void Generate();

    protected GameObject Get(Vector3 position, bool setActive = true)
    {
        GameObject obj = objectPool.Pop(position);
        activeObjects.Add(obj);
        return obj;
    }

    protected GameObject GetRandom(Vector3 position, bool setActive = true)
    {
        GameObject obj = objectPool.RandomPop(position);
        activeObjects.Add(obj);
        return obj;
    }

    protected void Remove(int idx)
    {
        GameObject obj = activeObjects[idx];
        activeObjects.RemoveAt(idx);
        objectPool.Push(obj);
    }
}
