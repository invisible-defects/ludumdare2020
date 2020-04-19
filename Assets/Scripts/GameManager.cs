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

    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);

    public float distance = 0f;

    private void Start()
    {
        HighScore = PlayerPrefs.GetFloat("HighScore", 0f);
    }

    private void Update()
    {
        if (state.Value == State.Playing)
        {
            distance += SpeedManager.Instance.Speed * Time.deltaTime;
            score.Value = Mathf.FloorToInt(distance * 10);
        }
    }
}
