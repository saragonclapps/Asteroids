using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MenuBase {

    public void ExecuteContinue()
    {
        print("hey");
        ManagerUpdate.instance.Pause(false);
        ManagerEvent.DispatchEvent(GameEvent.PAUSE);
        gameObject.SetActive(false);
    }
}
