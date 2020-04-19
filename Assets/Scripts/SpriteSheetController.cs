using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSheetController : MonoBehaviour
{
    public List<SpriteAnimation> animations = new List<SpriteAnimation>();
    public string defaultAnimation;

    public delegate void AnimationEnd(string name);
    public event AnimationEnd OnAnimationEnd;

    int frame = 0;
    float frameStart = 0;
    private SpriteAnimation current = null;

    public string Current { get { return current?.name; } }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Play(defaultAnimation);
    }

    public void Play(string name)
    {
        current = animations.Find(animation => animation.name == name);
        frame = 0;
        spriteRenderer.sprite = current[frame];
        frameStart = Time.time;
    }

    void Update()
    {
        if (current.FrameCount > 1)
        {
            var multiplier = current.useMultiplier ? SpeedManager.Instance.FrameMultiplier : 1;
            if ((Time.time - frameStart) >= current.interval * multiplier &&
                frame < current.FrameCount)
            {
                ++frame;
                if (frame == current.FrameCount)
                {
                    if (current.loop)
                    {
                        frame = 0;
                    }
                    else
                    {
                        OnAnimationEnd?.Invoke(current.name);
                        return;
                    }
                }
                spriteRenderer.sprite = current[frame];
                frameStart = Time.time;
            }
        }
    }
}
