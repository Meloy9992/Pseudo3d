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
        if (powerUpImage) // Если бустер получен
        {
            isCooldown = true; // Начать вычисление кулдауна
        }
        if (isCooldown)
        {
            powerUpImage.fillAmount -= 1 / cooldown * Time.deltaTime; //Уменьшение таймера бустера
            
            if (powerUpImage.fillAmount <= 0) // Если заливка бустера больше или равна нулю то
            {
                powerUpImage.fillAmount = 1; // заполнение таймера
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



