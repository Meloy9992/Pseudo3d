using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_attack : MonoBehaviour
{
    private float timeBtwAttack; // ����������� �����
    public float startTimeBtwAttack;

    public Transform attackPos; // ������� �����
    public LayerMask enemy; // �� ����� ���� ��������� ����
    public float attackRange; // ������ �����
    public int damage; // ���� ������
    public Animator animator; // �������� ������

    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetMouseButton(0)) // ���� ����� ������ ����
            {
                animator.SetTrigger("Attacking"); // ������ ������ �����

                Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange, enemy); // ������ ������
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<ReactForAttack>().takeDamage(damage); // �������� ��������� �����
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // ��������� ����� ������ � unity
        Gizmos.DrawWireSphere(attackPos.position, attackRange); // ������� ������� ����� ������ ������
    }
}
