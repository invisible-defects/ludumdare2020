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
                        return DrawGameUI();
                    case GameManager.State.GameOver:
                        return DrawGameOver();
                    case GameManager.State.Tutorial:
                        return DrawTutorial();
                    case GameManager.State.Credits:
                        return DrawCredits();
                    default:
                        return null;
                }
            })
        );
    }

    private UINode DrawMenu()
    {
        var scoreString = "HIGHSCORE: " + GameManager.Instance.HighScore;

        return Draw("MainMenu",
                Draw("Column",
                    DrawLeaf("Title"),
                    MenuButton.Draw("Start",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.Tutorial)
                    ),
                    MenuButton.Draw("Credits",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.Credits)
                    )
                ),
                DrawLeaf("LD"),
                DrawLeaf("BG"),
                Draw("HighScore", Set<TMP_Text>(t => t.text = scoreString),
                    Draw("Red", Set<TMP_Text>(t => t.text = scoreString)),
                    Draw("Purple", Set<TMP_Text>(t => t.text = scoreString)),
                    Draw("Green", Set<TMP_Text>(t => t.text = scoreString))
                )
            );
    }

    private UINode DrawGameUI()
    {
        var scoreString = "SCORE: " + GameManager.Instance.score.Value;
        return Draw("GameUI",
                Draw("Score", Set<TMP_Text>(t => t.text = scoreString),
                    Draw("Red", Set<TMP_Text>(t => t.text = scoreString)),
                    Draw("Purple", Set<TMP_Text>(t => t.text = scoreString)),
                    Draw("Green", Set<TMP_Text>(t => t.text = scoreString))
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

    private UINode DrawGameOver()
    {
        var scoreString = GameManager.Instance.score.Value.ToString();

        return Draw("GameOver",
                Draw("Column",
                    DrawLeaf("Title"),
                    Draw("Score", Set<TMP_Text>(t => t.text = scoreString),
                        Draw("Red", Set<TMP_Text>(t => t.text = scoreString)),
                        Draw("Purple", Set<TMP_Text>(t => t.text = scoreString)),
                        Draw("Green", Set<TMP_Text>(t => t.text = scoreString))
                    ),
                    DrawLeaf("Text"),
                    MenuButton.Draw("Ok",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.MainMenu)
                    )
                ),
                DrawLeaf("BG")
            );
    }

    private UINode DrawCredits()
    {
        return Draw("Credits",
                Draw("Column",
                    DrawLeaf("Title"),
                    DrawLeaf("Creds"),
                    MenuButton.Draw("Ok",
                        OnClick(_ => GameManager.Instance.state.Value = GameManager.State.MainMenu)
                    )
                ),
                DrawLeaf("BG")
            );
    }
}
