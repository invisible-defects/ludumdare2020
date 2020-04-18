using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteAnimation", menuName = "SpriteAnimation")]
public class SpriteAnimation : ScriptableObject
{
    [SerializeField]
    private Sprite[] frames;
    public float interval;
    public bool loop;

    public int FrameCount
    {
        get
        {
            return frames.Length;
        }
    }

    public Sprite this[int i]
    {
        get
        {
            return frames[i];
        }
    }
}
