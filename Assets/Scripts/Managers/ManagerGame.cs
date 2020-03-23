using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : MonoBehaviour{

    public int level;
    public float timeLoadNextLevel { get; private set; }
    public float currentTimeLoadNextLevel { get; private set; }

    public float score { get; private set; }
    public static ManagerGame instance;
    private bool _isWinState = false;
    private Player _player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        ResetValues();
        _player = FindObjectOfType<Player>();
        currentTimeLoadNextLevel = timeLoadNextLevel;
        ManagerUpdate.instance.update += Execute;
    }

    private void Execute()
    {
        currentTimeLoadNextLevel -= Time.deltaTime;

        if (currentTimeLoadNextLevel < 0)
        {
            Win();
        }

        if (_player != null && _player.life <= 0)
        {
            GameOver();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isWinState)
        {
            Pause();
        }

        //--------------------Debuging.........!!!!
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Win();
        }
        //---------------------Debuging.........!!!!
    }

    public void AddPoints(int value)
    {
        score += value;
    }

    #region Dispacth events.

    private void Pause()
    {
        ManagerUpdate.instance.Pause(!ManagerUpdate.instance.GetPauseState);
        ManagerEvent.DispatchEvent(GameEvent.PAUSE);
    }

    private void Win()
    {
        ManagerUpdate.instance.Pause(true);
        _isWinState = true;
        ManagerEvent.DispatchEvent(GameEvent.PAUSE_WIN);
    }

    public void NextLevel()
    {
        _isWinState = false;
        ManagerUpdate.instance.Pause(false);
        level++;
        timeLoadNextLevel += 60;
        currentTimeLoadNextLevel = timeLoadNextLevel;
        AddPoints(100);
        StopAllCoroutines();
        ManagerAsteroids.instance.LoadNextValueTime();
        ManagerEvent.DispatchEvent(GameEvent.NEXT_LEVEL);
    }

    private void GameOver()
    {
        ManagerEvent.DispatchEvent(GameEvent.GAME_OVER);
        ManagerUpdate.instance.Pause(true);
        Destroy(_player.gameObject);
    }

    private void ResetValues()
    {
        level = 0;
        score = 0;
        timeLoadNextLevel = 60;
        ManagerAsteroids.instance.ResetValues();
    }

    #endregion Dispatch events.
}
