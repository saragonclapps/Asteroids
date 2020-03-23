using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCredits : MenuBase {

    protected override void Start()
    {
        base.Start();
    }

    public void ExecuteMainMenu(GameObject goTo)
    {
        _goTo = goTo;
        StartCoroutine(FadingTransition());
    }
}
