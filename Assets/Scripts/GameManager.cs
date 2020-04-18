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

    public State state = State.MainMenu;

    public delegate void StateChange(State state);
    public event StateChange OnStateChange;

    void Start()
    {
        HighScore = PlayerPrefs.GetFloat("HighScore", 0f);
    }

    public void Play()
    {
        if(state == State.MainMenu)
        {
            OnStateChange?.Invoke(State.Playing);
            state = State.Playing;
        }
    }
}
