using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAllDirections : IMoveBehaviors
{
    public float MoveHorizontal(float speed)
    {
        if (Input.GetAxis(Const.AXIS_HORIZONTAL) != 0)
        {
            return speed * Input.GetAxis(Const.AXIS_ROTATION);
        }
        return 0;
    }

    public float MoveVertical(float speed)
    {
        if (Input.GetAxis(Const.AXIS_VERTICAL) != 0)
        {
            return speed * Input.GetAxis(Const.AXIS_VERTICAL);
        }
        return 0;
    }

    public float MoveRotation(float speed)
    {
        if (Input.GetAxis(Const.AXIS_ROTATION) != 0)
        {
            return speed * -Input.GetAxis(Const.AXIS_ROTATION) * 10;
        }
        return 0;
    }

}
