using UnityEngine;
using System.Collections;

public class RandomObjectPool : BaseObjectPool
{
    [SerializeField]
    float rarity;
    [SerializeField]
    float minDistance;

    protected override void Generate()
    {
        Vector3 lastActiveObjectPosition = activeObjects[activeObjects.Count - 1].transform.position;
        float distance = Mathf.Abs(transform.position.x - lastActiveObjectPosition.x);
        if (distance > minDistance && Random.Range(0f, 1f) <= rarity)
        {
            Vector3 position = transform.position;
            position.x += generateDistance;
            GetRandom(position);
        }
    }
}
