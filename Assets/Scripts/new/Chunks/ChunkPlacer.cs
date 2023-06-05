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
        //spawnedItems.LastOrDefault().enemies = ;
        if(data.chunks != null)
        {
            if (data.chunks.Count != 0)
            {
                Debug.LogError("������� ����� " + spawnedItems);
                spawnedItems = data.chunks; // ��������� ������ �� ����������

/*                foreach (Chunk chunk in spawnedItems)
                {
                    Instantiate(chunk.transform,
                            transform.position,
                            transform.rotation);
                }*/

                for(int i = 0; i < spawnedItems.Count; i++)
                {
/*                    Chunk chunk = new Chunk();*/
                    Debug.Log(data.chunksName[i]);
                    Debug.LogError(data.chunks[i].name);
/*                    this.name = data.chunksName[i];
                    this.transform.position = data.chunksPlace[i];*/

/*                    Instantiate(this.transform,
                                transform.position,
                                transform.rotation);*/

                }
                Debug.LogError("����������� ����� " + spawnedItems);
            }
            else
            {
                Debug.LogError("������ ���������� ����� ����");
            }
        }

        /*        for (int i = 0; i < data.spawnedEnemy.Count; i++)
                {
                    Debug.Log("������ �������� �� ������� " + i);
                    Destroy(spawnedItems.LastOrDefault().enemies[i]); // ��������� ������
                    Debug.Log("������ �������� �� ������� " + i);
                    spawnedItems.LastOrDefault().enemies.RemoveAt(i); // ������� ������ �� ���� ������
                    Debug.Log("������ ������� �� ������� " + i);

                    spawnedItems.LastOrDefault().enemies.Add(data.spawnedEnemy[i]); // ���������� ������ �� ����������
                    GameObject enemy = GameObject.Find(data.spawnedEnemy[i].ToString());
                    Instantiate(enemy); // ���������� ����� �������
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

        // ������� ������� � ������
        // �������� ������ ����� �������



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
