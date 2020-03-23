using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cam25D : MonoBehaviour {

    public static Cam25D instance;
    public Transform target;
    private const float OFFSET = -11.0f;
    private const float LERP = 0.1f;
    private float _shakeValue = 0.05f;
    public bool ShakeActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            instance = null;
            instance = this;
        }else
        {
            instance = this;
        }
    }

    void Start () {
        ShakeActive = false;
        ManagerUpdate.instance.updateLate += Execute;
    }

    public IEnumerator ShakeTime()
    {
        ShakeActive = true;
        yield return new WaitForSeconds(0.3f);
        ShakeActive = false;
    }

    private void Execute()
    {
        if (target)
        {
            var newPosition = Vector3.Lerp(transform.position, target.position, LERP * Time.deltaTime);
            transform.position = new Vector3( newPosition.x, newPosition.y, OFFSET );
            if (ShakeActive)
                Shake();
        }
    }

    private void Shake()
    {
        var randNrX = Random.Range(_shakeValue, -_shakeValue);
        var randNrY = Random.Range(_shakeValue, -_shakeValue);
        var randNrZ = Random.Range(_shakeValue, -_shakeValue);
        transform.position += new Vector3(randNrX, randNrY, randNrZ);
    }

    private void OnDrawGizmos()
    {
       if (target != null)
        {
            Gizmos.color = target ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
