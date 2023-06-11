using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    public Image bar;
    private int hpEnemy;
    private int maxHp;
    public float fill = 1f;
    private Enemy enemy;
    private float tempHp = 0;
    private void Start()
    {
        enemy  = gameObject.GetComponent<Enemy>();
        maxHp = enemy.health;
        
    }
    // Update is called once per frame
    void Update()
    {
        hpEnemy = enemy.health;
        tempHp = (float) hpEnemy / maxHp;
        bar.fillAmount = tempHp;
    }
}
