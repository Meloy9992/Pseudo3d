using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public static GameStorage Instance { get; private set; }

    private void Awake()
    {
        CheckInstance();
        InitManagers();
    }

    private void CheckInstance()
    {
        if (Instance == null) // ���� ������ ����� 0
        {
            Instance = this; // �������� ������
            DontDestroyOnLoad(this); //�� ��������� ������ ��� �������� �����
        }
        else
        {
            Destroy(gameObject); // ��������� ������
        }
    }

    private void InitManagers()
    {
        Debug.Log("� ������� �������� �������� ��������");
        // ����� ��������� ������ ������ ������� ����������� ������� Singleton
        // ������: FileManager = gameObject?.GetComponent<FileManager>();


    }
}
