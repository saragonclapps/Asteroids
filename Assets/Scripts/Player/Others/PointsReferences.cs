using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsReferences : MonoBehaviour {

    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public static PointsReferences instance;

    PointsReferences()
    {
        if(instance != null)
        {
            instance = null;
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public Vector3 GetCenter{ get { return transform.position; } }

    public float GetLeft  { get { return left.transform.position.x;  } }
    public float GetRight { get { return right.transform.position.x; } }
    public float GetUp    { get { return up.transform.position.y;    } }
    public float GetDown  { get { return down.transform.position.y;  } }
}
