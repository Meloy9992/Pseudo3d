using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); // Указываем формат и название файл куда будет сохраняться/загружаться
        this.dataPersistenceObjects = FindAllDataPersist(); // Найти все данные
        LoadGame(); // Загрузить игру
    }

    private void Update()
    {
        QuickSave(); // Быстрое сохранение
        QuickLoad(); // Быстрая загрузка

        if(button1 != null)
        {
            button1.onClick.AddListener(() => { btnIsDown = true; });
        }
    }

    private void Awake()
    {
        if (instance != null) // Если instance не равен null
        {
            Debug.LogError("Найден больше чем один data Manager");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.dataGame = new DataGame(); // Создать новый объект с данным для сохранения
    }

    public void LoadGame()
    {
        this.dataGame = dataHandler.Load(); // Вызвать метод загрузки

        if(this.dataGame == null) // Если данные равны null
        {
            Debug.Log("Данных не найдено. Инициализация новой игры");
            NewGame(); // Создать новые данные
        }
        button1 = FindObjectOfType<UnityEngine.UI.Button>();
        
        this.dataPersistenceObjects = FindAllDataPersist();
        if (dataPersistenceObjects == null)
        {
            Debug.LogError("Загрузочные данные равны нулю!");

            return;
        }

        foreach(IDataPersist dataPersist in dataPersistenceObjects) // Перечислить данные из всех классов которые унаследованы от IDataPersist
        {
            dataPersist.LoadData(dataGame); // загрузить данные в файл
            Debug.LogError("");
        }

        Debug.Log("Произошла загрузка параметров:  Местоположение персонажа: " + dataGame.currentPlacePlayer + " ХП: "
            + dataGame.currentHpPlayer + " игрок повернут на право? " + dataGame.isFlippedRight);

    }

    public void SaveGame()
    {
        foreach(IDataPersist dataPersist in dataPersistenceObjects) // Перечислить данные из всех классов которые унаследованы от IDataPersist
        {
            dataPersist.SaveData(ref dataGame); // Сохранить данные в объект
            if(dataPersist.GetType() == typeof(ChunkPlacer))
            {
                // Если тип чанк плейсер то выцепить из него список чанков и сохранить


                List<SerializeChunk> serializeChunks = new List<SerializeChunk>();
                foreach(var chunk in dataGame.chunks)
                {
                    SerializeChunk chunk1 = new SerializeChunk(chunk);
                    serializeChunks.Add(chunk1);
                }

                //string json = JsonConvert.SerializeObject(dataGame.chunks.ToArray());
                /*                string json = JsonSerializer.ToJsonString(serializeChunks.ToArray());
                                Debug.LogError(v);
                                Debug.LogError(json);*/
                //dataPersist.SaveData();
            }
        }

        Debug.Log("Произошло сохранение параметров:  Местоположение персонажа: " + dataGame.currentPlacePlayer + " ХП: "
            + dataGame.currentHpPlayer + " игрок повернут на право? " + dataGame.isFlippedRight);

        dataHandler.Save(dataGame); // Сохранить данные в файл
    }

    private void OnApplicationQuit()
    {
        SaveGame(); // Сохранить игру при выходе
    }

    public void QuickSave() // Быстрое сохранение
    {
        if(Input.GetKey(KeyCode.F5)) // Сохранить игру на нажатие f5
        {
            SaveGame();
        }
    }

    public void QuickLoad() // Быстрая загрузка
    {
        if (Input.GetKey(KeyCode.F6)) // Загрузить данные на нажатие f6
        {
            LoadGame();
        }
    }

    private List<IDataPersist> FindAllDataPersist() // Найти все доступные данные
    {
        Debug.Log("");

        IEnumerable<IDataPersist> dataPersistenceObjects = null;

        Debug.LogError(btnIsDown);
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // Найти все данные с типом MonoBehaviour и типом IDataPersist

        } else
        if(SceneManager.GetActiveScene().buildIndex == 0 && btnIsDown)
        {
            SceneLoader.GetSceneById(SceneManager.GetActiveScene().buildIndex + 1); // Получить следующую сцену
            dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // Найти все данные с типом MonoBehaviour и типом IDataPersist
            SceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        

        //IEnumerable<IDataPersist> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // Найти все данные с типом MonoBehaviour и типом IDataPersist
        //IEnumerable<IDataPersist> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // Найти все данные с типом MonoBehaviour и типом IDataPersist

        if (dataPersistenceObjects == null)
        {
            return null;
        }
        return new List<IDataPersist>(dataPersistenceObjects); // Вернуть новый список с даными
    }

    private void ButtonClicked()
    {
        btnIsDown = true;
    }
}
