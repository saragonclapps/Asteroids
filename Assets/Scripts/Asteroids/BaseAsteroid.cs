using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseAsteroid : MonoBehaviour {

    protected Dictionary<string, IMoveAsteroidBehaviors> _modelBehaviors;
    protected IMoveAsteroidBehaviors _modelCurrent;
    protected Player _player;
    public event Action dieAsteroid = delegate { };

    protected float _life = 100;
    protected float _speed;
    protected float _damageValue;
    protected const string KEY_MOVE_1 = "Move_1";
    protected const string KEY_MOVE_2 = "Move_2";

    public void Strategies()
    {
        //Models
        IMoveAsteroidBehaviors tempMove_1 = new MoveAsteroidForward();
        IMoveAsteroidBehaviors tempMove_2 = new MoveAsteroidCircular();

        _modelBehaviors = new Dictionary<string, IMoveAsteroidBehaviors>();
        _modelBehaviors.Add(KEY_MOVE_1, tempMove_1);
        _modelBehaviors.Add(KEY_MOVE_2, tempMove_2);

        //Set values.
        _modelCurrent = _modelBehaviors[KEY_MOVE_2];
    }

    public virtual void Damage(float value)
    {
        _life -= value;
        if (_life <= 0)
        {
            dieAsteroid();
        }
    }

    public abstract void DestroyVFX();}
