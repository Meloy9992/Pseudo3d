using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DataGame
{
    public Vector3 currentPlacePlayer; // ������� �������������� ���������
    public int currentHpPlayer; // ������� hp ���������
    public bool isFlippedRight; // ��������� �������� �� ����� � ������ �������

    public int SceneNumber; // ����� �����/������
    public int countChunks; // ���������� ������
    public List<Chunk> chunks; // ������ ������
    public Player player;
    public int enemyCount; // ���������� ����������� �� �����
    public List<Enemy> spawnedEnemy; // ����������� �����
    public Enemy[,] generateEnemy; // ����� �� ���������� �������
    /*    public Enemy[,] generateEnemy; // ����� �� ���������� ������� 
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
