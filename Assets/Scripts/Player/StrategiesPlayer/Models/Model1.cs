using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model1 : MonoBehaviour, IModelBehaviors
{  

    public GameObject Draw()
    {
        var prefab = Instantiate(Resources.Load("Models/Model1", typeof(GameObject))) as GameObject;
        prefab.transform.SetParent(FindObjectOfType<Player>().transform);
        return prefab;
    }
}
