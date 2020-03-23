using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotBackground : MonoBehaviour {
    
    public bool havePlayer { get; set; }

    private void Start()
    {
        havePlayer = false;
    }

    private void OnTriggerEnter (Collider c)
    {
        if (c.gameObject.layer == Const.LAYER_PLAYER && !havePlayer)
        {
            havePlayer = true;
            ManagerBackground.instance.UpdateBackground();
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == Const.LAYER_PLAYER)
        {
            havePlayer = false;
        }
    }
}
