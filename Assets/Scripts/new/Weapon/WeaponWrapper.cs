using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponWrapper : Weapon
{

    // ��� ����� �������(���������) ��� ��������������
    protected Weapon weapon;

    public void SetWeapon(Weapon weapon) => this.weapon = weapon;

    public override string GetDescription() => weapon.GetDescription();

    public override int GetDamage()
    {
        return weapon.GetDamage();
    }
}
