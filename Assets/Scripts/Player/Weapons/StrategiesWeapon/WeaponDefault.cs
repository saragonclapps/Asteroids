using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefault : IWeaponBehaviors
{
    private float _timeRefresh = 0.3f;
    private float _currentTimeRefresh;
    private bool _CanShoot = true;
    public float numBullets { get; private set; }

    public WeaponDefault()
    {
        _currentTimeRefresh = _timeRefresh;
    }

    public void Reload()
    {
        if (Input.GetButton("Reload"))
        {
            Debug.Log("Reload");
        }
    }

    public void Shoot(Transform entity)
    {
        if (Input.GetButton("Fire") && _CanShoot)
        {
            Fire(entity);
        }

        if (!_CanShoot)
        {
            _currentTimeRefresh -= Time.deltaTime;
            if (_currentTimeRefresh < 0)
            {
                _currentTimeRefresh = _timeRefresh;
                _CanShoot = true;
            }
        }
    }

    private void Fire(Transform entity)
    {
        var bullet = FactoryPool.instance.basicBulletsPool.GetObjectFromPool();
        bullet.transform.position = entity.transform.position;
        bullet.direction = (entity.up);
        _CanShoot = false;
    }

    public void AddBullets(int bullets)
    {
        numBullets += bullets;
    }

    public void Disable()
    {
    }
}
