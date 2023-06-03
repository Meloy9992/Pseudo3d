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
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(idPreviouse)); //��������� ���������� �����
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(idNextScene)); // ������� �������� ����� �����
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

        if(spawnedItems.Count < 4) // ���� ���������� ������ ������ 4, ��
        {
            AddChunk();
        }
        else
        if (spawnedItems.Count == 4) // ����� ���� ���������� ������ = 4
        {
            Chunk newChunk = Instantiate(bossChunk); // ������� ��������� ���� �����
            newChunk.transform.position = 
                spawnedItems[spawnedItems.Count - 1].end.position
                    - newChunk.begin.localPosition + new Vector3(0, 0, -25); // ��������� ������� �����

            spawnedItems.Add(newChunk); // �������� ���� ���� � ������ ������
            Debug.Log("���� � ������ ��������! " + newChunk.name + " " + newChunk.transform.position);
        }
        else
        if (spawnedItems.Count > 4)
        {
            Debug.Log("���������� ������!!!! " + spawnedItems.Count);
            LoadNewLevel();

        }
    }

    private void AddChunk()
    {
        Debug.Log("���������������!!!");
        Chunk newChunk = Instantiate(chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)]); // �������� ��������� ����

        newChunk.transform.position =
            spawnedItems[spawnedItems.Count - 1].end.position
                - newChunk.begin.localPosition + new Vector3(0, 0, -25); // ��������� ������� ������ ����� ����� ������� ���������� �����

        spawnedItems.Add(newChunk); // �������� ����� ����
    }

    private void LoadNewLevel()
    {
        int idNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (!SceneManager.GetSceneByBuildIndex(idNextScene).isLoaded)
        {
            Debug.Log("ID ��������� ����� " + idNextScene + " ID ������� �����" + SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(idNextScene, LoadSceneMode.Additive); //��������� ���� �����

            Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(idNextScene); //�������� ���� �����


            SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad); // ����������� ��������� �� ���� �����

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
