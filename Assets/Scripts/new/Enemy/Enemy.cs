using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    public int health;
    public float speed;
    public int damage;
    public float startStopTime;
    public float normalSpeed;
    public float startTimeBtwAttack;
    public float dist;


    public bool turnedAccess;

    private Transform myTrans;

    Vector3 target;

    private float timeBtwAttack; // перезарядка
    public float stopTime; // время остановки
    private Player player;
    private Animator anim;
    private Vector3 vector;
    private Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>(); // Получение аниматора
        player = FindObjectOfType<Player>(); //Найти объект игрок по классу Player_move
        normalSpeed = speed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Flip();

        if (stopTime <= 0) //Если время остановки больше чем 0 или равно 0
        {
            speed = normalSpeed; // Скорость становится нормальной
        }
        else
        {
            speed = 0; // Иначе скорость равна 0
            stopTime -= Time.deltaTime; // Время остаовки вычитается от дельта времени
        }

        killEnemy();

        isActiveBooster();

        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward);
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 2 + Vector3.down * 2);
        Debug.DrawRay(transform.position + Vector3.down * 0.5f, transform.right + Vector3.up * 0.5f);
        Debug.DrawRay(transform.position + Vector3.down * 0.5f, -transform.right + Vector3.up * 0.5f);
    }

    private void Awake()
    {
        myTrans = transform;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") // Если произошло столкновение с тегом игрок, то
        {
            if (timeBtwAttack <= 0) // Если перезарядка больше или равно нулю, то
            {
                anim.SetTrigger("Attacking"); // установить анимацию атака
            }
            else
            {
                timeBtwAttack -= Time.deltaTime; // Иначе вычитать дельта время
            }
        }
    }


    public void Flip()
    {
        if (player.transform.position.x > transform.position.x) // Если позиция по x игрока больше чем противника, то развернуть на 180 градусов
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void killEnemy()
    {
        if (health <= 0) // если здоровье меньше или равно нулю 
        {
            Destroy(gameObject); // разрушить объект
        }
    }

    public void isActiveBooster()
    {
        if (!player.ribovTimer.isActiveAndEnabled) // Если бустер рыбка не активна и выключена, то
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); // Идти к игроку
        }
        else
        {
            //transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, (speed * 2) * Time.deltaTime); // Иначе бежать от игрока
            RunFromPlayer();
            //отрисовка линий для теста

        }
    }

    public void ToDamage()
    {
        player.health -= damage; // Вычесть от хп игрока урон 
        timeBtwAttack = startTimeBtwAttack; // перезарядка равна времени начала времени перед атакой
    }

    void RunFromPlayer()
    {
        if (transform.position != Vector3.zero)
        {
            Vector3 direction = player.transform.position - transform.position;
            if (direction.magnitude > 2)
            {
                
                Vector3 fleeDirection = transform.position - player.transform.position; // Вычисляем вектор направления убегания


                fleeDirection = fleeDirection.normalized; // Нормализуем вектор направления убегания
                Debug.Log("DIRECTION MAGNITUDE = " + direction.magnitude + " DIRECTION " + direction);
                Debug.Log("FLEE DIRECTION " + fleeDirection);
                

                rb.AddForce(0, 0, -fleeDirection.magnitude * speed * Time.deltaTime, ForceMode.Impulse); // Двигаем противника в сторону убегания
            }
        }
    }
}
