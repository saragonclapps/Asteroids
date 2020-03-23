using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManagerBackground : MonoBehaviour {

    private SlotBackground[] _slots;
    public static ManagerBackground instance;

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance = null;
            instance = this;
        }
        _slots = FindObjectsOfType<SlotBackground>();
    }

    public void UpdateBackground()
    {
        //Save reference.
        var t = _slots.Where(x => x.havePlayer)
                      .Take(1)
                      .Select(x => x.transform)
                      .ToList();

        var reference = t[0];

        //Load news positions.
        // X
        _slots.Select(rsc => rsc.transform)
              .Where(rsc => Mathf.Abs(reference.position.x - rsc.position.x) > 45)
              .Aggregate(transform.position, (acum, current) => (current.position.x < reference.position.x)
                    ? current.position = new Vector3(current.position.x + 90, current.position.y, current.position.z)
                    : current.position = new Vector3(current.position.x - 90, current.position.y, current.position.z));

        // Y
        _slots.Select(rsc => rsc.transform)
              .Where(rsc => Mathf.Abs(reference.position.y - rsc.position.y) > 45)
              .Aggregate(transform.position, (acum, current) => (current.position.y < reference.position.y)
                    ? current.position = new Vector3(current.position.x, current.position.y + 90, current.position.z)
                    : current.position = new Vector3(current.position.x, current.position.y - 90, current.position.z));
    }
}