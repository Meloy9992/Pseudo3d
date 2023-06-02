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

    private float timeBtwAttack; // �����������
    public float stopTime; // ����� ���������
    private Player player;
    private Animator anim;
    private Vector3 vector;
    private Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>(); // ��������� ���������
        player = FindObjectOfType<Player>(); //����� ������ ����� �� ������ Player_move
        normalSpeed = speed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Flip();

        if (stopTime <= 0) //���� ����� ��������� ������ ��� 0 ��� ����� 0
        {
            speed = normalSpeed; // �������� ���������� ����������
        }
        else
        {
            speed = 0; // ����� �������� ����� 0
            stopTime -= Time.deltaTime; // ����� �������� ���������� �� ������ �������
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
        if (other.gameObject.tag == "Player") // ���� ��������� ������������ � ����� �����, ��
        {
            if (timeBtwAttack <= 0) // ���� ����������� ������ ��� ����� ����, ��
            {
                anim.SetTrigger("Attacking"); // ���������� �������� �����
            }
            else
            {
                timeBtwAttack -= Time.deltaTime; // ����� �������� ������ �����
            }
        }
    }


    public void Flip()
    {
        if (player.transform.position.x > transform.position.x) // ���� ������� �� x ������ ������ ��� ����������, �� ���������� �� 180 ��������
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
        if (health <= 0) // ���� �������� ������ ��� ����� ���� 
        {
            Destroy(gameObject); // ��������� ������
        }
    }

    public void isActiveBooster()
    {
        if (!player.ribovTimer.isActiveAndEnabled) // ���� ������ ����� �� ������� � ���������, ��
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); // ���� � ������
        }
        else
        {
            //transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, (speed * 2) * Time.deltaTime); // ����� ������ �� ������
            RunFromPlayer();
            //��������� ����� ��� �����

        }
    }

    public void ToDamage()
    {
        player.health -= damage; // ������� �� �� ������ ���� 
        timeBtwAttack = startTimeBtwAttack; // ����������� ����� ������� ������ ������� ����� ������
    }

    void RunFromPlayer()
    {
        if (transform.position != Vector3.zero)
        {
            Vector3 direction = player.transform.position - transform.position;
            if (direction.magnitude > 2)
            {
                
                Vector3 fleeDirection = transform.position - player.transform.position; // ��������� ������ ����������� ��������


                fleeDirection = fleeDirection.normalized; // ����������� ������ ����������� ��������
                Debug.Log("DIRECTION MAGNITUDE = " + direction.magnitude + " DIRECTION " + direction);
                Debug.Log("FLEE DIRECTION " + fleeDirection);
                

                rb.AddForce(0, 0, -fleeDirection.magnitude * speed * Time.deltaTime, ForceMode.Impulse); // ������� ���������� � ������� ��������
            }
        }
    }
}
