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
        string fullPath = Path.Combine(dataDirPath, dataFileName); // ������ ���� �� json

        DataGame loaded = null; // ������� ��������� ������� ������� ����� ���������/���������

        if(File.Exists(fullPath)) // ���� ���� ������������
        {
            try
            {
                string dataToLoad = ""; // ������ ��� �������� json ������
                using(FileStream stream = new FileStream(fullPath, FileMode.Open)) // ��������� ����� ����� ��� ������ �����
                {
                    using(StreamReader reader = new StreamReader(stream)) // ��������� � ����� ������ ������ �� �����
                    {
                        dataToLoad = reader.ReadToEnd();// ��������� ���� �� �����
                    }
                }

                loaded = JsonUtility.FromJson<DataGame>(dataToLoad); // ���������� JSON � ������� ������� json
            }
            catch(Exception e)
            {
                Debug.LogError("��������� ������ ��� �������� ������ �� ����: " + fullPath + "\n" + e);
            }
        }
        return loaded; // ������� �����������
    }

    public void Save(DataGame data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName); // ������ ���� ��� ���������� �����

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)); // ������� ����������

            string dataToStore = JsonUtility.ToJson(data, true); // ���������� � ������� json ������� ������

            using(FileStream stream = new FileStream(fullPath, FileMode.Create)) // ������� ����� ��� ������ � ����
            {
                using(StreamWriter writer = new StreamWriter(stream))  // ������� ����� ������ � ����
                {
                    writer.Write(dataToStore); // ���������� ������ � ����
                }
            }
            Debug.LogError("���� �� �����: " + fullPath);
        }
        catch(Exception e)
        {
            Debug.LogError("�������� �������� � ����������� �����: " + fullPath + "\n" +  e);
        }

    }
    public string GetFullPath()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        return fullPath;
    }
}
