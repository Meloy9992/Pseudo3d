using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Chunk : MonoBehaviour, IDataPersist
{
    public Transform begin;
    public Transform end;
    public Transform teleportEnds;
    public Transform playersDot;
    public Transform TeleportationPlace;
    public GameObject teleport;

    public GameObject DoorUp;
    public GameObject DoorDown;
    public GameObject DoorRight;
    public GameObject DoorLeft;

    public string FoundedTagName;

    public List<Enemy> enemies;

    public List<GameObject> allItems;

    public Enemy[,] spawnedEnemies;

    public Enemy[] EnemyPrefabs;

    public GameObject[,] spawnedGrasses;

    public Enemy startingEnemy;

    public GameObject startingGrass;

    //public Mesh[] GrassMesh;
    public Material[] GrassMaterials;

    private GameObject player;

    private IEnumerator Start()
    {
        teleport.SetActive(true); // TODO: Удалить эту строчку перед финалом

        spawnedEnemies = new Enemy[14, 10]; // Размер сетки спавна врагов

        spawnedEnemies[UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 9)] = startingEnemy; // Спавн начального противника


        findPlayer();

        GenerationGrass();

        for(int i = 0; i < 3; i++)
        {
            PlaceOneEnemy();
            yield return new WaitForSecondsRealtime(0.5f); // Размещение врагов с задержкой 0,5 секунды
        }

        FoundEnemy();

       // PlaceOneGrass();
    }


    private void Update()
    {
        if (enemies.Count != 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                }
            }

        }
        else
            if (enemies.Count == 0)
        {
            teleport.SetActive(true);
        }
    }
    private void findPlayer()
    {
        //Instantiate(GameObject.FindGameObjectWithTag("Player"), playersDot.position, playersDot.rotation);

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playersDot.position;
    }

    private void GenerationGrass()
    {
        foreach (var chunk in GetComponentsInChildren<MeshRenderer>()) // Пройти по всем компонентам MeshFilter
        {
            if (chunk != null)
            {
                if (GrassMaterials.Length != 0)
                {
                    if (chunk.sharedMaterials[0] == GrassMaterials[0]) // Если материал в переданном чанке будет равен первому элементу материалов травы, то
                    {
                        chunk.sharedMaterial = GrassMaterials[UnityEngine.Random.Range(0, GrassMaterials.Length)]; // Случайное размещение материала травы
                        chunk.transform.position += new Vector3(1, 0, 1 * UnityEngine.Random.Range(0, 5)); // Случайное размещение травы
                    }
                }
            }
        }
    }

    private void PlaceOneEnemy() // Создание сетки размещения объекта и его размещение
    {
        if (enemies.Count != 0)
        {
            HashSet<Vector2Int> vacantPlace = new HashSet<Vector2Int>(); // Список доступных мест

        for (int x = 0; x < spawnedEnemies.GetLength(0); x++) // Провести цикл по длине X
        {
            for (int y = 0; y < spawnedEnemies.GetLength(1); y++) // Провести цикл по длине Y
            {
                if (spawnedEnemies[x, y] == null) continue; // Если чанк по координатам равен нулю, то пропустить

                int maxX = spawnedEnemies.GetLength(0) - 1; // Получение максимального X
                int maxY = spawnedEnemies.GetLength(1) - 1; // Получение максимального Y

                if (x > 0 && spawnedEnemies[x - 1, y] == null) vacantPlace.Add(new Vector2Int(x - 1, y));// Если X больше 0 и предыдущий ряд чанков равен 0 // То добавить предыдущий ряд чанков

                if (y > 0 && spawnedEnemies[x, y - 1] == null) vacantPlace.Add(new Vector2Int(x, y - 1)); // Если Y больше 0 и предыдущий чанк равен 0 // Добавить новый чанк

                if (x < maxX && spawnedEnemies[x + 1, y] == null) vacantPlace.Add(new Vector2Int(x + 1, y)); // Если X меньше maxX и следующий ряд чанков равен 0 // То добавить следующий ряд чанков

                if (y < maxY && spawnedEnemies[x, y + 1] == null) vacantPlace.Add(new Vector2Int(x, y + 1)); // Если Y меньше maxY и следующий чанк равен 0 // То добавить следующий чанк
            }
        }

            Enemy newEnemy = Instantiate(EnemyPrefabs[UnityEngine.Random.Range(0, EnemyPrefabs.Length)]); // Создать противника из префаба
            Vector2Int position = vacantPlace.ElementAt(UnityEngine.Random.Range(0, vacantPlace.Count)); // Получить случайную позицию элемента
            int[] arr = RandomRange(position.x, position.y);
            newEnemy.transform.position = new Vector3((arr[0] - TeleportationPlace.position.x) * UnityEngine.Random.Range(-1, 1), 2, arr[1] + TeleportationPlace.position.z + 30); // Разместить противника на уровне (игровое поле x = 30, z = 20) position.y + TeleportationPlace.position.z + 30
            Debug.Log("МЕСТОНАХОЖДЕНИЕ ПРОТИВНИКА" + newEnemy.transform.position);
            Debug.Log("ТЕЛЕПОРТАТИОН ПЛЕЙС " + TeleportationPlace.position.x + " " + TeleportationPlace.position.z);
            spawnedEnemies[position.x, position.y] = newEnemy; // Разместить противника в матрице
            Debug.Log("МЕСТОНАХОЖДЕНИЕ ПРОТИВНИКА В МАТРИЦЕ " + position.x + " " + position.y);
        }
    } 


    private void PlaceOneGrass()
    {
        throw new NotImplementedException();
    }

    private void FoundEnemy()
    {
        Enemy[] items = FindObjectsOfType<Enemy>(); // Найти предметы по тегу
        for(int i = 0; i < items.Length; i++)
        {
            enemies.Add(items[i]); 
        }
    }


    private int[] RandomRange(int x, int z) // Вычисление двух чисел для процедурной генерации противников
    {
        int[] arr = new int[2];
        if (x > -14 && x < 14)
        {
            arr[0] = x;
        }
        if (z > -9 && z < 9)
        {
            arr[1] = z;
        }

        if (x > -14 && x < 14 && z > -9 && z < 9)
        {
            Debug.Log("X = " + x);
            Debug.Log("Z = " + z);
            return arr;
        }
        System.Random rnd = new System.Random();
        x -= rnd.Next(-10, 10); 
        z -= rnd.Next(-10, 10);
        return RandomRange(x, z);
    }

    public void LoadData(DataGame data)
    {
        this.spawnedEnemies = data.generateEnemy;
        
    }

    public void SaveData(ref DataGame data)
    {
        data.generateEnemy = this.spawnedEnemies;
    }
}
