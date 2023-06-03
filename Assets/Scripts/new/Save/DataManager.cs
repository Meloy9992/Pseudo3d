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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersist();
        LoadGame();
    }

    private void Update()
    {
        QuickSave();
        QuickLoad();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Найден больше чем один data Manager");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.dataGame = new DataGame();
    }

    public void LoadGame()
    {
        this.dataGame = dataHandler.Load();

        if(this.dataGame == null)
        {
            Debug.Log("Данных не найдено. Инициализация новой игры");
            NewGame();
        }

        foreach(IDataPersist dataPersist in dataPersistenceObjects)
        {
            dataPersist.LoadData(dataGame);
        }

        Debug.Log("Произошла загрузка параметров:  Местоположение персонажа: " + dataGame.currentPlacePlayer + " ХП: "
            + dataGame.currentHpPlayer + " игрок повернут на право? " + dataGame.isFlippedRight);

    }

    public void SaveGame()
    {
        foreach(IDataPersist dataPersist in dataPersistenceObjects)
        {
            dataPersist.SaveData(ref dataGame);
        }

        Debug.Log("Произошло сохранение параметров:  Местоположение персонажа: " + dataGame.currentPlacePlayer + " ХП: "
            + dataGame.currentHpPlayer + " игрок повернут на право? " + dataGame.isFlippedRight);

        dataHandler.Save(dataGame);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void QuickSave()
    {
        if(Input.GetKey(KeyCode.F5)) 
        {
            SaveGame();
        }
    }

    public void QuickLoad()
    {
        if (Input.GetKey(KeyCode.F6))
        {
            LoadGame();
        }
    }

    private List<IDataPersist> FindAllDataPersist()
    {
        IEnumerable<IDataPersist> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>();

        return new List<IDataPersist>(dataPersistenceObjects);
    }
}
