using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    public int damage;

    [SerializeField]
    private string nameWeapon;
    public abstract string GetDescription();
    public string GetWeaponName() => nameWeapon;

    public abstract int GetDamage();
}
