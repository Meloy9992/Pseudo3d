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
        if (Instance == null) // Если пример равен 0
        {
            Instance = this; // Получить пример
            DontDestroyOnLoad(this); //Не разрушать объект при загрузке сцены
        }
        else
        {
            Destroy(gameObject); // Разрушить объект
        }
    }

    private void InitManagers()
    {
        Debug.Log("Я являюсь примером синглтон паттерна");
        // Здесь находятся другие классы которые интегрируют паттерн Singleton
        // Пример: FileManager = gameObject?.GetComponent<FileManager>();


    }
}
