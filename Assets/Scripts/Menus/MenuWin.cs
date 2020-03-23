using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWin : MenuBase{

    protected override void Start()
    {
        base.Start();
    }

    public void ExecuteContinue()
    {
        ManagerGame.instance.NextLevel();
    }
}
