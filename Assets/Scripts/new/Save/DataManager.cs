using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataManager : MonoBehaviour
{

    [Header("File Storage Config")]

    [SerializeField] private string fileName = "Save.json";

    private DataGame dataGame;
    private List<IDataPersist> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataManager instance { get; private set; }
    UnityEngine.UI.Button button1;

    bool btnIsDown = false;

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); // ��������� ������ � �������� ���� ���� ����� �����������/�����������
        this.dataPersistenceObjects = FindAllDataPersist(); // ����� ��� ������
        LoadGame(); // ��������� ����
    }

    private void Update()
    {
        QuickSave(); // ������� ����������
        //QuickLoad(); // ������� ��������
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            button1 = GameObject.FindGameObjectWithTag("Button Load").GetComponent<UnityEngine.UI.Button>(); // ����� ������ ������
        }
        if (button1 != null)
        {
            button1.onClick.AddListener(() => { btnIsDown = true; }); // ������� ������ �� ������
            Debug.Log("������ ������? " + btnIsDown + button1.name);
        }
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
        File.Delete(new FileDataHandler(Application.persistentDataPath, fileName).GetFullPath());
    }

    public void LoadGame()
    {
        this.dataGame = dataHandler.Load(); // ������� ����� ��������

        if(this.dataGame == null) // ���� ������ ����� null
        {
            Debug.Log("������ �� �������. ������������� ����� ����");
            NewGame(); // ������� ����� ������
        }       
        this.dataPersistenceObjects = FindAllDataPersist(); // ����� ������ ����������
        if (dataPersistenceObjects == null) // ���� ������ ���������� ����� ����
        {
            Debug.LogError("����������� ������ ����� ����!");

            return; // ����� �� ��������� ������
        }

            foreach (IDataPersist dataPersist in dataPersistenceObjects) // ����������� ������ �� ���� ������� ������� ������������ �� IDataPersist
            {
                dataPersist.LoadData(dataGame); // ��������� ������ � ����
            }

            Debug.Log("��������� �������� ����������:  �������������� ���������: " + dataGame.currentPlacePlayer + " ��: "
                + dataGame.currentHpPlayer + " ����� �������� �� �����? " + dataGame.isFlippedRight);
    }

    public void SaveGame()
    {
        this.dataPersistenceObjects = FindAllDataPersist(); // ����� ������ ����������
        foreach (IDataPersist dataPersist in dataPersistenceObjects) // ����������� ������ �� ���� ������� ������� ������������ �� IDataPersist
        {
            dataPersist.SaveData(ref dataGame); // ��������� ������ � ������
/*            if(dataPersist.GetType() == typeof(ChunkPlacer))
            {
                // ���� ��� ���� ������� �� �������� �� ���� ������ ������ � ���������

                List<SerializeChunk> serializeChunks = new List<SerializeChunk>(); 
                foreach(var chunk in dataGame.chunks)
                {
                    SerializeChunk chunk1 = new SerializeChunk(chunk);
                    serializeChunks.Add(chunk1);
                }

                //string json = JsonConvert.SerializeObject(dataGame.chunks.ToArray());
                *//*                string json = JsonSerializer.ToJsonString(serializeChunks.ToArray());
                                Debug.LogError(v);
                                Debug.LogError(json);*//*
                //dataPersist.SaveData();
            }*/
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
        IEnumerable<IDataPersist> dataPersistenceObjects = null; 
        if (SceneManager.GetActiveScene().buildIndex > 0) // ���� ������� ����� ������ 0
        {
            // ������� �������� ��� �������� ������� IEnumerable
            // ����� ���� �������� 0 ������� � ���������
            dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // ����� ��� ������ � ����� MonoBehaviour � ����� IDataPersist
            
        } else
        if(SceneManager.GetActiveScene().buildIndex == 0 && btnIsDown) // ���� ������� ����� =0 � ������ ���� ������
        {
            SceneLoader.GetSceneById(SceneManager.GetActiveScene().buildIndex + 1); // �������� ��������� �����
            dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // ����� ��� ������ � ����� MonoBehaviour � ����� IDataPersist
            SceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ��������� ��������� ����� � ������� ����������
        }

        if (dataPersistenceObjects == null) // ���� ������ �� ���� ���������
        {
            return null; // ������� null
        }
        
        return new List<IDataPersist>(Enumerable.Reverse(dataPersistenceObjects).ToList()); // ������� ����� ������ � ������
    }
}
