using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DataGame
{
    public Vector3 currentPlacePlayer; // Текущее местоположение персонажа
    public int currentHpPlayer; // Текущее hp персонажа
    public bool isFlippedRight; // Положение повернут ли игрок в правую сторону

    public int SceneNumber; // Номер сцены/уровня
    public int countChunks; // Количество чанков
    public List<Chunk> chunks; // Список чанков
    public Player player;
    public int enemyCount; // Количество противников на чанке
    public List<Enemy> spawnedEnemy; // Заспавненые враги
    public Enemy[,] generateEnemy; // Сетка со случайными врагами
    /*    public Enemy[,] generateEnemy; // Сетка со случайными врагами 
        public int HpEnemy;
        public int damageEnemy;
        public Vector3 currentPlaceEnemy;
        public float speedEnemy;
        public Enemy randomEnemy;*/

    public DataGame()
    {
        this.currentHpPlayer = -1;
        this.isFlippedRight = false;
        this.SceneNumber = 0;
        this.countChunks = 1;
/*        this.chunks = new List<Chunk>();
        this.spawnedEnemy = new List<Enemy>();*/
        // this.enemyCount = chunks.LastOrDefault().enemies.Count;

    }
}
