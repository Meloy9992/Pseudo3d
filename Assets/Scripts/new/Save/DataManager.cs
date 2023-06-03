using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    [Header("File Storage Config")]

    [SerializeField] private string fileName = "Save.json";

    private DataGame dataGame;
    private List<IDataPersist> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataManager instance { get; private set; }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); // ��������� ������ � �������� ���� ���� ����� �����������/�����������
        this.dataPersistenceObjects = FindAllDataPersist(); // ����� ��� ������
        LoadGame(); // ��������� ����
    }

    private void Update()
    {
        QuickSave(); // ������� ����������
        QuickLoad(); // ������� ��������
    }

    private void Awake()
    {
        if (instance != null) // ���� instance �� ����� null
        {
            Debug.LogError("������ ������ ��� ���� data Manager");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.dataGame = new DataGame(); // ������� ����� ������ � ������ ��� ����������
    }

    public void LoadGame()
    {
        this.dataGame = dataHandler.Load(); // ������� ����� ��������

        if(this.dataGame == null) // ���� ������ ����� null
        {
            Debug.Log("������ �� �������. ������������� ����� ����");
            NewGame(); // ������� ����� ������
        }

        foreach(IDataPersist dataPersist in dataPersistenceObjects) // ����������� ������ �� ���� ������� ������� ������������ �� IDataPersist
        {
            dataPersist.LoadData(dataGame); // ��������� ������ � ����
        }

        Debug.Log("��������� �������� ����������:  �������������� ���������: " + dataGame.currentPlacePlayer + " ��: "
            + dataGame.currentHpPlayer + " ����� �������� �� �����? " + dataGame.isFlippedRight);

    }

    public void SaveGame()
    {
        foreach(IDataPersist dataPersist in dataPersistenceObjects) // ����������� ������ �� ���� ������� ������� ������������ �� IDataPersist
        {
            dataPersist.SaveData(ref dataGame); // ��������� ������ � ������
        }

        Debug.Log("��������� ���������� ����������:  �������������� ���������: " + dataGame.currentPlacePlayer + " ��: "
            + dataGame.currentHpPlayer + " ����� �������� �� �����? " + dataGame.isFlippedRight);

        dataHandler.Save(dataGame); // ��������� ������ � ����
    }

    private void OnApplicationQuit()
    {
        SaveGame(); // ��������� ���� ��� ������
    }

    public void QuickSave() // ������� ����������
    {
        if(Input.GetKey(KeyCode.F5)) // ��������� ���� �� ������� f5
        {
            SaveGame();
        }
    }

    public void QuickLoad() // ������� ��������
    {
        if (Input.GetKey(KeyCode.F6)) // ��������� ������ �� ������� f6
        {
            LoadGame();
        }
    }

    private List<IDataPersist> FindAllDataPersist() // ����� ��� ��������� ������
    {
        IEnumerable<IDataPersist> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // ����� ��� ������ � ����� MonoBehaviour � ����� IDataPersist

        return new List<IDataPersist>(dataPersistenceObjects); // ������� ����� ������ � ������
    }
}
