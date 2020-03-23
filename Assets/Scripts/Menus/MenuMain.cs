using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : MenuBase{

	protected override void Start () {
        base.Start();
	}

    public void ExecuteOptions(GameObject goTo)
    {
        _goTo = goTo;
        StartCoroutine(FadingTransition());
    }

    public void ExecuteCredits(GameObject goTo)
    {
        _goTo = goTo;
        StartCoroutine(FadingTransition());
    }
}