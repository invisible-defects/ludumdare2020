using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static Vector3 RandomPoint(this BoxCollider box, float z = 0)
    {
        var delta = new Vector2(
                Random.Range(-box.bounds.extents.x, box.bounds.extents.x),
                Random.Range(-box.bounds.extents.y, box.bounds.extents.y)
            );

        var result = (Vector2)box.bounds.center + delta;

        return new Vector3(result.x, result.y, z);
    }
}
