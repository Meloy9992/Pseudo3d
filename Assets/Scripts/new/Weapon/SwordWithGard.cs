using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWithGard : Weapon
{
    private WeaponWrapper swordWithGard = new Gard(); // Получаем объект Gard под видом объекта WeaponWrapper

    public override string GetDescription()
    {
        return swordWithGard.GetDescription(); // Получить описание из класса Gard
    }

    public override int GetDamage()
    {
        return swordWithGard.GetDamage(); // вернуть количество урона из класса Gard
    }

    private void Start()
    {
        swordWithGard.SetWeapon(this); // При получении объекта впервые, передать его в класс обертку
    }
}
