using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat;

using static Neat.DSL;

[RequireComponent(typeof(RectTransform))]
public class Button : MonoBehaviour
{
    [SerializeField]
    private Vector2 delta = new Vector2(10, 10);

    private bool hover = false;

    private RectTransform rt;
    private Vector2 defaultSize;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        defaultSize = rt.sizeDelta;
    }

    private void Update()
    {
        if (hover)
        {
            rt.sizeDelta = defaultSize + delta;
        }
        else
        {
            rt.sizeDelta = defaultSize;
        }
    }

    public void Enter()
    {
        hover = true;
    }

    public void Exit()
    {
        hover = false;
    }

    public static UINode Draw(string name, params Node[] children)
    {
        var modChildren = new List<Node>(children)
        {
            OnPointerEnter(rt => rt.GetComponent<Button>().Enter()),
            OnPointerExit(rt => rt.GetComponent<Button>().Exit())
        };

        return DrawLeaf(name, modChildren.ToArray());
    }
}
