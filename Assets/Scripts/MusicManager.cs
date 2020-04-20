using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource mainSource;

    [SerializeField]
    private AudioClip intro;
    [SerializeField]
    private AudioClip playStart;
    [SerializeField]
    private AudioClip playMain;

    bool waitingForMain = false;
    float mainDelay = 0;

    void Start()
    {
        GameManager.Instance.state.OnChanged += this.OnStateChange;
        mainSource.clip = intro;
        mainSource.loop = true;
    }

    void OnStateChange()
    {
        switch(GameManager.Instance.state.Value)
        {
            case GameManager.State.Playing:
                mainSource.clip = playStart;
                mainSource.loop = false;
                mainSource.Play();

                waitingForMain = true;
                mainDelay = playStart.length;

                break;
            case GameManager.State.GameOver:
                mainSource.clip = intro;
                mainSource.loop = true;
                mainSource.Play();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(!waitingForMain)
            return;
        
        mainDelay -= Time.deltaTime;
        if(mainDelay < 0)
        {
            waitingForMain = false;
            mainDelay = 0;

            mainSource.clip = playMain;
            mainSource.loop = true;
            mainSource.Play();
        }
    }
}
