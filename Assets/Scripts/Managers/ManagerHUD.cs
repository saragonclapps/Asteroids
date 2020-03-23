using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ManagerHUD : MonoBehaviour, IObserver {

    public GameObject menuPause;
    public GameObject menuWin;
    public GameObject menuGameOver;

    public Image[] damageImages;
    public Text numLevel;
    public Text numAsteroids;
    public Text numScore;
    public Text numTime;
    public Image life;
    public GameObject splash;
    public Text splashText;
    public Image splashImage;
    private Player _player;
    private ManagerGame _managerGame;
    private float _timeLeft;
    private Fading _fading;

    void Start () {
        _fading = FindObjectOfType<Fading>();
        _fading.BeginFade(-1);
        
        splash.SetActive(false);
        menuPause.SetActive(false);
        menuGameOver.SetActive(false);
        menuWin.SetActive(false);

        _player = FindObjectOfType<Player>();
        _player.Subscribe(OnNotify);//Suscribe to Notify damage player by Obserber.
        _timeLeft = ManagerGame.instance.currentTimeLoadNextLevel;

        ManagerEvent.AddEventListener(GameEvent.GAME_OVER, GameOver);
        ManagerEvent.AddEventListener(GameEvent.PAUSE, Pause);
        ManagerEvent.AddEventListener(GameEvent.PAUSE_WIN, Win);
        ManagerEvent.AddEventListener(GameEvent.NEXT_LEVEL, NextLevel);
        ManagerUpdate.instance.update += Execute;
    }

    public void UpdateTime(float time)
    {
        _timeLeft = time;
    }

    public void Execute()
    {
        numAsteroids.text = ManagerAsteroids.instance.GetTotalAsteroids.ToString();
        _timeLeft = ManagerGame.instance.currentTimeLoadNextLevel;
        numScore.text = ManagerGame.instance.score.ToString();

        var minutes = _timeLeft / 120;
        var seconds = _timeLeft % 60;//Use the euclidean division for the seconds.
        var fraction = (_timeLeft * 100) % 100;

        //update the label value
        numTime.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);

    }

    #region Events.
    private void Pause(object[] parameterContainer)
    {
        splash.SetActive(ManagerUpdate.instance.GetPauseState);
        menuPause.SetActive(ManagerUpdate.instance.GetPauseState);
        splashImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        splashText.text = "PAUSE";
    }


    private void GameOver(object[] parameterContainer)
    {
        splash.SetActive(true);
        menuGameOver.SetActive(true);
        splashImage.color = new Color(1, 0, 0, 0.5f);
        splashText.text = "GAME OVER";
    }

    private void Win(object[] parameterContainer)
    {
        splash.SetActive(true);
        menuWin.SetActive(true);
        splashImage.color = new Color(1, 0, 1, 0.5f);
        //Write UI.
        splashText.text = "YOU WON";
        numLevel.text = ManagerGame.instance.level.ToString();
    }

    private void NextLevel(object[] parameterContainer){
        splash.SetActive(false);
        menuWin.SetActive(false);
    }

    //Damage Notify and execute feedback.
    public void OnNotify()
    {
        life.fillAmount = _player.life / _player.maxLife;
        StartCoroutine(DamageImage());
        StartCoroutine(Cam25D.instance.ShakeTime());
    }

    private IEnumerator DamageImage()
    {
        float t = 0;
        while (t < 1)
        {
            t += 0.1f;
            foreach (var item in damageImages)
                item.color = new Color(1, 0, 0, Mathf.Lerp(0.2f, 0, t));
            yield return null;
        }
        while (t > 0)
        {
            t -= 0.1f;
            foreach (var item in damageImages)
                item.color = new Color(1, 0, 0, Mathf.Lerp(0, 0.2f, t));
            yield return null;
        }
    }

    #endregion Events.

}
