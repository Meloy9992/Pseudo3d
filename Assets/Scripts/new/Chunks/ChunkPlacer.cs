using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UIElements.VisualElement;

public class ChunkPlacer : MonoBehaviour, IDataPersist
{
    public GameObject player;
    //public Teleport teleport;
    public Chunk firstChunk;
    public Chunk[] chunkPrefabs;
    public Chunk bossChunk;

    private List<Chunk> spawnedItems = new List<Chunk>();
    private bool loadedIsDone = false;
    private bool newGameIsDone = false;

    private void Start()
    {
        spawnedItems.Add(firstChunk);
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        int idNextScene = SceneManager.GetActiveScene().buildIndex + 1;

        try
        {
            if (SceneManager.GetSceneByBuildIndex(idNextScene).isLoaded)
            {
                int idPreviouse = idNextScene - 1;
                int indexMaxScene = SceneManager.sceneCountInBuildSettings - 1;
                Debug.Log("МАКСИМАЛЬНОЕ КОЛИЧЕСТВО СЦЕН" + SceneManager.sceneCountInBuildSettings);
                if(SceneManager.GetActiveScene().buildIndex == indexMaxScene)
                {
                    Debug.Log("Конец игры");
                } else
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(idPreviouse)); //Выгрузить предыдущую сцену
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(idNextScene)); // Сделать активным новую сцену
                    player.transform.position = firstChunk.playersDot.position;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }

        Debug.Log("" + spawnedItems.Count);

        if (player.transform.position.z < spawnedItems[spawnedItems.Count - 1].end.position.z)
        {
            Debug.Log("Z OS PLAYER " + player.transform.position.z);
            Debug.Log("LAST CHUNK NAME " + spawnedItems[spawnedItems.Count - 1].name);
            Debug.Log("Z OS " + spawnedItems[spawnedItems.Count - 1].transform.position.z);
            SpawnChunk();

        }
    }

    public void SpawnChunk()
    {

        if(spawnedItems.Count < 4) // Если количество чанков меньше 4, то
        {
            AddChunk(); // Добавить чанк
        }
        else
        if (spawnedItems.Count == 4) // Иначе если количество чанков = 4
        {
            Chunk newChunk = Instantiate(bossChunk); // Создать экземпляр босс чанка
            newChunk.transform.position = 
                spawnedItems[spawnedItems.Count - 1].end.position
                    - newChunk.begin.localPosition + new Vector3(0, 0, -25); // Настройка позиции чанка

            spawnedItems.Add(newChunk); // Добавить босс чанк в список чанков
            Debug.Log("Чанк с боссом добавлен! " + newChunk.name + " " + newChunk.transform.position);
        }
        else
        if (spawnedItems.Count > 4)
        {
            Debug.Log("Количество чанков = " + spawnedItems.Count);
            LoadNewLevel();

        }
    }

    private void AddChunk()
    {
        Debug.Log("Телепортируемся!");
        Chunk newChunk = Instantiate(chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)]); // Получить случайный чанк

        newChunk.transform.position =
            spawnedItems[spawnedItems.Count - 1].end.position
                - newChunk.begin.localPosition + new Vector3(0, 0, -25); // Получение позиции нового чанка через позицию последнего чанка
        Debug.Log("Название чанка куда телепортируемся " + newChunk.name);
        Debug.Log("Координаты Z " + newChunk.transform.position.z);
        spawnedItems.Add(newChunk); // Добавить новый чанк
    }

    private void LoadNewLevel()
    {
        int idNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (!SceneManager.GetSceneByBuildIndex(idNextScene).isLoaded)
        {
            Debug.Log("ID следующей сцены " + idNextScene + " ID текущей сцены" + SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(idNextScene, LoadSceneMode.Additive); //Загрузить след сцену

            Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(idNextScene); //Получить след сцену

            SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad); // Переместить персонажа на след сцену

        }

    }

    public void LoadData(DataGame data)
    {       
            List<Chunk> prefabs = chunkPrefabs.ToList(); // Префабы уровней
            List<Chunk> placeChunk = new List<Chunk>(); // Список чанков которые будут размещены при загрузке

            for (int i = 0; i < data.chunksName.Count; i++) // Получить количество сохраненных чанков
            { // Перебрать все имена из сохранения
                for (int j = 0; j < prefabs.Count; j++) // Получить количество префабов
                { // Перебрать все имена из префабов
                
                if (prefabs[j].name.Equals(data.chunksName[i])) // Если имя префаба совпало с именем чанка
                    {
                        placeChunk.Add(prefabs[j]); // добавить в список 
                        Debug.LogError("Префаб добавлен " + prefabs[j].name);
                        loadedIsDone = true;
                }
                }
            }

            if (data.chunksPlace != null)
            {
                if(data.chunksPlace.Count != 0)
                {
                data.chunksPlace.RemoveAt(0);
                }

            }

            if(placeChunk.Count == 4)
                {
                    placeChunk.Add(bossChunk);
                }

            for (int i = 0; i < data.chunksPlace.Count; i++) // Перечислить все места чанков из сохранения
            {
                Debug.LogError(" I в цикле = " + i + " Количество чанков из координат = " + data.chunksPlace.Count);
                Debug.LogError("Название чанка " + placeChunk[i].name + " Координаты этого чанка по оси Z " + data.chunksPlace[i]);
                placeChunk[i].transform.position = data.chunksPlace[i];
                Instantiate(placeChunk[i]); // Разместить чанк по координам из сохранения
                Debug.LogError("Координаты которые на самом деле " + placeChunk[i].transform.position.z);
        }
                if(spawnedItems.Count == 1)
        {
            foreach (Chunk chunk in placeChunk)
            {
                spawnedItems.Add(chunk);
            }
        }

        if(spawnedItems.Count == 0)
        {
            spawnedItems.Add(firstChunk);
            newGameIsDone = true;
        }
    }

    public void SaveData(ref DataGame data)
    {
        Debug.Log("");
        data.chunks = spawnedItems;

        List<string> nameChunks = new List<string>();
        List<Vector3> vector3s = new List<Vector3>();

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            nameChunks.Add(spawnedItems[i].name.Replace("(Clone)", String.Empty));
            vector3s.Add(spawnedItems[i].transform.position);
        }

        data.chunksName = nameChunks;
        data.chunksPlace = vector3s;

    }

    public static GameObject getObjectById(int id)
    {
        Dictionary<int, GameObject> m_instanceMap = new Dictionary<int, GameObject>();

        m_instanceMap.Clear();
        List<GameObject> gos = new List<GameObject>();
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (gos.Contains(go))
            {
                continue;
            }
            gos.Add(go);
            m_instanceMap[go.GetInstanceID()] = go;
        }

        if (m_instanceMap.ContainsKey(id))
        {
            return m_instanceMap[id];
        }
        else
        {
            return null;
        }
    }
}
