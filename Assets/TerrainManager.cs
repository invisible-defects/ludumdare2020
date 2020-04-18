using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform terrain1;
    public Transform terrain2;
    public SpeedManager speedManager;

    void Update()
    {
        moveTerrain(terrain1);
        moveTerrain(terrain2);
    }

    void moveTerrain(Transform t)
    {
        var rot = t.rotation;
        var pos = t.position;
        pos.x -= speedManager.Speed * Time.deltaTime;
        if(pos.x < -21.5f)
        {
            pos.x = 21.5f;
        }
        t.SetPositionAndRotation(pos, rot);
    }
}
