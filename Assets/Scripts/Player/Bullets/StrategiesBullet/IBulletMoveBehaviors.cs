using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletMoveBehaviors {
    void Move(Transform entity, Vector3 dir, float speed);
    void Damage();
}
