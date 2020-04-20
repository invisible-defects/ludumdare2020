using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int HighScore { get; private set; } = 0;

    public enum State 
    {
        MainMenu,
        Playing,
        GameOver,
        Credits,
        Tutorial
    }

    public enum GameMode
    {
        Barrels,
        Drones
    }

    public ReactiveProperty<State> state = new ReactiveProperty<State>(State.MainMenu);

    public ReactiveProperty<int> score = new ReactiveProperty<int>(0);

    [HideInInspector]
    public float distance = 0f;

    public ReactiveProperty<GameMode> gameMode = new ReactiveProperty<GameMode>(GameMode.Barrels);

    [SerializeField]
    private float barrelsLength = 20f;

    private float lastGameModeChange = 0;

    private int dronesCount = 0;

    private void Start()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        state.OnChanged += OnStateChanged;
    }

    private void Update()
    {
        if (state.Value == State.Playing)
        {
            distance += SpeedManager.Instance.Speed * Time.deltaTime;
            score.Value = Mathf.FloorToInt(distance * 10);

            if (gameMode.Value == GameMode.Barrels)
            {
                if (Time.time > lastGameModeChange + barrelsLength)
                {
                    lastGameModeChange = Time.time;
                    gameMode.Value = GameMode.Drones;
                }
            }
        }
    }

    public void RegisterDrone()
    {
        ++dronesCount;
    }

    public void UnRegisterDrone()
    {
        --dronesCount;

        if (dronesCount == 0)
        {
            lastGameModeChange = Time.time;
            gameMode.Value = GameMode.Barrels;
        }
    }

    private void OnStateChanged()
    {
        if(state.Value == State.MainMenu)
        {
            if(score.Value > HighScore)
            {
                HighScore = score.Value;
                PlayerPrefs.SetInt("HighScore", HighScore);
            }

            score.Value = 0;
            distance = 0;
        }
    }
}
