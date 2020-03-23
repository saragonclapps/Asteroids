using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : IWeaponBehaviors
{
    LineRenderer _linerenderer;
    private float maxDistance = 20;
    //private float _timeRefresh = 0.3f;
    private float _damage = 0.3f;
    private float _currentTimeRefresh;

    public WeaponLaser(GameObject parent)
    {
        _linerenderer = parent.GetComponent<LineRenderer>();
    }

    public void Disable()
    {
        _linerenderer.SetPosition(0, Vector3.zero);
        _linerenderer.SetPosition(1, Vector3.zero);
    }

    public void Reload()
    {

    }

    public void Shoot(Transform entity)
    {

        if (Input.GetButton("Fire"))
        {
            Fire(entity);
        }
        else
        {
            Disable();
        }
    }

    private void Fire(Transform entity)
    {
        _linerenderer.SetPosition(0, entity.position);
        _linerenderer.SetPosition(1, entity.position + (entity.up * maxDistance));

        RaycastHit ray;

        if (Physics.Raycast(entity.position, entity.up, out ray, maxDistance, (1 << Const.LAYER_ASTEROIDS)))
        {
            _linerenderer.SetPosition(1, entity.position + (entity.up * ray.distance));
            ray.transform.GetComponent<BaseAsteroid>().Damage(_damage);
        }
    }
}
