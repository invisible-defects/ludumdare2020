using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float HighScore { get; private set; } = 0f;

    public enum State 
    {
        MainMenu,
        Playing,
        GameOver,
        Credits
    }

    public ReactiveProperty<State> state = new ReactiveProperty<State>(State.MainMenu);

    void Start()
    {
        HighScore = PlayerPrefs.GetFloat("HighScore", 0f);
    }
}
