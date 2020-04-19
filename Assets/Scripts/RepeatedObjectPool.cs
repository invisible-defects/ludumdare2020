using UnityEngine;

public class RepeatedObjectPool : BaseObjectPool
{
    [SerializeField]
    float generateOffset;

    [SerializeField]
    bool random;

    protected override void Generate()
    {
        Vector3 lastActiveObjectPosition = activeObjects[activeObjects.Count - 1].transform.position;
        float distance = Mathf.Abs(transform.position.x - lastActiveObjectPosition.x);
        if (distance < generateDistance)
        {
            Vector3 position = lastActiveObjectPosition;
            position.x += generateOffset;
            if (random)
            {
                GetRandom(position);
            }
            else
            {
                Get(position);
            }
        }
    }
}
