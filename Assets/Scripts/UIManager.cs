using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Neat;
using static Neat.DSL;

public class UIManager : UIBehaviour
{
    [SerializeField]
    private Transform root;
    protected override Transform GetRoot() => root;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.state.OnChanged += OnUpdate;
    }

    private void OnUpdate()
    {
        this.ReDraw();
    }

    protected override UINode Render()
    {
        return Draw("Root",
            DrawMenu()
        );
    }

    private UINode DrawMenu()
    {
        if (GameManager.Instance.state.Value != GameManager.State.MainMenu)
        {
            return null;
        }

        return Draw("MainMenu",
                Draw("Column",
                    DrawLeaf("Title"),
                    Button.Draw("Start",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.Playing)
                    ),
                    Button.Draw("Credits")
                ),
                DrawLeaf("LD")
            );
    }
}
