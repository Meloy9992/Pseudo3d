using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pseudo3d : MonoBehaviour
{


    void Update()
    {
        if (GameObject.FindWithTag("MainCamera"))
        {

            Vector3 pos = GameObject.FindWithTag("MainCamera").transform.position - transform.position; // получение камеры по тегу и вычитание позиции объекта от камеры
            Quaternion rotation = Quaternion.LookRotation(pos); // установка позиции объекта лицом к камере
            transform.rotation = rotation; // поворт объекта


        }
    }
}
