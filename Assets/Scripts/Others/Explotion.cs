using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour {

    public float time;
    private float _currentTime = 5;

    private void Awake()
    {
       gameObject.SetActive(false);
    }

    private void Start () {
        _currentTime = time;
        ManagerUpdate.instance.update += Execute;
	}

    public static void Initialize(Explotion exlotion)
    {
        exlotion.gameObject.SetActive(true);
    }

    public static void Dispose(Explotion explotion)
    {
        explotion.gameObject.SetActive(false);
    }
    
    private void Execute () {
		_currentTime -= Time.deltaTime;
        if (_currentTime < 0)
        {
            _currentTime = time;
            FactoryPool.instance.ReturnExplotionBulletToPool(this);
        }
	}
}
