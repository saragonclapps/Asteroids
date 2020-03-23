using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveBehaviors
{
    float MoveHorizontal(float speed);
    float MoveVertical(float speed);
    float MoveRotation(float speed);
}
