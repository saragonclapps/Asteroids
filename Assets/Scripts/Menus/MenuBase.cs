using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour {

    private Fading _fading;
    protected GameObject _goTo;
    protected event Action<GameObject> _executeInFading = delegate { };
    protected event Action _executeInFadingIn = delegate { };

    protected virtual void Start()
    {
        _fading = FindObjectOfType<Fading>();
        _executeInFading += DisableAndEnable;
    }

    protected IEnumerator FadingTransition()
    {
        _fading.BeginFade(1);
        yield return new WaitForSeconds(0.6f);
        _executeInFading(_goTo);
        _fading.BeginFade(-1);
    }

    protected IEnumerator FadingIn()
    {
        _fading.BeginFade(1);
        yield return new WaitForSeconds(0.6f);
        _executeInFadingIn();
    }

    public void FadingOut()
    {
        _fading.BeginFade(-1);
    }

    protected void DisableAndEnable(GameObject objectEnable)
    {
        objectEnable.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
