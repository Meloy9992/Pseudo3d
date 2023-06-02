using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public override string GetDescription()
    {
        return GetWeaponName();
    }

    public override int GetDamage()
    {
        this.damage = damage; 
        return damage;
    }
}
