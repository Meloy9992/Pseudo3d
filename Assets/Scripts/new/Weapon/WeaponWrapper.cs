using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponWrapper : Weapon
{

    // Ёто класс обертка(ƒекоратор) дл€ взаимодействи€
    protected Weapon weapon;

    public void SetWeapon(Weapon weapon) => this.weapon = weapon;

    public override string GetDescription() => weapon.GetDescription();

    public override int GetDamage()
    {
        return weapon.GetDamage();
    }
}
