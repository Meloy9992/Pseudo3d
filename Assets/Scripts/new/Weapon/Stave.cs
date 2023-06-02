using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stave : Weapon
{

    public override int GetDamage()
    {
        this.damage = damage;
        return damage;
    }

    public override string GetDescription()
    {
        return GetWeaponName();
    }
}
