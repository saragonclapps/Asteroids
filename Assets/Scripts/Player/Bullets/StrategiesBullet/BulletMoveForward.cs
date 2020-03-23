using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveForward : IBulletMoveBehaviors
{

    public void Damage()
    {
        throw new NotImplementedException();
    }

    public void Move(Transform entity,Vector3 dir, float speed)
    {
       entity.position = (entity.position +( dir * speed) * Time.deltaTime);
    }
}
