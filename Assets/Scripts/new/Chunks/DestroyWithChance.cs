using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithChance : MonoBehaviour
{
    [Range(0, 1)]
    public float ChangeOfStaying = 0.5f;
    void Start()
    {
        if (Random.value > ChangeOfStaying) Destroy(gameObject); // Если случайное число больше чем установаленный уровень - то удалить объект
    }

}
