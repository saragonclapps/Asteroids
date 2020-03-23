using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MenuBase {

    protected override void Start()
    {
        base.Start();
    }

    public void ExecuteMain(GameObject goTo)
    {
        _goTo = goTo;
        StartCoroutine(FadingTransition());
    }
}
