using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactForAttack : MonoBehaviour , ITakeDamage
{
    Enemy enemy;
    Weapon weapon;

    private void OnTriggerStay(Collider other)
    {
        weapon = other.gameObject.GetComponent<Weapon>();
    }

    private void Update()
    {
        attack();
    }

    public void attack()
    {
        enemy = GetComponent<Enemy>();

        if (weapon == null)
        {
            return;
        }
        else
        {
            takeDamage(weapon.GetDamage());
            WeaponLog(weapon);
        }
    }

    void WeaponLog(Weapon weapon)
    {
        Debug.Log("Type " + weapon.GetType() + " Damage " + weapon.GetDamage()); // Логирование типа и урона оружия
        Debug.Log("Description weapon: " + weapon.GetDescription() + " Type weapon - " + weapon.GetType());
    }

    public void takeDamage(int damage) // Нанесение урона 
    {
        enemy.stopTime = enemy.startStopTime; // время остановки равно времени перезарядки
        Debug.Log("Урон который передали в метод нанести урон = " + damage);
        enemy.health -= damage; // нанести урон противнику
        Debug.Log("Enemy Health = " + enemy.health); // логирование здоровья противника
    }
}
