using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity; // ����������
    public float currentGravity; // ������� ����������
    public float constantGravity; // ��������� ����������
    public float maxGravity; // ������������ ���������� 
    public Vector3 gravityDirection; // ����������� ����������
    public Vector3 gravityMovement; // �������� ����������

    public void Gravitation()
    {
        if (GetComponent<Character>().isGrounded()) // ���� �� �����
        {
            currentGravity = constantGravity; // ������� ���������� = ���������

        }
        else
        {
            if (currentGravity > maxGravity) // ���� ������� ���������� ������ ������������ ��
            {
                currentGravity -= gravity * Time.deltaTime; // ������� ���������� � ����� (������� ����� �������� ��� ������ ���������)
            }
        }
        gravityMovement = gravityDirection * -currentGravity;
        gravityDirection = Vector3.down; // ��������� ������ ��������� ����
    }
}
