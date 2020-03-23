using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponBehaviors{
    void Reload();
    void Shoot(Transform entity);
    void Disable();
}
