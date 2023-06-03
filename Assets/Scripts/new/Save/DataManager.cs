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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); // Указываем формат и название файл куда будет сохраняться/загружаться
        this.dataPersistenceObjects = FindAllDataPersist(); // Найти все данные
        LoadGame(); // Загрузить игру
    }

    private void Update()
    {
        QuickSave(); // Быстрое сохранение
        QuickLoad(); // Быстрая загрузка
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

        foreach(IDataPersist dataPersist in dataPersistenceObjects) // Перечислить данные из всех классов которые унаследованы от IDataPersist
        {
            dataPersist.LoadData(dataGame); // загрузить данные в файл
        }

        Debug.Log("Произошла загрузка параметров:  Местоположение персонажа: " + dataGame.currentPlacePlayer + " ХП: "
            + dataGame.currentHpPlayer + " игрок повернут на право? " + dataGame.isFlippedRight);

    }

    public void SaveGame()
    {
        foreach(IDataPersist dataPersist in dataPersistenceObjects) // Перечислить данные из всех классов которые унаследованы от IDataPersist
        {
            dataPersist.SaveData(ref dataGame); // Сохранить данные в объект
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
        IEnumerable<IDataPersist> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersist>(); // Найти все данные с типом MonoBehaviour и типом IDataPersist

        return new List<IDataPersist>(dataPersistenceObjects); // Вернуть новый список с даными
    }
}
