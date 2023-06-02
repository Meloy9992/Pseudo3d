using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gard : WeaponWrapper
{
    public override string GetDescription()
    {
        return weapon.GetWeaponName() + " � ������"; // ���������������� ���������
    }

    public override int GetDamage()
    {
        Debug.Log("���� ������ " + this.GetDescription() + " ����� = " + weapon.damage);
        return weapon.damage + 5;
    }
}
