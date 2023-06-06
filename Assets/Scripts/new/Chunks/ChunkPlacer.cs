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
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(idPreviouse)); //Выгрузить предыдущую сцену
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(idNextScene)); // Сделать активным новую сцену
                player.transform.position = firstChunk.playersDot.position;
            }
        }
        catch(Exception ex)
        {
            //Debug.LogException(ex);
        }
        Debug.Log(spawnedItems.Count);
        Debug.Log(spawnedItems.Count);

        // TODO - Баг. Последняя позиция телепорта больше чеи игрока. Слишком быстрое обновление
        //Fade();
        // spawnedItems = -8
        // player = -89

        if (player.transform.position.z < spawnedItems[spawnedItems.Count - 1].end.position.z)
        {
            Debug.Log("Z OS PLAYER " + player.transform.position.z);
            Debug.Log("LAST CHUNK NAME " + spawnedItems[spawnedItems.Count - 1].name);
            Debug.Log("Z OS TELEPORT " + spawnedItems[spawnedItems.Count - 1].teleportEnds.position.z);
            SpawnChunk();

        }
    }


    IEnumerator Fade()
    {
        if (player.transform.position.z < spawnedItems[spawnedItems.Count - 1].end.position.z)
        {
            Debug.Log("Z OS PLAYER " + player.transform.position.z);
            Debug.Log("LAST CHUNK NAME " + spawnedItems[spawnedItems.Count - 1].name);
            Debug.Log("Z OS TELEPORT " + spawnedItems[spawnedItems.Count - 1].teleportEnds.position.z);
            SpawnChunk();

        }
        //yield return new WaitForSeconds(.1f);
        yield return null;
    }
    public void SpawnChunk()
    {

        if(spawnedItems.Count < 4) // Если количество чанков меньше 4, то
        {
            AddChunk();
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
            Debug.Log("КОЛИЧЕСТВО ЧАНКОВ!!!! " + spawnedItems.Count);
            LoadNewLevel();

        }
    }

    private void AddChunk()
    {
        Debug.Log("Телепортируемся!!!");
        Chunk newChunk = Instantiate(chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)]); // Получить случайный чанк

        newChunk.transform.position =
            spawnedItems[spawnedItems.Count - 1].end.position
                - newChunk.begin.localPosition + new Vector3(0, 0, -25); // Получение позиции нового чанка через позицию последнего чанка

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
            player.transform.position = data.currentPlacePlayer.normalized;
            
            List<Chunk> prefabs = chunkPrefabs.ToList(); // Префабы уровней
            List<Chunk> placeChunk = new List<Chunk>(); // Список чанков которые будут размещены при загрузке


            for (int i = 0; i < data.chunksName.Count; i++) // Получить количество сохраненных чанков
            { // Перебрать все имена из сохранения
                for (int j = 0; j < prefabs.Count; j++) // Получить количество префабов
                { // Перебрать все имена из префабов
                
                if (prefabs[j].name.Equals(data.chunksName[i])) // Если имя префаба совпало с именем чанка
                    {
                        placeChunk.Add(prefabs[j]); // добавить в список 
                        Debug.LogError("ПРЕФАБ ДОБАВЛЕН " + prefabs[j].name);
                        loadedIsDone = true;
                }
                }
            }

        //placer.spawnedItems = placeChunk;
            if (data.chunksPlace != null || data.chunksPlace.Count != 0)
            {
                data.chunksPlace.RemoveAt(0);
            }

            for (int i = 0; i < data.chunksPlace.Count; i++) // Перечислить все места чанков из сохранения
            {
                // Префабов = 2 [0, 1]
                // ЧАНКОВ 3  [0, 1, 2]
                Debug.LogError(" I в цикле = " + i + " Количество чанков из координат = " + data.chunksPlace.Count);
                //placeChunk[i].name = data.chunksName[i]; // Имя чанка = имени чанка их сохранения

                Instantiate(placeChunk[i],
                            data.chunksPlace[i],
                            transform.rotation); // Разместить чанк по координам из сохранения
            }
            spawnedItems = placeChunk;


        if(spawnedItems.Count == 0)
        {
            spawnedItems.Add(firstChunk);
            newGameIsDone = true;
        }

        
        //spawnedItems = data.chunks;

        /*        //spawnedItems.LastOrDefault().enemies = ;
                if(data.chunks != null)
                {
                    if (data.chunks.Count != 0)
                    {
                        Debug.LogError("Текущие чанки " + spawnedItems);
                        spawnedItems = data.chunks; // Получение чанков из сохранения

        *//*                foreach (Chunk chunk in spawnedItems)
                        {
                            Instantiate(chunk.transform,
                                    transform.position,
                                    transform.rotation);
                        }*//*

                        for(int i = 0; i < spawnedItems.Count; i++)
                        {
        *//*                    Chunk chunk = new Chunk();*//*
                            Debug.Log(data.chunksName[i]);
        *//*                    this.name = data.chunksName[i];
                            this.transform.position = data.chunksPlace[i];*/

        /*                    Instantiate(this.transform,
                                        transform.position,
                                        transform.rotation);*//*

                        }
                        Debug.LogError("Обновленные чанки " + spawnedItems);
                    }
                    else
                    {
                        Debug.LogError("Данные сохранения равны нулю");
                    }
                }*/

        /*        for (int i = 0; i < data.spawnedEnemy.Count; i++)
                {
                    Debug.Log("Начало удаления по индексу " + i);
                    Destroy(spawnedItems.LastOrDefault().enemies[i]); // Разрушить объект
                    Debug.Log("Объект разрушен по индексу " + i);
                    spawnedItems.LastOrDefault().enemies.RemoveAt(i); // Удалить ссылки на этот объект
                    Debug.Log("Ссылка удалена по индексу " + i);

                    spawnedItems.LastOrDefault().enemies.Add(data.spawnedEnemy[i]); // Добавление данных из сохранения
                    GameObject enemy = GameObject.Find(data.spawnedEnemy[i].ToString());
                    Instantiate(enemy); // Добавление копии объекта
                }
                spawnedItems.LastOrDefault().spawnedEnemies = null;

                //this.spawnedItems = data.chunks;
                // GameObject.Find(data.chunks[0].ToString()).transform.position = whatever;
                // updatePlayerVector(new Vector3(4.69f, 1.02f, -43.68f));
                Debug.LogError(data);
                Debug.LogError(data);
                //spawnedItems = getObjectById();
                Vector3 vector = new Vector3(data.currentPlacePlayer.x, data.currentPlacePlayer.y, data.currentPlacePlayer.z);
                */
    }

    public void SaveData(ref DataGame data)
    {

        /*        data.spawnedEnemy = this.spawnedItems.LastOrDefault().enemies;

                data.HpEnemy = spawnedItems.LastOrDefault().enemies.LastOrDefault().health;
                data.damageEnemy = spawnedItems.LastOrDefault().enemies.LastOrDefault().damage;
                data.currentPlaceEnemy = spawnedItems.LastOrDefault().enemies.LastOrDefault().transform.position;
                data.speedEnemy = spawnedItems.LastOrDefault().enemies.LastOrDefault().normalSpeed;*/



        data.chunks = spawnedItems;

        // Собрать обхекты в список
        // Заменить список новым списком



        List<string> nameChunks = new List<string>();
        List<Vector3> vector3s = new List<Vector3>();

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            nameChunks.Add(spawnedItems[i].name);
            vector3s.Add(spawnedItems[i].transform.position);
        }

        data.chunksName = nameChunks;
        data.chunksPlace = vector3s;


        /*        for (int i = 0; i < spawnedItems.Count; i++)
                {
                    data.chunks[i].begin = spawnedItems[i].begin;
                    data.chunks[i].end = spawnedItems[i].end;
                    data.chunks[i].teleportEnds = spawnedItems[i].teleportEnds;
                    data.chunks[i].playersDot = spawnedItems[i].playersDot;
                    data.chunks[i].TeleportationPlace = spawnedItems[i].TeleportationPlace;
                    data.chunks[i].teleport = spawnedItems[i].teleport;
                    data.chunks[i].DoorUp = spawnedItems[i].DoorUp;
                    data.chunks[i].spawnedEnemies = spawnedItems[i].spawnedEnemies;

                }*/

        // data.chunks = JsonHelper.ToJson()
    }

    public static GameObject getObjectById(int id)
    {
        Dictionary<int, GameObject> m_instanceMap = new Dictionary<int, GameObject>();
        //record instance map

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

    private void updatePlayerVector(Vector3 vector)
    {
        player.transform.position = vector;
    }
}
