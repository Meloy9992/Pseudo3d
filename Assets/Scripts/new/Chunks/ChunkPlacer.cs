using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.VisualElement;

public class ChunkPlacer : MonoBehaviour, IDataPersist
{
    public GameObject player;
    //public Teleport teleport;
    public Chunk firstChunk;
    public Chunk[] chunkPrefabs;
    public Chunk bossChunk;



    private List<Chunk> spawnedItems = new List<Chunk>();
    private bool isDone = true;

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

        if (player.transform.position.z < spawnedItems[spawnedItems.Count - 1].end.position.z)
        {
            Debug.Log("Z OS PLAYER " + player.transform.position.z);
            Debug.Log("Z OS TELEPORT " + spawnedItems[spawnedItems.Count - 1].teleportEnds.position.z);
            SpawnChunk();


        }
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
        spawnedItems.LastOrDefault().enemies = data.spawnedEnemy;
    }

    public void SaveData(ref DataGame data)
    {
       
        data.spawnedEnemy = this.spawnedItems.LastOrDefault().enemies;
    }
}
