using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAsteroid : BaseAsteroid {

    private Vector3 _directionMove = Vector3.zero;
    private Rigidbody _rigidbody;

    public Vector3 direction
    {
        get { return _directionMove; }
        set { _directionMove = value; }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _speed = 1;
        _life = 5;
        _damageValue = 5;

        Strategies();
        ManagerUpdate.instance.updateFixed += Execute;
        dieAsteroid += DestroyVFX;
    }

    private void Execute()
    {
        if (this.gameObject.active)
        {
            Check();
            _rigidbody.MovePosition(transform.position + _directionMove * _speed * Time.deltaTime);
        }
    }

    private void Check()
    {
        #region Position adjustment
        //----Vertical limit.
        if (transform.position.y > PointsReferences.instance.GetUp)
        {
            ManagerGame.instance.AddPoints(3);
            ManagerAsteroids.instance.ReturnMiniAsteroidToPool(this);
        }
        else if (transform.position.y < PointsReferences.instance.GetDown)
        {
            ManagerGame.instance.AddPoints(3);
            ManagerAsteroids.instance.ReturnMiniAsteroidToPool(this);
        }

        //----Horizontal limit.
        if (transform.position.x > PointsReferences.instance.GetRight)
        {
            ManagerGame.instance.AddPoints(3);
            ManagerAsteroids.instance.ReturnMiniAsteroidToPool(this);
        }
        else if (transform.position.x < PointsReferences.instance.GetLeft)
        {
            ManagerGame.instance.AddPoints(3);
            ManagerAsteroids.instance.ReturnMiniAsteroidToPool(this);
        }
        #endregion Position adjustment
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == Const.LAYER_PLAYER)
        {
            c.gameObject.GetComponent<Player>().Damage(_damageValue);
        }
    }

    public static void Initialize(MiniAsteroid bulletObj)
    {
        ManagerAsteroids.instance.NotifyEnableAsteroid();
        bulletObj.gameObject.SetActive(true);
    }

    public static void Dispose(MiniAsteroid bulletObj)
    {
        ManagerAsteroids.instance.NotifyDiableAsteroid();
        bulletObj.gameObject.SetActive(false);
    }

    #region Abstracts and virtuals methods

    public override void Damage(float value)
    {
        base.Damage(value);
    }

    public override void DestroyVFX()
    {
        var explotion = FactoryPool.instance.explotionAsteroidPool.GetObjectFromPool();
        explotion.transform.localScale = transform.localScale;
        explotion.transform.position = transform.position;
        ManagerGame.instance.AddPoints(7);
        ManagerAsteroids.instance.ReturnMiniAsteroidToPool(this);
    }

    #endregion  Abstracts and virtuals methods
}
