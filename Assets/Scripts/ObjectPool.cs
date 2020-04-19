using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    private List<GameObject> objects;

    public ObjectPool(params GameObject[] gameObjects)
    {
        objects = new List<GameObject>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].activeSelf)
                gameObjects[i].SetActive(false);
            gameObjects[i].transform.position = new Vector3(0, -100, 0);
            objects.Add(gameObjects[i]);
        }
    }

    public void Push(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(0, -100, 0);
        objects.Add(gameObject);
    }
    public GameObject Pop(bool setActive = true)
    {
        GameObject obj = objects[0];
        objects.RemoveAt(0);
        if (setActive)
            obj.SetActive(true);
        return obj;
    }
    public GameObject Pop(Vector3 position, bool setActive = true)
    {
        GameObject obj = objects[0];
        objects.RemoveAt(0);
        obj.transform.position = position;
        if (setActive)
            obj.SetActive(true);
        return obj;
    }
    public GameObject RandomPop(bool setActive = true)
    {
        int i = Random.Range(0, objects.Count);
        GameObject obj = objects[i];
        objects.RemoveAt(i);
        if (setActive)
            obj.SetActive(true);
        return obj;
    }
    public GameObject RandomPop(Vector3 position, bool setActive = true)
    {
        int i = Random.Range(0, objects.Count);
        GameObject obj = objects[i];
        objects.RemoveAt(i);
        obj.transform.position = position;
        if (setActive)
            obj.SetActive(true);
        return obj;
    }
}
