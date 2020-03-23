using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour {

    protected Vector3 _directionMove = Vector3.zero;
    protected float _damage = 10;

    public Vector3 direction
    {
        get { return _directionMove; }
        set { _directionMove = value; }
    }

    protected Dictionary<string, IBulletMoveBehaviors> moveBulletBehaviours;
    protected IBulletMoveBehaviors _currentMove;

    public const string KEY_BULLET_FORWARD = "bullet_forward";
    public const string KEY_BULLET_WAVE = "bullet_wave";

    public float speed { get; protected set; }

    public void ChangeMove(string key)
    {
        _currentMove = moveBulletBehaviours[key];
    }

    protected void Strategies()
    {
        moveBulletBehaviours = new Dictionary<string, IBulletMoveBehaviors>();
        moveBulletBehaviours.Add(KEY_BULLET_FORWARD, new BulletMoveForward());
        moveBulletBehaviours.Add(KEY_BULLET_WAVE, new BulletMoveWave());

        _currentMove = moveBulletBehaviours[KEY_BULLET_FORWARD];
    }
}
