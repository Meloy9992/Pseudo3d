using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_attack : MonoBehaviour
{
    private float timeBtwAttack; // перезарядка атаки
    public float startTimeBtwAttack;

    public Transform attackPos; // позиция атаки
    public LayerMask enemy; // на каком слое находится враг
    public float attackRange; // радиус атаки
    public int damage; // урон оружия
    public Animator animator; // анимация оружия

    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetMouseButton(0)) // если левая кнопка мыши
            {
                animator.SetTrigger("Attacking"); // задать тригер атака

                Collider[] enemies = Physics.OverlapSphere(attackPos.position, attackRange, enemy); // массив врагов
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<ReactForAttack>().takeDamage(damage); // фиксация нанесения урона
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
        Gizmos.color = Color.red; // показание сферы дамага в unity
        Gizmos.DrawWireSphere(attackPos.position, attackRange); // рисовка красной сферы вокруг оружия
    }
}
