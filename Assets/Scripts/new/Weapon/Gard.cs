using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gard : WeaponWrapper
{
    public override string GetDescription()
    {
        return weapon.GetWeaponName() + " с гардой"; // Модифицированное сообщение
    }

    public override int GetDamage()
    {
        Debug.Log("Урон оружия " + this.GetDescription() + " равен = " + weapon.damage);
        return weapon.damage + 5;
    }
}
