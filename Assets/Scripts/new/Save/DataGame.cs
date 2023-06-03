using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DataGame
{
    public Vector3 currentPlacePlayer;
    public int currentHpPlayer;
    public bool isFlippedRight;
    public int SceneNumber;
    public int countChunks;
    public List<Chunk> chunks;
    public int enemyCount;
    public Enemy[,] spawnedEnemy;

    public DataGame()
    {
        this.currentHpPlayer = -1;
        this.isFlippedRight = false;
        this.SceneNumber = 0;
        this.countChunks = 1;
        this.chunks = new List<Chunk>();
        this.spawnedEnemy = chunks.LastOrDefault().spawnedEnemies;
        this.enemyCount = chunks.LastOrDefault().enemies.Count;
        
    }
}
