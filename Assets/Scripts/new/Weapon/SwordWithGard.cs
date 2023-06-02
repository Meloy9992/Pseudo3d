using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWithGard : Weapon
{
    private WeaponWrapper swordWithGard = new Gard(); // �������� ������ Gard ��� ����� ������� WeaponWrapper

    public override string GetDescription()
    {
        return swordWithGard.GetDescription(); // �������� �������� �� ������ Gard
    }

    public override int GetDamage()
    {
        return swordWithGard.GetDamage(); // ������� ���������� ����� �� ������ Gard
    }

    private void Start()
    {
        swordWithGard.SetWeapon(this); // ��� ��������� ������� �������, �������� ��� � ����� �������
    }
}
