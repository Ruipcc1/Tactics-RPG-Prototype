using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject TilePrefab;
    public GameObject UserPlayerPrefab;
    public GameObject AiPlayerPrefab;

    public int mapSize = 11;

    List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();

    public int currentPlayerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        generateMap();
        generatePlayers();
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (players[currentPlayerIndex].HP <= 0)
        {
            players.Remove(players[currentPlayerIndex]);
            nextTurn();
        }
        players[currentPlayerIndex].TurnUpdate();
    }

    void OnGUI()
    {
        players[currentPlayerIndex].TurnOnGUI();
    }

    public void nextTurn()
    {
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }
    }

    public void moveCurrentPlayer(Tile destTile)
    {
        players[currentPlayerIndex].gridPosition = destTile.gridPosition;
        players[currentPlayerIndex].moveDestination = destTile.transform.position + 1.5f * Vector3.up;
    }

    public void attackWithCurrentPlayer(Tile destTile)
    {
        Player target = null;
        foreach (Player p in players)
        {
            if (p.gridPosition == destTile.gridPosition)
            {
                target = p;
            }
        }

        if (target != null)
        {
            if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 && 
                players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1)
            {
                players[currentPlayerIndex].actionPoints--;
                bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].attackChance;
                if (hit)
                {
                    int amountofDamage = players[currentPlayerIndex].damageBase + Random.Range(0, players[currentPlayerIndex].damageRollSides);

                    target.HP -= amountofDamage;
                    Debug.Log(players[currentPlayerIndex].playerName + "hit" + target.playerName + "for" + amountofDamage);
                }
                else
                {
                    Debug.Log(players[currentPlayerIndex].playerName + "missed" + target.playerName);
                }
            }
            else
            {
                Debug.Log("No Target");
            }
        }
    }

    void generateMap()
    {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSize; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSize; j++)
            {
                Tile tile = GameObject.Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    void generatePlayers()
    {
        PlayerMovement player;
        AIMovement aiplayer;

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 1.5f, 0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(0, 0);
        player.playerName = "Heyward";
        players.Add(player);

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), 1.5f, -(mapSize - 1) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(mapSize - 1, mapSize - 1);
        player.playerName = "Cory";
        players.Add(player);

        player = GameObject.Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3())).GetComponent<PlayerMovement>();
        player.gridPosition = new Vector2(4, 4);
        player.playerName = "Chas";
        players.Add(player);
    }
}

