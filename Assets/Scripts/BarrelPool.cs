using UnityEngine;
using System.Collections;

public class BarrelPool : BaseObjectPool
{
    [SerializeField]
    float rarity;
    [SerializeField]
    float minDistance;

    protected override void Generate()
    {
        int idx = activeObjects.Count - 1;
        float distance = idx >= 0 ?
            transform.position.x + generateDistance - activeObjects[activeObjects.Count - 1].transform.position.x :
            Mathf.Infinity;
        if (distance > minDistance && Random.Range(0f, 1f) <= rarity)
        {
            Vector3 position = transform.position;
            position.x += generateDistance;
            GetRandom(position);
        }
    }
}
