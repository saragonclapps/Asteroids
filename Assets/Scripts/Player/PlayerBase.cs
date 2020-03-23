using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerBase : MonoBehaviour {

    protected Dictionary<string, IMoveBehaviors> _moveBehaviors;
    protected IMoveBehaviors _moveCurrent;

    public float life { get; protected set; }
    public float maxLife { get; protected set; }
    public float speed;
    public float speedRotation;
    public Vector2 maxVelocity = new Vector3(5, 5);
    public Vector2 minVelocity = new Vector3(-5,-5);
    protected Vector3 _velocity;

    protected const string KEY_MODEL_1 = "Model_1";
    protected const string KEY_MODEL_2 = "Model_2";
    protected const string KEY_MOVE_ALL_DIR = "Move_all_dir";
    protected const string KEY_MOVE_TWO_DIR = "Move_two_dir";

    public float Velocity
    {
        get
        {
            return Velocity;
        }
    }

    public void Strategies()
    {
        //Move
        _moveBehaviors = new Dictionary<string, IMoveBehaviors>();
        _moveBehaviors.Add(KEY_MOVE_TWO_DIR, new MoveTwoDirections());
        _moveBehaviors.Add(KEY_MOVE_ALL_DIR, new MoveAllDirections());

        //Set values.
        _moveCurrent = _moveBehaviors[KEY_MOVE_TWO_DIR];
    }

    public    abstract void SetPosition(Vector3 value);
    public abstract void Damage(float value);
    protected abstract void Move();
}