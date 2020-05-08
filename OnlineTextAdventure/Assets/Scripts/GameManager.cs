using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Generics.Singleton;
using Game.Inventory;
using System;

public class GameManager : GenericSingleton<GameManager, GameManager>
{
    public int amountOfPlayers = 1;
    public Character playerPrefab;
    public Vector3[] playerSpawnPositions;
    public List<Character> players;

    public Character enemyPrefab;
    public Vector3[] enemySpawnPositions;
    public List<Character> enemies;

    public HealthBar healthBarPrefab;

    public RectTransform canvas;
    public Inventory inventory;


    public string[] GetEnemyNames()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < enemies.Count; i++)
        {
            names.Add(enemies[i].name);
        }

        return names.ToArray();
    }

    public string[] GetItemNames()
    {
        return inventory.GetAllItems().ToArray();
    }


    [EasyAttributes.Button]
    public void SpawnRoom()
    {
        SpawnPlayers();
        SpawnEnemies();
    }

    private void SpawnPlayers()
    {
        players = new List<Character>();
        for (int i = 0; i < amountOfPlayers; i++)
        {
            Character p = Instantiate(playerPrefab, playerSpawnPositions[i], Quaternion.identity);
            players.Add(p);
        }

        SpawnHealthBar(players);
    }

    private void SpawnEnemies()
    {
        Character e = Instantiate(enemyPrefab, enemySpawnPositions[0], Quaternion.identity);
        enemies.Add(e);

        SpawnHealthBar(enemies);
    }

    private void SpawnHealthBar(List<Character> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            Debug.Log(characters[i].name);
            HealthBar hp = Instantiate(healthBarPrefab, characters[i].transform.position + Vector3Int.up, Quaternion.identity);
            hp.AssignCharacter(characters[i]);

            hp.transform.SetParent(canvas, false);
        }
    }
}
