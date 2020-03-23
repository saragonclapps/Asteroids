using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBasic : BulletBase {

    private void Start()
    {
        Strategies();
        speed = 5;
        ManagerUpdate.instance.updateFixed += Execute;
    }

    private void Execute()
    {
        Check();
        if (_currentMove != null && this.gameObject.active)
            _currentMove.Move(this.transform, direction, speed);
    }

    private void Check()
    {
        #region Position adjustment
        //----Vertical limit.
        if (transform.position.y > PointsReferences.instance.GetUp)
        {
            FactoryPool.instance.ReturnBulletsBasicsToPool(this);
        }
        else if (transform.position.y < PointsReferences.instance.GetDown)
        {
            FactoryPool.instance.ReturnBulletsBasicsToPool(this);
        }

        //----Horizontal limit.
        if (transform.position.x > PointsReferences.instance.GetRight)
        {
            FactoryPool.instance.ReturnBulletsBasicsToPool(this);
        }
        else if (transform.position.x < PointsReferences.instance.GetLeft)
        {
            FactoryPool.instance.ReturnBulletsBasicsToPool(this);
        }
        #endregion Position adjustment
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == Const.LAYER_ASTEROIDS)
        {
            ContactPoint contact = c.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            c.gameObject.GetComponent<BaseAsteroid>().Damage(_damage);
            var explotion = FactoryPool.instance.explotionBulletPool.GetObjectFromPool();
            explotion.transform.position = contact.point;
            FactoryPool.instance.ReturnBulletsBasicsToPool(this);
        }
    }

    public static void Initialize(BulletBasic bulletObj)
    {
        bulletObj.gameObject.GetComponent<TrailRenderer>().Clear();
        bulletObj.gameObject.SetActive(true);
    }

    public static void Dispose(BulletBasic bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
    }
}
