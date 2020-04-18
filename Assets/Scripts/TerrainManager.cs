using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Transform terrain1;
    public Transform terrain2;

    void Update()
    {
        if(GameManager.Instance.state == GameManager.State.Playing)
        {
            MoveTerrain(terrain1);
            MoveTerrain(terrain2);
        }
    }

    void MoveTerrain(Transform t)
    {
        var position = t.position;
        position -= Vector3.right * SpeedManager.Instance.Speed * Time.deltaTime;
        if(position.x < -21.5f)
        {
            position.x += 43f;
        }
        t.position = position;
    }
}
