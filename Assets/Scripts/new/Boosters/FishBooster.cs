using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishBooster : MonoBehaviour
{
    public Vector3 target;
    public int moveSpeed;
    public float rotationSpeed;
    public Vector3 vector;
    public float cooldown;
    public bool isCooldown;

    public float speedEnemy;

    private Image powerUpImage;
    private Player player;
    private Enemy enemy;

    void Start()
    {
        powerUpImage = GetComponent<Image>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        isCooldown = false;
    }

    void Update()
    {
        if (powerUpImage) // ���� ������ �������
        {
            isCooldown = true; // ������ ���������� ��������
        }
        if (isCooldown)
        {
            powerUpImage.fillAmount -= 1 / cooldown * Time.deltaTime; //���������� ������� �������
            
            if (powerUpImage.fillAmount <= 0) // ���� ������� ������� ������ ��� ����� ���� ��
            {
                powerUpImage.fillAmount = 1; // ���������� �������
                isCooldown = false;
                player.booster.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetTimer()
    {
        powerUpImage.fillAmount = 1;
    }

}



