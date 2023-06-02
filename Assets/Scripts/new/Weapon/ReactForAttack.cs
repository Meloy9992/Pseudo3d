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
        Debug.Log("Type " + weapon.GetType() + " Damage " + weapon.GetDamage()); // ����������� ���� � ����� ������
        Debug.Log("Description weapon: " + weapon.GetDescription() + " Type weapon - " + weapon.GetType());
    }

    public void takeDamage(int damage) // ��������� ����� 
    {
        enemy.stopTime = enemy.startStopTime; // ����� ��������� ����� ������� �����������
        Debug.Log("���� ������� �������� � ����� ������� ���� = " + damage);
        enemy.health -= damage; // ������� ���� ����������
        Debug.Log("Enemy Health = " + enemy.health); // ����������� �������� ����������
    }
}
