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
                Debug.Log("������������ ���������� ����" + SceneManager.sceneCountInBuildSettings);
                if(SceneManager.GetActiveScene().buildIndex == indexMaxScene)
                {
                    Debug.Log("����� ����");
                } else
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(idPreviouse)); //��������� ���������� �����
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(idNextScene)); // ������� �������� ����� �����
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

        if(spawnedItems.Count < 4) // ���� ���������� ������ ������ 4, ��
        {
            AddChunk(); // �������� ����
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
            Debug.Log("���������� ������ = " + spawnedItems.Count);
            LoadNewLevel();

        }
    }

    private void AddChunk()
    {
        Debug.Log("���������������!");
        Chunk newChunk = Instantiate(chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)]); // �������� ��������� ����

        newChunk.transform.position =
            spawnedItems[spawnedItems.Count - 1].end.position
                - newChunk.begin.localPosition + new Vector3(0, 0, -25); // ��������� ������� ������ ����� ����� ������� ���������� �����
        Debug.Log("�������� ����� ���� ��������������� " + newChunk.name);
        Debug.Log("���������� Z " + newChunk.transform.position.z);
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
            List<Chunk> prefabs = chunkPrefabs.ToList(); // ������� �������
            List<Chunk> placeChunk = new List<Chunk>(); // ������ ������ ������� ����� ��������� ��� ��������

            for (int i = 0; i < data.chunksName.Count; i++) // �������� ���������� ����������� ������
            { // ��������� ��� ����� �� ����������
                for (int j = 0; j < prefabs.Count; j++) // �������� ���������� ��������
                { // ��������� ��� ����� �� ��������
                
                if (prefabs[j].name.Equals(data.chunksName[i])) // ���� ��� ������� ������� � ������ �����
                    {
                        placeChunk.Add(prefabs[j]); // �������� � ������ 
                        Debug.LogError("������ �������� " + prefabs[j].name);
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

            for (int i = 0; i < data.chunksPlace.Count; i++) // ����������� ��� ����� ������ �� ����������
            {
                Debug.LogError(" I � ����� = " + i + " ���������� ������ �� ��������� = " + data.chunksPlace.Count);
                Debug.LogError("�������� ����� " + placeChunk[i].name + " ���������� ����� ����� �� ��� Z " + data.chunksPlace[i]);
                placeChunk[i].transform.position = data.chunksPlace[i];
                Instantiate(placeChunk[i]); // ���������� ���� �� ��������� �� ����������
                Debug.LogError("���������� ������� �� ����� ���� " + placeChunk[i].transform.position.z);
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
