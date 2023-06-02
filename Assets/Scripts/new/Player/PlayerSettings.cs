using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Player/Settings", fileName = "PlayerData")] // �������� ��������� ���� � ����� ��� �������� �������� ������ � �����������
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float speed = 15f;

    public float Speed { get { return speed; } }

}
