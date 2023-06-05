using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SerializeChunk : MonoBehaviour
{
    [SerializeField] public Transform begin;
    [SerializeField] public Transform end;
    [SerializeField] public Transform teleportEnds;
    [SerializeField] public Transform playersDot;
    [SerializeField] public Transform TeleportationPlace;
    [SerializeField] public GameObject teleport;

    [SerializeField] public GameObject DoorUp;
    [SerializeField] public GameObject DoorDown;
    [SerializeField] public GameObject DoorRight;
    [SerializeField] public GameObject DoorLeft;

    [SerializeField] public string FoundedTagName;

    [SerializeField] public List<Enemy> enemies;

    [SerializeField] public List<GameObject> allItems;

    [SerializeField] public Enemy[,] spawnedEnemies;

    [SerializeField] public Enemy[] EnemyPrefabs;

    [SerializeField] public GameObject[,] spawnedGrasses;

    [SerializeField] public Enemy startingEnemy;

    [SerializeField] public GameObject startingGrass;

    //public Mesh[] GrassMesh;
    [SerializeField] public Material[] GrassMaterials;

    public SerializeChunk(Chunk chunk)
    {
        this.begin = chunk.begin;
        this.end = chunk.end;
        this.teleportEnds = chunk.teleportEnds;
        this.playersDot = chunk.playersDot;
        this.TeleportationPlace = chunk.TeleportationPlace;
        this.teleport = chunk.teleport;
        this.DoorUp = chunk.DoorUp;
        this.DoorDown = chunk.DoorDown;
        this.DoorLeft = chunk.DoorLeft;
        this.DoorRight = chunk.DoorRight;
        this.FoundedTagName = chunk.FoundedTagName;
        this.enemies = chunk.enemies;
        this.allItems = chunk.allItems;
        this.spawnedEnemies = chunk.spawnedEnemies;
        this.EnemyPrefabs = chunk.EnemyPrefabs;
        this.spawnedGrasses = chunk.spawnedGrasses;
        this.startingEnemy = chunk.startingEnemy;
        this.startingGrass = chunk.startingGrass;
        this.GrassMaterials = chunk.GrassMaterials;
    }
}
