using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandom : MonoBehaviour
{
    public int CountToLeave = 1;
    void Start()
    {
        while(transform.childCount > CountToLeave) // Пока количество вложенных объектов больше чем указанное количество
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount)); // Принять вложенный объект от 0 до указнного количества
            DestroyImmediate(childToDestroy.gameObject); // Удалить объект
        }
    }
}
