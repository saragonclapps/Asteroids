using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagerUpdate : MonoBehaviour {

    public event Action update = delegate { };
    public event Action updateFixed = delegate { };
    public event Action updateLate = delegate { };
    public static ManagerUpdate instance;
    private bool _isPause = false;
    public bool GetPauseState { get{return _isPause;} }

    private void Awake()
    {
        //Singleton
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

    public void Pause(bool pauseState)
    {
        _isPause = pauseState;
    }

    #region UPDATES

    private void Update()
    {
        if (!_isPause && update != null)
        {
            update();
        }  
    }

    private void LateUpdate()
    {
        if (!_isPause && updateLate != null)
            updateLate();
    }

    private void FixedUpdate()
    {
        if (!_isPause && updateFixed != null)
            updateFixed();
    }

    #endregion UPDATES
}

public class CoroutineUpdate{

    private event Action _OnUpdate = delegate { };
    private bool activeLoop = true;
    public float time { get; set; }
    public bool activeTime { get; set; }

    public CoroutineUpdate(bool activeTime,float time = 0.3f)
    {
        this.activeTime = activeTime;
        this.time = time;
    }

    public void Subscribe(Action action)
    {
        _OnUpdate += action;
    }

    public void Unsubscribe(Action action)
    {
        _OnUpdate -= action;
    }

    public void Clean()
    {
        _OnUpdate = delegate { };
        activeLoop = false;
        time = 0;
    }

    /// <summary>
    /// Execute coroutine in start and method you use this class.
    /// </summary>
    public IEnumerator CoroutineMethod()
    {
        while (activeLoop)
        {
            if (!ManagerUpdate.instance.GetPauseState)
            {
                if (activeTime)
                {
                    _OnUpdate();
                    yield return new WaitForSeconds(time);
                }
                else
                {
                    _OnUpdate();
                    yield return null;
                }
            }
            yield return null;
        }
    }
}
