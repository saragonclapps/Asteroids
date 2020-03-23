using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : PlayerBase, IObservable
{
    private event Action _observerSubscribedDamage = delegate {};
    private Rigidbody _rigidbody;
    private Vector3 _verticalMove;
    private Vector3 _horizontalMove;

    private void Awake(){
        maxLife = life = 100;
    }

    private void Start () {
        Strategies();
        _rigidbody = GetComponent<Rigidbody>();
        ManagerUpdate.instance.updateLate += Execute;
    }
    

    private void Execute()
    {
        Check();
        Move();
    }

    private void Check()
    {
        #region Position adjustment
        //----Vertical adjustment
        if (transform.position.y > PointsReferences.instance.GetUp)
        {
            var newPosition = new Vector3(transform.position.x, PointsReferences.instance.GetDown);
            SetPosition(newPosition);
        }
        else if (transform.position.y < PointsReferences.instance.GetDown)
        {
            var newPosition = new Vector3(transform.position.x, PointsReferences.instance.GetUp);
            SetPosition(newPosition);
        }

        //----Horizontal adjustment
        if (transform.position.x > PointsReferences.instance.GetRight)
        {
            var newPosition = new Vector3(PointsReferences.instance.GetLeft, transform.position.y);
            SetPosition(newPosition);
        }
        else if (transform.position.x < PointsReferences.instance.GetLeft)
        {
            var newPosition = new Vector3(PointsReferences.instance.GetRight, transform.position.y);
            SetPosition(newPosition);
        }
        #endregion Position adjustment
    }

    protected override void Move()
    {
        //Move
        _horizontalMove += transform.right * _moveCurrent.MoveHorizontal(speed);
        _verticalMove += transform.up * _moveCurrent.MoveVertical(speed);
        _velocity = (_horizontalMove + _verticalMove) * Time.deltaTime;
        _velocity = new Vector3(Mathf.Clamp(_velocity.x, minVelocity.x, maxVelocity.x), 
                                Mathf.Clamp(_velocity.y, minVelocity.y, maxVelocity.y));

        _rigidbody.MovePosition(transform.position + _velocity);

        //Rotation
        Vector3 eulerAngleVelocity = (Vector3.forward * _moveCurrent.MoveRotation(speedRotation));
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    public override void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    public override void Damage(float value)
    {
        life -= value;
        _observerSubscribedDamage();
    }

    public void Subscribe(Action observer)
    {
        _observerSubscribedDamage += observer;
    }

    public void UnSubscribe(Action observer)
    {
        _observerSubscribedDamage -= observer;
    }

    private void OnDestroy()
    {
        ManagerUpdate.instance.updateLate -= Execute;
        var childs = GetComponentsInChildren<Transform>();
        foreach (var item in childs)
        {
            Destroy(gameObject);
        }
    }
}
