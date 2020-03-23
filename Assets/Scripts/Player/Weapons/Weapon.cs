using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    protected Dictionary<string, IWeaponBehaviors> _weaponBehaviors;
    protected IWeaponBehaviors _currentWeapont;

    public const string KEY_WEAPON_LASER = "weapon_laser";
    public const string KEY_WEAPON_DEFAULT ="weapon_default";
    public const string KEY_WEAPON_BOMBS = "weapon_bombs";


    public void Start(){

        _weaponBehaviors = new Dictionary<string, IWeaponBehaviors>();
        _weaponBehaviors.Add(KEY_WEAPON_DEFAULT, new WeaponDefault());
        _weaponBehaviors.Add(KEY_WEAPON_LASER, new WeaponLaser(FindObjectOfType<Player>().gameObject));
        //_weaponBehaviors.Add(KEY_WEAPON_BOMBS, new WeaponLaser());
        _currentWeapont = _weaponBehaviors[KEY_WEAPON_DEFAULT];

        ManagerUpdate.instance.updateFixed += Execute;
    }

    private void Execute(){
        if (transform != null)
        {
            _currentWeapont.Shoot(transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(KEY_WEAPON_DEFAULT);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(KEY_WEAPON_LASER);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(KEY_WEAPON_BOMBS);
        }
    }

    /// <summary>
    /// by strategy i can change the current weapon.
    /// </summary>
    /// <param name="key">Diccionary key</param>
    public void ChangeWeapon(string key){
        _currentWeapont.Disable();
        _currentWeapont = _weaponBehaviors[key];
    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 3));
    }

    private void OnDestroy()
    {
        ManagerUpdate.instance.updateFixed -= Execute;
    }
}
