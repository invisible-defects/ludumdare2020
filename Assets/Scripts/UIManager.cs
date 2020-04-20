using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Neat;
using static Neat.DSL;
using TMPro;

public class UIManager : UIBehaviour
{
    [SerializeField]
    private Transform root;
    protected override Transform GetRoot() => root;

    [SerializeField]
    private PlayerController player;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.state.OnChanged += OnUpdate;
        GameManager.Instance.score.OnChanged += OnUpdate;
        player.cooldown.OnChanged += OnUpdate;
    }

    private void OnUpdate()
    {
        this.ReDraw();
    }

    protected override UINode Render()
    {
        var state = GameManager.Instance.state.Value;
        return Draw("Root",
            Do(() =>
            {
                switch (state)
                {
                    case GameManager.State.MainMenu:
                        return DrawMenu();
                    case GameManager.State.Playing:
                    case GameManager.State.GameOver:
                        return DrawGameUI();
                    case GameManager.State.Tutorial:
                        return DrawTutorial();
                    default:
                        return null;
                }
            })
        );
    }

    private UINode DrawMenu()
    {
        return Draw("MainMenu",
                Draw("Column",
                    DrawLeaf("Title"),
                    MenuButton.Draw("Start",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.Tutorial)
                    ),
                    MenuButton.Draw("Credits")
                ),
                DrawLeaf("LD"),
                DrawLeaf("BG")
            );
    }

    private UINode DrawGameUI()
    {
        return Draw("GameUI",
                DrawLeaf("Text (TMP)",
                    Set<TMP_Text>(t => t.text = "SCORE: " + GameManager.Instance.score.Value)
                ),
                DrawLeaf("Cooldown",
                    Set<Image>(i => i.fillAmount = player.cooldown.Value)
                )
            );
    }

    private UINode DrawTutorial()
    {
        return Draw("Tutorial",
                Draw("Column",
                    DrawLeaf("Title"),
                    DrawLeaf("Tutorial"),
                    MenuButton.Draw("Ok",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.Playing)
                    )
                ),
                DrawLeaf("BG")
            );
    }
}
