using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public DataGame Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // полный путь до json

        DataGame loaded = null; // Создаем экземпляр объекта который будем сохранять/загружать

        if(File.Exists(fullPath)) // Если файл присутствует
        {
            try
            {
                string dataToLoad = ""; // Строка для принятия json строки
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)) // Открываем новый поток для чтения файла
                {
                    using(StreamReader reader = new StreamReader(stream)) // Считываем в новом потоке данные из файла
                    {
                        dataToLoad = reader.ReadToEnd();// Прочитать файл до конца
                    }
                }

                loaded = JsonUtility.FromJson<DataGame>(dataToLoad); // Обработать JSON с помощью утилиты json
            }
            catch(Exception e)
            {
                Debug.LogError("Произошла ошибка при загрузке данных по пути: " + fullPath + "\n" + e);
            }
        }
        return loaded; // Вернуть загруженное
    }

    public void Save(DataGame data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // Полный путь для сохранения файла

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // Создаем директорию

            string dataToStore = JsonUtility.ToJson(data, true); // Записываем с помощью json утилиты данные

            using(FileStream stream = new FileStream(fullPath, FileMode.Create)) // Создаем поток для записи в файл
            {
                using(StreamWriter writer = new StreamWriter(stream))  // Создаем поток записи в файл
                {
                    writer.Write(dataToStore); // Записываем данные в файл
                }
            }
            Debug.LogError("Путь до файла: " + fullPath);
        }
        catch(Exception e)
        {
            Debug.LogError("Возникла проблема с сохранением файла: " + fullPath + "\n" +  e);
        }

    }
    public string GetFullPath()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        return fullPath;
    }
}
