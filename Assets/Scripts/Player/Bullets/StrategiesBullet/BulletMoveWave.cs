using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveWave : IBulletMoveBehaviors{

    float _wavySpeed = 5;
    float _wavyTime = 1f;

    public void Damage()
    {
        throw new NotImplementedException();
    }

    public void Move(Transform entity, Vector3 dir, float speed)
    {
        _wavyTime += Time.deltaTime;
        var vector = entity.right * Mathf.Sin(_wavySpeed * _wavyTime) * Time.deltaTime * 10f;

        entity.transform.position = ((entity.position + entity.up) * speed + vector) * Time.deltaTime;
    }
}
