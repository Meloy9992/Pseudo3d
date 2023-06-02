using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity; // гравитация
    public float currentGravity; // текущая гравитация
    public float constantGravity; // константа гравитации
    public float maxGravity; // максимальная гравитация 
    public Vector3 gravityDirection; // направление гравитации
    public Vector3 gravityMovement; // движение гравитации

    public void Gravitation()
    {
        if (GetComponent<Character>().isGrounded()) // если на земле
        {
            currentGravity = constantGravity; // текущая гравитация = константе

        }
        else
        {
            if (currentGravity > maxGravity) // если текущая гравитация больше максимальной то
            {
                currentGravity -= gravity * Time.deltaTime; // вычесть гравитацию и время (Сделано чтобы персонаж мог падать корректно)
            }
        }
        gravityMovement = gravityDirection * -currentGravity;
        gravityDirection = Vector3.down; // направить вектор персонажа вниз
    }
}
