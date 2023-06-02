using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pseudo3d : MonoBehaviour
{


    void Update()
    {
        if (GameObject.FindWithTag("MainCamera"))
        {

            Vector3 pos = GameObject.FindWithTag("MainCamera").transform.position - transform.position; // ��������� ������ �� ���� � ��������� ������� ������� �� ������
            Quaternion rotation = Quaternion.LookRotation(pos); // ��������� ������� ������� ����� � ������
            transform.rotation = rotation; // ������ �������


        }
    }
}
